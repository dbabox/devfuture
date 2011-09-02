using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rtp.Driver.RfidReader
{
    public class RfidT9:SerialComRfidBase
    {
        /// <summary>
        /// T9读卡器通信协议
        /// </summary>
        public class Protocol
        {
            public const int PROTOCOL_MAX_LEN = 0x0400;
            //以下都是协议区的成员
            public const byte HEAD_BYTE = 0x02;
            public const byte END_BYTE = 0x03;
            public UInt32 len;
            public UInt16 cmd;
            public readonly byte[] para = new byte[PROTOCOL_MAX_LEN];

            public int BytesToWrite
            {
                get { return (int)(7 + len); }
            }

            /// <summary>
            /// 将协议命令字转换到buff中，添加了head，校验，end；
            /// </summary>
            /// <param name="p"></param>
            /// <param name="wBuff"></param>
            public static void Protocol2WBuff(Protocol p, byte[] wBuff)
            {
                Array.Clear(wBuff, 0, wBuff.Length);
                wBuff[0] = HEAD_BYTE;
                wBuff[6 + p.len] = END_BYTE;
                Utility.ConvertUInt32ToByteArray(p.len, wBuff, 1);//从第二个字节开始写
                Utility.ConvertUInt16ToByteArray(p.cmd, wBuff, 5);
                //拷贝参数域
                System.Buffer.BlockCopy(p.para, 0, wBuff, 7, (int)(p.len - 2));
                //计算这个区域的checkSum
                wBuff[5 + p.len] = Utility.CheckSumXor(wBuff, (int)(5 + p.len));
                logger.InfoFormat("Protocol2WBuff:{0}", Utility.ByteArrayToHexStr(wBuff, p.BytesToWrite));
            }

            public static int RBuff2Protocol(byte[] rBuff, int recLen, Protocol p)
            {
                int rc = 0;
                if (rBuff != null && recLen >=9)
                {
                    if (rBuff[0] != HEAD_BYTE || rBuff[recLen-1]!=END_BYTE) 
                    {
                        logger.ErrorFormat("命令格式错误，Header={0},End={1},应为[0x02 .. .. 0x03]格式", rBuff[0], rBuff[recLen - 1]);
                        rc = -2; 
                    }
                    else
                    {
                        #region 解析响应
                        p.len =(UInt32)Utility.ConvertByteArrayToInt32(rBuff, 1);
                        if ((p.len + 7) != recLen)
                        {
                            logger.ErrorFormat("数据长度不匹配: {0}",Utility.ByteArrayToHexStr(rBuff,recLen));
                            rc = -3;
                        }
                        else
                        {
                            p.cmd = Utility.ConvertByteArrayToUInt16(rBuff, 5);
                            //参数块拷贝 //校验位 // 截止位
                            System.Buffer.BlockCopy(rBuff, 7, p.para, 0, (int)(p.len - 2));
                            rc = 0;
                            if (rBuff[p.len + 5] != Utility.CheckSumXor(rBuff, (int)(5 + p.len)))
                            {
                                logger.ErrorFormat("校验码错误:{0}", Utility.ByteArrayToHexStr(rBuff, recLen));
                                rc = -4;
                            }
                        }
                        #endregion
                    }
                   
                }
                else
                {
                    logger.Error("参数错误");
                    rc = -1;
                }
                return rc;
            }
        }
       
        
        //写缓冲区
        public readonly byte[] wBuff = new byte[0x0400+8];
        private int wLen = 0;

              
        /// <summary>
        /// 需要写入的长度
        /// </summary>
        public int WLen
        {
            get { return wLen; }            
        }
        

        private readonly Protocol _currentRequest=new Protocol();
        private readonly Protocol _currentResponse = new Protocol();


        private byte antennaIndex=0;
        public byte AntennaIndex
        {
            get { return antennaIndex; }
            set { antennaIndex = value; }
        }
            
         

        public override int DeviceReset()
        {
            int rc = 0;
            _currentRequest.len = 3;
            _currentRequest.cmd= 0x0400;
            _currentRequest.para[0] = antennaIndex;
            Protocol.Protocol2WBuff(_currentRequest, wBuff);           
            rc=Write(wBuff, wLen);
            if (rc==0 && responseEvent.WaitOne())
            {
                //收到响应数据了,请处理recBuff,recLen
                rc = Protocol.RBuff2Protocol(recBuff, recLen, _currentResponse);
                if (0 == rc)
                {
                    //解析响应成功
                    logger.Info("DeviceReset response ok.");
                }
            }
            return rc;
            
        }

        public override string Request(out CardPhysicalType cpt)
        {
            throw new NotImplementedException();
        }

        public override string DeviceVersion()
        {
            throw new NotImplementedException();
        }

        public override int CPU_Reset(out byte rlen, byte[] rbuff)
        {
            throw new NotImplementedException();
        }

        public override int CPU_APDU(byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff)
        {
            throw new NotImplementedException();
        }

        public override int SAM_Reset(byte slot, ref byte rlen, byte[] dataBuff)
        {
            throw new NotImplementedException();
        }

        public override int SAM_SetSlot(byte slot)
        {
            throw new NotImplementedException();
        }

        public override int SAM_APDU(byte slot, byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff)
        {
            throw new NotImplementedException();
        }

        public override int SAM_SetPara(byte slot, byte cpupro, byte cpuetu)
        {
            throw new NotImplementedException();
        }

        public override int UL_read(byte addr, byte[] rbuff)
        {
            throw new NotImplementedException();
        }

        public override int UL_write(byte addr, byte[] wbuff)
        {
            throw new NotImplementedException();
        }
    }
}
