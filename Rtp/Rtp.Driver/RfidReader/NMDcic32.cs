/*
 * 深圳德卡T6等接触式IC卡通用操作函数库。
 * 
 * 2011-1-28
 * 
 * 
 * **/
namespace Rtp.Driver.RfidReader
{

    internal sealed class NMDcic32
    {
        public const short TypeNo_4406 = 0x02;
        public const short TypeNo_4418_4428 = 0x04;
        public const short TypeNo_4404 = 0x08;
        /// <summary>
        ///  CPU卡(主卡装)
        /// </summary>
        public const short TypeNo_SAM0 = 0x0C;
        /// <summary>
        ///  SAM卡(附卡座上)
        /// </summary>
        public const short TypeNo_SAM_ATT = 0x0D;
        /// <summary>
        /// SAM卡(SAM1卡座上)
        /// </summary>
        public const short TypeNo_SAM1 = 0x0E;
        /// <summary>
        ///  SAM卡(SAM2卡座上)
        /// </summary>
        public const short TypeNo_SAM2 = 0x0F;
        public const short TypeNo_4432_4442 = 0x10;
        public const short TypeNo_101_102_103 = 0x20;
        public const short TypeNo_24C01A_02_04_08_16 = 0x40;
        public const short TypeNo_45DB041 = 0x41;
        public const short TypeNo_SF1101 = 0x42;
        public const short TypeNo_1604_1604B = 0xA0;
        public const short TypeNo_24C64 = 0xC0;
        public const short TypeNo_1608 = 0xD0;
        public const short TypeNo_153 = 0xD1;

        /// Return Type: short
        ///key: char*
        ///ptrSource: char*
        ///msgLen: unsigned short
        ///ptrDest: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Encrypt", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Encrypt(System.IntPtr key, System.IntPtr ptrSource, ushort msgLen, System.IntPtr ptrDest);


        /// Return Type: short
        ///key: char*
        ///ptrSource: char*
        ///msgLen: unsigned short
        ///ptrDest: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Decrypt", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Decrypt(System.IntPtr key, System.IntPtr ptrSource, ushort msgLen, System.IntPtr ptrDest);


        /// Return Type: short
        ///key: char*
        ///ptrSource: char*
        ///msgLen: unsigned short
        ///ptrDest: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Encrypt_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Encrypt_Hex(System.IntPtr key, System.IntPtr ptrSource, ushort msgLen, System.IntPtr ptrDest);


        /// Return Type: short
        ///key: char*
        ///ptrSource: char*
        ///msgLen: unsigned short
        ///ptrDest: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Decrypt_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Decrypt_Hex(System.IntPtr key, System.IntPtr ptrSource, ushort msgLen, System.IntPtr ptrDest);


        /// Return Type: HANDLE->void*
        ///port: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_InitComm", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr IC_InitComm(short port);


        /// Return Type: HANDLE->void*
        ///port: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_InitCommAdvanced", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr IC_InitCommAdvanced(short port);


        /// Return Type: HANDLE->void*
        ///port: short
        ///combaud: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_InitComm_Baud", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr IC_InitComm_Baud(short port, uint combaud);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ExitComm", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ExitComm(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Status", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Status(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Down", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Down(System.IntPtr idComDev);


        /// Return Type: short
        ///lTime_ms: DWORD->unsigned int
        ///sTime_ms: DWORD->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_SetCommTimeout", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_SetCommTimeout(uint lTime_ms, uint sTime_ms);


        /// Return Type: short
        ///ntimes: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_SetUsbTimeout", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_SetUsbTimeout(byte ntimes);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Ver: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadVer", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadVer(System.IntPtr idComDev,byte[] Ver);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///beeptime: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_DevBeep", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_DevBeep(System.IntPtr idComDev, byte beeptime);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///delaytime: unsigned char
        ///beeptime: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Beep", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Beep(System.IntPtr idComDev, byte delaytime, byte beeptime);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadDevice", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadDevice(System.IntPtr idComDev, short offset, short len, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteDevice", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteDevice(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadDevice_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadDevice_Hex(System.IntPtr idComDev, short offset, short len, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteDevice_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteDevice_Hex(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: short
        ///sendbuffer: unsigned char*
        ///rlen: short*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuApduSourceEXT", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuApduSourceEXT(System.IntPtr idComDev, short slen, System.IntPtr sendbuffer, ref short rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: short
        ///sendbuffer: unsigned char*
        ///rlen: short*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuApduSourceEXT_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuApduSourceEXT_Hex(System.IntPtr idComDev, short slen, System.IntPtr sendbuffer, ref short rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: short
        ///sendbuffer: unsigned char*
        ///rlen: short*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuApduEXT", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuApduEXT(System.IntPtr idComDev, short slen, System.IntPtr sendbuffer, ref short rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: short
        ///sendbuffer: unsigned char*
        ///rlen: short*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuApduEXT_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuApduEXT_Hex(System.IntPtr idComDev, short slen, System.IntPtr sendbuffer, ref short rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuReset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuReset(System.IntPtr idComDev, ref byte rlen, byte[] rbuff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuApdu", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuApdu(System.IntPtr idComDev, byte slen, byte[] sendbuffer, ref byte rlen, byte[] databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuApduSource", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuApduSource(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuReset_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuReset_Hex(System.IntPtr idComDev, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuApdu_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuApdu_Hex(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuApduSource_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuApduSource_Hex(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuGetProtocol", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuGetProtocol(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuApduRespon", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuApduRespon(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///cardp: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuSetProtocol", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuSetProtocol(System.IntPtr idComDev, byte cardp);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Pushout", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Pushout(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///type: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_InitType", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_InitType(System.IntPtr idComDev, short type);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Read", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Read(System.IntPtr idComDev, short offset, short len, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///fdata: float*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Read_Float", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Read_Float(System.IntPtr idComDev, short offset, ref float fdata);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///fdata: int*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Read_Int", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Read_Int(System.IntPtr idComDev, short offset, ref int fdata);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///fdata: int
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write_Int", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write_Int(System.IntPtr idComDev, short offset, int fdata);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadProtection", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadProtection(System.IntPtr idComDev, short offset, short len, System.IntPtr protbuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadProtection_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadProtection_Hex(System.IntPtr idComDev, short offset, short len, System.IntPtr protbuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadWithProtection", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadWithProtection(System.IntPtr idComDev, short offset, short len, System.IntPtr protbuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadWithProtection_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadWithProtection_Hex(System.IntPtr idComDev, short offset, short len, System.IntPtr protbuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///fdata: float
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write_Float", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write_Float(System.IntPtr idComDev, short offset, float fdata);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteProtection", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteProtection(System.IntPtr idComDev, short offset, short len, System.IntPtr protbuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteWithProtection", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteWithProtection(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///protbuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteProtection_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteProtection_Hex(System.IntPtr idComDev, short offset, short len, System.IntPtr protbuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteWithProtection_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteWithProtection_Hex(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadPass_SLE4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadPass_SLE4442(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadCount_SLE4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadCount_SLE4442(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_SLE4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_SLE4442(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_SLE4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_SLE4442(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_4442hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_4442hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_4442hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_4442hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadPass_4442hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadPass_4442hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadCount_SLE4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadCount_SLE4428(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_SLE4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_SLE4428(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_SLE4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_SLE4428(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_4428hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_4428hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_4428hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_4428hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Erase", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Erase(System.IntPtr idComDev, short offset, short len);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Erase_102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Erase_102(System.IntPtr idComDev, short offset, short len);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Fuse_102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Fuse_102(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadCount_102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadCount_102(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_102(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_102(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckAZPass_102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckAZPass_102(System.IntPtr idComDev, short zone, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangeAZPass_102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangeAZPass_102(System.IntPtr idComDev, short zone, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_102hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_102hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_102hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_102hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckAZPass_102hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckAZPass_102hex(System.IntPtr idComDev, short zone, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangeAZPass_102hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangeAZPass_102hex(System.IntPtr idComDev, short zone, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Fuse_1604", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Fuse_1604(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///area: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadCount_1604", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadCount_1604(System.IntPtr idComDev, short area);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///area: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_1604", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_1604(System.IntPtr idComDev, short area, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///area: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_1604", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_1604(System.IntPtr idComDev, short area, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///area: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_1604hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_1604hex(System.IntPtr idComDev, short area, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///area: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_1604hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_1604hex(System.IntPtr idComDev, short area, System.IntPtr password);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Fuse_4404", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Fuse_4404(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadCount_4404", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadCount_4404(System.IntPtr icdev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_4404", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_4404(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_4404", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_4404(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckAZPass_4404", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckAZPass_4404(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangeAZPass_4404", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangeAZPass_4404(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_4404hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_4404hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_4404hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_4404hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckAZPass_4404hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckAZPass_4404hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangeAZPass_4404hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangeAZPass_4404hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_RValue", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_RValue(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///num: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_DEValue", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_DEValue(System.IntPtr idComDev, short num);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///value: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Fuse_4406", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Fuse_4406(System.IntPtr icdev, short value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadCount_4406", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadCount_4406(System.IntPtr icdev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_4406", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_4406(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Erase_4406", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Erase_4406(System.IntPtr idComDev, byte offset);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_4406hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_4406hex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_4406userhex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_4406userhex(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_4406user", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_4406user(System.IntPtr idComDev, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write24", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write24(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write64", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write64(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write_Hex(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Read_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Read_Hex(System.IntPtr idComDev, short offset, short len, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write24_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write24_Hex(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write64_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write64_Hex(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///page: short
        ///offset: short
        ///bytes: short
        ///buff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_DirectRead", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_DirectRead(System.IntPtr idComDev, short page, short offset, short bytes, System.IntPtr buff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///page: short
        ///offset: short
        ///bytes: short
        ///buff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_DirectWrite", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_DirectWrite(System.IntPtr idComDev, short page, short offset, short bytes, System.IntPtr buff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///page: short
        ///offset: short
        ///bytes: short
        ///buff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_DirectRead_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_DirectRead_Hex(System.IntPtr idComDev, short page, short offset, short bytes, System.IntPtr buff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///page: short
        ///offset: short
        ///bytes: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_DirectWrite_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_DirectWrite_Hex(System.IntPtr idComDev, short page, short offset, short bytes, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Fuse_1604B", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Fuse_1604B(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///area: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadCount_1604B", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadCount_1604B(System.IntPtr idComDev, short area);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///area: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_1604B", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_1604B(System.IntPtr idComDev, short area, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///area: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_1604B", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_1604B(System.IntPtr idComDev, short area, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///area: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangePass_1604Bhex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangePass_1604Bhex(System.IntPtr idComDev, short area, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///area: short
        ///password: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckPass_1604Bhex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckPass_1604Bhex(System.IntPtr idComDev, short area, System.IntPtr password);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///page: short
        ///offset: short
        ///bytes: short
        ///buff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Read_1101", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Read_1101(System.IntPtr idComDev, short page, short offset, short bytes, System.IntPtr buff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///page: short
        ///offset: short
        ///bytes: short
        ///buff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write_1101", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write_1101(System.IntPtr idComDev, short page, short offset, short bytes, System.IntPtr buff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///page: short
        ///offset: short
        ///bytes: short
        ///buff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Read_1101hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Read_1101hex(System.IntPtr idComDev, short page, short offset, short bytes, System.IntPtr buff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///page: short
        ///offset: short
        ///bytes: short
        ///buff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write_1101hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write_1101hex(System.IntPtr idComDev, short page, short offset, short bytes, System.IntPtr buff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Q0: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_InitAuth_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_InitAuth_Hex(System.IntPtr idComDev, System.IntPtr Q0);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Q1: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckAuth_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckAuth_Hex(System.IntPtr idComDev, System.IntPtr Q1);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Q0: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_InitAuth", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_InitAuth(System.IntPtr idComDev, System.IntPtr Q0);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Q1: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckAuth", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckAuth(System.IntPtr idComDev, System.IntPtr Q1);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: unsigned char
        ///Pin: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckRPassword_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckRPassword_Hex(System.IntPtr idComDev, byte zone, System.IntPtr Pin);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: unsigned char
        ///Pin: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckWPassword_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckWPassword_Hex(System.IntPtr idComDev, byte zone, System.IntPtr Pin);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: unsigned char
        ///Pin: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckRPassword", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckRPassword(System.IntPtr idComDev, byte zone, System.IntPtr Pin);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: unsigned char
        ///Pin: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckWPassword", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckWPassword(System.IntPtr idComDev, byte zone, System.IntPtr Pin);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: unsigned char
        ///Pin: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangeRPassword", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangeRPassword(System.IntPtr idComDev, byte zone, System.IntPtr Pin);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: unsigned char
        ///Pin: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangeWPassword", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangeWPassword(System.IntPtr idComDev, byte zone, System.IntPtr Pin);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: unsigned char
        ///Pin: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangeRPassword_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangeRPassword_Hex(System.IntPtr idComDev, byte zone, System.IntPtr Pin);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: unsigned char
        ///Pin: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ChangeWPassword_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ChangeWPassword_Hex(System.IntPtr idComDev, byte zone, System.IntPtr Pin);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///fusetype: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteFuse", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteFuse(System.IntPtr idComDev, byte fusetype);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadFuse", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadFuse(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///WDataBuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteConfigZone", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteConfigZone(System.IntPtr idComDev, short Offset, short len, System.IntPtr WDataBuff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///WDataBuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteUserZone", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteUserZone(System.IntPtr idComDev, short Offset, short len, System.IntPtr WDataBuff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///RDataBuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadConfigZone", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadConfigZone(System.IntPtr idComDev, short Offset, short len, System.IntPtr RDataBuff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///RDataBuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadUserZone", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadUserZone(System.IntPtr idComDev, short Offset, short len, System.IntPtr RDataBuff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadWPasswordCount", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadWPasswordCount(System.IntPtr idComDev, byte zone);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///zone: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadRPasswordCount", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadRPasswordCount(System.IntPtr idComDev, byte zone);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadAuthCount", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadAuthCount(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///WDataBuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteConfigZone_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteConfigZone_Hex(System.IntPtr idComDev, short Offset, short len, System.IntPtr WDataBuff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///WDataBuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteUserZone_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteUserZone_Hex(System.IntPtr idComDev, short Offset, short len, System.IntPtr WDataBuff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///RDataBuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadConfigZone_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadConfigZone_Hex(System.IntPtr idComDev, short Offset, short len, System.IntPtr RDataBuff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///RDataBuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadUserZone_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadUserZone_Hex(System.IntPtr idComDev, short Offset, short len, System.IntPtr RDataBuff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Zone: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_SetUserZone", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_SetUserZone(System.IntPtr idComDev, byte Zone);


        /// Return Type: short
        ///Ci: unsigned char*
        ///Gc: unsigned char*
        ///Q0: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "SetInit", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short SetInit(System.IntPtr Ci, System.IntPtr Gc, System.IntPtr Q0);


        /// Return Type: short
        ///Q1: unsigned char*
        ///Q2: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "Authenticate", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short Authenticate(System.IntPtr Q1, System.IntPtr Q2);


        /// Return Type: short
        ///Ci: unsigned char*
        ///Gc: unsigned char*
        ///Q0: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "SetInit_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short SetInit_Hex(System.IntPtr Ci, System.IntPtr Gc, System.IntPtr Q0);


        /// Return Type: short
        ///Q1: unsigned char*
        ///Q2: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "Authenticate_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short Authenticate_Hex(System.IntPtr Q1, System.IntPtr Q2);


        /// Return Type: short
        ///strhex: unsigned char*
        ///strasc: unsigned char*
        ///length: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "hex2asc", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short hex2asc(System.IntPtr strhex, System.IntPtr strasc, short length);


        /// Return Type: short
        ///strasc: unsigned char*
        ///strhex: unsigned char*
        ///length: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "asc2hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short asc2hex(System.IntPtr strasc, System.IntPtr strhex, short length);


        /// Return Type: unsigned short
        ///idComDev: HANDLE->void*
        ///kid: unsigned char
        ///randifd: unsigned char*
        ///retlen: unsigned char
        ///encrand: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "ICC_Internal_Auth", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern ushort ICC_Internal_Auth(System.IntPtr idComDev, byte kid, System.IntPtr randifd, byte retlen, System.IntPtr encrand);


        /// Return Type: unsigned short
        ///idComDev: HANDLE->void*
        ///kid: unsigned char
        ///encrand: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "ICC_External_Auth", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern ushort ICC_External_Auth(System.IntPtr idComDev, byte kid, System.IntPtr encrand);


        /// Return Type: unsigned short
        ///idComDev: HANDLE->void*
        ///kid: unsigned char
        ///pin_len: unsigned char
        ///pin: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "ICC_Verify", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern ushort ICC_Verify(System.IntPtr idComDev, byte kid, byte pin_len, System.IntPtr pin);


        /// Return Type: unsigned short
        ///idComDev: HANDLE->void*
        ///offset: unsigned short
        ///len: unsigned short
        ///data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "ICC_Write_Bin", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern ushort ICC_Write_Bin(System.IntPtr idComDev, ushort offset, ushort len, System.IntPtr data);


        /// Return Type: unsigned short
        ///idComDev: HANDLE->void*
        ///offset: unsigned short
        ///len: unsigned short
        ///resp: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "ICC_Read_Bin", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern ushort ICC_Read_Bin(System.IntPtr idComDev, ushort offset, ushort len, System.IntPtr resp);


        /// Return Type: unsigned short
        ///idComDev: HANDLE->void*
        ///sflag: unsigned char
        ///fid: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "ICC_Select_File", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern ushort ICC_Select_File(System.IntPtr idComDev, byte sflag, ushort fid);


        /// Return Type: unsigned short
        ///idComDev: HANDLE->void*
        ///len: unsigned char
        ///rand: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "ICC_Get_Challenge", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern ushort ICC_Get_Challenge(System.IntPtr idComDev, byte len, System.IntPtr rand);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///snrdata: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadUsbSnr", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadUsbSnr(System.IntPtr idComDev, System.IntPtr snrdata);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctype: unsigned char
        ///delaytime: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Control", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Control(System.IntPtr idComDev, byte ctype, byte delaytime);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CheckCard", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CheckCard(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_4442", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_4442(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_4428", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_4428(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_102(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_1604", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_1604(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_1604B", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_1604B(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_24C01", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_24C01(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_24C02", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_24C02(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_24C04", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_24C04(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_24C08", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_24C08(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_24C16", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_24C16(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_24C64", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_24C64(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_45DB041", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_45DB041(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_1101", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_1101(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_CPU", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_CPU(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_153", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_153(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_1608", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_1608(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_4404", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_4404(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Check_4406", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Check_4406(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write_102", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write_102(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write_102hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write_102hex(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write_1604", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write_1604(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: short
        ///len: short
        ///writebuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Write_1604hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Write_1604hex(System.IntPtr idComDev, short offset, short len, System.IntPtr writebuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///cputype: unsigned char
        ///cpupro: unsigned char
        ///cpuetu: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_SetCpuPara", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_SetCpuPara(System.IntPtr idComDev, byte cputype, byte cpupro, byte cpuetu);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuColdReset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuColdReset(System.IntPtr idComDev, ref byte rlen, byte[] databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuColdReset_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuColdReset_Hex(System.IntPtr idComDev, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuHotReset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuHotReset(System.IntPtr idComDev, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CpuHotReset_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CpuHotReset_Hex(System.IntPtr idComDev, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///flag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_SwitchPcsc", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_SwitchPcsc(System.IntPtr idComDev, byte flag);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctimeout: unsigned char
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "DEV_CommandMcu", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short DEV_CommandMcu(System.IntPtr idComDev, byte ctimeout, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctimeout: unsigned char
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "DEV_CommandMcu_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short DEV_CommandMcu_Hex(System.IntPtr idComDev, byte ctimeout, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///flag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_DispLcd", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_DispLcd(System.IntPtr idComDev, byte flag);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctime: unsigned char
        ///rlen: unsigned char*
        ///cpass: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_GetInputPass", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_GetInputPass(System.IntPtr idComDev, byte ctime, System.IntPtr rlen, System.IntPtr cpass);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctime: unsigned char
        ///pTrack2Data: unsigned char*
        ///pTrack2Len: unsigned int*
        ///pTrack3Data: unsigned char*
        ///pTrack3Len: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadMagCard", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadMagCard(System.IntPtr idComDev, byte ctime, System.IntPtr pTrack2Data, ref uint pTrack2Len, System.IntPtr pTrack3Data, ref uint pTrack3Len);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctime: unsigned char
        ///pTrack1Data: unsigned char*
        ///pTrack1Len: unsigned int*
        ///pTrack2Data: unsigned char*
        ///pTrack2Len: unsigned int*
        ///pTrack3Data: unsigned char*
        ///pTrack3Len: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadMagCardAll", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadMagCardAll(System.IntPtr idComDev, byte ctime, System.IntPtr pTrack1Data, ref uint pTrack1Len, System.IntPtr pTrack2Data, ref uint pTrack2Len, System.IntPtr pTrack3Data, ref uint pTrack3Len);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_TestDeviceComm", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_TestDeviceComm(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_DispMainMenu", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_DispMainMenu(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///year: unsigned char
        ///month: unsigned char
        ///date: unsigned char
        ///hour: unsigned char
        ///minute: unsigned char
        ///second: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_SetDeviceTime", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_SetDeviceTime(System.IntPtr idComDev, byte year, byte month, byte date, byte hour, byte minute, byte second);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///year: unsigned char*
        ///month: unsigned char*
        ///date: unsigned char*
        ///hour: unsigned char*
        ///minute: unsigned char*
        ///second: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_GetDeviceTime", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_GetDeviceTime(System.IntPtr idComDev, System.IntPtr year, System.IntPtr month, System.IntPtr date, System.IntPtr hour, System.IntPtr minute, System.IntPtr second);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///line: unsigned char
        ///offset: unsigned char
        ///data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_DispInfo", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_DispInfo(System.IntPtr idComDev, byte line, byte offset, System.IntPtr data);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///offset: unsigned char
        ///data: char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_DispMainInfo", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_DispMainInfo(System.IntPtr idComDev, byte offset, System.IntPtr data);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///beeptime: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_PosBeep", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_PosBeep(System.IntPtr idComDev, byte beeptime);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///writebuffer: unsigned char*
        ///pass: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteDeviceEn", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteDeviceEn(System.IntPtr idComDev, short Offset, short len, System.IntPtr writebuffer, System.IntPtr pass);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///readbuffer: unsigned char*
        ///pass: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadDeviceEn", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadDeviceEn(System.IntPtr idComDev, short Offset, short len, System.IntPtr readbuffer, System.IntPtr pass);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///writebuffer: unsigned char*
        ///pass: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteDeviceEn_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteDeviceEn_Hex(System.IntPtr idComDev, short Offset, short len, System.IntPtr writebuffer, System.IntPtr pass);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Offset: short
        ///len: short
        ///readbuffer: unsigned char*
        ///pass: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadDeviceEn_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadDeviceEn_Hex(System.IntPtr idComDev, short Offset, short len, System.IntPtr readbuffer, System.IntPtr pass);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///Controlp: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "DEV_SetControl", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short DEV_SetControl(System.IntPtr idComDev, byte Controlp);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///snr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadDevSnr", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadDevSnr(System.IntPtr idComDev, System.IntPtr snr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///snr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadDevSnr_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadDevSnr_Hex(System.IntPtr idComDev, System.IntPtr snr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///cOpenFlag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CtlBackLight", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CtlBackLight(System.IntPtr idComDev, byte cOpenFlag);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///cLed: unsigned char
        ///cOpenFlag: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_CtlLed", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_CtlLed(System.IntPtr idComDev, byte cLed, byte cOpenFlag);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///cLine: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_LcdClrScrn", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_LcdClrScrn(System.IntPtr idComDev, byte cLine);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///year: unsigned char
        ///month: unsigned char
        ///date: unsigned char
        ///hour: unsigned char
        ///minute: unsigned char
        ///second: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_SetReaderTime", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_SetReaderTime(System.IntPtr idComDev, byte year, byte month, byte date, byte hour, byte minute, byte second);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///year: unsigned char*
        ///month: unsigned char*
        ///date: unsigned char*
        ///hour: unsigned char*
        ///minute: unsigned char*
        ///second: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_GetReaderTime", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_GetReaderTime(System.IntPtr idComDev, System.IntPtr year, System.IntPtr month, System.IntPtr date, System.IntPtr hour, System.IntPtr minute, System.IntPtr second);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///ctime: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_PassIn", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_PassIn(System.IntPtr idComDev, byte ctime);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///rlen: unsigned char*
        ///cpass: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_PassGet", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_PassGet(System.IntPtr idComDev, System.IntPtr rlen, System.IntPtr cpass);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_PassCancel", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_PassCancel(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///disptype: unsigned char
        ///line: unsigned char
        ///ctime: unsigned char
        ///rlen: unsigned char*
        ///ckeydata: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_GetInputKey", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_GetInputKey(System.IntPtr idComDev, byte disptype, byte line, byte ctime, System.IntPtr rlen, System.IntPtr ckeydata);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bMode: BYTE->unsigned char
        ///_wTagType: short*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Request", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Request(System.IntPtr idComDev, byte _bMode, ref short _wTagType);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bBcnt: BYTE->unsigned char
        ///_dwSnr: DWORD*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Anticoll", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Anticoll(System.IntPtr idComDev, byte _bBcnt, ref uint _dwSnr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_dwSnr: DWORD->unsigned int
        ///_bSize: BYTE*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Select", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Select(System.IntPtr idComDev, uint _dwSnr, ref byte _bSize);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bMode: BYTE->unsigned char
        ///_bSecNr: BYTE->unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Authentication", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Authentication(System.IntPtr idComDev, byte _bMode, byte _bSecNr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Halt", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Halt(System.IntPtr idComDev);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bAdr: BYTE->unsigned char
        ///_bData: BYTE*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadMifare", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadMifare(System.IntPtr idComDev, byte _bAdr, ref byte _bData);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bAdr: BYTE->unsigned char
        ///_bData: BYTE*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadMifare_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadMifare_Hex(System.IntPtr idComDev, byte _bAdr, ref byte _bData);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bAdr: BYTE->unsigned char
        ///_bData: BYTE*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteMifare", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteMifare(System.IntPtr idComDev, byte _bAdr, ref byte _bData);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bAdr: BYTE->unsigned char
        ///_bData: BYTE*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_WriteMifare_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_WriteMifare_Hex(System.IntPtr idComDev, byte _bAdr, ref byte _bData);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bAdr: BYTE->unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Transfer", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Transfer(System.IntPtr idComDev, byte _bAdr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bAdr: BYTE->unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Restore", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Restore(System.IntPtr idComDev, byte _bAdr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bAdr: BYTE->unsigned char
        ///_dwValue: DWORD->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Increment", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Increment(System.IntPtr idComDev, byte _bAdr, uint _dwValue);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bAdr: BYTE->unsigned char
        ///_dwValue: DWORD->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Decrement", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Decrement(System.IntPtr idComDev, byte _bAdr, uint _dwValue);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bMode: BYTE->unsigned char
        ///_bSecNr: BYTE->unsigned char
        ///_bNKey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Load_Key", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Load_Key(System.IntPtr idComDev, byte _bMode, byte _bSecNr, System.IntPtr _bNKey);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bMode: BYTE->unsigned char
        ///_bSecNr: BYTE->unsigned char
        ///_bNKey: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Load_Keyhex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Load_Keyhex(System.IntPtr idComDev, byte _bMode, byte _bSecNr, System.IntPtr _bNKey);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_wMsec: short
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ResetMifare", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ResetMifare(System.IntPtr idComDev, short _wMsec);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bAdr: BYTE->unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Init_Value", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Init_Value(System.IntPtr idComDev, byte _bAdr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bAdr: BYTE->unsigned char
        ///_lValue: int*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Read_Value", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Read_Value(System.IntPtr idComDev, byte _bAdr, ref int _lValue);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Snr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Card", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Card(System.IntPtr icdev, byte _Mode, ref uint _Snr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///snrstr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Card_Hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Card_Hex(System.IntPtr icdev, byte _Mode, System.IntPtr snrstr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Addr: unsigned char
        ///passbuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Authentication_Passaddr", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Authentication_Passaddr(System.IntPtr icdev, byte _Mode, byte _Addr, System.IntPtr passbuff);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Addr: unsigned char
        ///passbuff: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Authentication_Passaddrhex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Authentication_Passaddrhex(System.IntPtr icdev, byte _Mode, byte _Addr, System.IntPtr passbuff);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///snr: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ReadReaderSnr", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ReadReaderSnr(System.IntPtr idComDev, System.IntPtr snr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_ResetDevice", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_ResetDevice(System.IntPtr idComDev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///rlen: unsigned char*
        ///receive_data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Pro_Reset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Pro_Reset(System.IntPtr icdev, System.IntPtr rlen, System.IntPtr receive_data);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///slen: unsigned char
        ///sendbuffer: unsigned char*
        ///rlen: unsigned char*
        ///databuffer: unsigned char*
        ///timeout: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Pro_Commandsource", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Pro_Commandsource(System.IntPtr idComDev, byte slen, System.IntPtr sendbuffer, System.IntPtr rlen, System.IntPtr databuffer, byte timeout);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Config_Card", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Config_Card(System.IntPtr icdev, byte _Mode);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///AFI: unsigned char
        ///N: unsigned char
        ///ATQB: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Request_B", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Request_B(System.IntPtr icdev, byte _Mode, byte AFI, byte N, System.IntPtr ATQB);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///PUPI: unsigned char*
        ///CID: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Attrib", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Attrib(System.IntPtr icdev, System.IntPtr PUPI, byte CID);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_bBcnt: BYTE->unsigned char
        ///_dwSnr: DWORD*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Anticoll2", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Anticoll2(System.IntPtr idComDev, byte _bBcnt, ref uint _dwSnr);


        /// Return Type: short
        ///idComDev: HANDLE->void*
        ///_dwSnr: DWORD->unsigned int
        ///_bSize: BYTE*
        [System.Runtime.InteropServices.DllImportAttribute("dcic32.dll", EntryPoint = "IC_Select2", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short IC_Select2(System.IntPtr idComDev, uint _dwSnr, ref byte _bSize);

    }

}