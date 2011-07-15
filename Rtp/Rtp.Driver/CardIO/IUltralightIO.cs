/*
 * MIFARE Ultralight卡读写操作接口。
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.CardIO
{
    public interface IUltralightIO
    {   
        /// <summary>
        /// 读取Ultralight卡的1个Page,即4字节；
        /// 成功则返回 0
        /// </summary>
        /// <param name="addr">起始地址</param>
        /// <param name="rbuff">读取结果缓冲区</param>
        /// <returns></returns>
        int UL_read(byte addr,byte[] rbuff);

        /// <summary>
        /// 写Ultralight卡的1个Page，即写4字节数据；
        /// </summary>
        /// <param name="addr">起始地址</param>
        /// <param name="wbuff">写入内存缓冲区</param>
        /// <returns></returns>
        int UL_write(byte addr, byte[] wbuff);
    }
}
