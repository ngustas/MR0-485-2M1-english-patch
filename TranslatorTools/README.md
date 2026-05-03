# TranslatorTools

Advanced source-based patcher for `MR0_MODBUS_2M1.exe`.

This tool uses `Mono.Cecil` to patch the compiled .NET assembly directly. Unlike the legacy Python byte-replacement script, it can modify:

- string literals
- WinForms control sizes
- WinForms control positions
- WinForms font names

That makes it the preferred path for the cleanest English output.

## Requirements

- Windows
- .NET 8 SDK

## Run From Source

```powershell
& 'C:\Program Files\dotnet\dotnet.exe' run --project .\TranslatorTools.csproj -- patch ..\MR0_MODBUS_2M1.exe .\translations.json ..\MR0_MODBUS_2M1_EN.exe
```

## Build A Standalone EXE

From the repo root:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\publish-patcher.ps1
```

That produces a self-contained single-file Windows patcher under `dist\`.
