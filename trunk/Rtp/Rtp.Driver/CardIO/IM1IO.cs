/*
 * M1����д����.
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
        /// ��ȡM1����1��BLOCK,����ȡ16�ֽ����ݣ�
        /// �ɹ��򷵻� 0;
        /// </summary>
        /// <param name="addr">��ʼ��ַ</param>
        /// <param name="rbuff">��ȡ���������</param>
        /// <returns></returns>
        int readM1Block(byte addr, byte[] rbuff);

        /// <summary>
        /// дM1����1��BLOCK,��д16�ֽ����ݣ�
        /// �ɹ��򷵻� 0;
        /// </summary>
        /// <param name="addr">��ʼ��ַ</param>
        /// <param name="wbuff">д���ڴ滺����</param>
        /// <returns></returns>
        int writeM1Block(byte addr, byte[] wbuff);
    }
}
