# Release Title

`MR0-485-2M1 Patcher v1.0.0`

Suggested Git tag: `v1.0.0`

## Release Notes

First standalone release of the MR0-485-2M1 English patcher.

This release adds a small Windows GUI patcher EXE for `MR0_MODBUS_2M1.exe` / `RAEC_RS485_Tool`.

It is framework-dependent, so end users need the **.NET 8 Desktop Runtime x64** installed. The full SDK is not required.

### What's included

- `MR0-485-2M1-patcher.exe`
- `patcher-usage.txt`

### What this patcher does

- patches `.NET` string literals in the original EXE
- adjusts `WinForms` layout so English labels fit properly
- switches Chinese UI fonts to a normal Windows UI font
- writes a patched output EXE without modifying the original input file
- launches as a simple double-click GUI patcher for normal users

### Result

The patcher generates:

`MR0_MODBUS_2M1_EN.exe`

### Important notes

- The original vendor EXE is not included in this release.
- Download `MR0_MODBUS_2M1.exe` separately from the vendor/distributor link in the repository README.
- This release is intended for Windows.
- Requires **.NET 8 Desktop Runtime x64**.

### Trust / usage options in the repo

- Prebuilt patcher EXE: easiest path
- `TranslatorTools` source: auditable advanced patcher
- `patch_exe.py`: simple legacy fallback

### Suggested release asset names

- `MR0-485-2M1-patcher.exe`
- `patcher-usage.txt`
