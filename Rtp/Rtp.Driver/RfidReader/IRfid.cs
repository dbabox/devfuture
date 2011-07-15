using System;
using System.Collections.Generic;
using System.Text;
using Rtp.Driver.CardIO;

namespace Rtp.Driver.RfidReader
{
    /// <summary>
    /// 卡类型枚举
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
        /// 打开设备，成功返回设备句柄，失败返回负数值。
        /// </summary>
        /// <returns>成功返回设备句柄，失败返回负数值。</returns>
        int Open();
        /// <summary>
        /// 关闭设备。成功返回0，否则返回正的错误码。
        /// </summary>
        /// <returns></returns>
        int Close();

        /// <summary>
        /// 射频设备本身复位
        /// </summary>
        /// <returns></returns>
        int DeviceReset();


        /// <summary>
        /// 设备处于打开状态，返回true，否则返回false。
        /// </summary>
        /// <returns></returns>
        bool IsOpened();

        /// <summary>
        /// 通用寻卡函数，应注意防冲突。
        /// 返回卡的字符串格式物理序列号，返回的卡号长度，根据硬件不同而不同。
        /// 同时需反馈得到卡类型。
        /// </summary>
        /// <param name="cpt">卡类型</param>
        /// <returns>物理卡号低4字节的字符串表示。</returns>
        string Request(out CardPhysicalType cpt);

        /// <summary>
        /// 获取设备版本号
        /// </summary>
        /// <returns></returns>
        string DeviceVersion();



    }
}
