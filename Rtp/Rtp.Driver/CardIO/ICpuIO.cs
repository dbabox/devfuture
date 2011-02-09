/*
 * CPU�������ӿڶ��塣
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.CardIO
{
    #region CPU��IO�¼�����

    /// <summary>
    /// CPU������
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
    /// CPU����Ӧ
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
        

        #region �ǽӴ�ʽCPU����������
        /// <summary>
        /// CPU��Ѱ����λ���������ظ�λ��Ϣ�Ϳ����ͣ�type='A'��ʾA����'B'��ʾB����
        /// </summary>
        /// <param name="rlen"></param>
        /// <param name="rbuff"></param>
        /// <param name="type">type='A'��ʾA����'B'��ʾB����</param>
        /// <returns></returns>
        int CPU_Reset(out byte rlen, byte[] rbuff);

        /// <summary>
        /// ִ��CPU��COS����ɹ�����0��ʧ�ܷ���COS������루����������
        /// </summary>
        /// <param name="slen">�������ݳ���</param>
        /// <param name="sbuff">�������ݻ�����</param>
        /// <param name="rlen">���س���</param>
        /// <param name="rbuff">�������ݻ�����</param>       
        /// <returns></returns>
        int CPU_APDU(byte slen, byte[] sbuff, ref byte rlen, byte[] rbuff);

        #endregion

    }
}
