/*
 * ÉîÛÚÃ÷»ªRsim-361 ¶ÁÐ´Æ÷
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.RfidReader
{

    internal sealed class NMRsimM361
    {

        /// Return Type: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "open_device", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern System.IntPtr open_device();


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "close_device", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short close_device(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///len: short*
        ///receive_data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "sam_reset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short sam_reset(System.IntPtr icdev, ref short len, byte[] receive_data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///sLen: short
        ///send_cmd: unsigned char*
        ///rlen: unsigned short*
        ///receive_data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "sam_protocol", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short sam_protocol(System.IntPtr icdev, short sLen, byte[] send_cmd, ref ushort rlen, byte[] receive_data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_open_reader", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_open_reader(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_close_reader", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_close_reader(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Msec: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_beep", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_beep(System.IntPtr icdev, ushort _Msec);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///status: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_ledctrol", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_ledctrol(System.IntPtr icdev, byte status);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_Snr: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_card", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_card(System.IntPtr icdev, byte _Mode, ref uint _Snr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Mode: unsigned char
        ///_BlockNr: unsigned char
        ///_Key: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_authentication_key", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_authentication_key(System.IntPtr icdev, byte _Mode, byte _BlockNr, System.IntPtr _Key);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_halt", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_halt(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_read", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_read(System.IntPtr icdev, byte _Adr, byte[] _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_read_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_read_hex(System.IntPtr icdev, byte _Adr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_write", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_write(System.IntPtr icdev, byte _Adr,byte[] _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Data: char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_write_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_write_hex(System.IntPtr icdev, byte _Adr, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Value: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_increment", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_increment(System.IntPtr icdev, byte _Adr, uint _Value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Value: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_decrement", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_decrement(System.IntPtr icdev, byte _Adr, uint _Value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_restore", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_restore(System.IntPtr icdev, byte _Adr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_transfer", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_transfer(System.IntPtr icdev, byte _Adr);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Value: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_initval", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_initval(System.IntPtr icdev, byte _Adr, uint _Value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Adr: unsigned char
        ///_Value: unsigned int*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_readval", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_readval(System.IntPtr icdev, byte _Adr, ref uint _Value);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Msec: unsigned short
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_reset", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_reset(System.IntPtr icdev, ushort _Msec);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Status: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_get_status", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_get_status(System.IntPtr icdev, System.IntPtr _Status);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Snr: unsigned int*
        ///datalen: unsigned int*
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "open_card", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short open_card(System.IntPtr icdev, ref uint _Snr, ref uint datalen, byte[] _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///slen: short
        ///datasend: unsigned char*
        ///rlen: unsigned short*
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_cpu_trn", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_cpu_trn(System.IntPtr icdev, short slen, byte[] datasend, ref ushort rlen, byte[] _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_sim_connect", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_sim_connect(System.IntPtr icdev, System.IntPtr _Data);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_sim_disconnect", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_sim_disconnect(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_sim_check", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_sim_check(System.IntPtr icdev);


        /// Return Type: short
        ///icdev: HANDLE->void*
        ///slen: short
        ///datasend: unsigned char*
        ///rlen: unsigned short*
        ///_Data: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_sim_trn", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_sim_trn(System.IntPtr icdev, short slen, System.IntPtr datasend, ref ushort rlen, System.IntPtr _Data);


        /// Return Type: short
        ///key: unsigned char*
        ///ptrSource: unsigned char*
        ///msgLen: unsigned short
        ///ptrDest: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_encrypt", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_encrypt(System.IntPtr key, System.IntPtr ptrSource, ushort msgLen, System.IntPtr ptrDest);


        /// Return Type: short
        ///key: unsigned char*
        ///ptrSource: unsigned char*
        ///msgLen: unsigned short
        ///ptrDest: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "rf_decrypt", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short rf_decrypt(System.IntPtr key, System.IntPtr ptrSource, ushort msgLen, System.IntPtr ptrDest);


        /// Return Type: unsigned char
        ///len: short
        ///bcc_buffer: unsigned char*
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "cr_bcc", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern byte cr_bcc(short len, System.IntPtr bcc_buffer);


        /// Return Type: short
        ///hex: unsigned char*
        ///a: char*
        ///length: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "hex_a", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short hex_a(System.IntPtr hex, System.IntPtr a, uint length);


        /// Return Type: short
        ///a: char*
        ///hex: unsigned char*
        ///len: unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("Rfsim.dll", EntryPoint = "a_hex", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern short a_hex(System.IntPtr a, System.IntPtr hex, uint len);

    }

}
