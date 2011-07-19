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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyIdx">װ������ģʽ(����)
        /// ��D8���ԣ�����M1����ÿ���������ڶ�д���о���Ӧ���������루KEYSET0��KEYSET1��KEYSET2����ÿ���������A���루KEYA����B���루KEYB�������������룬��0��2��4��6����ʾ���������룺
        /// 0����KEYSET0��KEYA`
        /// 1����KEYSET1��KEYA
        /// 2����KEYSET2��KEYA
        /// 4����KEYSET0��KEYB
        /// 5����KEYSET1��KEYB
        /// 6����KEYSET2��KEYB
        /// </param>
        /// <param name="secNr">�����ţ�M1����0��15��  ML����0��</param>
        /// <param name="nKey">д���д���еĿ�����</param>
        /// <returns></returns>
        int loadKeyM1(byte keyIdx,byte secNr,byte[] nKey);

        /// <summary>
        /// ��֤M1����ִ��ǰ�轫������ص��������ڲ�.��M1����addrΪBlock��ַ��
        /// </summary>
        /// <returns></returns>
        int authenticationM1(byte keyIdx, byte addr);
        
        /// <summary>
        /// ��֤M1�����ô˺���ʱ�����Բ���ִ��dc_load_key()������
        /// </summary>
        /// <param name="keyIdx"></param>
        /// <param name="blockAddr"></param>
        /// <param name="nKey"></param>
        /// <returns></returns>
        int authenticationPassAddrM1(byte keyIdx, byte blockAddr,byte[] nKey);
       
        /// <summary>
        /// ��ʼ����ֵ
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        int initvalM1(byte addr,ulong val);

        /// <summary>
        /// ���ֵ
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        int incrementM1(byte addr,ulong val);
        /// <summary>
        /// ���ֵ
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        int decrementM1(byte addr, ulong val);
        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        int readvalM1(byte addr,ref ulong val);


    }
}
