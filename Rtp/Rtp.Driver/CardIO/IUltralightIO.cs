/*
 * MIFARE Ultralight����д�����ӿڡ�
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
        /// ��ȡUltralight����1��Page,��4�ֽڣ�
        /// �ɹ��򷵻� 0
        /// </summary>
        /// <param name="addr">��ʼ��ַ</param>
        /// <param name="rbuff">��ȡ���������</param>
        /// <returns></returns>
        int UL_read(byte addr,byte[] rbuff);

        /// <summary>
        /// дUltralight����1��Page����д4�ֽ����ݣ�
        /// </summary>
        /// <param name="addr">��ʼ��ַ</param>
        /// <param name="wbuff">д���ڴ滺����</param>
        /// <returns></returns>
        int UL_write(byte addr, byte[] wbuff);
    }
}
