/*
 * 深圳德卡T10N读卡器.
 * 
 * **/
using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.RfidReader
{

    internal partial class Trf32Constants
    {

        /// KEY_DIGIT0 -> 0x45
        public const int KEY_DIGIT0 = 69;

        /// KEY_DIGIT1 -> 0x16
        public const int KEY_DIGIT1 = 22;

        /// KEY_DIGIT2 -> 0x1E
        public const int KEY_DIGIT2 = 30;

        /// KEY_DIGIT3 -> 0x26
        public const int KEY_DIGIT3 = 38;

        /// KEY_DIGIT4 -> 0x25
        public const int KEY_DIGIT4 = 37;

        /// KEY_DIGIT5 -> 0x2E
        public const int KEY_DIGIT5 = 46;

        /// KEY_DIGIT6 -> 0x36
        public const int KEY_DIGIT6 = 54;

        /// KEY_DIGIT7 -> 0x3D
        public const int KEY_DIGIT7 = 61;

        /// KEY_DIGIT8 -> 0x3E
        public const int KEY_DIGIT8 = 62;

        /// KEY_DIGIT9 -> 0x46
        public const int KEY_DIGIT9 = 70;

        /// KEY_DOT -> 0x49
        public const int KEY_DOT = 73;

        /// KEY_F1 -> 0x05
        public const int KEY_F1 = 5;

        /// KEY_F2 -> 0x06
        public const int KEY_F2 = 6;

        /// KEY_F3 -> 0x04
        public const int KEY_F3 = 4;

        /// KEY_F4 -> 0x0C
        public const int KEY_F4 = 12;

        /// KEY_F5 -> 0x03
        public const int KEY_F5 = 3;

        /// KEY_F6 -> 0x0B
        public const int KEY_F6 = 11;

        /// KEY_ESC -> 0x76
        public const int KEY_ESC = 118;

        /// KEY_CANCEL -> 0x66
        public const int KEY_CANCEL = 102;

        /// KEY_ENTER -> 0x5A
        public const int KEY_ENTER = 90;

        public const byte SELECT_MODE_IDLE = 0; //表示IDLE模式，一次只对一张卡操作；
        public const byte SELECT_MODE_ALL = 1;  //表示ALL模式，一次可对多张卡操作;

        public const ushort RQ_TRAIT_MIFARE1 = 4; public const ushort SL_TRAIT_MIFARE1 = 136;
        public const ushort RQ_TRAIT_FM11RF32 = 4; public const ushort SL_TRAIT_FM11RF32 = 83;
        public const ushort RQ_TRAIT_S70 = 2; public const ushort SL_TRAIT_S70 = 24;
        public const ushort RQ_TRAIT_ULTRALIGHT = 68; public const ushort SL_TRAIT_ULTRALIGHT = 4;
        public const ushort RQ_TRAIT_SHC1102 = 13056;
        public const ushort RQ_TRAIT_MIFARE_LIGHT = 16; public const ushort SL_TRAIT_MIFARE_LIGHT = 129;
        public const ushort RQ_TRAIT_MIFARE_PRO = 8; public const ushort SL_TRAIT_MIFARE_PRO = 32;
        public const ushort RQ_TRAIT_DESFIRE = 836; public const ushort SL_TRAIT_DESFIRE = 32;
        public const ushort RQ_TRAIT_FM11RF005 = 5;
        public const ushort RQ_TRAIT_MIFARE_PLUS = 180;
    }

    public partial class NMTrf32
    {

        /// Return Type: int
        ///DeviceName: char*
        ///Baudrate: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_init_comm", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int DC_init_comm([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string DeviceName, uint Baudrate);


        /// Return Type: int
        ///DeviceHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_exit_comm", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int DC_exit_comm(int DeviceHandle);


        /// Return Type: int
        ///DeviceHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_find_i_d", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int DC_find_i_d(int DeviceHandle);


        /// Return Type: int
        ///DeviceHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_start_i_d", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int DC_start_i_d(int DeviceHandle);


        /// Return Type: char*
        ///IdHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_name", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr DC_i_d_query_name(int IdHandle);


        /// Return Type: char*
        ///IdHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_sex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr DC_i_d_query_sex(int IdHandle);


        /// Return Type: char*
        ///IdHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_nation", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr DC_i_d_query_nation(int IdHandle);


        /// Return Type: char*
        ///IdHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_birth", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr DC_i_d_query_birth(int IdHandle);


        /// Return Type: char*
        ///IdHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_address", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr DC_i_d_query_address(int IdHandle);


        /// Return Type: char*
        ///IdHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_id_number", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr DC_i_d_query_id_number(int IdHandle);


        /// Return Type: char*
        ///IdHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_department", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr DC_i_d_query_department(int IdHandle);


        /// Return Type: char*
        ///IdHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_expire_day", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr DC_i_d_query_expire_day(int IdHandle);


        /// Return Type: unsigned char*
        ///IdHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_photo", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr DC_i_d_query_photo(int IdHandle);


        /// Return Type: unsigned int
        ///IdHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_photo_len", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern uint DC_i_d_query_photo_len(int IdHandle);


        /// Return Type: int
        ///IdHandle: int
        ///FileName: char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_photo_file", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int DC_i_d_query_photo_file(int IdHandle, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string FileName);


        /// Return Type: int
        ///IdHandle: int
        ///BmpBuffer: unsigned char*
        ///BmpLength: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_i_d_query_photo_bmp_buffer", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int DC_i_d_query_photo_bmp_buffer(int IdHandle, System.IntPtr BmpBuffer, ref uint BmpLength);


        /// Return Type: void
        ///IdHandle: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "DC_end_i_d", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern void DC_end_i_d(int IdHandle);


        /// Return Type: int
        ///port: int
        ///baud: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_init", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_init(int port, uint baud);


        /// Return Type: int
        ///icdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_exit", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_exit(int icdev);


        /// Return Type: int
        ///icdev: int
        ///ms: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_beep", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_beep(int icdev, ushort ms);


        /// Return Type: int
        ///icdev: int
        ///offset: unsigned int
        ///len: unsigned int
        ///data_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_srd_eeprom", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_srd_eeprom(int icdev, uint offset, uint len, System.IntPtr data_buffer);


        /// Return Type: int
        ///icdev: int
        ///offset: unsigned int
        ///len: unsigned int
        ///data_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_srd_eepromhex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_srd_eepromhex(int icdev, uint offset, uint len, System.IntPtr data_buffer);


        /// Return Type: int
        ///icdev: int
        ///offset: unsigned int
        ///len: unsigned int
        ///data_buffer: char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_swr_eeprom", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_swr_eeprom(int icdev, uint offset, uint len, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string data_buffer);


        /// Return Type: int
        ///icdev: int
        ///offset: unsigned int
        ///len: unsigned int
        ///data_buffer: char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_swr_eepromhex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_swr_eepromhex(int icdev, uint offset, uint len, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string data_buffer);


        /// Return Type: int
        ///icdev: int
        ///data_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_getver", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_getver(int icdev, byte[] data_buffer);


        /// Return Type: int
        ///icdev: int
        ///baud: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_set_baudrate", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_set_baudrate(int icdev, uint baud);


        /// Return Type: int
        ///icdev: int
        ///_Mode: unsigned char
        ///TagType: unsigned short*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_request", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_request(int icdev, byte _Mode, ref ushort TagType);


        /// Return Type: int
        ///icdev: int
        ///_Bcnt: unsigned char
        ///_Snr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_anticoll", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_anticoll(int icdev, byte _Bcnt, ref uint _Snr);


        /// Return Type: int
        ///icdev: int
        ///_Snr: unsigned int
        ///_Size: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_select", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_select(int icdev, uint _Snr, ref byte _Size);


        /// Return Type: int
        ///icdev: int
        ///_Mode: unsigned char
        ///_Snr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_card", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_card(int icdev, byte _Mode, ref uint _Snr);


        /// Return Type: int
        ///icdev: int
        ///_Mode: unsigned char
        ///_SecNr: unsigned char
        ///_NKey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_load_key", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_load_key(int icdev, byte _Mode, byte _SecNr, System.IntPtr _NKey);


        /// Return Type: int
        ///icdev: int
        ///_Mode: unsigned char
        ///_SecNr: unsigned char
        ///_NKey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_load_key_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_load_key_hex(int icdev, byte _Mode, byte _SecNr, System.IntPtr _NKey);


        /// Return Type: int
        ///icdev: int
        ///_Mode: unsigned char
        ///_SecNr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_authentication", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_authentication(int icdev, byte _Mode, byte _SecNr);


        /// Return Type: int
        ///icdev: int
        ///_Mode: unsigned char
        ///_SecAddr: unsigned char
        ///passbuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_authentication_pass", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_authentication_pass(int icdev, byte _Mode, byte _SecAddr, System.IntPtr passbuff);


        /// Return Type: int
        ///icdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_halt", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_halt(int icdev);


        /// Return Type: int
        ///icdev: int
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_read", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_read(int icdev, byte _Adr, byte[] _Data);


        /// Return Type: int
        ///icdev: int
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_read_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_read_hex(int icdev, byte _Adr, System.IntPtr _Data);


        /// Return Type: int
        ///icdev: int
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_write", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_write(int icdev, byte _Adr, byte[] _Data);


        /// Return Type: int
        ///icdev: int
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_write_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_write_hex(int icdev, byte _Adr, byte[] _Data);


        /// Return Type: int
        ///icdev: int
        ///_Adr: unsigned char
        ///_Value: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_initval", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_initval(int icdev, byte _Adr, uint _Value);


        /// Return Type: int
        ///icdev: int
        ///_Adr: unsigned char
        ///_Value: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_increment", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_increment(int icdev, byte _Adr, uint _Value);


        /// Return Type: int
        ///icdev: int
        ///_Adr: unsigned char
        ///_Value: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_decrement", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_decrement(int icdev, byte _Adr, uint _Value);


        /// Return Type: int
        ///icdev: int
        ///_Adr: unsigned char
        ///_Value: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_readval", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_readval(int icdev, byte _Adr, ref uint _Value);


        /// Return Type: int
        ///icdev: int
        ///_OriginalAdr: unsigned char
        ///_BackupAdr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_valuebackup", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_valuebackup(int icdev, byte _OriginalAdr, byte _BackupAdr);


        /// Return Type: int
        ///icdev: int
        ///Cardtype: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_setcpu", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_setcpu(int icdev, byte Cardtype);


        /// Return Type: int
        ///icdev: int
        ///cardtype: unsigned char
        ///baudrate: unsigned char
        ///Volt: unsigned char
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///protocol: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_cpureset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_cpureset(int icdev, byte cardtype, byte baudrate, byte Volt, ref byte rlen, byte[] databuffer, ref byte protocol);


        /// Return Type: int
        ///icdev: int
        ///cardtype: unsigned char
        ///baudrate: unsigned char
        ///Volt: unsigned char
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///protocol: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_cpureset_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_cpureset_hex(int icdev, byte cardtype, byte baudrate, byte Volt, System.IntPtr rlen, System.IntPtr databuffer, System.IntPtr protocol);


        /// Return Type: int
        ///icdev: int
        ///Cardtype: unsigned char
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///protocol: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_cpuapdusource", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_cpuapdusource(int icdev, byte Cardtype, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte protocol);


        /// Return Type: int
        ///icdev: int
        ///Cardtype: unsigned char
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///protocol: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_cpuapdusource_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_cpuapdusource_hex(int icdev, byte Cardtype, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte protocol);


        /// Return Type: int
        ///icdev: int
        ///Cardtype: unsigned char
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///protocol: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_cpuapdu", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_cpuapdu(int icdev, byte Cardtype, byte slen,byte[] sendbuffer, ref byte rlen, byte[] databuffer, byte protocol);


        /// Return Type: int
        ///icdev: int
        ///Cardtype: unsigned char
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///protocol: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_cpuapdu_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_cpuapdu_hex(int icdev, byte Cardtype, byte slen, byte[] sendbuffer, ref byte rlen, byte[] databuffer, byte protocol);


        /// Return Type: int
        ///icdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_reset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_reset(int icdev);


        /// Return Type: int
        ///icdev: int
        ///cardtype: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_config_card", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_config_card(int icdev, byte cardtype);


        /// Return Type: int
        ///icdev: int
        ///rlen: unsigned char*
        ///_Snr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_card_pro", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_card_pro(int icdev, System.IntPtr rlen, System.IntPtr _Snr);


        /// Return Type: int
        ///icdev: int
        ///rlen: unsigned char*
        ///_Snr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_card_prohex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_card_prohex(int icdev, ref byte rlen,byte[] _Snr);


        /// Return Type: int
        ///icdev: int
        ///rlen: unsigned char*
        ///databuf: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_pro_reset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_pro_reset(int icdev, ref byte rlen,byte[] databuf);


        /// Return Type: int
        ///icdev: int
        ///rlen: unsigned char*
        ///databuf: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_pro_resethex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_pro_resethex(int icdev, System.IntPtr rlen, System.IntPtr databuf);


        /// Return Type: int
        ///icdev: int
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///timeout: unsigned char
        ///FG: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_pro_commandlink", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_pro_commandlink(int icdev, byte slen, byte[] sendbuffer, ref byte rlen, byte[] databuffer, byte timeout, byte FG);


        /// Return Type: int
        ///icdev: int
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///timeout: unsigned char
        ///FG: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_pro_commandlink_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_pro_commandlink_hex(int icdev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte timeout, byte FG);


        /// Return Type: int
        ///icdev: int
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///time: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_pro_commandsource", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_pro_commandsource(int icdev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte time);


        /// Return Type: int
        ///icdev: int
        ///ctime: unsigned char
        ///pTrack1Data: unsigned char*
        ///pTrack1Len: unsigned int*
        ///pTrack2Data: unsigned char*
        ///pTrack2Len: unsigned int*
        ///pTrack3Data: unsigned char*
        ///pTrack3Len: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_readmagcardall", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_readmagcardall(int icdev, byte ctime, System.IntPtr pTrack1Data, ref uint pTrack1Len, System.IntPtr pTrack2Data, ref uint pTrack2Len, System.IntPtr pTrack3Data, ref uint pTrack3Len);


        /// Return Type: int
        ///icdev: int
        ///ctime: unsigned char
        ///rlen: unsigned char*
        ///cpass: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_getinputpass", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_getinputpass(int icdev, byte ctime, System.IntPtr rlen, System.IntPtr cpass);


        /// Return Type: int
        ///icdev: int
        ///LedSel: unsigned char
        ///LedCtr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_ctlled", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_ctlled(int icdev, byte LedSel, byte LedCtr);


        /// Return Type: int
        ///icdev: int
        ///OpenFlag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_ctlbacklight", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_ctlbacklight(int icdev, byte OpenFlag);


        /// Return Type: int
        ///icdev: int
        ///LineFlag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_lcdclrscrn", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_lcdclrscrn(int icdev, byte LineFlag);


        /// Return Type: int
        ///icdev: int
        ///line: unsigned char
        ///offset: unsigned char
        ///info: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_dispsingle", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_dispsingle(int icdev, byte line, byte offset, byte info);


        /// Return Type: int
        ///icdev: int
        ///line: unsigned char
        ///offset: unsigned char
        ///length: unsigned char
        ///data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_dispinfo", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_dispinfo(int icdev, byte line, byte offset, byte length, System.IntPtr data);


        /// Return Type: int
        ///icdev: int
        ///line: unsigned char
        ///offset: unsigned char
        ///info: char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_dispsingleinfo", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_dispsingleinfo(int icdev, byte line, byte offset, [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string info);


        /// Return Type: int
        ///icdev: int
        ///year: unsigned short
        ///month: unsigned short
        ///date: unsigned short
        ///hour: unsigned short
        ///minute: unsigned short
        ///second: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_setreadertime", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_setreadertime(int icdev, ushort year, ushort month, ushort date, ushort hour, ushort minute, ushort second);


        /// Return Type: int
        ///icdev: int
        ///year: unsigned short*
        ///month: unsigned short*
        ///date: unsigned short*
        ///hour: unsigned short*
        ///minute: unsigned short*
        ///second: unsigned short*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_getreadertime", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_getreadertime(int icdev, ref ushort year, ref ushort month, ref ushort date, ref ushort hour, ref ushort minute, ref ushort second);


        /// Return Type: int
        ///idComDev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_down", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_down(int idComDev);


        /// Return Type: int
        ///hDev: int
        ///type: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_inittype", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_inittype(int hDev, byte type);


        /// Return Type: int
        ///idcomdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_check_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_check_4442(int idcomdev);


        /// Return Type: int
        ///idcomdev: int
        ///offset: unsigned char
        ///len: short
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_read_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_read_4442(int idcomdev, byte offset, short len, System.IntPtr databuffer);


        /// Return Type: int
        ///idcomdev: int
        ///offset: unsigned char
        ///len: short
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_write_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_write_4442(int idcomdev, byte offset, short len, System.IntPtr databuffer);


        /// Return Type: int
        ///idcomdev: int
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_readpass_sle4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_readpass_sle4442(int idcomdev, System.IntPtr password);


        /// Return Type: int
        ///idcomdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_readcount_sle4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_readcount_sle4442(int idcomdev);


        /// Return Type: int
        ///idcomdev: int
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_checkpass_sle4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_checkpass_sle4442(int idcomdev, System.IntPtr password);


        /// Return Type: int
        ///idcomdev: int
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_changepass_sle4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_changepass_sle4442(int idcomdev, System.IntPtr password);


        /// Return Type: int
        ///dcdev: int
        ///offset: unsigned char
        ///len: unsigned char
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_readprotection", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_readprotection(int dcdev, byte offset, byte len, System.IntPtr protbuffer);


        /// Return Type: int
        ///dcdev: int
        ///offset: unsigned char
        ///len: unsigned char
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_writeprotection", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_writeprotection(int dcdev, byte offset, byte len, System.IntPtr protbuffer);


        /// Return Type: int
        ///icdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_down_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_down_4442(int icdev);


        /// Return Type: int
        ///idcomdev: int
        ///offset: unsigned char
        ///len: unsigned char
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_writeprotection_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_writeprotection_hex(int idcomdev, byte offset, byte len, System.IntPtr protbuffer);


        /// Return Type: int
        ///idcomdev: int
        ///offset: unsigned char
        ///len: unsigned char
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_readprotection_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_readprotection_hex(int idcomdev, byte offset, byte len, System.IntPtr protbuffer);


        /// Return Type: int
        ///idcomdev: int
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_checkpass_4442hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_checkpass_4442hex(int idcomdev, System.IntPtr password);


        /// Return Type: int
        ///idcomdev: int
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_changepass_4442hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_changepass_4442hex(int idcomdev, System.IntPtr password);


        /// Return Type: int
        ///idcomdev: int
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_readpass_4442hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_readpass_4442hex(int idcomdev, System.IntPtr password);


        /// Return Type: int
        ///idcomdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_check_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_check_4428(int idcomdev);


        /// Return Type: int
        ///idcomdev: int
        ///offset: short
        ///len: short
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_read_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_read_4428(int idcomdev, short offset, short len, System.IntPtr databuffer);


        /// Return Type: int
        ///idcomdev: int
        ///offset: short
        ///len: short
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_write_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_write_4428(int idcomdev, short offset, short len, System.IntPtr databuffer);


        /// Return Type: int
        ///idcomdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_readcount_sle4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_readcount_sle4428(int idcomdev);


        /// Return Type: int
        ///idcomdev: int
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_checkpass_sle4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_checkpass_sle4428(int idcomdev, System.IntPtr password);


        /// Return Type: int
        ///idcomdev: int
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_changepass_sle4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_changepass_sle4428(int idcomdev, System.IntPtr password);


        /// Return Type: int
        ///dcdev: int
        ///offset: short
        ///len: short
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_writewithprotection", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_writewithprotection(int dcdev, short offset, short len, System.IntPtr protbuffer);


        /// Return Type: int
        ///dcdev: int
        ///offset: short
        ///len: short
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_readwithprotection", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_readwithprotection(int dcdev, short offset, short len, System.IntPtr protbuffer);


        /// Return Type: int
        ///icdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_down_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_down_4428(int icdev);


        /// Return Type: int
        ///idcomdev: int
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_writewithprotection_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_writewithprotection_hex(int idcomdev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: int
        ///idcomdev: int
        ///offset: short
        ///len: short
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_readwithprotection_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_readwithprotection_hex(int idcomdev, short offset, short len, System.IntPtr protbuffer);


        /// Return Type: int
        ///idcomdev: int
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_checkpass_4428hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_checkpass_4428hex(int idcomdev, System.IntPtr password);


        /// Return Type: int
        ///idcomdev: int
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_changepass_4428hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_changepass_4428hex(int idcomdev, System.IntPtr password);


        /// Return Type: int
        ///idcomdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_check_24c01", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_check_24c01(int idcomdev);


        /// Return Type: int
        ///idcomdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_check_24c02", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_check_24c02(int idcomdev);


        /// Return Type: int
        ///idcomdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_check_24c04", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_check_24c04(int idcomdev);


        /// Return Type: int
        ///idcomdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_check_24c08", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_check_24c08(int idcomdev);


        /// Return Type: int
        ///idcomdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_check_24c16", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_check_24c16(int idcomdev);


        /// Return Type: int
        ///idcomdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_check_24c64", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_check_24c64(int idcomdev);


        /// Return Type: int
        ///idcomdev: int
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_read24", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_read24(int idcomdev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: int
        ///idcomdev: int
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_write24", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_write24(int idcomdev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: int
        ///idcomdev: int
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_write24_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_write24_hex(int idcomdev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: int
        ///idcomdev: int
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_read64", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_read64(int idcomdev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: int
        ///idcomdev: int
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_write64", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_write64(int idcomdev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: int
        ///idcomdev: int
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_write64_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_write64_hex(int idcomdev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: int
        ///icdev: int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_down_24c", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_down_24c(int icdev);


        /// Return Type: int
        ///pHexChar: unsigned char*
        ///pStdChar: unsigned char*
        ///StdCharLen: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "a_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int a_hex(System.IntPtr pHexChar, System.IntPtr pStdChar, uint StdCharLen);


        /// Return Type: int
        ///pStdChar: unsigned char*
        ///pHexChar: unsigned char*
        ///StdCharLen: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "hex_a", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int hex_a(System.IntPtr pStdChar, System.IntPtr pHexChar, uint StdCharLen);


        /// Return Type: int
        ///icdev: int
        ///sdata: unsigned char*
        ///slen: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_setID", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_setID(int icdev, System.IntPtr sdata, uint slen);


        /// Return Type: int
        ///icdev: int
        ///slen: unsigned char
        ///cmd: unsigned char*
        ///rlen: unsigned char*
        ///Num_sim: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Trf32.dll", EntryPoint = "dc_rf_sim", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern int dc_rf_sim(int icdev, byte slen, System.IntPtr cmd, System.IntPtr rlen, System.IntPtr Num_sim);

    }

}
