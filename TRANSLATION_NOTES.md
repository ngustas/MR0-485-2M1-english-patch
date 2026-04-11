# MR0_MODBUS_2M1.exe — English Patch Notes

## What Was Done

`MR0_MODBUS_2M1_EN.exe` is a patched copy of `MR0_MODBUS_2M1.exe` with all Chinese UI strings replaced with English. The original file is unchanged.

**Method:** The executable is a .NET assembly. Chinese strings are stored in the `#US` (User Strings) heap inside the .NET metadata section. Python was used to locate each Chinese UTF-16LE string in-place and overwrite it with the English equivalent.

**Key constraint:** Each string slot has a fixed byte length baked into the binary. Since both Chinese and ASCII characters occupy 2 bytes each in UTF-16LE, the English translation must fit in exactly the same number of characters as the original Chinese text. Shorter translations are padded with trailing spaces; longer ones are truncated. The "Displayed in EXE" column below shows the actual result.

---

## Files

| File | Description |
|---|---|
| `MR0_MODBUS_2M1.exe` | Original — unchanged |
| `MR0_MODBUS_2M1_EN.exe` | Patched English version |
| `patch_exe.py` | Python script that performed the patch |

---

## Translation Reference

> **Columns:**
> - **Chinese** — original text in the binary
> - **Full English Meaning** — complete translation (for reference)
> - **Displayed in EXE** — what actually appears (may be truncated to fit the slot)

### Main Window — Left Panel (COM Port Setup)

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 选择连接参数 | Select connection parameters | `Serial` |
| 串口号： | COM port number: | `Port` |
| 波特率： | Baud rate: | `Baud` |
| 校验位： | Parity bits: | `Pari` |
| 数据位： | Data bits: | `Data` |
| 停止位： | Stop bits: | `Stop` |
| 打开串口 | Open COM port | `Open` |
| 关闭串口 | Close COM port | `Shut` |
| 搜索串口 | Search/scan COM port | `Scan` |
| 设备连接状态 | Device connection status | `Status` |
| 固件版本： | Firmware version: | `FW   ` |
| 读取参数 | Read parameters | `Read` |
| 一键配置所有参数 | One-click configure all parameters | `QuickCfg` |

### Main Window — Right Panel (Parameter Configuration)

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 参数配置 | Parameter configuration (tab/section) | `Prms` |
| 从站口参数配置 | Slave port parameter configuration | `SlvPort` |
| 主站口1通信参数配置 | Master port 1 communication parameter config | `Mstr Port1` |
| 主站口2通信参数配置 | Master port 2 communication parameter config | `Mstr Port2` |
| 主站口1超时时间配置 | Master port 1 timeout configuration | `P1 T/O Cfg` |
| 主站口2超时时间配置 | Master port 2 timeout configuration | `P2 T/O Cfg` |
| 设置 | Set / Configure (button) | `OK` |

### Status & Error Messages

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 设备连接状态 | Device connection status | `Status` |
| 连接成功 | Connection successful | `Connected` |
| 连接断开 | Connection disconnected | `Disconn.` |
| 设备已连接 | Device connected | `Dev Connected` |
| 设备未连接 | Device not connected | `Dev Not Conn.` |
| 错误提示： | Error prompt: | `Error:` |
| 开启串口前，请配置参数... | Configure COM params before opening... | `Config COM Pa` *(truncated from 14 chars)* |
| 串口打开失败，请检查串口是否存在... | COM open failed, check if port exists... | `COM open failed, check po` *(truncated)* |
| 串口没有打开... | COM port not open... | `COM port not open...` |
| 参数错误... | Parameter error... | `Param err.` |
| 设置成功 | Set successfully | `Set OK` |
| 设置成功！ | Set successfully! | `Set success!` |
| 连接失败...！ | Connection failed...! | `Connect failed...!` |
| 连接没有建立...！ | Connection not established...! | `Connection not established!` |
| 参数错误...！ | Parameter error...! | `Param error...!` |
| 数据长度超出范围...！ | Data length out of range...! | `Data length out of range...!` |
| 写入成功 | Write successful | `Write OK` |
| 操作提示： | Operation prompt: | `Operation Note:` |
| 参数设置成功！ | Parameters set successfully! | `Params set OK!` |
| 参数设置成功，断电重新启动！ | Params set OK, power cycle required! | `Params set OK. Power cycle!` |
| 参数设置成功，断电重启动！ | Params OK, reboot required! | `Params OK. Reboot needed!` |
| 提示 | Prompt / Note | `Note` |
| 提示： | Note/Prompt: | `Note:` |
| 创建串口失败: | Failed to create COM port: | `Failed create COM:` |
| 串口通信出错提示: | COM communication error: | `COM comm error:` |

### Calibration — General

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 模块校准配置 | Module calibration configuration | `Module Cal Cfg` |
| 确认校准 | Confirm calibration | `Confirm Cal` |
| 单独校准 | Individual/single calibration | `Single  Cal` |
| 整体校准 | Overall calibration | `Global  Cal` |
| 整体确认校准 | Confirm overall calibration | `Confirm All Cal` |
| 启用单独校准 | Enable individual calibration | `Enable Single Cal` |
| 特别单独校准 | Special individual calibration | `Special Sngl Cal` |
| 启动校准 | Start calibration | `Start Cal.` |
| 关闭校准 | Stop calibration | `Stop  Cal.` |
| 强制校准 | Force calibration | `Force Cal.` |
| 退出校准 | Exit calibration | `Exit Cal.` |
| 校准系数 | Calibration coefficient | `Cal Factor` |
| 校准系数设置 | Calibration coefficient setting | `Cal Factor Set` |
| 写入校准系数 | Write calibration coefficient | `Write Cal Factor` |
| 强制0点校准 | Force zero-point calibration | `Force Zero Cal` |
| 强制量程标定 | Force range/scale calibration | `Force Scale Cal` |
| 零点校准 | Zero-point calibration | `Zero Calib` |
| 0点校准 | Zero-point calibration | `0pt Calib` |
| 满量程校准 | Full-scale calibration | `Full-Scl Calib` |
| 校准 | Calibration | `Cal` |

### Calibration — Analog Channels

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 模拟量校准 | Analog calibration | `Analog Calib` |
| 模拟量输入 | Analog input | `Analog Input` |
| 模拟量输出 | Analog output | `Analog Output` |
| 模拟输入 | Analog input (short) | `Anlg In` |
| 模拟输出 | Analog output (short) | `Anlg Out` |
| 模拟量参数 | Analog parameters | `Analog Params` |
| 0V校准 | 0V calibration | `0V Calib` |
| 0mA校准 | 0mA calibration | `0mA Calib` |
| 4mA校准 / 4ma校准 | 4mA calibration | `4mA Calib` |
| 10V校准 | 10V calibration | `10V Calib` |
| 20mA校准 / 20ma校准 | 20mA calibration | `20mA Calib` |
| 电流校准 | Current calibration | `Curr Calib` |
| 电压校准 | Voltage calibration | `Volt Calib` |
| 信号类别 | Signal type | `Sig Type` |
| 设置信号类别 | Set signal type | `Set Signal Type` |
| 设置4~20ma | Set 4~20mA | `Set 4~20mA` |
| 全部设电流 | Set all channels to current mode | `Set All Current` |
| 全部设电压 | Set all channels to voltage mode | `Set All Voltage` |
| 全部设4-20ma | Set all channels to 4–20mA | `Set All 4-20mA` |
| 设置全部电压 | Set all to voltage mode | `Set All Voltage` |
| 设置全电流 | Set all to current mode | `Set All Current` |
| 输出模式切换 | Switch output mode | `Switch Output Mode` *(truncated)* |
| 输出模式切换: | Switch output mode: | `Switch Output Mode` *(truncated)* |
| 0通道0点校准成功 | Ch0 zero-point calibration success | `Ch0 zero-cal OK` |
| 1通道0点校准成功 | Ch1 zero-point calibration success | `Ch1 zero-cal OK` |
| 2通道0点校准成功 | Ch2 zero-point calibration success | `Ch2 zero-cal OK` |
| 3通道0点校准成功 | Ch3 zero-point calibration success | `Ch3 zero-cal OK` |
| 4通道0点校准成功 | Ch4 zero-point calibration success | `Ch4 zero-cal OK` |
| 5通道0点校准成功 | Ch5 zero-point calibration success | `Ch5 zero-cal OK` |
| 6通道0点校准成功 | Ch6 zero-point calibration success | `Ch6 zero-cal OK` |
| 7通道0点校准成功 | Ch7 zero-point calibration success | `Ch7 zero-cal OK` |
| 0通道满量程校准成功 | Ch0 full-scale calibration success | `Ch0 full-scale cal OK` *(truncated)* |
| 1通道满量程校准成功 | Ch1 full-scale calibration success | `Ch1 full-scale cal OK` *(truncated)* |
| 2通道满量程校准成功 | Ch2 full-scale calibration success | `Ch2 full-scale cal OK` *(truncated)* |
| 3通道满量程校准成功 | Ch3 full-scale calibration success | `Ch3 full-scale cal OK` *(truncated)* |
| 4通道满量程校准成功 | Ch4 full-scale calibration success | `Ch4 full-scale cal OK` *(truncated)* |
| 5通道满量程校准成功 | Ch5 full-scale calibration success | `Ch5 full-scale cal OK` *(truncated)* |
| 6通道满量程校准成功 | Ch6 full-scale calibration success | `Ch6 full-scale cal OK` *(truncated)* |
| 7通道满量程校准成功 | Ch7 full-scale calibration success | `Ch7 full-scale cal OK` *(truncated)* |
| 模拟量输出校准成功 | Analog output calibration success | `Analog output cal OK` *(truncated)* |

### Calibration — Resistance & Coefficients

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 校准电阻类型 | Calibration resistance type | `Cal Resist Type` *(truncated)* |
| 校准电阻参数 | Calibration resistance parameter | `Cal Resist Para` *(truncated)* |
| 采样电阻1 | Sampling resistor 1 | `Sample Resist1` *(truncated)* |
| 采样电阻2 | Sampling resistor 2 | `Sample Resist2` *(truncated)* |
| 电阻1内码值 | Resistor 1 raw ADC code | `Resist1 Code` |
| 电阻2内码值 | Resistor 2 raw ADC code | `Resist2 Code` |
| 校准的电阻参数错误！ | Calibration resistance parameter error! | `Calib resist param error!` *(truncated)* |
| 欧 | Ohm (Ω) | `R` |
| 欧（单位） | Ohm (unit) | `Ohm (unit)` |
| 默认1000，若得到的值偏小，可设置较大的校准系数 | Default 1000; if value too small, increase cal coefficient | `Default 1000; if value too small, incre` *(truncated)* |

### Channels & Signals

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 通道编号 | Channel number | `Ch Num` |
| 通道号 | Channel number (short) | `Ch No.` |
| 通道0 | Channel 0 | `Ch0` |
| 通道1 | Channel 1 | `Ch1` |
| 通道2 | Channel 2 | `Ch2` |
| 通道3 | Channel 3 | `Ch3` |
| 通道4 | Channel 4 | `Ch4` |
| 通道5 | Channel 5 | `Ch5` |
| 通道6 | Channel 6 | `Ch6` |
| 通道7 | Channel 7 | `Ch7` |
| 通道8 | Channel 8 | `Ch8` |
| 通道9 | Channel 9 | `Ch9` |
| 通道10 | Channel 10 | `Chan 10` |
| 通道11 | Channel 11 | `Chan 11` |
| 通道12 | Channel 12 | `Chan 12` |
| 通道13 | Channel 13 | `Chan 13` |
| 通道14 | Channel 14 | `Chan 14` |
| 通道15 | Channel 15 | `Chan 15` |
| 通道0： | Channel 0: | `Chan 0:` |
| 通道1： | Channel 1: | `Chan 1:` |
| 通道2： | Channel 2: | `Chan 2:` |
| 通道3： | Channel 3: | `Chan 3:` |
| 通道4： | Channel 4: | `Chan 4:` |
| 通道5： | Channel 5: | `Chan 5:` |
| 通道数 | Channel count | `Chan Count` *(label context)* |
| 通道数：  | Channel count: | `Channels:` |
| 通道数范围:1~8! | Channel count range: 1~8! | `Channel range: 1~8!` |
| 通道数范围:1~4! | Channel count range: 1~4! | `Channel range: 1~4!` |
| 内码 | Raw/internal ADC code | `Code` |
| 电压/电流值 | Voltage/current value | `Voltage/Current` *(truncated)* |

### Module & Device Parameters

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 模块参数配置 | Module parameter configuration | `Module Params Cfg` *(truncated)* |
| 模块型号: | Module model: | `Module Model:` |
| 模块地址： | Module address: | `Module Addr:` |
| 模块端口号： | Module port number: | `Module Port:` |
| 模块IP地址： | Module IP address: | `Module IP Addr:` |
| 设备地址： | Device address: | `Dev Addr:` |
| 其他参数配置 | Other parameter configuration | `Other Params Cfg` *(truncated)* |
| 通信参数配置 | Communication parameter config | `Comm Params Cfg` |
| 通信参数 | Communication parameters | `Comm Params` |
| 通讯参数 | Communication parameters (alt.) | `Comm Params` |
| 配置参数 | Configure parameters | `Cfg Params` |
| 写入配置 | Write configuration | `Write Cfg` |
| 读出配置 | Read configuration | `Read  Cfg` |
| 模式配置 | Mode configuration | `Mode  Cfg` |

### Network / Ethernet

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 配置以太网参数 | Configure Ethernet parameters | `Config Ethernet Params` *(truncated)* |
| IP地址： | IP address: | `IP Addr:` |
| 子网掩码： | Subnet mask: | `Subnet Mask:` |
| 网关： | Gateway: | `Gateway:` |
| 设置IP | Set IP address | `Set IP` |
| 设置子网掩码 | Set subnet mask | `Set Subnet Mask` *(truncated)* |
| 设置网关 | Set gateway | `Set Gateway` *(truncated)* |
| 设置设备地址 | Set device address | `Set Device Address` *(truncated)* |
| TCP连接配置 | TCP connection configuration | `TCP Conn Config` *(truncated)* |
| 断开连接 | Disconnect | `Disconnect` *(truncated)* |
| 启动MODBUS TCP | Start MODBUS TCP | `Start MODBUS TCP` *(truncated)* |
| 自动连接 | Auto connect | `Auto Connect` *(truncated)* |
| IP或者端口号错误... | IP or port number error... | `IP or port error...` *(truncated)* |
| 第1路连接接收： | Port 1 connection receive: | `Port 1 Connect Re` *(truncated)* |

### Bus / RS485 Parameters

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 总线地址： | Bus address: | `Bus Addr:` |
| 判定总线错误时间阀值： | Bus error determination time threshold: | `Bus error time threshold:` *(truncated)* |
| 总线错误保持 | Bus error hold | `Bus Err Hold` |
| 总线错误复位 | Bus error reset | `Bus Err Reset` |
| 设置总线错误参数 | Set bus error parameters | `Set Bus Error Params` *(truncated)* |
| 设置总线错参数 | Set bus error parameters (short) | `Set Bus Error Params` |
| 设置波特率系数 | Set baud rate coefficient | `Set Baud Rate Coeff` *(truncated)* |
| RS232波特率： | RS232 baud rate: | `RS232 Baud Rate:` *(truncated)* |
| 写入总线地址成功,请断电重启 | Bus address write OK, power cycle | `Bus addr write OK, power cycle` *(truncated)* |
| 一键写入配置 | One-click write configuration | `One-Click Write Cfg` *(truncated)* |
| 一键读出配置 | One-click read configuration | `One-Click Read Cfg` *(truncated)* |
| 扫描时间： | Scan time: | `Scan Time:` |

### Register Operations

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 寄存器操作 | Register operations | `Register Ops` *(truncated)* |
| 保持寄存器器 | Holding register | `Hold Register` |
| 保持寄存器：  | Holding register: | `Hold Register:` *(truncated)* |
| 保持寄存器个数： | Holding register count: | `Hold Register Count:` *(truncated)* |
| 输入寄存器： | Input register: | `Input Register:` *(truncated)* |
| 只读寄存器个数： | Read-only register count: | `ReadOnly Reg Count:` *(truncated)* |
| 输入线圈： | Input coil: | `Input Coil:` |
| 输出线圈： | Output coil: | `Output Coil:` |
| 数字输入个数： | Digital input count: | `Digital Input Count:` *(truncated)* |
| 数字输出个数： | Digital output count: | `Digital Output Count` *(truncated)* |
| 开关量输入 | Digital/discrete input | `Digital Input` *(truncated)* |
| 开关量输出 | Digital/discrete output | `Digital Output` *(truncated)* |
| 起始地址： | Start address: | `Start Addr:` |
| 数据长度： | Data length: | `Data Len:` |
| 地址 | Address | `Addr` |
| 类型 | Type | `Type` |
| 写入地址 | Write address | `Write Addr` |
| 多个写入 | Multiple write | `Multi Write` |
| 写入 | Write | `Write` |
| 读取 | Read | `Read` |

### Sensor / Temperature

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 传感器类型选择 | Sensor type selection | `Sensor Type Select` *(truncated)* |
| 传感器状态 | Sensor status | `Sensor Status` *(truncated)* |
| 温度值 | Temperature value | `Temp` |
| 温度修正值 | Temperature correction value | `Temp Offset` *(truncated)* |
| 冷端温度 | Cold junction temperature | `Cold Junc Temp` *(truncated)* |
| 冷端温度修正 | Cold junction temperature correction | `Cold Junc Offset` *(truncated)* |
| 热电偶类型正值 | Thermocouple type positive value | `TC type pos value` *(truncated)* |
| NTC采样深度： | NTC sample depth: | `NTC Sample Depth:` *(truncated)* |
| 环境温度： | Ambient temperature: | `Ambient Temp:` *(truncated)* |
| 请选择热电偶类型 | Please select thermocouple type | `Select thermocouple type` *(truncated)* |
| 请输入正确温度值 | Please enter correct temperature | `Enter correct temp value` *(truncated)* |
| 查询 | Query / Inquire | `Query` |
| 查询参数 | Query parameters | `Query Params` *(truncated)* |

### Weighing / Scale Functions

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 称重信息配置 | Weighing/scale info configuration | `Scale Info Config` *(truncated)* |
| 称重通道： | Weighing channel: | `Weigh Chan:` |
| 采样速率： | Sample rate: | `Sample Rate:` *(truncated)* |
| 砝码重量 | Calibration weight | `Weight` |
| 砝码内码 | Calibration weight raw code | `Weight Code` *(truncated)* |
| 实时重量 | Real-time weight | `Live Weight` *(truncated)* |
| 实时内码 | Real-time raw ADC code | `Live Code` |
| 零点内码 | Zero-point raw code | `Zero Code` |
| 皮重内码 | Tare weight raw code | `Tare Code` |
| 校正内码 | Correction raw code | `Correct Code` *(truncated)* |
| 标定 | Calibrate / Mark | `Calibrate` *(truncated)* |
| 去皮 | Tare | `Tare` |
| 取消去皮 | Cancel tare | `Cancel Tare` *(truncated)* |
| 零点校准 | Zero-point calibration | `Zero Calib` *(truncated)* |
| 默认单位：克 | Default unit: grams | `Default unit: g` |
| 小数点 | Decimal point | `Dec Pt.` |
| 设置满量程重量 | Set full-scale weight | `Set Full Scale Weight` *(truncated)* |
| 设置满量程内码值 | Set full-scale raw code value | `Set Full Scale Code` *(truncated)* |
| 强制标定量程 | Force range calibration | `Force Scale Calib` *(truncated)* |
| 设置0点校准值 | Set zero-point calibration value | `Set Zero Cal Value` *(truncated)* |
| 保存参数 | Save parameters | `Save Params` *(truncated)* |
| 请输入砝码重量... | Please enter calibration weight... | `Enter weight...` |
| 请输入正确的内码... | Please enter valid raw code... | `Enter valid code...` |

### I/O Point Configuration

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 设置IO点个数 | Set I/O point count | `Set IO Point Cnt` *(truncated)* |
| 设置IO点 | Set I/O points | `Set IO Pts` |
| Q点个数 | Q-point count (outputs) | `Q Points` |
| I点个数 | I-point count (inputs) | `I Points` |
| 16路 | 16-channel | `16-Ch` |
| 全部关闭 | Disable all | `Disable All` *(truncated)* |
| 全部开启 | Enable all | `Enable  All` *(truncated)* |

### Sampling & Filtering

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 采样深度： | Sample depth: | `SampDepth:` |
| 采样深度范围:10~99！ | Sample depth range: 10~99! | `Sample depth range: 10~99!` *(truncated)* |
| 采样深度范围:15~99！ | Sample depth range: 15~99! | `Sample depth range: 15~99!` *(truncated)* |
| 采样深度1~20！ | Sample depth 1~20! | `Sample depth 1~20!` |
| 采样深度只能是1~10！ | Sample depth must be 1~10! | `Sample depth must be 1~10!` *(truncated)* |
| 通道数只能是1 | Channel count must be 1 | `Channel count must be 1` *(truncated)* |
| 设置采样深度 | Set sample depth | `Set Sample Depth` *(truncated)* |
| 滤波值： | Filter value: | `Filter:` |
| 滤波值范围:需要大于400！ | Filter value range: must be >400! | `Filter range: must be >400!` *(truncated)* |
| 设置分辨率 | Set resolution | `Set Resolution` *(truncated)* |
| 设置通道数 | Set channel count | `Set Chan Count` *(truncated)* |

### Factory Reset & Misc Operations

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 恢复出厂设置 | Restore factory settings | `Factory Reset` |
| 确定要回复出厂设置吗？ | Confirm restore factory settings? | `Restore factory defaults?` *(truncated)* |
| 参数恢复出厂成功，断电重新启动！ | Factory reset success, power cycle required! | `Factory reset OK. Power cycle!` *(truncated)* |
| 设置参数成功，断电重新启动！ | Params set OK, power cycle required! | `Params set OK. Power cycle!` *(truncated)* |
| 启动校验按钮 | Start checksum/verify button | `Start Check Btn` *(truncated)* |
| 请输入参数... | Please enter parameter... | `Enter parameter...` |
| 输入的参数错误... | Input parameter error... | `Invalid parameter...` |
| 正常 | Normal / OK | `OK` |
| 断线 | Disconnected / line broken | `Disconn` |
| 正  常 | Normal (spaced display) | `OK` |
| 已经保存！ | Already saved! | `Already saved!` |
| 成功提示： | Success prompt: | `Success:` |
| 保存模拟量配置参数 | Save analog configuration parameters | `Save Analog Config P` *(truncated)* |
| 指令发送成功. | Command sent successfully. | `Cmd sent OK.` |
| 请正确填入地址 | Please enter valid address | `Enter valid address` *(truncated)* |
| 请选择正确类型 | Please select valid type | `Select valid type` *(truncated)* |

### About / Title Bar

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| MR0系列MODBUS工具 | MR0 Series MODBUS Tool | `MR0 Series MODBUS Tool` *(truncated)* |
| MODBUS工具 | MODBUS Tool | `MODBUS Tool` |
| MODBUS-2AI配置 | MODBUS-2AI Configuration | `MODBUS-2AI Config` *(truncated)* |
| 东莞市艾莫迅自动化科技有限公司v1.5.8 | Dongguan Aimaoxun Automation Technology Co., Ltd. v1.5.8 | `Dongguan Aimaoxun Automation v1.5.8` *(truncated)* |
| 版本： | Version: | `Ver:` |
| 帮    助 | Help | `Help` |

### Supported Device List (Info Text)

| Chinese | Full English Meaning | Displayed in EXE |
|---|---|---|
| 支持EMB-AQ8 | Supports EMB-AQ8 | `Supports EMB-AQ8` |
| 支持EMB-AM8 | Supports EMB-AM8 | `Supports EMB-AM8` |
| 支持EMB-AE8 | Supports EMB-AE8 | `Supports EMB-AE8` |
| 支持EMB-AR8G | Supports EMB-AR8G | `Supports EMB-AR8G` |
| 支持MR2A-AR8G | Supports MR2A-AR8G | `Supports MR2A-AR8G` |
| 支持EMA-AW4G | Supports EMA-AW4G | `Supports EMA-AW4G` |
| 支持MR2A-AW4G | Supports MR2A-AW4G | `Supports MR2A-AW4G` |
| 支持JY-MODBUS-4AI4AO | Supports JY-MODBUS-4AI4AO | `Supports JY-MODBUS-4AI4AO` |
| 支持MODBUS-8TC / 支持MODBUS-2PT4NTC | Supports MODBUS-8TC / Supports MODBUS-2PT4NTC | `Supports MODBUS-8TC / Supports MODBUS-2PT4NTC` |
| 支持MODBUS-4AI4AO | Supports MODBUS-4AI4AO | `Supports MODBUS-4AI4AO` |
| 支持MODBUS-8AO / 支持JY-MODBUS-8AO | Supports MODBUS-8AO / Supports JY-MODBUS-8AO | `Supports MODBUS-8AO / Supports JY-MODBUS-8AO` |
| 支持JY-MODBUS-8AI | Supports JY-MODBUS-8AI | `Supports JY-MODBUS-8AI` |
| 支持MODBUS-2AI | Supports MODBUS-2AI | `Supports MODBUS-2AI` |
| 支持MODBUS-8PT | Supports MODBUS-8PT | `Supports MODBUS-8PT` |

---

## Notes on Truncation

Strings where the English meaning is notably longer than what fits in the slot:

| Slot size | Chinese | Intended meaning | What fits |
|---|---|---|---|
| 2 chars | 设置 | Set / Configure | `OK` |
| 4 chars | 打开串口 | Open COM port | `Open` |
| 4 chars | 关闭串口 | Close COM port | `Shut` |
| 4 chars | 搜索串口 | Search COM port | `Scan` |
| 4 chars | 读取参数 | Read parameters | `Read` |
| 4 chars | 数据位： | Data bits: | `Data` |
| 4 chars | 串口号： | COM port number: | `Port` |
| 4 chars | 停止位： | Stop bits: | `Stop` |
| 4 chars | 校验位： | Parity bits: | `Pari` |
| 4 chars | 波特率： | Baud rate: | `Baud` |
| 5 chars | 固件版本： | Firmware version: | `FW   ` |
| 6 chars | 选择连接参数 | Select connection parameters | `Serial` |
| 6 chars | 设备连接状态 | Device connection status | `Status` |

---

*Patch script: `patch_exe.py` — modifies strings in-place in the .NET #US heap*
