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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyIdx">装入密码模式(索引)
        /// 对D8而言，对于M1卡的每个扇区，在读写器中均对应有三套密码（KEYSET0、KEYSET1、KEYSET2），每套密码包括A密码（KEYA）和B密码（KEYB），共六个密码，用0～2、4～6来表示这六个密码：
        /// 0——KEYSET0的KEYA`
        /// 1——KEYSET1的KEYA
        /// 2——KEYSET2的KEYA
        /// 4——KEYSET0的KEYB
        /// 5——KEYSET1的KEYB
        /// 6——KEYSET2的KEYB
        /// </param>
        /// <param name="secNr">扇区号（M1卡：0～15；  ML卡：0）</param>
        /// <param name="nKey">写入读写器中的卡密码</param>
        /// <returns></returns>
        int loadKeyM1(byte keyIdx,byte secNr,byte[] nKey);

        /// <summary>
        /// 验证M1卡，执行前需将密码加载到读卡器内部.对M1卡，addr为Block地址；
        /// </summary>
        /// <returns></returns>
        int authenticationM1(byte keyIdx, byte addr);
        
        /// <summary>
        /// 验证M1卡，用此函数时，可以不用执行dc_load_key()函数；
        /// </summary>
        /// <param name="keyIdx"></param>
        /// <param name="blockAddr"></param>
        /// <param name="nKey"></param>
        /// <returns></returns>
        int authenticationPassAddrM1(byte keyIdx, byte blockAddr,byte[] nKey);
       
        /// <summary>
        /// 初始化块值
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        int initvalM1(byte addr,ulong val);

        /// <summary>
        /// 块加值
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        int incrementM1(byte addr,ulong val);
        /// <summary>
        /// 块减值
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        int decrementM1(byte addr, ulong val);
        /// <summary>
        /// 读块值
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        int readvalM1(byte addr,ref ulong val);


    }
}
