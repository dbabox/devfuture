/*
 * 深圳德卡D8U读卡器.
 * 
 * */

namespace Rtp.Driver.RfidReader
{
    internal sealed class NMDcrf32V8
    {

        public const byte SELECT_MODE_IDLE = 0; //表示IDLE模式，一次只对一张卡操作；
        public const byte SELECT_MODE_ALL = 1;  //表示ALL模式，一次可对多张卡操作;
        public const byte SELECT_MODE_SNR = 2;  //表示指定卡模式，只对序列号等于snr的卡操作（高级函数才有）


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
        


        /// Return Type: HANDLE->void*
        ///port: short
        ///baud: int
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_init", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr dc_init(short port, int baud);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_exit", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_exit(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Baud: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_config", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_config(System.IntPtr icdev, byte _Mode, byte _Baud);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///TagType: unsigned short*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_request", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_request(System.IntPtr icdev, byte _Mode, ref ushort TagType);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Bcnt: unsigned char
        ///_Snr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_anticoll", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_anticoll(System.IntPtr icdev, byte _Bcnt, byte[] _Snr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Snr: unsigned int
        ///_Size: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_select", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_select(System.IntPtr icdev, uint _Snr, ref byte _Size);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_SecNr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_authentication", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_authentication(System.IntPtr icdev, byte _Mode, byte _SecNr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_halt", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_halt(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read(System.IntPtr icdev, byte _Adr, byte[] _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_hex(System.IntPtr icdev, byte _Adr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write(System.IntPtr icdev, byte _Adr, byte[] _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_hex(System.IntPtr icdev, byte _Adr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_TS", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_TS(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_SecNr: unsigned char
        ///_NKey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_load_key", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_load_key(System.IntPtr icdev, byte _Mode, byte _SecNr, System.IntPtr _NKey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_SecNr: unsigned char
        ///_NKey: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_load_key_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_load_key_hex(System.IntPtr icdev, byte _Mode, byte _SecNr, System.IntPtr _NKey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Snr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_card", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_card(System.IntPtr icdev, byte _Mode, ref uint _Snr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///snrstr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_card_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_card_hex(System.IntPtr icdev, byte _Mode, byte[] snrstr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_SecNr: unsigned char
        ///_KeyA: unsigned char*
        ///_B0: unsigned char
        ///_B1: unsigned char
        ///_B2: unsigned char
        ///_B3: unsigned char
        ///_Bk: unsigned char
        ///_KeyB: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_changeb3", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_changeb3(System.IntPtr icdev, byte _SecNr, System.IntPtr _KeyA, byte _B0, byte _B1, byte _B2, byte _B3, byte _Bk, System.IntPtr _KeyB);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_restore", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_restore(System.IntPtr icdev, byte _Adr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_transfer", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_transfer(System.IntPtr icdev, byte _Adr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Value: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_increment", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_increment(System.IntPtr icdev, byte _Adr, uint _Value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Value: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_decrement", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_decrement(System.IntPtr icdev, byte _Adr, uint _Value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Value: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_initval", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_initval(System.IntPtr icdev, byte _Adr, uint _Value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Value: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readval", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readval(System.IntPtr icdev, byte _Adr, ref uint _Value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Value: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_initval_ml", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_initval_ml(System.IntPtr icdev, ushort _Value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Value: unsigned short*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readval_ml", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readval_ml(System.IntPtr icdev, ref ushort _Value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Value: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_decrement_ml", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_decrement_ml(System.IntPtr icdev, ushort _Value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///KeyNr: unsigned char
        ///Adr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_authentication_2", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_authentication_2(System.IntPtr icdev, byte _Mode, byte KeyNr, byte Adr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Bcnt: unsigned char
        ///_Snr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_anticoll2", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_anticoll2(System.IntPtr icdev, byte _Bcnt, ref uint _Snr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Snr: unsigned int
        ///_Size: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_select2", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_select2(System.IntPtr icdev, uint _Snr, System.IntPtr _Size);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Adr: unsigned char
        ///_Snr: unsigned int*
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_HL_write", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_HL_write(System.IntPtr icdev, byte _Mode, byte _Adr, ref uint _Snr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Adr: unsigned char
        ///_Snr: unsigned int*
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_HL_writehex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_HL_writehex(System.IntPtr icdev, byte _Mode, byte _Adr, ref uint _Snr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Adr: unsigned char
        ///_Snr: unsigned int
        ///_Data: unsigned char*
        ///_NSnr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_HL_read", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_HL_read(System.IntPtr icdev, byte _Mode, byte _Adr, uint _Snr, System.IntPtr _Data, ref uint _NSnr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Adr: unsigned char
        ///_Snr: unsigned int
        ///_Data: unsigned char*
        ///_NSnr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_HL_readhex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_HL_readhex(System.IntPtr icdev, byte _Mode, byte _Adr, uint _Snr, System.IntPtr _Data, ref uint _NSnr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///reqmode: unsigned char
        ///snr: unsigned int
        ///authmode: unsigned char
        ///secnr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_HL_authentication", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_HL_authentication(System.IntPtr icdev, byte reqmode, uint snr, byte authmode, byte secnr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///Snr: unsigned int
        ///authmode: unsigned char
        ///Adr: unsigned char
        ///_data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_check_write", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_check_write(System.IntPtr icdev, uint Snr, byte authmode, byte Adr, System.IntPtr _data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///Snr: unsigned int
        ///authmode: unsigned char
        ///Adr: unsigned char
        ///_data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_check_writehex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_check_writehex(System.IntPtr icdev, uint Snr, byte authmode, byte Adr, System.IntPtr _data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///sver: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_getver", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_getver(System.IntPtr icdev, byte[] sver);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_b: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_clr_control_bit", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_clr_control_bit(System.IntPtr icdev, byte _b);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_b: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_set_control_bit", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_set_control_bit(System.IntPtr icdev, byte _b);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Msec: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_reset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_reset(System.IntPtr icdev, ushort _Msec);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Msec: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_beep", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_beep(System.IntPtr icdev, ushort _Msec);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///dispstr: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_disp_str", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_disp_str(System.IntPtr icdev, System.IntPtr dispstr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///rec_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_srd_eeprom", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_srd_eeprom(System.IntPtr icdev, short offset, short lenth, System.IntPtr rec_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///send_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_swr_eeprom", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_swr_eeprom(System.IntPtr icdev, short offset, short lenth, System.IntPtr send_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///snd_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "swr_alleeprom", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short swr_alleeprom(System.IntPtr icdev, short offset, short lenth, System.IntPtr snd_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///receive_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "srd_alleeprom", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short srd_alleeprom(System.IntPtr icdev, short offset, short lenth, System.IntPtr receive_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///rec_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_srd_eepromhex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_srd_eepromhex(System.IntPtr icdev, short offset, short lenth, System.IntPtr rec_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///send_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_swr_eepromhex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_swr_eepromhex(System.IntPtr icdev, short offset, short lenth, System.IntPtr send_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///time: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_gettime", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_gettime(System.IntPtr icdev, System.IntPtr time);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///time: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_gettimehex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_gettimehex(System.IntPtr icdev, System.IntPtr time);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///time: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_settime", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_settime(System.IntPtr icdev, System.IntPtr time);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///time: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_settimehex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_settimehex(System.IntPtr icdev, System.IntPtr time);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///bright: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_setbright", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_setbright(System.IntPtr icdev, byte bright);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///mode: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_ctl_mode", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_ctl_mode(System.IntPtr icdev, byte mode);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///mode: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_disp_mode", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_disp_mode(System.IntPtr icdev, byte mode);


        /// Return Type: short
        ///key: unsigned char*
        ///sour: unsigned char*
        ///dest: unsigned char*
        ///m: short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dcdeshex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dcdeshex(System.IntPtr key, System.IntPtr sour, System.IntPtr dest, short m);


        /// Return Type: short
        ///key: unsigned char*
        ///sour: unsigned char*
        ///dest: unsigned char*
        ///m: short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dcdes", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dcdes(System.IntPtr key, System.IntPtr sour, System.IntPtr dest, short m);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_OnOff: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_light", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_light(System.IntPtr icdev, ushort _OnOff);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: unsigned char
        ///displen: unsigned char
        ///dispstr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_high_disp", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_high_disp(System.IntPtr icdev, byte offset, byte displen, System.IntPtr dispstr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Byte: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_setcpu", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_setcpu(System.IntPtr icdev, byte _Byte);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_cpureset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_cpureset(System.IntPtr icdev, ref byte rlen, byte[] databuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_cpuapdusource", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_cpuapdusource(System.IntPtr icdev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_cpuapdu", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_cpuapdu(System.IntPtr icdev, byte slen, byte[] sendbuffer, ref byte rlen, byte[] databuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///rlen: unsigned char*
        ///databuffer: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_cpureset_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_cpureset_hex(System.IntPtr icdev, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: char*
        ///rlen: unsigned char*
        ///databuffer: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_cpuapdusource_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_cpuapdusource_hex(System.IntPtr icdev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: char*
        ///rlen: unsigned char*
        ///databuffer: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_cpuapdu_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_cpuapdu_hex(System.IntPtr icdev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_cpuapdurespon", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_cpuapdurespon(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_cpuapdurespon_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_cpuapdurespon_hex(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_cpudown", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_cpudown(System.IntPtr icdev);


        /// Return Type: short
        ///saddr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_set_addr", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_set_addr(byte saddr);


        /// Return Type: HANDLE->void*
        ///port: short
        ///baud: int
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_init_485", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr dc_init_485(short port, int baud);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///baud: int
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_changebaud_485", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_changebaud_485(System.IntPtr icdev, int baud);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///saddr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_change_addr", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_change_addr(System.IntPtr icdev, byte saddr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///rlen: unsigned char*
        ///receive_data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_reset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_reset(System.IntPtr icdev, System.IntPtr rlen, System.IntPtr receive_data);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///timeout: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_command", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_command(System.IntPtr idComDev, byte slen, byte[] sendbuffer, ref byte rlen, byte[] databuffer, byte timeout);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///rlen: unsigned char*
        ///receive_data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_resethex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_resethex(System.IntPtr icdev, System.IntPtr rlen, System.IntPtr receive_data);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: char*
        ///rlen: unsigned char*
        ///databuffer: char*
        ///timeout: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_commandhex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_commandhex(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte timeout);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///timeout: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_commandsource", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_commandsource(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte timeout);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: char*
        ///rlen: unsigned char*
        ///databuffer: char*
        ///timeout: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_commandsourcehex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_commandsourcehex(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte timeout);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_halt", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_halt(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///TagType: unsigned short*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_request_shc1102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_request_shc1102(System.IntPtr icdev, byte _Mode, ref ushort TagType);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_auth_shc1102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_auth_shc1102(System.IntPtr icdev, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_shc1102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_shc1102(System.IntPtr icdev, byte _Adr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_shc1102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_shc1102(System.IntPtr icdev, byte _Adr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_halt_shc1102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_halt_shc1102(System.IntPtr icdev);


        /// Return Type: short
        ///hex: unsigned char*
        ///a: unsigned char*
        ///length: short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "hex_a", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short hex_a(System.IntPtr hex, System.IntPtr a, short length);


        /// Return Type: short
        ///a: unsigned char*
        ///hex: unsigned char*
        ///len: short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "a_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short a_hex(System.IntPtr a, System.IntPtr hex, short len);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///cardtype: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_config_card", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_config_card(System.IntPtr icdev, byte cardtype);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///AFI: unsigned char
        ///N: unsigned char
        ///ATQB: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_request_b", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_request_b(System.IntPtr icdev, byte _Mode, byte AFI, byte N, System.IntPtr ATQB);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///N: unsigned char
        ///ATQB: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_slotmarker", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_slotmarker(System.IntPtr icdev, byte N, System.IntPtr ATQB);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///PUPI: unsigned char*
        ///CID: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_attrib", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_attrib(System.IntPtr icdev, System.IntPtr PUPI, byte CID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///cflag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_open_door", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_open_door(System.IntPtr icdev, byte cflag);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///utime: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_open_timedoor", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_open_timedoor(System.IntPtr icdev, ushort utime);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_random", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_random(System.IntPtr icdev, System.IntPtr data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///len: short
        ///data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_random", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_random(System.IntPtr icdev, short len, System.IntPtr data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_random_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_random_hex(System.IntPtr icdev, System.IntPtr data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///len: short
        ///data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_random_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_random_hex(System.IntPtr icdev, short len, System.IntPtr data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///len: short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_erase_random", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_erase_random(System.IntPtr icdev, short len);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///keyno: unsigned char
        ///keylen: unsigned char
        ///authkey: unsigned char*
        ///randAdata: unsigned char*
        ///randBdata: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_mfdes_auth", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_mfdes_auth(System.IntPtr icdev, byte keyno, byte keylen, System.IntPtr authkey, System.IntPtr randAdata, System.IntPtr randBdata);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Addr: unsigned char
        ///passbuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_authentication_pass", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_authentication_pass(System.IntPtr icdev, byte _Mode, byte _Addr, System.IntPtr passbuff);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///dispstr: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_disp_neg", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_disp_neg(System.IntPtr icdev, System.IntPtr dispstr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///timeout: unsigned char
        ///FG: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_commandlink", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_commandlink(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte timeout, byte FG);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///timeout: unsigned char
        ///FG: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_commandlink_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_commandlink_hex(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte timeout, byte FG);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Snr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_card_double", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_card_double(System.IntPtr icdev, byte _Mode, byte[] _Snr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Snr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_card_double_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_card_double_hex(System.IntPtr icdev, byte _Mode, System.IntPtr _Snr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///times: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_idcard", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_idcard(System.IntPtr icdev, byte times, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///times: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_idcard_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_idcard_hex(System.IntPtr icdev, byte times, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Addr: unsigned char
        ///passbuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_authentication_pass_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_authentication_pass_hex(System.IntPtr icdev, byte _Mode, byte _Addr, System.IntPtr passbuff);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///cputype: unsigned char
        ///cpupro: unsigned char
        ///cpuetu: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_setcpupara", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_setcpupara(System.IntPtr icdev, byte cputype, byte cpupro, byte cpuetu);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///cmd: unsigned char
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_command", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_command(System.IntPtr idComDev, byte cmd, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///cmd: unsigned char
        ///slen: unsigned char
        ///sendbuffer: char*
        ///rlen: unsigned char*
        ///databuffer: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_command_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_command_hex(System.IntPtr idComDev, byte cmd, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///KeyLen: unsigned char
        ///Key: unsigned char*
        ///DataLen: unsigned short
        ///Data: unsigned char*
        ///InitData: unsigned char*
        ///AutoFixFlag: unsigned char
        ///FixChar: unsigned char
        ///MacData: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_creat_mac", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_creat_mac(byte KeyLen, System.IntPtr Key, ushort DataLen, System.IntPtr Data, System.IntPtr InitData, byte AutoFixFlag, byte FixChar, System.IntPtr MacData);


        /// Return Type: short
        ///KeyLen: unsigned char
        ///Key: unsigned char*
        ///DataLen: unsigned short
        ///Data: unsigned char*
        ///InitData: unsigned char*
        ///AutoFixFlag: unsigned char
        ///FixChar: unsigned char
        ///MacData: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_creat_mac_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_creat_mac_hex(byte KeyLen, System.IntPtr Key, ushort DataLen, System.IntPtr Data, System.IntPtr InitData, byte AutoFixFlag, byte FixChar, System.IntPtr MacData);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Adr: unsigned char
        ///_Snr: unsigned int*
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_HL_write_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_HL_write_hex(System.IntPtr icdev, byte _Mode, byte _Adr, ref uint _Snr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Adr: unsigned char
        ///_Snr: unsigned int
        ///_Data: unsigned char*
        ///_NSnr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_HL_read_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_HL_read_hex(System.IntPtr icdev, byte _Mode, byte _Adr, uint _Snr, System.IntPtr _Data, ref uint _NSnr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///Snr: unsigned int
        ///authmode: unsigned char
        ///Adr: unsigned char
        ///_data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_check_write_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_check_write_hex(System.IntPtr icdev, uint Snr, byte authmode, byte Adr, System.IntPtr _data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///rec_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_srd_eeprom_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_srd_eeprom_hex(System.IntPtr icdev, short offset, short lenth, System.IntPtr rec_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///send_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_swr_eeprom_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_swr_eeprom_hex(System.IntPtr icdev, short offset, short lenth, System.IntPtr send_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///time: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_gettime_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_gettime_hex(System.IntPtr icdev, System.IntPtr time);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///time: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_settime_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_settime_hex(System.IntPtr icdev, System.IntPtr time);


        /// Return Type: short
        ///key: unsigned char*
        ///sour: unsigned char*
        ///dest: unsigned char*
        ///m: short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_des_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_des_hex(System.IntPtr key, System.IntPtr sour, System.IntPtr dest, short m);


        /// Return Type: short
        ///key: unsigned char*
        ///sour: unsigned char*
        ///dest: unsigned char*
        ///m: short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_des", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_des(System.IntPtr key, System.IntPtr sour, System.IntPtr dest, short m);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///rlen: unsigned char*
        ///receive_data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_reset_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_reset_hex(System.IntPtr icdev, System.IntPtr rlen, System.IntPtr receive_data);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: char*
        ///rlen: unsigned char*
        ///databuffer: char*
        ///timeout: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_command_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_command_hex(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte timeout);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: char*
        ///rlen: unsigned char*
        ///databuffer: char*
        ///timeout: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_commandsource_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_commandsource_hex(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte timeout);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///baud: int
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_switch_unix", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_switch_unix(System.IntPtr icdev, int baud);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Addr: unsigned char
        ///passbuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_authentication_passaddr", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_authentication_passaddr(System.IntPtr icdev, byte _Mode, byte _Addr, System.IntPtr passbuff);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Addr: unsigned char
        ///passbuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_authentication_passaddr_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_authentication_passaddr_hex(System.IntPtr icdev, byte _Mode, byte _Addr, System.IntPtr passbuff);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Snr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_card_fm11rf005", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_card_fm11rf005(System.IntPtr icdev, byte _Mode, ref uint _Snr);


        /// Return Type: short
        ///ntimes: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_setusbtimeout", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_setusbtimeout(byte ntimes);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///para: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_mfdes_baud", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_mfdes_baud(System.IntPtr icdev, byte _Mode, byte para);


        /// Return Type: short
        ///key: unsigned char*
        ///src: unsigned char*
        ///dest: unsigned char*
        ///m: short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_tripledes", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_tripledes(System.IntPtr key, System.IntPtr src, System.IntPtr dest, short m);


        /// Return Type: short
        ///key: unsigned char*
        ///src: unsigned char*
        ///dest: unsigned char*
        ///m: short
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_tripledes_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_tripledes_hex(System.IntPtr key, System.IntPtr src, System.IntPtr dest, short m);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///keyno: unsigned char
        ///keylen: unsigned char
        ///authkey: unsigned char*
        ///randAdata: unsigned char*
        ///randBdata: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_mfdes_auth_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_mfdes_auth_hex(System.IntPtr icdev, byte keyno, byte keylen, System.IntPtr authkey, System.IntPtr randAdata, System.IntPtr randBdata);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///timeout: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_sendcommandsource", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_sendcommandsource(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, byte timeout);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_pro_receivecommandsource", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_pro_receivecommandsource(System.IntPtr idComDev, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///AFI: unsigned char
        ///masklen: unsigned char
        ///rlen: unsigned char*
        ///rbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_inventory", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_inventory(System.IntPtr icdev, byte flags, byte AFI, byte masklen, System.IntPtr rlen, System.IntPtr rbuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///AFI: unsigned char
        ///masklen: unsigned char
        ///rlen: unsigned char*
        ///rbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_inventory_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_inventory_hex(System.IntPtr icdev, byte flags, byte AFI, byte masklen, System.IntPtr rlen, System.IntPtr rbuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_stay_quiet", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_stay_quiet(System.IntPtr icdev, byte flags, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_stay_quiet_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_stay_quiet_hex(System.IntPtr icdev, byte flags, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_select_uid", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_select_uid(System.IntPtr icdev, byte flags, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_select_uid_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_select_uid_hex(System.IntPtr icdev, byte flags, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_reset_to_ready", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_reset_to_ready(System.IntPtr icdev, byte flags, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_reset_to_ready_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_reset_to_ready_hex(System.IntPtr icdev, byte flags, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///startblock: unsigned char
        ///blocknum: unsigned char
        ///UID: unsigned char*
        ///rlen: unsigned char*
        ///rbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readblock", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readblock(System.IntPtr icdev, byte flags, byte startblock, byte blocknum, System.IntPtr UID, System.IntPtr rlen, System.IntPtr rbuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///startblock: unsigned char
        ///blocknum: unsigned char
        ///UID: unsigned char*
        ///rlen: unsigned char*
        ///rbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readblock_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readblock_hex(System.IntPtr icdev, byte flags, byte startblock, byte blocknum, System.IntPtr UID, System.IntPtr rlen, System.IntPtr rbuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///startblock: unsigned char
        ///blocknum: unsigned char
        ///UID: unsigned char*
        ///wlen: unsigned char
        ///rbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_writeblock", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_writeblock(System.IntPtr icdev, byte flags, byte startblock, byte blocknum, System.IntPtr UID, byte wlen, System.IntPtr rbuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///startblock: unsigned char
        ///blocknum: unsigned char
        ///UID: unsigned char*
        ///wlen: unsigned char
        ///rbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_writeblock_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_writeblock_hex(System.IntPtr icdev, byte flags, byte startblock, byte blocknum, System.IntPtr UID, byte wlen, System.IntPtr rbuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///block: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_lock_block", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_lock_block(System.IntPtr icdev, byte flags, byte block, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///block: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_lock_block_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_lock_block_hex(System.IntPtr icdev, byte flags, byte block, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///AFI: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_afi", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_afi(System.IntPtr icdev, byte flags, byte AFI, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///AFI: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_afi_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_afi_hex(System.IntPtr icdev, byte flags, byte AFI, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///AFI: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_lock_afi", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_lock_afi(System.IntPtr icdev, byte flags, byte AFI, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///AFI: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_lock_afi_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_lock_afi_hex(System.IntPtr icdev, byte flags, byte AFI, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///DSFID: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_dsfid", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_dsfid(System.IntPtr icdev, byte flags, byte DSFID, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///DSFID: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_dsfid_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_dsfid_hex(System.IntPtr icdev, byte flags, byte DSFID, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///DSFID: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_lock_dsfid", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_lock_dsfid(System.IntPtr icdev, byte flags, byte DSFID, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///DSFID: unsigned char
        ///UID: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_lock_dsfid_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_lock_dsfid_hex(System.IntPtr icdev, byte flags, byte DSFID, System.IntPtr UID);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///UID: unsigned char*
        ///rlen: unsigned char*
        ///rbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_get_systeminfo", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_get_systeminfo(System.IntPtr icdev, byte flags, System.IntPtr UID, System.IntPtr rlen, System.IntPtr rbuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///UID: unsigned char*
        ///rlen: unsigned char*
        ///rbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_get_systeminfo_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_get_systeminfo_hex(System.IntPtr icdev, byte flags, System.IntPtr UID, System.IntPtr rlen, System.IntPtr rbuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///startblock: unsigned char
        ///blocknum: unsigned char
        ///UID: unsigned char*
        ///rlen: unsigned char*
        ///rbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_get_securityinfo", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_get_securityinfo(System.IntPtr icdev, byte flags, byte startblock, byte blocknum, System.IntPtr UID, System.IntPtr rlen, System.IntPtr rbuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///flags: unsigned char
        ///startblock: unsigned char
        ///blocknum: unsigned char
        ///UID: unsigned char*
        ///rlen: unsigned char*
        ///rbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_get_securityinfo_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_get_securityinfo_hex(System.IntPtr icdev, byte flags, byte startblock, byte blocknum, System.IntPtr UID, System.IntPtr rlen, System.IntPtr rbuffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Snr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_getsnr_fm11rf005", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_getsnr_fm11rf005(System.IntPtr icdev, ref uint _Snr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///snrstr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_getsnr_fm11rf005_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_getsnr_fm11rf005_hex(System.IntPtr icdev, System.IntPtr snrstr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_fm11rf005", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_fm11rf005(System.IntPtr icdev, byte _Adr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_fm11rf005", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_fm11rf005(System.IntPtr icdev, byte _Adr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_fm11rf005_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_fm11rf005_hex(System.IntPtr icdev, byte _Adr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_fm11rf005_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_fm11rf005_hex(System.IntPtr icdev, byte _Adr, System.IntPtr _Data);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctimeout: unsigned char
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "DCDEV_CommandMcu", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short DCDEV_CommandMcu(System.IntPtr idComDev, byte ctimeout, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctimeout: unsigned char
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "DCDEV_CommandMcu_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short DCDEV_CommandMcu_Hex(System.IntPtr idComDev, byte ctimeout, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///flag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_displcd", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_displcd(System.IntPtr idComDev, byte flag);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctime: unsigned char
        ///rlen: unsigned char*
        ///cpass: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_getinputpass", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_getinputpass(System.IntPtr idComDev, byte ctime, System.IntPtr rlen, System.IntPtr cpass);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctime: unsigned char
        ///pTrack2Data: unsigned char*
        ///pTrack2Len: unsigned int*
        ///pTrack3Data: unsigned char*
        ///pTrack3Len: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readmagcard", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readmagcard(System.IntPtr idComDev, byte ctime, System.IntPtr pTrack2Data, ref uint pTrack2Len, System.IntPtr pTrack3Data, ref uint pTrack3Len);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_testdevicecomm", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_testdevicecomm(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_dispmainmenu", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_dispmainmenu(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///year: unsigned char
        ///month: unsigned char
        ///date: unsigned char
        ///hour: unsigned char
        ///minute: unsigned char
        ///second: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_setdevicetime", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_setdevicetime(System.IntPtr idComDev, byte year, byte month, byte date, byte hour, byte minute, byte second);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///year: unsigned char*
        ///month: unsigned char*
        ///date: unsigned char*
        ///hour: unsigned char*
        ///minute: unsigned char*
        ///second: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_getdevicetime", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_getdevicetime(System.IntPtr idComDev, System.IntPtr year, System.IntPtr month, System.IntPtr date, System.IntPtr hour, System.IntPtr minute, System.IntPtr second);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///line: unsigned char
        ///offset: unsigned char
        ///data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_dispinfo", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_dispinfo(System.IntPtr idComDev, byte line, byte offset, System.IntPtr data);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: unsigned char
        ///data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_dispmaininfo", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_dispmaininfo(System.IntPtr idComDev, byte offset, System.IntPtr data);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///beeptime: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_posbeep", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_posbeep(System.IntPtr idComDev, byte beeptime);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///cOpenFlag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_ctlbacklight", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_ctlbacklight(System.IntPtr idComDev, byte cOpenFlag);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///cLed: unsigned char
        ///cOpenFlag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_ctlled", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_ctlled(System.IntPtr idComDev, byte cLed, byte cOpenFlag);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///cLine: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_lcdclrscrn", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_lcdclrscrn(System.IntPtr idComDev, byte cLine);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctime: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_passin", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_passin(System.IntPtr idComDev, byte ctime);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///rlen: unsigned char*
        ///cpass: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_passget", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_passget(System.IntPtr idComDev, System.IntPtr rlen, System.IntPtr cpass);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_passcancel", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_passcancel(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///disptype: unsigned char
        ///line: unsigned char
        ///ctime: unsigned char
        ///rlen: unsigned char*
        ///ckeydata: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_getinputkey", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_getinputkey(System.IntPtr idComDev, byte disptype, byte line, byte ctime, System.IntPtr rlen, System.IntPtr ckeydata);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///flag: unsigned char
        ///row: unsigned char
        ///offset: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_displcd_ext", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_displcd_ext(System.IntPtr idComDev, byte flag, byte row, byte offset);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctime: unsigned char
        ///pTrack1Data: unsigned char*
        ///pTrack1Len: unsigned int*
        ///pTrack2Data: unsigned char*
        ///pTrack2Len: unsigned int*
        ///pTrack3Data: unsigned char*
        ///pTrack3Len: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readmagcardall", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readmagcardall(System.IntPtr idComDev, byte ctime, System.IntPtr pTrack1Data, ref uint pTrack1Len, System.IntPtr pTrack2Data, ref uint pTrack2Len, System.IntPtr pTrack3Data, ref uint pTrack3Len);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///snr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readdevsnr", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readdevsnr(System.IntPtr idComDev, System.IntPtr snr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///snr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readreadersnr", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readreadersnr(System.IntPtr idComDev, System.IntPtr snr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_resetdevice", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_resetdevice(System.IntPtr idComDev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_4442(System.IntPtr icdev, short offset, short lenth, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_4442_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_4442_hex(System.IntPtr icdev, short offset, short lenth, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_4442(System.IntPtr icdev, short offset, short lenth, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_4442_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_4442_hex(System.IntPtr icdev, short offset, short lenth, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_verifypin_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_verifypin_4442(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_verifypin_4442_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_verifypin_4442_hex(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readpin_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readpin_4442(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readpin_4442_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readpin_4442_hex(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readpincount_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readpincount_4442(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_changepin_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_changepin_4442(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_changepin_4442_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_changepin_4442_hex(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///leng: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readwrotect_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readwrotect_4442(System.IntPtr icdev, short offset, short leng, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///leng: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readwrotect_4442_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readwrotect_4442_hex(System.IntPtr icdev, short offset, short leng, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///leng: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_writeprotect_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_writeprotect_4442(System.IntPtr icdev, short offset, short leng, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///leng: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_writeprotect_4442_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_writeprotect_4442_hex(System.IntPtr icdev, short offset, short leng, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///snd_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_24c", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_24c(System.IntPtr icdev, short offset, short lenth, System.IntPtr snd_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///snd_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_24c_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_24c_hex(System.IntPtr icdev, short offset, short lenth, System.IntPtr snd_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///snd_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_24c64", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_24c64(System.IntPtr icdev, short offset, short lenth, System.IntPtr snd_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///snd_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_24c64_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_24c64_hex(System.IntPtr icdev, short offset, short lenth, System.IntPtr snd_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///receive_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_24c", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_24c(System.IntPtr icdev, short offset, short lenth, System.IntPtr receive_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///receive_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_24c_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_24c_hex(System.IntPtr icdev, short offset, short lenth, System.IntPtr receive_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///receive_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_24c64", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_24c64(System.IntPtr icdev, short offset, short lenth, System.IntPtr receive_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///receive_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_24c64_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_24c64_hex(System.IntPtr icdev, short offset, short lenth, System.IntPtr receive_buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_4428(System.IntPtr icdev, short offset, short lenth, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_read_4428_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_read_4428_hex(System.IntPtr icdev, short offset, short lenth, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_4428(System.IntPtr icdev, short offset, short lenth, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///offset: short
        ///lenth: short
        ///buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_write_4428_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_write_4428_hex(System.IntPtr icdev, short offset, short lenth, System.IntPtr buffer);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_verifypin_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_verifypin_4428(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_verifypin_4428_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_verifypin_4428_hex(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readpin_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readpin_4428(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readpin_4428_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readpin_4428_hex(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_readpincount_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_readpincount_4428(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_changepin_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_changepin_4428(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///passwd: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_changepin_4428_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_changepin_4428_hex(System.IntPtr icdev, System.IntPtr passwd);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_Check_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_Check_4442(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_Check_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_Check_4428(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_Check_24C01", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_Check_24C01(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_Check_24C02", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_Check_24C02(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_Check_24C04", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_Check_24C04(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_Check_24C08", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_Check_24C08(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_Check_24C16", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_Check_24C16(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_Check_24C64", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_Check_24C64(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_Check_CPU", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_Check_CPU(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_CheckCard", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_CheckCard(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///info: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_getrcinfo", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_getrcinfo(System.IntPtr icdev, System.IntPtr info);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///info: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_getrcinfo_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_getrcinfo_hex(System.IntPtr icdev, System.IntPtr info);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///sver: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_getlongver", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_getlongver(System.IntPtr icdev, System.IntPtr sver);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///Strsnr: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_cardstr", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_cardstr(System.IntPtr icdev, byte _Mode, System.IntPtr Strsnr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///rlen: unsigned char*
        ///rbuf: unsigned char*
        ///type: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_cardAB", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_cardAB(System.IntPtr icdev, out byte rlen, byte[] rbuf, out byte type);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///rbuf: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_card_b", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_card_b(System.IntPtr icdev, System.IntPtr rbuf);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///rbuf: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_card_b_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_card_b_hex(System.IntPtr icdev, System.IntPtr rbuf);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///line: unsigned char
        ///offset: unsigned char
        ///data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_dispinfo_T8", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_dispinfo_T8(System.IntPtr idComDev, byte line, byte offset, System.IntPtr data);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: unsigned char
        ///data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_dispinfo_pro_T8", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_dispinfo_pro_T8(System.IntPtr idComDev, byte offset, System.IntPtr data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///line: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_clearlcd_T8", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_clearlcd_T8(System.IntPtr icdev, byte line);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///cled: unsigned char
        ///cflag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_led_T8", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_led_T8(System.IntPtr icdev, byte cled, byte cflag);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///BNr: unsigned int
        ///dataperso: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL0_writeperso", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL0_writeperso(System.IntPtr icdev, uint BNr, System.IntPtr dataperso);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///BNr: unsigned int
        ///dataperso: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL0_writeperso_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL0_writeperso_hex(System.IntPtr icdev, uint BNr, System.IntPtr dataperso);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL0_commitperso", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL0_commitperso(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///authkey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL1_authl1key", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL1_authl1key(System.IntPtr icdev, System.IntPtr authkey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///authkey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL1_authl1key_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL1_authl1key_hex(System.IntPtr icdev, System.IntPtr authkey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///authkey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL1_switchtol2", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL1_switchtol2(System.IntPtr icdev, System.IntPtr authkey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///authkey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL1_switchtol3", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL1_switchtol3(System.IntPtr icdev, System.IntPtr authkey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///authkey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL2_switchtol3", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL2_switchtol3(System.IntPtr icdev, System.IntPtr authkey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///keyBNr: unsigned int
        ///authkey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_authl3key", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_authl3key(System.IntPtr icdev, uint keyBNr, System.IntPtr authkey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///keyBNr: unsigned int
        ///authkey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_authl3key_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_authl3key_hex(System.IntPtr icdev, uint keyBNr, System.IntPtr authkey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///mode: unsigned char
        ///sectorBNr: unsigned int
        ///authkey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_authl3sectorkey", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_authl3sectorkey(System.IntPtr icdev, byte mode, uint sectorBNr, System.IntPtr authkey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///mode: unsigned char
        ///sectorBNr: unsigned int
        ///authkey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_authl3sectorkey_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_authl3sectorkey_hex(System.IntPtr icdev, byte mode, uint sectorBNr, System.IntPtr authkey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///BNr: unsigned int
        ///Numblock: unsigned char
        ///readdata: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_readinplain", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_readinplain(System.IntPtr icdev, uint BNr, byte Numblock, System.IntPtr readdata);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///BNr: unsigned int
        ///Numblock: unsigned char
        ///readdata: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_readinplain_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_readinplain_hex(System.IntPtr icdev, uint BNr, byte Numblock, System.IntPtr readdata);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///BNr: unsigned int
        ///Numblock: unsigned char
        ///readdata: unsigned char*
        ///flag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_readencrypted", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_readencrypted(System.IntPtr icdev, uint BNr, byte Numblock, System.IntPtr readdata, byte flag);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///BNr: unsigned int
        ///Numblock: unsigned char
        ///readdata: unsigned char*
        ///flag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_readencrypted_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_readencrypted_hex(System.IntPtr icdev, uint BNr, byte Numblock, System.IntPtr readdata, byte flag);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///BNr: unsigned int
        ///Numblock: unsigned char
        ///writedata: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_writeinplain", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_writeinplain(System.IntPtr icdev, uint BNr, byte Numblock, System.IntPtr writedata);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///BNr: unsigned int
        ///Numblock: unsigned char
        ///writedata: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_writeinplain_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_writeinplain_hex(System.IntPtr icdev, uint BNr, byte Numblock, System.IntPtr writedata);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///BNr: unsigned int
        ///Numblock: unsigned char
        ///writedata: unsigned char*
        ///flag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_writeencrypted", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_writeencrypted(System.IntPtr icdev, uint BNr, byte Numblock, System.IntPtr writedata, byte flag);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///BNr: unsigned int
        ///Numblock: unsigned char
        ///writedata: unsigned char*
        ///flag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_MFPL3_writeencrypted_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_MFPL3_writeencrypted_hex(System.IntPtr icdev, uint BNr, byte Numblock, System.IntPtr writedata, byte flag);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///key: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_auth_ulc", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_auth_ulc(System.IntPtr icdev, System.IntPtr key);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///key: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_auth_ulc_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_auth_ulc_hex(System.IntPtr icdev, System.IntPtr key);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///newkey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_changekey_ulc", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_changekey_ulc(System.IntPtr icdev, System.IntPtr newkey);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///newkey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcrf32.dll", EntryPoint = "dc_changekey_ulc_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short dc_changekey_ulc_hex(System.IntPtr icdev, System.IntPtr newkey);

    }
}