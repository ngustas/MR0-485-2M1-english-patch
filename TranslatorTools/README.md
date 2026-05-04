# TranslatorTools

Advanced source-based patcher for `MR0_MODBUS_2M1.exe`.

This tool uses `Mono.Cecil` to patch the compiled .NET assembly directly. Unlike the legacy Python byte-replacement script, it can modify:

- string literals
- WinForms control sizes
- WinForms control positions
- WinForms font names

That makes it the preferred path for the cleanest English output.

When published as `MR0-485-2M1-patcher.exe`, it can also run as a simple Windows GUI patcher:

- double-click the EXE
- let it auto-detect `MR0_MODBUS_2M1.exe` if present next to the patcher
- generate `MR0_MODBUS_2M1_EN.exe`

## Requirements

- Windows
- .NET 8 SDK

## Run From Source

Double-click the built EXE to launch the GUI patcher, or use the CLI commands below.

```powershell
& 'C:\Program Files\dotnet\dotnet.exe' run --project .\TranslatorTools.csproj -- patch ..\MR0_MODBUS_2M1.exe ..\MR0_MODBUS_2M1_EN.exe
```

Optional explicit translation map:

```powershell
& 'C:\Program Files\dotnet\dotnet.exe' run --project .\TranslatorTools.csproj -- patch ..\MR0_MODBUS_2M1.exe .\translations.json ..\MR0_MODBUS_2M1_EN.exe
```

## Build A Patcher EXE

From the repo root:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\publish-patcher.ps1
```

That produces a small framework-dependent single-file Windows patcher under `dist\`.

End users need the **.NET 8 Desktop Runtime x64** installed. They do not need the SDK.
