using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows.Forms;

TrySetConsoleOutputEncoding();

if (args.Length == 0)
{
    return RunGuiMode();
}

if (args.Length < 2)
{
    PrintUsage();
    return 1;
}

var command = args[0].ToLowerInvariant();
var assemblyPath = Path.GetFullPath(args[1]);
if (!File.Exists(assemblyPath))
{
    Console.Error.WriteLine($"File not found: {assemblyPath}");
    return 1;
}

return command switch
{
    "scan" => ScanAssembly(assemblyPath),
    "extract" => ExtractStrings(assemblyPath, args.Length > 2 ? args[2] : "strings.json"),
    "patch" => PatchAssemblyFromCommand(assemblyPath, args),
    "dump-type" => DumpType(assemblyPath, args),
    _ => UnknownCommand(command)
};

static int RunGuiMode()
{
    try
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new PatchToolForm(
            FindDefaultAssemblyPath,
            GetSuggestedOutputPath,
            (inputPath, outputPath) => PatchAssembly(inputPath, outputPath, LoadBundledTranslations())));
        return 0;
    }
    catch (Exception ex)
    {
        MessageBox.Show(
            ex.Message,
            "MR0-485-2M1 Patcher",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
        return 1;
    }
}

static int ScanAssembly(string assemblyPath)
{
    var module = ModuleDefinition.ReadModule(assemblyPath);
    var seen = new HashSet<string>();

    foreach (var type in module.Types.OrderBy(t => t.FullName))
    {
        foreach (var method in type.Methods.Where(m => m.HasBody))
        {
            var instructions = method.Body.Instructions;
            for (var i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].OpCode != OpCodes.Ldstr || instructions[i].Operand is not string value)
                {
                    continue;
                }

                if (!ContainsChinese(value) || !seen.Add(value))
                {
                    continue;
                }

                Console.WriteLine($"STRING: {value}");
                Console.WriteLine($"  TYPE:   {type.FullName}");
                Console.WriteLine($"  METHOD: {method.Name}");
                Console.WriteLine($"  NEXT:   {DescribeNextUse(instructions, i)}");
                Console.WriteLine();
            }
        }
    }

    return 0;
}

static void TrySetConsoleOutputEncoding()
{
    try
    {
        Console.OutputEncoding = Encoding.UTF8;
    }
    catch (IOException)
    {
    }
}

static int ExtractStrings(string assemblyPath, string outputPath)
{
    var module = ModuleDefinition.ReadModule(assemblyPath);
    var strings = new SortedSet<string>(StringComparer.Ordinal);

    foreach (var type in module.Types)
    {
        foreach (var method in type.Methods.Where(m => m.HasBody))
        {
            foreach (var instruction in method.Body.Instructions)
            {
                if (instruction.OpCode == OpCodes.Ldstr && instruction.Operand is string value && ContainsChinese(value))
                {
                    strings.Add(value);
                }
            }
        }
    }

    var items = strings.Select(s => new TranslationItem(s, "")).ToArray();
    var options = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true
    };
    File.WriteAllText(Path.GetFullPath(outputPath), JsonSerializer.Serialize(items, options), Encoding.UTF8);
    Console.WriteLine($"Extracted {items.Length} strings to {Path.GetFullPath(outputPath)}");
    return 0;
}

static int PatchAssemblyFromCommand(string assemblyPath, string[] args)
{
    if (args.Length < 3)
    {
        Console.Error.WriteLine("Usage: TranslatorTools patch <assembly-path> <output-assembly>");
        Console.Error.WriteLine("   or: TranslatorTools patch <assembly-path> <translations.json> <output-assembly>");
        return 1;
    }

    IReadOnlyDictionary<string, string> translations;
    string outputPath;

    if (args.Length >= 4)
    {
        translations = LoadTranslationsFromFile(args[2]);
        outputPath = Path.GetFullPath(args[3]);
    }
    else
    {
        translations = LoadBundledTranslations();
        outputPath = Path.GetFullPath(args[2]);
    }

    var result = PatchAssembly(assemblyPath, outputPath, translations);
    Console.WriteLine($"Patched {result.PatchedStrings} string loads and {result.PatchedLayoutValues} layout values into {result.OutputPath}");
    return 0;
}

static PatchResult PatchAssembly(string assemblyPath, string outputPath, IReadOnlyDictionary<string, string> translations)
{
    var module = ModuleDefinition.ReadModule(assemblyPath);
    var patchedCount = 0;
    var layoutPatchedCount = 0;

    foreach (var type in module.Types)
    {
        foreach (var method in type.Methods.Where(m => m.HasBody))
        {
            foreach (var instruction in method.Body.Instructions)
            {
                if (instruction.OpCode == OpCodes.Ldstr &&
                    instruction.Operand is string value &&
                    translations.TryGetValue(value, out var translated) &&
                    !string.Equals(value, translated, StringComparison.Ordinal))
                {
                    instruction.Operand = translated;
                    patchedCount++;
                }
            }
        }
    }

    layoutPatchedCount += ApplyMr0LayoutTweaks(module);
    module.Write(outputPath);
    return new PatchResult(patchedCount, layoutPatchedCount, outputPath);
}

static int DumpType(string assemblyPath, string[] args)
{
    if (args.Length < 3)
    {
        Console.Error.WriteLine("Usage: TranslatorTools dump-type <assembly-path> <full-type-name>");
        return 1;
    }

    var typeName = args[2];
    var module = ModuleDefinition.ReadModule(assemblyPath);
    var type = module.Types.FirstOrDefault(t => string.Equals(t.FullName, typeName, StringComparison.Ordinal));
    if (type is null)
    {
        Console.Error.WriteLine($"Type not found: {typeName}");
        return 1;
    }

    Console.WriteLine($"TYPE {type.FullName}");
    foreach (var method in type.Methods)
    {
        Console.WriteLine();
        Console.WriteLine($"METHOD {method.FullName}");
        if (!method.HasBody)
        {
            Console.WriteLine("  <no body>");
            continue;
        }

        foreach (var instruction in method.Body.Instructions)
        {
            var operand = instruction.Operand switch
            {
                null => "",
                string s => $"\"{s}\"",
                MethodReference m => m.FullName,
                FieldReference f => f.FullName,
                ParameterDefinition p => p.Name,
                VariableDefinition v => $"V_{v.Index}",
                Instruction target => $"IL_{target.Offset:X4}",
                Instruction[] targets => string.Join(", ", targets.Select(t => $"IL_{t.Offset:X4}")),
                _ => instruction.Operand.ToString() ?? ""
            };

            Console.WriteLine($"  IL_{instruction.Offset:X4}: {instruction.OpCode,-12} {operand}");
        }
    }

    return 0;
}

static string DescribeNextUse(Mono.Collections.Generic.Collection<Instruction> instructions, int index)
{
    for (var i = index + 1; i < Math.Min(index + 8, instructions.Count); i++)
    {
        var instruction = instructions[i];
        if (instruction.Operand is MethodReference method)
        {
            return $"{instruction.OpCode} {method.FullName}";
        }

        if (instruction.Operand is FieldReference field)
        {
            return $"{instruction.OpCode} {field.FullName}";
        }

        if (instruction.Operand is string str)
        {
            return $"{instruction.OpCode} \"{str}\"";
        }
    }

    return "no nearby call";
}

static bool ContainsChinese(string value)
{
    foreach (var ch in value)
    {
        if (ch >= 0x4E00 && ch <= 0x9FFF)
        {
            return true;
        }
    }

    return false;
}

static int ApplyMr0LayoutTweaks(ModuleDefinition module)
{
    var type = module.Types.FirstOrDefault(t => string.Equals(t.FullName, "MODBUS_Tool.JYX_MODBUS_2M1", StringComparison.Ordinal));
    var initializeComponent = type?.Methods.FirstOrDefault(m => string.Equals(m.Name, "InitializeComponent", StringComparison.Ordinal) && m.HasBody);
    if (initializeComponent is null)
    {
        return 0;
    }

    var changes = 0;

    changes += PatchControlSize(initializeComponent, "groupBox2", width: 360, height: 177);
    changes += PatchControlSize(initializeComponent, "gpBox_ComSet", width: 230, height: 134);
    changes += PatchControlLocation(initializeComponent, "btKEY_OpenCOM", x: 246, y: 26);
    changes += PatchControlLocation(initializeComponent, "btKEY_FindComNum", x: 246, y: 75);
    changes += PatchControlLocation(initializeComponent, "btConnState", x: 247, y: 132);
    changes += PatchControlLocation(initializeComponent, "Read_All", x: 266, y: 201);
    changes += PatchControlLocation(initializeComponent, "Set_All", x: 171, y: 428);
    changes += PatchControlLocation(initializeComponent, "Parameter_Set", x: 377, y: 4);
    changes += PatchFormClientSize(initializeComponent, width: 674, height: 507);

    return changes;
}

static int PatchControlSize(MethodDefinition method, string fieldName, int width, int height)
{
    return PatchObjectIntPairSetter(
        method,
        fieldName,
        "System.Drawing.Size",
        "System.Void System.Windows.Forms.Control::set_Size(System.Drawing.Size)",
        width,
        height);
}

static int PatchControlLocation(MethodDefinition method, string fieldName, int x, int y)
{
    return PatchObjectIntPairSetter(
        method,
        fieldName,
        "System.Drawing.Point",
        "System.Void System.Windows.Forms.Control::set_Location(System.Drawing.Point)",
        x,
        y);
}

static int PatchFormClientSize(MethodDefinition method, int width, int height)
{
    var instructions = method.Body.Instructions;
    for (var i = 0; i <= instructions.Count - 5; i++)
    {
        if (instructions[i].OpCode != OpCodes.Ldarg_0)
        {
            continue;
        }

        if (!TryGetInt32(instructions[i + 1], out _) ||
            !TryGetInt32(instructions[i + 2], out _) ||
            instructions[i + 3].OpCode != OpCodes.Newobj ||
            instructions[i + 3].Operand is not MethodReference ctor ||
            ctor.DeclaringType.FullName != "System.Drawing.Size" ||
            instructions[i + 4].Operand is not MethodReference setter ||
            setter.FullName != "System.Void System.Windows.Forms.Form::set_ClientSize(System.Drawing.Size)")
        {
            continue;
        }

        SetInt32(instructions[i + 1], width);
        SetInt32(instructions[i + 2], height);
        return 2;
    }

    return 0;
}

static int PatchObjectIntPairSetter(
    MethodDefinition method,
    string fieldName,
    string valueTypeFullName,
    string setterFullName,
    int firstValue,
    int secondValue)
{
    var instructions = method.Body.Instructions;
    for (var i = 0; i <= instructions.Count - 6; i++)
    {
        if (instructions[i].OpCode != OpCodes.Ldarg_0 ||
            instructions[i + 1].OpCode != OpCodes.Ldfld ||
            instructions[i + 1].Operand is not FieldReference field ||
            !string.Equals(field.Name, fieldName, StringComparison.Ordinal))
        {
            continue;
        }

        if (!TryGetInt32(instructions[i + 2], out _) ||
            !TryGetInt32(instructions[i + 3], out _) ||
            instructions[i + 4].OpCode != OpCodes.Newobj ||
            instructions[i + 4].Operand is not MethodReference ctor ||
            ctor.DeclaringType.FullName != valueTypeFullName ||
            instructions[i + 5].Operand is not MethodReference setter ||
            setter.FullName != setterFullName)
        {
            continue;
        }

        SetInt32(instructions[i + 2], firstValue);
        SetInt32(instructions[i + 3], secondValue);
        return 2;
    }

    return 0;
}

static bool TryGetInt32(Instruction instruction, out int value)
{
    switch (instruction.OpCode.Code)
    {
        case Code.Ldc_I4_M1:
            value = -1;
            return true;
        case Code.Ldc_I4_0:
            value = 0;
            return true;
        case Code.Ldc_I4_1:
            value = 1;
            return true;
        case Code.Ldc_I4_2:
            value = 2;
            return true;
        case Code.Ldc_I4_3:
            value = 3;
            return true;
        case Code.Ldc_I4_4:
            value = 4;
            return true;
        case Code.Ldc_I4_5:
            value = 5;
            return true;
        case Code.Ldc_I4_6:
            value = 6;
            return true;
        case Code.Ldc_I4_7:
            value = 7;
            return true;
        case Code.Ldc_I4_8:
            value = 8;
            return true;
        case Code.Ldc_I4_S:
            value = (sbyte)instruction.Operand;
            return true;
        case Code.Ldc_I4:
            value = (int)instruction.Operand;
            return true;
        default:
            value = 0;
            return false;
    }
}

static void SetInt32(Instruction instruction, int value)
{
    switch (value)
    {
        case -1:
            instruction.OpCode = OpCodes.Ldc_I4_M1;
            instruction.Operand = null;
            return;
        case 0:
            instruction.OpCode = OpCodes.Ldc_I4_0;
            instruction.Operand = null;
            return;
        case 1:
            instruction.OpCode = OpCodes.Ldc_I4_1;
            instruction.Operand = null;
            return;
        case 2:
            instruction.OpCode = OpCodes.Ldc_I4_2;
            instruction.Operand = null;
            return;
        case 3:
            instruction.OpCode = OpCodes.Ldc_I4_3;
            instruction.Operand = null;
            return;
        case 4:
            instruction.OpCode = OpCodes.Ldc_I4_4;
            instruction.Operand = null;
            return;
        case 5:
            instruction.OpCode = OpCodes.Ldc_I4_5;
            instruction.Operand = null;
            return;
        case 6:
            instruction.OpCode = OpCodes.Ldc_I4_6;
            instruction.Operand = null;
            return;
        case 7:
            instruction.OpCode = OpCodes.Ldc_I4_7;
            instruction.Operand = null;
            return;
        case 8:
            instruction.OpCode = OpCodes.Ldc_I4_8;
            instruction.Operand = null;
            return;
    }

    if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
    {
        instruction.OpCode = OpCodes.Ldc_I4_S;
        instruction.Operand = (sbyte)value;
        return;
    }

    instruction.OpCode = OpCodes.Ldc_I4;
    instruction.Operand = value;
}

static int UnknownCommand(string command)
{
    Console.Error.WriteLine($"Unknown command: {command}");
    PrintUsage();
    return 1;
}

static void PrintUsage()
{
    Console.Error.WriteLine("Usage:");
    Console.Error.WriteLine("  TranslatorTools           (launch GUI patcher)");
    Console.Error.WriteLine("  TranslatorTools scan <assembly-path>");
    Console.Error.WriteLine("  TranslatorTools extract <assembly-path> [output-json]");
    Console.Error.WriteLine("  TranslatorTools patch <assembly-path> <output-assembly>");
    Console.Error.WriteLine("  TranslatorTools patch <assembly-path> <translations.json> <output-assembly>");
    Console.Error.WriteLine("  TranslatorTools dump-type <assembly-path> <full-type-name>");
}

static IReadOnlyDictionary<string, string> LoadTranslationsFromFile(string mapPath)
{
    return LoadTranslationsJson(File.ReadAllText(Path.GetFullPath(mapPath), Encoding.UTF8), mapPath);
}

static IReadOnlyDictionary<string, string> LoadBundledTranslations()
{
    var assembly = Assembly.GetExecutingAssembly();
    var resourceName = assembly.GetManifestResourceNames()
        .FirstOrDefault(name => name.EndsWith("translations.json", StringComparison.OrdinalIgnoreCase));

    if (resourceName is null)
    {
        throw new InvalidOperationException("Bundled translations.json resource was not found.");
    }

    using var stream = assembly.GetManifestResourceStream(resourceName);
    if (stream is null)
    {
        throw new InvalidOperationException($"Bundled translation resource could not be opened: {resourceName}");
    }

    using var reader = new StreamReader(stream, Encoding.UTF8);
    return LoadTranslationsJson(reader.ReadToEnd(), resourceName);
}

static IReadOnlyDictionary<string, string> LoadTranslationsJson(string json, string sourceLabel)
{
    var items = JsonSerializer.Deserialize<List<TranslationItem>>(json);
    if (items is null)
    {
        throw new InvalidOperationException($"Translation file could not be parsed: {sourceLabel}");
    }

    return items
        .Where(x => !string.IsNullOrWhiteSpace(x.English))
        .GroupBy(x => x.Chinese, StringComparer.Ordinal)
        .ToDictionary(g => g.Key, g => g.Last().English, StringComparer.Ordinal);
}

static string GetSuggestedOutputPath(string assemblyPath)
{
    var fullPath = Path.GetFullPath(assemblyPath);
    var directory = Path.GetDirectoryName(fullPath) ?? AppContext.BaseDirectory;
    var fileName = Path.GetFileNameWithoutExtension(fullPath);
    return Path.Combine(directory, $"{fileName}_EN.exe");
}

static string? FindDefaultAssemblyPath()
{
    var candidates = new[]
    {
        AppContext.BaseDirectory,
        Environment.CurrentDirectory
    }
    .Distinct(StringComparer.OrdinalIgnoreCase);

    foreach (var directory in candidates)
    {
        var candidate = Path.Combine(directory, "MR0_MODBUS_2M1.exe");
        if (File.Exists(candidate))
        {
            return candidate;
        }
    }

    return null;
}

internal sealed class PatchToolForm : Form
{
    private readonly Func<string?> _findDefaultAssemblyPath;
    private readonly Func<string, string> _getSuggestedOutputPath;
    private readonly Func<string, string, PatchResult> _patchAssembly;
    private readonly TextBox _inputPathTextBox;
    private readonly TextBox _outputPathTextBox;
    private readonly Label _statusLabel;

    public PatchToolForm(
        Func<string?> findDefaultAssemblyPath,
        Func<string, string> getSuggestedOutputPath,
        Func<string, string, PatchResult> patchAssembly)
    {
        _findDefaultAssemblyPath = findDefaultAssemblyPath;
        _getSuggestedOutputPath = getSuggestedOutputPath;
        _patchAssembly = patchAssembly;

        Text = "MR0-485-2M1 English Patcher";
        ClientSize = new Size(720, 272);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        var introLabel = new Label
        {
            AutoSize = false,
            Location = new Point(16, 14),
            Size = new Size(688, 36),
            Text = "Pick the original MR0_MODBUS_2M1.exe. The patcher will generate a new English EXE without modifying the original file."
        };

        var inputLabel = new Label
        {
            AutoSize = true,
            Location = new Point(16, 72),
            Text = "Original EXE:"
        };

        _inputPathTextBox = new TextBox
        {
            Location = new Point(16, 96),
            Size = new Size(586, 23)
        };
        _inputPathTextBox.TextChanged += (_, _) => UpdateSuggestedOutputPath();

        var browseInputButton = new Button
        {
            Location = new Point(611, 95),
            Size = new Size(93, 32),
            Text = "Browse..."
        };
        browseInputButton.Click += (_, _) => BrowseForInput();

        var outputLabel = new Label
        {
            AutoSize = true,
            Location = new Point(16, 132),
            Text = "Patched EXE:"
        };

        _outputPathTextBox = new TextBox
        {
            Location = new Point(16, 156),
            Size = new Size(586, 23)
        };

        var browseOutputButton = new Button
        {
            Location = new Point(611, 155),
            Size = new Size(93, 32),
            Text = "Save as..."
        };
        browseOutputButton.Click += (_, _) => BrowseForOutput();

        var patchButton = new Button
        {
            Location = new Point(533, 225),
            Size = new Size(171, 36),
            Text = "Create English EXE"
        };
        patchButton.Click += (_, _) => RunPatch();

        _statusLabel = new Label
        {
            AutoSize = false,
            Location = new Point(16, 226),
            Size = new Size(500, 32),
            Text = "Looking for MR0_MODBUS_2M1.exe next to the patcher..."
        };

        Controls.Add(introLabel);
        Controls.Add(inputLabel);
        Controls.Add(_inputPathTextBox);
        Controls.Add(browseInputButton);
        Controls.Add(outputLabel);
        Controls.Add(_outputPathTextBox);
        Controls.Add(browseOutputButton);
        Controls.Add(_statusLabel);
        Controls.Add(patchButton);

        Load += (_, _) => InitializePaths();
    }

    private void InitializePaths()
    {
        var defaultPath = _findDefaultAssemblyPath();
        if (defaultPath is null)
        {
            _statusLabel.Text = "Original EXE not found automatically. Browse to MR0_MODBUS_2M1.exe.";
            return;
        }

        _inputPathTextBox.Text = defaultPath;
        _statusLabel.Text = "Found MR0_MODBUS_2M1.exe next to the patcher.";
    }

    private void BrowseForInput()
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "Executable (*.exe)|*.exe|All files (*.*)|*.*",
            Title = "Select MR0_MODBUS_2M1.exe",
            FileName = Path.GetFileName(_inputPathTextBox.Text)
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            _inputPathTextBox.Text = dialog.FileName;
        }
    }

    private void BrowseForOutput()
    {
        using var dialog = new SaveFileDialog
        {
            Filter = "Executable (*.exe)|*.exe|All files (*.*)|*.*",
            Title = "Save patched EXE as",
            FileName = Path.GetFileName(string.IsNullOrWhiteSpace(_outputPathTextBox.Text)
                ? _getSuggestedOutputPath(_inputPathTextBox.Text)
                : _outputPathTextBox.Text)
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            _outputPathTextBox.Text = dialog.FileName;
        }
    }

    private void UpdateSuggestedOutputPath()
    {
        if (string.IsNullOrWhiteSpace(_inputPathTextBox.Text))
        {
            return;
        }

        var suggestedPath = _getSuggestedOutputPath(_inputPathTextBox.Text);
        if (string.IsNullOrWhiteSpace(_outputPathTextBox.Text) ||
            _outputPathTextBox.Text.EndsWith("_EN.exe", StringComparison.OrdinalIgnoreCase))
        {
            _outputPathTextBox.Text = suggestedPath;
        }
    }

    private void RunPatch()
    {
        try
        {
            var inputPath = Path.GetFullPath(_inputPathTextBox.Text.Trim());
            var outputPath = Path.GetFullPath(_outputPathTextBox.Text.Trim());

            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException("Original EXE not found.", inputPath);
            }

            if (string.Equals(inputPath, outputPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Output path must be different from the original EXE.");
            }

            var outputDirectory = Path.GetDirectoryName(outputPath);
            if (string.IsNullOrWhiteSpace(outputDirectory))
            {
                throw new InvalidOperationException("Output path is invalid.");
            }

            Directory.CreateDirectory(outputDirectory);

            _statusLabel.Text = "Patching EXE...";
            UseWaitCursor = true;
            var result = _patchAssembly(inputPath, outputPath);
            _statusLabel.Text = $"Done. Patched {result.PatchedStrings} strings and {result.PatchedLayoutValues} layout values.";

            MessageBox.Show(
                this,
                $"Patched {result.PatchedStrings} string loads and {result.PatchedLayoutValues} layout values.\n\nCreated:\n{result.OutputPath}",
                "MR0-485-2M1 Patcher",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            _statusLabel.Text = "Patch failed.";
            MessageBox.Show(
                this,
                ex.Message,
                "MR0-485-2M1 Patcher",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            UseWaitCursor = false;
        }
    }
}

internal sealed record TranslationItem(string Chinese, string English);
internal sealed record PatchResult(int PatchedStrings, int PatchedLayoutValues, string OutputPath);
