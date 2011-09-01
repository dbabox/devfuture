/*
 * 所有基于串口的读卡器，应重写此类。
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO.Ports;
 

namespace Rtp.Driver.RfidReader
{
    public abstract partial class SerialComRfidBase : RfidBase
    {

        protected System.Threading.AutoResetEvent responseEvent = new System.Threading.AutoResetEvent(false);
        private System.IO.Ports.SerialPort com=null;
        const int REC_BUFF_LEN=1024;
        private readonly byte[] recBuff=new byte[REC_BUFF_LEN];
        private int _recLen = 0;
        

        public SerialComRfidBase()
        {
            portName = SerialPort.GetPortNames()[0];
            baudRate = 9600;
        }

        public SerialComRfidBase(string portName_,int baudRate_)
        {
            portName = portName_;
            baudRate = baudRate_;
        }
 
        private string portName;
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }
        private int baudRate;
        public int BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }

        public override int Open()
        {
            int rc = 0;
            try
            {
                com = new System.IO.Ports.SerialPort(portName, baudRate);
                com.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(com_DataReceived);
                com.Open();
                rc = 0;
            }
            catch (System.IO.IOException ex)
            {
                logger.ErrorFormat("打开串口{0},波特率：{1}失败：{2}", portName, baudRate, ex.Message);
                rc = -1;
            }
            return rc;  
        }

        void com_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {           
            _recLen = 0;
            while (com.BytesToRead > 0 && com.BytesToRead < REC_BUFF_LEN)
            {
                System.Buffer.SetByte(recBuff, _recLen++, (byte)com.ReadByte());
            }

            responseEvent.Set();

        }
        /// <summary>
        /// 对读取到的数据进行缓存，然后处理；该函数必须重写；
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        //protected virtual int ProcessBuffer(byte[] buff, int len)
        //{
        //    //该函数应该重写，每个读卡器有自己特定的协议
        //    logger.Info(Utility.ByteArrayToHexStr(buff, len));
        //    return 0;
        //}

        public override int Close()
        {
            int rc = 0;
            try
            {
                com.Close();
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                rc = -1;
            }
            return rc;
        }

        /// <summary>
        /// 将数据写入串口
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public int Write(byte[] buff, int len)
        {
            int rc = 0;
            try
            {
                com.Write(buff, 0, len);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
                rc = -1;
            }
            return rc;

        }

        public override bool IsOpened()
        {
           
            return com.IsOpen;
        }

     
    }
}
