/*
 * CPU卡操作接口定义。
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.CardIO
{
    #region CPU卡IO事件参数

    /// <summary>
    /// CPU卡请求
    /// </summary>
    public class CpuRequestEventArgs : EventArgs
    {
        private string _stringValue;
        private UInt16 cmd;
        private string cmdstr;

        public string Cmdstr
        {
            get { return cmdstr; }
            set { cmdstr = value; }
        }

        public UInt16 Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }
      
        public CpuRequestEventArgs(UInt16 cmd_, string cmdstr_)
        {
            cmd = cmd_;
          
            cmdstr = cmdstr_;
            _stringValue = String.Format("{0,4:X4}:{1}", cmd_, cmdstr_);
        }

        public override string ToString()
        {
            return _stringValue;
        }
    }

    /// <summary>
    /// CPU卡响应
    /// </summary>
    public class CpuResponseEventArgs : EventArgs
    {
        private string _stringValue;
        private UInt16 sw;

        public UInt16 Sw
        {
            get { return sw; }
            set { sw = value; }
        }
        
        public CpuResponseEventArgs(UInt16 sw_, string responseStr_)
        {
            sw = sw_;
          
            responseString = responseStr_;
            _stringValue = String.Format("{0,4:X4}:{1}", sw, responseStr_);
        }

        public override string ToString()
        {
            return _stringValue;
        }

        private string responseString;

        public string ResponseString
        {
            get { return responseString; }
            set { responseString = value; }
        }
    }


    #endregion

    public interface ICpuIO
    {

        int CPU_GetChallenge(byte le, ref byte rlen, byte[] rbuff);
        

        #region 非接触式CPU卡操作函数
        /// <summary>
        /// CPU卡寻卡复位函数。返回复位信息和卡类型，type='A'表示A卡，'B'表示B卡。
        /// </summary>
        /// <param name="rlen"></param>
        /// <param name="rbuff"></param>
        /// <param name="type">type='A'表示A卡，'B'表示B卡。</param>
        /// <returns></returns>
        int CPU_Reset(out byte rlen, byte[] rbuff);

        /// <summary>
        /// 执行CPU卡COS命令。成功返回0，失败返回COS错误代码（正整数）。
        /// </summary>
        /// <param name="slen">发送数据长度</param>
        /// <param name="sbuff">发送数据缓冲区</param>
        /// <param name="rlen">返回长度</param>
        /// <param name="rbuff">返回数据缓冲区</param>       
        /// <returns></returns>
        int CPU_APDU(byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff);

        #endregion

    }
}
