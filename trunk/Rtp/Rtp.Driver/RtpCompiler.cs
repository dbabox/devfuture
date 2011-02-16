using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver
{
    public class RtpCompiler
    {
        /// <summary>
        /// 全局TLV映射
        /// </summary>
        Dictionary<string, TLVItem> GvTlvMap=new Dictionary<string,TLVItem>();
        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, string> GvStringMap = new Dictionary<string, string>();

        /// <summary>
        /// 执行指令序,执行指令可以引用GVMap中的TLV。
        /// </summary>
        IList<TLVItem> insList = new List<TLVItem>();

        public bool compile(string line)
        {

            return false;
        }
        /// <summary>
        /// 变量分析，将变量放到Map中
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool AnalyzeOperatorGV(string line)
        {
             //把所有的SET指令解析到GV中
                

            return false;
        }

    }
}
