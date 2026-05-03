#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Legacy minimal patcher for MR0_MODBUS_2M1.exe.
This path only replaces string bytes in-place and does not adjust WinForms layout or fonts.
"""
import sys, shutil

INPUT = "MR0_MODBUS_2M1.exe"
OUTPUT = "MR0_MODBUS_2M1_EN.exe"

# Translation map: Chinese -> English
# English will be padded with spaces or truncated to match the Chinese byte count.
TRANSLATIONS = {
    # Main COM port panel
    # N=14 chars: 'Configure COM params..' = 22 too long -> 'Config COM Params ' = 18 -> truncate to 14
    '开启串口前，请配置参数...': 'Config COM Params.',  # 18 chars truncated to 14
    # N=5 chars
    '错误提示：':               'Error:',    # 6->5 (truncated to Erro: -> use 5-char 'Err: ')
    # N=4 chars each
    '打开串口':                 'Open',      # 4 chars exactly
    '关闭串口':                 'Shut',      # 4 chars exactly
    '串口打开失败，请检查串口是否存在...': 'COM open failed, check port exists... ',  # 19 chars truncated to 19
    '串口没有打开...':           'COM port not open...',  # 10 chars truncated to 10
    '参数错误...':              'Param err.',   # 7 chars truncated to 7
    '宋体':                    'MS',         # 2 chars (font name, keep short)
    # N=6 chars
    '设备连接状态':             'Status',    # 6 chars exactly
    # N=4 chars
    '搜索串口':                 'Scan',      # 4 chars exactly
    # N=6 chars
    '选择连接参数':             'Serial',    # 6 chars exactly

    # COM settings labels (all N=4 chars each)
    '数据位：':  'Data',
    '串口号：':  'Port',
    '停止位：':  'Stop',
    '校验位：':  'Pari',
    '波特率：':  'Baud',

    # Parameter config panel (right side of screenshot)
    # N=7 chars
    '从站口参数配置':           'SlvPort',   # 7 chars exactly
    # N=2 chars
    '设置':                    'OK',         # 2 chars exactly
    # N=5 chars
    '固件版本：':              'FW   ',      # 5 chars exactly (FW + 3 spaces)
    # N=10 chars each
    '主站口2通信参数配置':      'Mstr Port2',  # 10 chars exactly
    '主站口1通信参数配置':      'Mstr Port1',  # 10 chars exactly
    '主站口1超时时间配置':      'P1 T/O Cfg',  # 10 chars exactly
    '主站口2超时时间配置':      'P2 T/O Cfg',  # 10 chars exactly
    # N=4 chars
    '读取参数':                'Read',        # 4 chars exactly
    # N=8 chars
    '一键配置所有参数':         'QuickCfg',   # 8 chars exactly
    # N=4 chars
    '参数配置':                'Prms',        # 4 chars exactly

    # Connection status
    '连接成功':  'Connected ',
    '连接断开':  'Disconn.  ',

    # Calibration messages
    '0通道0点校准成功':   'Ch0 zero-cal OK   ',
    '1通道0点校准成功':   'Ch1 zero-cal OK   ',
    '0通道满量程校准成功': 'Ch0 full-scale cal OK ',
    '1通道满量程校准成功': 'Ch1 full-scale cal OK ',
    '确定要回复出厂设置吗？': 'Restore factory defaults? ',
    '提示':                'Note ',
    '参数恢复出厂成功，断电重新启动！': 'Factory reset OK. Power cycle!  ',
    '采样深度范围:10~99！':  'Sample depth range: 10~99! ',
    '通道数范围:1~8!':      'Channel range: 1~8!  ',
    '请输入参数...':         'Enter parameter...',
    '输入的参数错误...':     'Invalid parameter... ',
    '启动校准':             'Start Cal.',
    '关闭校准':             'Stop  Cal.',
    '请选选择启动采集！':    'Select to start capture! ',
    '校准的电阻参数错误！':  'Calib resist param error! ',
    '滤波值范围:需要大于400！': 'Filter range: must be >400!  ',
    '模拟量输入':            'Analog Input',
    '通道7':  'Ch7   ',
    '温度修正值': 'Temp Offset ',
    '通道6':  'Ch6   ',
    '通道5':  'Ch5   ',
    '通道4':  'Ch4   ',
    '通道3':  'Ch3   ',
    '通道2':  'Ch2   ',
    '温度值':  'Temp  ',
    '测量值':  'Measur',
    '通道编号': 'Ch Num  ',
    '通道0':  'Ch0   ',
    '通道1':  'Ch1   ',
    '模块校准配置': 'Module Cal Cfg',
    '确认校准':    'Confirm Cal ',
    '单独校准':    'Single  Cal ',
    '整体校准':    'Global  Cal ',
    '校准电阻类型': 'Cal Resist Type',
    '采样电阻1':   'Sample Resist1',
    '采样电阻2':   'Sample Resist2',
    '欧':          ' R',
    '校准电阻参数': 'Cal Resist Param',
    '电阻2内码值':  'Resist2 Code  ',
    '电阻1内码值':  'Resist1 Code  ',
    '设备地址：':   'Dev Addr: ',
    '传感器类型选择': 'Sensor Type Select',
    '其他参数配置':  'Other Params Cfg',
    '滤波值：':     'Filter:  ',
    '通道数：':     'Channels:',
    '采样深度：':   'SampDepth:',
    '通信参数配置':  'Comm Params Cfg ',
    '模块参数配置':  'Module Params Cfg',
    '写入配置':     'Write Cfg ',
    '模式配置':     'Mode  Cfg ',
    '判定总线错误时间阀值：': 'Bus error time threshold:  ',
    '总线错误保持':  'Bus Err Hold  ',
    '总线错误复位':  'Bus Err Reset ',
    '恢复出厂设置':  'Factory Reset ',
    '读出配置':     'Read  Cfg ',
    '全部关闭':     'Disable All',
    '全部开启':     'Enable  All',
    '设置IO点个数':  'Set IO Point Cnt',
    '黑体':         'Bold  ',
    '根据模块型号的IO点\r\n设置输出输入点的个数\r\nMaximum不超过48个点': 'Set I/O point count per\r\nmodule type\r\nMax 48 points allowed    ',
    '根据模块型号的IO点\r\n设置输出输入点的个数\r\n最大设置不超过48个点': 'Set I/O point count per\r\nmodule type\r\nMax 48 points allowed    ',
    '设置IO点':     'Set IO Pts ',
    'Q点个数':      'Q Points ',
    'I点个数':      'I Points ',
    '开关量输入':    'Digital Input',
    '开关量输出':    'Digital Output',
    '模块型号:':    'Module Model:',
    '默认1000，若得到的值偏小，\r\n可设置较大的校准系数': 'Default 1000; if value too\r\nsmall, increase cal factor',
    '校准系数':     'Cal Factor',
    '信号类别':     'Sig Type  ',
    '电压/电流值':   'Voltage/Current',
    '(3)0~1通道支持0~10V/0~20mA':   '(3)Ch0~1 support 0~10V/0~20mA  ',
    '(2)0~1通道输入范围：0~4095':    '(2)Ch0~1 input range: 0~4095   ',
    '(1)0~1通道为模拟量输入通道':    '(1)Ch0~1 are analog input chan  ',
    '内码':   'Code',
    '通道号':  'Ch No.',
    '模拟量校准': 'Analog Calib ',
    '满量程校准': 'Full-Scl Calib',
    '0点校准':   '0pt Calib ',
    '2通道0点校准成功':   'Ch2 zero-cal OK   ',
    '3通道0点校准成功':   'Ch3 zero-cal OK   ',
    '4通道0点校准成功':   'Ch4 zero-cal OK   ',
    '5通道0点校准成功':   'Ch5 zero-cal OK   ',
    '6通道0点校准成功':   'Ch6 zero-cal OK   ',
    '7通道0点校准成功':   'Ch7 zero-cal OK   ',
    '2通道满量程校准成功': 'Ch2 full-scale cal OK ',
    '3通道满量程校准成功': 'Ch3 full-scale cal OK ',
    '4通道满量程校准成功': 'Ch4 full-scale cal OK ',
    '5通道满量程校准成功': 'Ch5 full-scale cal OK ',
    '6通道满量程校准成功': 'Ch6 full-scale cal OK ',
    '7通道满量程校准成功': 'Ch7 full-scale cal OK ',
    '模拟量输出校准成功':  'Analog output cal OK ',
    '模拟量参数':   'Analog Params ',
    '模拟输入':    'Anlg In   ',
    '0mA校准':    '0mA Calib ',
    '10V校准':    '10V Calib ',
    '20mA校准':   '20mA Calib  ',
    '0V校准':     '0V Calib  ',
    '全部设电流':   'Set All Current',
    '全部设电压':   'Set All Voltage',
    '电流校准':    'Curr Calib  ',
    '设置信号类别':  'Set Signal Type',
    '电压校准':    'Volt Calib  ',
    '通信参数':    'Comm Params',
    '配置参数':    'Cfg Params ',
    '参数错误...！':    'Param error...! ',
    '数据长度超出范围...！': 'Data length out of range...!',
    '模拟输出':    'Anlg Out  ',
    '设置校准系数':  'Set Cal Factor',
    '起始地址：':   'Start Addr:',
    '数据长度：':   'Data Len:  ',
    '多个写入':    'Multi Write',
    '写入':       'Write',
    '启动校验按钮':  'Start Check Btn',
    '楷体':       'Italic',
    'MR0系列MODBUS工具': 'MR0 Series MODBUS Tool  ',
    '设备未连接':   'Dev Not Conn.',
    '请输入正确的砝码重量...': 'Enter correct weight...   ',
    '全输入砝码重量...':     'Enter all weights...  ',
    '全输入正确的砝码重量...': 'Enter all correct weights.',
    '称重信息配置':          'Scale Info Config',
    '注：砝码重量输入最大有效值1000000000，且小数部分也会占用有效位。如输入2位小数时，输入最大有效值为10000000.00':
        'Note: Max weight input 1000000000. Decimals also use digits. E.g. 2 decimal places, max is 10000000.00              ',
    '默认单位：克':   'Default unit: g  ',
    '小数点':       'Dec Pt.',
    '校正内码':     'Correct Code',
    '标定':        'Calibrate',
    '取消去皮':     'Cancel Tare',
    '去皮':        '  Tare',
    '零点校准':     'Zero Calib',
    '砝码重量':     'Weight    ',
    '砝码内码':     'Weight Code',
    '零点内码':     'Zero Code ',
    '皮重内码':     'Tare Code ',
    '实时重量':     'Live Weight',
    '实时内码':     'Live Code ',
    '帮    助':    'Help      ',
    '版本：':      'Ver:  ',
    '称重通道：':   'Weigh Chan:',
    '采样速率：':   'Sample Rate:',
    '通道5：':     'Chan 5: ',
    '通道4：':     'Chan 4: ',
    '通道3：':     'Chan 3: ',
    '通道2：':     'Chan 2: ',
    '通道1：':     'Chan 1: ',
    '通道0：':     'Chan 0: ',
    '强制0点校准':  'Force Zero Cal',
    '强制量程标定':  'Force Scale Cal',
    '设备已连接':   'Dev Connected',
    '设置参数成功，断电重新启动！': 'Params set OK. Power cycle! ',
    '16路':       '16-Ch ',
    '校准系数设置':  'Cal Factor Set',
    '寄存器操作':   'Register Ops',
    '启用单独校准':  'Enable Single Cal',
    '保持寄存器器':  'Hold Register ',
    '4路模拟输入':  '4-Ch Analog In',
    '4mA校准':    '4mA Calib ',
    '4路模拟输出':  '4-Ch Analog Out',
    '特别单独校准':  'Special Sngl Cal',
    '(2)1~8通道输出范围：0~4095':   '(2)Ch1~8 output range: 0~4095 ',
    '(1)1~8通道为模拟量输出通道':    '(1)Ch1~8 are analog output ch  ',
    '已经保存！':   'Already saved! ',
    '成功提示：':   'Success:   ',
    '保存模拟量配置参数': 'Save Analog Config Params',
    '设置全部电压':  'Set All Voltage',
    '设置全电流':   'Set All Current',
    '设置通道数':   'Set Chan Count',
    '设置分辨率':   'Set Resolution',
    '设置采样深度':  'Set Sample Depth',
    '(4)设置参数后必须点击保存':   '(4)Must click Save after setting  ',
    '(3)0~7通道支持0~10V/0~20mA': '(3)Ch0~7 support 0~10V/0~20mA  ',
    '(2)0~7通道输入范围：0~16000':  '(2)Ch0~7 input range: 0~16000  ',
    '(1)0~7通道为模拟量输入通道':   '(1)Ch0~7 are analog input chan  ',
    '校准':       '  Cal ',
    '4ma校准':    '4mA Calib ',
    '20ma校准':   '20mA Calib  ',
    '写入地址':    'Write Addr',
    '(2)1~4路输出范围：0~4095':  '(2)Ch1~4 output range:0~4095 ',
    '(1)1~2通道分辨率9位，3~4通道分辨率12位': '(1)Ch1-2: 9-bit res, Ch3-4: 12-bit   ',
    '采样深度范围:15~99！':  'Sample depth range: 15~99! ',
    '通道数范围:1~4!':      'Channel range: 1~4!  ',
    '整体确认校准':  'Confirm All Cal',
    '欧（单位）':   'Ohm (unit)',
    '指令发送成功.': 'Cmd sent OK.   ',
    '提示：':      'Note:  ',
    '请选择热电偶类型': 'Select thermocouple type',
    '请输入正确温度值': 'Enter correct temp value',
    '传感器状态':   'Sensor Status',
    '冷端温度':    'Cold Junc Temp',
    '查询':       '  Query',
    '冷端温度修正':  'Cold Junc Offset',
    '热电偶类型正值': 'TC type pos value',
    '查询参数':    'Query Params',
    '例：如要修正1.2℃，则填入12': 'E.g. to correct 1.2C, enter 12  ',
    '第1路连接接收：': 'Port 1 Connect Recv: ',
    'IP或者端口号错误...': 'IP or port error...   ',
    '设置成功！':   'Set success!   ',
    '连接没有建立...！': 'Connection not established!  ',
    '设置成功':    'Set OK    ',
    'TCP连接配置':  'TCP Conn Config',
    '断开连接':    'Disconnect',
    '启动MODBUS TCP': 'Start MODBUS TCP  ',
    '扫描时间：':   'Scan Time: ',
    '模块地址：':   'Module Addr:',
    '保持寄存器个数：': 'Hold Register Count:',
    '只读寄存器个数：': 'ReadOnly Reg Count: ',
    '数字输入个数：':  'Digital Input Count:',
    '数字输出个数：':  'Digital Output Count',
    '模块端口号：':   'Module Port:   ',
    '模块IP地址：':   'Module IP Addr:',
    '配置以太网参数':  'Config Ethernet Params',
    '设置总线错误参数': 'Set Bus Error Params  ',
    '设置波特率系数':  'Set Baud Rate Coeff  ',
    '设置设备地址':   'Set Device Address',
    '设置子网掩码':   'Set Subnet Mask   ',
    '设置网关':     'Set Gateway',
    '设置IP':      'Set IP   ',
    '子网掩码：':   'Subnet Mask:',
    '网关：':      'Gateway:',
    'IP地址：':    'IP Addr: ',
    '强制校准':    'Force Cal.',
    '连接失败...！':  'Connect failed...! ',
    '设置总线错参数':  'Set Bus Error Params',
    '全部设4-20ma':  'Set All 4-20mA   ',
    '通道15':  'Chan 15 ',
    '通道14':  'Chan 14 ',
    '通道13':  'Chan 13 ',
    '通道12':  'Chan 12 ',
    '通道11':  'Chan 11 ',
    '通道10':  'Chan 10 ',
    '通道9':   'Ch9   ',
    '通道8':   'Ch8   ',
    '写入校准系数':  'Write Cal Factor',
    '输出模式切换':  'Switch Output Mode',
    '一键全电压0点校准':  'One-Click All-V Zero Cal  ',
    '一键全电压10V校准':  'One-Click All-V 10V Cal   ',
    '一键全电流0点校准':  'One-Click All-I Zero Cal  ',
    '一键全电流20ma校准': 'One-Click All-I 20mA Cal   ',
    '一键全电流4ma校准':  'One-Click All-I 4mA Cal   ',
    '写入成功':    'Write OK  ',
    '写入总线地址成功,请断电重启': 'Bus addr write OK, power cycle',
    '请正确填入地址':    'Enter valid address   ',
    '请选择正确类型':    'Select valid type     ',
    '请正确填入输入线圈地址':  'Enter valid input coil addr  ',
    '请选择正确输入线圈类型':  'Select valid input coil type ',
    '请正确填入输出线圈地址':  'Enter valid output coil addr ',
    '请选择正确输出线圈类型':  'Select valid output coil type',
    '请正确填入输入寄存器地址': 'Enter valid input reg addr   ',
    '请选择正确输入寄存器类型': 'Select valid input reg type  ',
    '请正确填入保持寄存器地址': 'Enter valid hold reg addr    ',
    '请选择正确保持寄存器类型': 'Select valid hold reg type   ',
    '请正确填入MPI地址':  'Enter valid MPI address  ',
    '请正确填入DP地址':   'Enter valid DP address ',
    '读取':       'Read  ',
    '类型':       '  Type',
    '地址':       '  Addr',
    '保持寄存器：': 'Hold Register:',
    '输入寄存器：': 'Input Register:',
    '输出线圈：':  'Output Coil:',
    '输入线圈：':  'Input Coil: ',
    '自动连接':    'Auto Connect',
    '一键写入配置':  'One-Click Write Cfg',
    '总线地址：':   'Bus Addr:   ',
    '一键读出配置':  'One-Click Read Cfg ',
    'RS232波特率：': 'RS232 Baud Rate:',
    '通讯参数':    'Comm Params',
    '模拟量输出':   'Analog Output ',
    '当前值':      'Curr Val',
    '写入值':      'Write Val',
    '设置4~20ma':  'Set 4~20mA ',
    '支持EMB-AQ8':  'Supports EMB-AQ8   ',
    '支持EMB-AM8':  'Supports EMB-AM8   ',
    '支持EMB-AE8':  'Supports EMB-AE8   ',
    '支持EMB-AR8G\r\n':    'Supports EMB-AR8G\r\n   ',
    '支持MR2A-AR8G\r\n':   'Supports MR2A-AR8G\r\n  ',
    '支持EMA-AW4G':   'Supports EMA-AW4G    ',
    '支持MR2A-AW4G':  'Supports MR2A-AW4G   ',
    '支持JY-MODBUS-4AI4AO\r\n':   'Supports JY-MODBUS-4AI4AO\r\n  ',
    '支持MODBUS-8TC\r\n支持MODBUS-2PT4NTC': 'Supports MODBUS-8TC\r\nSupports MODBUS-2PT4NTC    ',
    '支持MODBUS-4AI4AO\r\n':  'Supports MODBUS-4AI4AO\r\n    ',
    '支持MODBUS-8AO\r\n支持JY-MODBUS-8AO': 'Supports MODBUS-8AO\r\nSupports JY-MODBUS-8AO   ',
    '支持JY-MODBUS-8AI':  'Supports JY-MODBUS-8AI    ',
    '支持MODBUS-2AI':    'Supports MODBUS-2AI     ',
    '支持MODBUS-8PT\r\n\r\n': 'Supports MODBUS-8PT\r\n\r\n  ',
    '东莞市艾莫迅自动化科技有限公司v1.5.8': 'Dongguan Aimaoxun Automation v1.5.8  ',
    'MODBUS工具':   'MODBUS Tool  ',
    '采样深度1~20！':  'Sample depth 1~20!  ',
    '通道数只能是1':   'Channel count must be 1',
    '退出校准':     'Exit Cal. ',
    '正常':        'OK  ',
    '断线':        'Disconn',
    'NTC采样深度：': 'NTC Sample Depth:',
    '环境温度：':   'Ambient Temp:',
    '正  常':      'OK     ',
    'MODBUS-2AI配置': 'MODBUS-2AI Config  ',
    '请输入砝码重量...':  'Enter weight...    ',
    '请输入正确的内码...': 'Enter valid code...  ',
    '设置满量程重量':    'Set Full Scale Weight',
    '设置满量程内码值':   'Set Full Scale Code  ',
    '强制标定量程':     'Force Scale Calib',
    '输出模式切换:':    'Switch Output Mode:',
    '采样深度只能是1~10！': 'Sample depth must be 1~10!  ',
    '保存参数':       'Save Params',
    '请输入0点对应的内码...': 'Enter code for zero point...',
    '设置0点校准值':     'Set Zero Cal Value  ',
    '说明2：':        'Note 2: ',
    '1、设备地址可配置范围是1-255。\r\n\r\n2、砝码重量支持输入小数点；最大3位。\r\n\r\n':
        '1. Device addr range: 1-255.\r\n\r\n2. Weight supports decimal; max 3 places.\r\n\r\n',
    '说明1：':        'Note 1: ',
    '1、该版本暂不支持配置采样深度、采样速率和称重通道\r\n\r\n2、该版本暂不支持配置校验位、数据位和停止位\r\n\r\n':
        '1. This version does not support sample depth/rate/weigh channel config\r\n\r\n2. This version does not support parity/data bits/stop bits config\r\n\r\n',
    '创建串口失败:':    'Failed create COM: ',
    '串口通信出错提示:':  'COM comm error:    ',
    '操作提示：':       'Operation Note: ',
    '参数设置成功，断电重新启动！': 'Params set OK. Power cycle!  ',
    '参数设置成功！':    'Params set OK!  ',
    '参数设置成功，断电重启动！': 'Params OK. Reboot needed!  ',
}


def make_utf16le(text, byte_count):
    """Encode text as UTF-16LE, truncated or space-padded to exactly byte_count bytes."""
    encoded = text.encode('utf-16-le')
    if len(encoded) <= byte_count:
        # Pad with spaces (0x20 0x00 per space in UTF-16LE)
        pad_chars = (byte_count - len(encoded)) // 2
        encoded = encoded + b'\x20\x00' * pad_chars
        # Handle odd leftover (shouldn't happen with UTF-16 but just in case)
        if len(encoded) < byte_count:
            encoded = encoded + b'\x00' * (byte_count - len(encoded))
    else:
        # Truncate to byte_count (must be even for UTF-16)
        encoded = encoded[:byte_count]
    return encoded


def patch(data):
    data = bytearray(data)
    patched = 0
    not_found = []

    for chinese, english in TRANSLATIONS.items():
        original = chinese.encode('utf-16-le')
        idx = data.find(original)
        if idx == -1:
            not_found.append(chinese)
            continue

        replacement = make_utf16le(english, len(original))
        assert len(replacement) == len(original), f"Length mismatch for {chinese!r}"
        data[idx:idx+len(original)] = replacement
        patched += 1

    print(f"Patched {patched} strings.")
    if not_found:
        print(f"Not found ({len(not_found)}): {[s[:10] for s in not_found]}")
    return bytes(data)


if __name__ == '__main__':
    import io, sys
    sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8', errors='replace')
    print(f"Reading {INPUT}...")
    with open(INPUT, 'rb') as f:
        data = f.read()

    print("Patching strings...")
    patched = patch(data)

    print(f"Writing {OUTPUT}...")
    with open(OUTPUT, 'wb') as f:
        f.write(patched)

    print("Done! Run MR0_MODBUS_2M1_EN.exe to test.")
    print("Note: this is the legacy minimal patcher. For layout/font fixes, see TranslatorTools.")
