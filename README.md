# MR0-485-2M1 Configuration Tool English Patch

English patch for the **MR0-485-2M1** RS485 hub/repeater configuration tool (`MR0_MODBUS_2M1.exe`, also referred to as `RAEC_RS485_Tool` in the product manual).

Patches the official Chinese-only EXE to display English, producing `MR0_MODBUS_2M1_EN.exe`, a drop-in replacement with the same functionality.

This repo does **not** redistribute the vendor's original `MR0_MODBUS_2M1.exe`. Instead, it provides patchers that transform the original Chinese-only tool into an English version after you download it yourself.

## Before / After

Original older patch result:

![Original English patch result](translated.png)

Improved patch result with layout and font fixes:

![Improved English patch output](fancytranslation.png)

The improved patcher now edits both:

- `.NET` string literals
- `WinForms` layout and font settings

---

## Device: MR0-485-2M1 RS485 Hub / Repeater

**2 x RS485 Masters -> 1 x RS485 Slave**

The MR0-485-2M1 is a compact DIN-rail RS485 hub that lets two independent RS485 master devices share a single slave device on the same bus. It buffers and arbitrates commands from both masters with a configurable per-port timeout, so neither master needs to know about the other.

A companion model, the **MR0-485-1T2**, does the reverse: one master to two isolated slave segments.

**Manufacturer:** Dongguan Aimoxun Automation Technology Co., Ltd.  
**Product page:** [https://www.amxmotion.com/product/mr0-485-2m1/](https://www.amxmotion.com/product/mr0-485-2m1/)  
**Series overview:** [https://www.amxmotion.com/protocol-conversion-gateway/](https://www.amxmotion.com/protocol-conversion-gateway/)

### Key Specifications

| Parameter | MR0-485-2M1 |
|---|---|
| Topology | 2 masters -> 1 slave |
| Interfaces | 3 x RS485 (screw terminals) |
| Baud rate | 1200-256000 bps (default 9600) |
| Default format | 8N1 (configurable) |
| Max command buffer | 20 instructions |
| Master port timeout | Configurable per port (default 500 ms) |
| Transmission distance | Up to 1200 m at 9600 bps |
| Power supply | DC 24 V |
| Power consumption | < 0.2 W |
| Operating temp | -10 C to +50 C |
| Dimensions | 82 x 54 x 32 mm |
| Mounting | DIN rail |

### Terminal Pinout

| Terminal | Description |
|---|---|
| `24V` | DC 24V positive |
| `0V` / `GND` | DC 24V negative / 485 ground |
| `A+` / `B-` | **Slave** port |
| `A0+` / `B0-` | **Master 1** port |
| `A1+` / `B1-` | **Master 2** port |

### How to Enter Configuration Mode

1. Connect a **USB-to-RS485 adapter** to the **slave port** (`A+`/`B-`) and to your PC.
2. Apply DC 24V power.
3. Within 60 seconds of power-on, press the **SET** button. `SYS` will fast-blink, indicating config mode is active.
4. Open the configuration tool and connect on the matching COM port at the current baud rate.

### Factory Reset

Hold the **SET** button for 3 seconds within 60 seconds of power-on, then release. After `SYS` resumes slow blinking, power-cycle the module. All ports reset to: 9600 bps, 8 data bits, 1 stop bit, no parity, 500 ms timeout.

---

## Finding the Configuration Tool

> **If you arrived here searching for `RAEC_RS485_Tool` - you're in the right place.**

The product manual refers to the configuration software as **`RAEC_RS485_Tool`**, but the actual file distributed by the manufacturer is named **`MR0_MODBUS_2M1.exe`**. The two names refer to the same program.

The tool is only linked from the protocol gateway series page, not from the individual product page, which makes it easy to miss:

- **Series page**: [https://www.amxmotion.com/protocol-conversion-gateway/](https://www.amxmotion.com/protocol-conversion-gateway/)
- **Direct download** (`.rar` archive containing `MR0_MODBUS_2M1.exe`): [https://oss.amsamotion.com/uploads/MR0_MODBUS_2M1.rar](https://oss.amsamotion.com/uploads/MR0_MODBUS_2M1.rar)

The tool is Chinese-only out of the box. This repo provides multiple ways to patch it to English.

---

## Patch Options

This repo supports three trust / comfort levels:

| Path | Audience | Requires | Notes |
|---|---|---|---|
| Prebuilt patcher EXE | Easiest path | Nothing beyond Windows | Recommended for most users. Intended for GitHub Releases. |
| `TranslatorTools` source | Auditable advanced patcher | .NET 8 SDK | Recommended source path. Patches text, layout, and fonts. |
| `patch_exe.py` | Maximum simplicity / legacy fallback | Python 3 | String-only patch. Does not fix layout or font issues. |

### Files

| File | Description |
|---|---|
| `MR0_MODBUS_2M1.exe` | Original vendor tool (`RAEC_RS485_Tool`) - **not included** |
| `patch_exe.py` | Legacy minimal Python patcher |
| `TranslatorTools/` | Advanced Mono.Cecil-based patcher source |
| `scripts/publish-patcher.ps1` | Builds a standalone patcher EXE |
| `TRANSLATION_NOTES.md` | Translation reference |
| `MR0-485.pdf` | Original product manual |

---

## Recommended Patch Path

The recommended patch path is the advanced `.NET` patcher because it can do more than in-place byte replacement:

- replace strings without the original same-length limitation
- widen cramped sections of the UI
- move controls to make English text fit
- switch Chinese UI fonts to a normal Windows UI font

### Build a Standalone Patcher EXE

From the repo root:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\publish-patcher.ps1
```

This publishes a self-contained single-file Windows patcher under:

```text
dist\MR0-485-2M1-patcher-win-x64\MR0-485-2M1-patcher.exe
```

### Run the Advanced Patcher From Source

```powershell
& 'C:\Program Files\dotnet\dotnet.exe' run --project .\TranslatorTools\TranslatorTools.csproj -- patch .\MR0_MODBUS_2M1.exe .\TranslatorTools\translations.json .\MR0_MODBUS_2M1_EN.exe
```

---

## Legacy Python Patch Path

Place `patch_exe.py` in the same directory as `MR0_MODBUS_2M1.exe`, then run:

```powershell
python .\patch_exe.py
```

This produces `MR0_MODBUS_2M1_EN.exe` in the same directory.

### How the Python Script Works

`patch_exe.py` is the original simple patcher. It locates Chinese UTF-16LE strings in the compiled binary and overwrites them in place.

**Constraint:** each string slot has a fixed byte length. The replacement must fit in the same space, so shorter translations are padded and longer ones are truncated.

This path is still useful if you want the most accessible and easy-to-read patcher, but it does **not** fix:

- cramped buttons
- group box widths
- font issues
- more polished English layout

---

## What Changes

Examples from the improved patcher:

| Original (Chinese) | Patched (English) | Full meaning |
|---|---|---|
| `选择连接参数` | `Select connection parameters` | Select connection parameters |
| `串口号：` | `COM port:` | COM port number |
| `打开串口` | `Open COM` | Open serial port |
| `关闭串口` | `Close COM` | Close serial port |
| `连接成功` | `Connected` | Connection successful |
| `主站口1通信参数配置` | `Master port 1` | Master port 1 comm config |
| `主站口2通信参数配置` | `Master port 2` | Master port 2 comm config |
| `读取参数` | `Read params` | Read parameters |
| `一键配置所有参数` | `Apply all` | One-click configure all |

See [TRANSLATION_NOTES.md](TRANSLATION_NOTES.md) for the broader translation reference.

---

## Workflow (Quick Start)

1. Download `MR0_MODBUS_2M1.rar` from the vendor link above.
2. Extract `MR0_MODBUS_2M1.exe`.
3. Choose one patch path above.
4. Run the patcher to generate `MR0_MODBUS_2M1_EN.exe`.
5. Connect a USB-to-RS485 adapter to the **slave** port.
6. Power the module with DC 24V.
7. Press **SET** within 60 seconds to enter config mode.
8. Open `MR0_MODBUS_2M1_EN.exe`.
9. Read current parameters, adjust settings as needed, then apply them.
10. Power-cycle the module for changes to take effect.

---

## Legal / Distribution Posture

This repo intentionally distributes patchers rather than the vendor EXE itself.

- Original vendor executable: not included
- Patchers and supporting source: included
- Patched output EXE: generated locally by the user

---

## License

The patchers and supporting source in this repo are released under the MIT License.

The original `MR0_MODBUS_2M1.exe` remains the property of its original vendor and is not redistributed here.
