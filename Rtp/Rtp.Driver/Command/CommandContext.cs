/*
 * 命令上下文。
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.CardIO;
using Rtp.Driver.RfidReader;

namespace Rtp.Driver.Command
{
    public class CommandContext
    {
        /// <summary>
        /// 默认命令上下文，必须绑定到一个rfid。
        /// </summary>
        public CommandContext(IRfid rfid_)
        {
            rfid = rfid_;
            gVDic = new Dictionary<string, string>();
            gVDic.Add("CARD_RAND_8BYTE", "00 00 00 00 00 00 00 00 ");
            gVDic.Add("CARD_RAND_4BYTE", "00 00 00 00 ");
            gVDic.Add("MAC_KEY", "FF FF FF FF FF FF FF FF ");//FM1208-43 JSB CPU卡的初始密钥为16个F。
            gVDic.Add("CPU_CCK", "FF FF FF FF FF FF FF FF ");//FM1208-43 JSB CPU卡的初始主控密钥为16个F。

            slen = 0;
            rlen = 0;
            sbuff = new byte[128];
            rbuff = new byte[128];

            //加载COS描述文件
            //cosIO = new FileMapCosIO().ReadCosFile("default.cos");
        }

        /// <summary>
        /// 组合构造函数。
        /// </summary>
        /// <param name="sbuff_"></param>
        /// <param name="rbuff_"></param>
        /// <param name="cosIO_"></param>
        /// <param name="gVDic_"></param>
        public CommandContext(byte[] sbuff_, byte[] rbuff_, Dictionary<string, string> gVDic_, IRfid rfid_)
        {
            sbuff = sbuff_;
            rbuff = rbuff_;           
            gVDic = gVDic_;
            rfid = rfid_;
        }


        #region 命令上下文成员

        public int rc;//代表上一次操作的返回码


        bool isMacOn = false; 

        public bool IsMacOn
        {
            get { return isMacOn; }
            set { isMacOn = value; }
        }

        public byte slen;        
        public byte[] sbuff;
        public byte rlen;       
        public byte[] rbuff;

       
 

        Dictionary<string, string> gVDic;

        public Dictionary<string, string> GVDIC
        {
            get { return gVDic; }
            set { gVDic = value; }
        }

        IRfid rfid;

        public IRfid Rfid
        {
            get { return rfid; }
            set { rfid = value; }
        }

        string cmdTarget="CPU";
        /// <summary>
        /// 命令目标,可以是CPU,SAM ##。默认CPU。如果命令不指定对象，则为CPU。
        /// </summary>
        public string CmdTarget
        {
            get { return cmdTarget; }
            set { cmdTarget = value; }
        }


        #endregion


        /// <summary>
        /// 给CPU卡/SAM卡指令加MAC。自动根据MAC_KEY的长度选择DES还是3DES计算。
        /// </summary>
        /// <param name="rf"></param>
        /// <returns></returns>
        public bool AppandMac()
        {
            int rc = 0;
            byte cla = sbuff[0];
            if ((cla & 0x0F) != 0x04)
            {
                Console.WriteLine("WARN:[AppandMac] 要加MAC的命令的CLA必须左半字节为0x04.");
                return true;
            }
            byte lc = sbuff[4];
            if (lc < 4) throw new ArgumentException(String.Format("{0}命令LC不正确,LC是数据域长度，其包含MAC长度，因此必须大于等于4.", Utility.ByteArrayToHexStr(sbuff, slen)));
            byte[] data = new byte[5 + lc - 4];
            Array.Copy(sbuff, data, data.Length);//待计算MAC的部分

            byte[] init = new byte[8];//8字节随机数

            if (CmdTarget == "CPU")
            {
                rc = rfid.CPU_GetChallenge(8, ref rlen, rbuff);
            }
            else //取SAM卡的随机数
            {
                rc = rfid.SAM_GetChallenge(rfid.CurrentSamSlot, 8, ref rlen, rbuff);
            }
            
            if (rc != 0)
            {
                Console.WriteLine("ERROR:GetChallenge RC={0}", rc);
                return false;
            }
            Array.Copy(rbuff, init, 8);
            byte[] MAC_KEY_BUF = new byte[16];
            int mac_key_len = Utility.HexStrToByteArray(GVDIC["MAC_KEY"], ref MAC_KEY_BUF);
            if (mac_key_len == 16)
            {
                //3DES计算MAC
                Utility.PBOC_GetKey16MAC(data, MAC_KEY_BUF, init, ref rbuff);//TODO:这里使用的是PC计算MAC，实际可以使用SAM卡计算MAC
            }
            else
            {
                //DES计算MAC
                byte[] MAC_KEY = new byte[8];
                Array.Copy(MAC_KEY_BUF, MAC_KEY, 8);
                Utility.PBOC_GetKey8MAC(data, MAC_KEY, init, ref rbuff);//TODO:这里使用的是PC计算MAC，实际可以使用SAM卡计算MAC
            }
            //判断sbuff中是否有le
            if ((5 + lc - 4) == slen)
            {
                //不包含le
                Array.Copy(rbuff, 0, sbuff, slen, 4);
            }
            else if ((5 + lc - 4 + 1) == slen)
            {
                //包含le
                sbuff[slen + 3] = sbuff[slen - 1];
                Array.Copy(rbuff, 0, sbuff, slen, 4);
            }
            else
            {
                throw new ArgumentException(String.Format("{0}命令格式不正确，请检查LC的值，LE是否存在。", Utility.ByteArrayToHexStr(sbuff, slen)));
            }
            slen += 4;//Note:修改了待发送命令的长度
            return true;
        }

      

    }
}
