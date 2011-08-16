using System;
using System.Collections.Generic;
using System.Text;

namespace Rtp.Driver.CardIO
{
    public class FileMapCosIO:ICosDictionary
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string cosName;
        public FileMapCosIO()
        {
            cosName = "PBOC/ס������׼COS";
        }

        private readonly Dictionary<UInt16, string> cosDic = new Dictionary<UInt16, string>();

        #region ���ļ��ж�ȡCOS�����붨��
        public ICosDictionary ReadCosFile(string file)
        {
            if (System.IO.File.Exists(file))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(file, Encoding.Default))
                {
                    //Ҫ��������COS˵��
                    string line=sr.ReadLine();
                    if (line.Length<2 || !line.StartsWith("!"))
                    {
                        throw new ApplicationException("��������COS�����ļ���ʽ�������б�����[!COS����].");
                    }
                    cosName = line.Substring(1, line.Length - 1);
                    KeyValuePair<UInt16, string> kvp;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        //ȥ��ע��
                        if (line.Length == 0 || line.StartsWith("#", StringComparison.OrdinalIgnoreCase)
                           || line.StartsWith("//", StringComparison.OrdinalIgnoreCase)
                           || line.StartsWith("==", StringComparison.OrdinalIgnoreCase)
                           || line.StartsWith("--", StringComparison.OrdinalIgnoreCase)
                           ) continue;
                        if (Utility.ReadCosIOItem(line, out kvp))
                        {
                            if (!cosDic.ContainsKey(kvp.Key))
                            {
                                cosDic.Add(kvp.Key, kvp.Value);
                            }
                            else
                            {
                                if (cosDic[kvp.Key].Contains(kvp.Value) == false)
                                {
                                    logger.WarnFormat("Cos [{1}]������{0,8:X}�ж��ֺ���!", kvp.Key, CosName);
                                    cosDic[kvp.Key] += ("/" + kvp.Value);
                                }
                            }

                        }
                    }

                }
           
                logger.InfoFormat("Cos [{0}] Cmd ��ʼ�����!", CosName);

            }
            else
            {
                logger.ErrorFormat("Cos [{1}]  ������ļ� {0} �����ڣ�", file, CosName);
            }
            return this;

        }
        #endregion

        #region ICosIO ��Ա

        public IDictionary<UInt16, string> CosDic
        {
            get { return cosDic; }
        }

        public string GetDescription(ushort cmd)
        {
            if (cosDic.ContainsKey(cmd)) return cosDic[cmd];
            UInt16 tmp = 0;
            foreach (UInt16 k in cosDic.Keys)
            {                
                tmp = (UInt16)(k ^ cmd);
                if (tmp < 0x00FF)
                {
                    if (cosDic[k].Contains("xx"))
                    {
                        return cosDic[k].Replace("xx", String.Format("0x{0,2:X2}", tmp));
                    }
                    else
                    {
                        return cosDic[k].Replace("x", String.Format("{0,1:X1}", tmp));
                    }
                }               
            }
            return "δ֪������";
        }

        public string GetDescription(byte B1, byte B2)
        {
            //��λ���ֽ�
            ushort us = 0;
            us |= B1;
            us <<= 8;
            us |= B2;
            return GetDescription(us);
        }

        public string CosName
        {
            get { return cosName; }
        }

        #endregion
    }
}
