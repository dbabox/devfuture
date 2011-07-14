/*
 * M1卡读写操作.
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.CardIO
{
    public interface IM1IO
    {
        /// <summary>
        /// 读取M1卡的1个BLOCK,即读取16字节数据；
        /// 成功则返回 0;
        /// </summary>
        /// <param name="addr">起始地址</param>
        /// <param name="rbuff">读取结果缓冲区</param>
        /// <returns></returns>
        int readM1Block(byte addr, byte[] rbuff);

        /// <summary>
        /// 写M1卡的1个BLOCK,即写16字节数据；
        /// 成功则返回 0;
        /// </summary>
        /// <param name="addr">起始地址</param>
        /// <param name="wbuff">写入内存缓冲区</param>
        /// <returns></returns>
        int writeM1Block(byte addr, byte[] wbuff);
    }
}
