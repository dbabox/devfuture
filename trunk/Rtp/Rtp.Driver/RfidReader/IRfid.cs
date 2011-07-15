using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.CardIO;

namespace Rtp.Driver.RfidReader
{
    /// <summary>
    /// ������ö��
    /// </summary>
    public enum CardPhysicalType
    {
        Unknown,
        UltraLight,
        MifareOne,
        MifareLight,
        CPU_TypeA,
        CPU_TypeB

    }

    public interface IRfid : IDisposable, ICpuIO,ISamIO,IUltralightIO
    {
        /// <summary>
        /// ���豸���ɹ������豸�����ʧ�ܷ��ظ���ֵ��
        /// </summary>
        /// <returns>�ɹ������豸�����ʧ�ܷ��ظ���ֵ��</returns>
        int Open();
        /// <summary>
        /// �ر��豸���ɹ�����0�����򷵻����Ĵ����롣
        /// </summary>
        /// <returns></returns>
        int Close();

        /// <summary>
        /// ��Ƶ�豸����λ
        /// </summary>
        /// <returns></returns>
        int DeviceReset();


        /// <summary>
        /// �豸���ڴ�״̬������true�����򷵻�false��
        /// </summary>
        /// <returns></returns>
        bool IsOpened();

        /// <summary>
        /// ͨ��Ѱ��������Ӧע�����ͻ��
        /// ���ؿ����ַ�����ʽ�������кţ����صĿ��ų��ȣ�����Ӳ����ͬ����ͬ��
        /// ͬʱ�跴���õ������͡�
        /// </summary>
        /// <param name="cpt">������</param>
        /// <returns>�����ŵ�4�ֽڵ��ַ�����ʾ��</returns>
        string Request(out CardPhysicalType cpt);

        /// <summary>
        /// ��ȡ�豸�汾��
        /// </summary>
        /// <returns></returns>
        string DeviceVersion();



    }
}
