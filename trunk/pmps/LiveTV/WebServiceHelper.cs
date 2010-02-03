#if V1
/*
 * ��̬����Web����İ����ࡣ
 * 
 * ʹ�÷�����
            //ʾ��1��(WCF�������) 
            string url = "http://www.webservicex.net/globalweather.asmx" ;
            string[] args = new string[2] ;
            args[0] = "ShenYang";
            args[1] = "China" ;
            object result = WebServiceHelper.InvokeWebService(url ,"GetWeather" ,args) ;
            return result.ToString() ;
            //ʾ��2��(IIS Host Web Service����)
            string url = "http://localhost:4155/Pmps.asmx";
            object result = WebServiceHelper.InvokeWebService(url, "PmpsService", "HelloWorld", null);
 * 
 * 
 * 
 * */

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Services.Description;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace DevFuture.Common
{
    class WebServiceHelper
    {
        #region InvokeWebService
        //��̬����web����
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return InvokeWebService(url, null, methodname, args);
        }

        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            string @namespace = "Pmps.Common";
            if ((classname == null) || (classname == ""))
            {
                classname = WebServiceHelper.GetWsClassName(url);
            }

            try
            {
                //��ȡWSDL
                using (WebClient wc = new WebClient())
                {
                    ServiceDescription sd = null;
                    using (Stream stream = wc.OpenRead(url + "?WSDL"))
                    {
                        sd = ServiceDescription.Read(stream);                       
                    }

                    ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                    sdi.AddServiceDescription(sd, "", "");
                    CodeNamespace cn = new CodeNamespace(@namespace);

                    //���ɿͻ��˴��������
                    CodeCompileUnit ccu = new CodeCompileUnit();
                    ccu.Namespaces.Add(cn);
                    sdi.Import(cn, ccu);

                    CompilerResults cr = null;
                    using (CSharpCodeProvider csc = new CSharpCodeProvider())
                    {
                        //ICodeCompiler icc = csc.CreateCompiler();

                        //�趨�������
                        CompilerParameters cplist = new CompilerParameters();
                        cplist.GenerateExecutable = false;
                        cplist.GenerateInMemory = true;
                        cplist.ReferencedAssemblies.Add("System.dll");
                        cplist.ReferencedAssemblies.Add("System.XML.dll");
                        cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                        cplist.ReferencedAssemblies.Add("System.Data.dll");

                        //���������
                        //CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                        cr = csc.CompileAssemblyFromDom(cplist, ccu);
                    }
                    if (true == cr.Errors.HasErrors)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                        {
                            sb.Append(ce.ToString());
                            sb.Append(System.Environment.NewLine);
                        }
                        throw new Exception(sb.ToString());
                    }

                    //���ɴ���ʵ���������÷���
                    System.Reflection.Assembly assembly = cr.CompiledAssembly;
                    Type t = assembly.GetType(@namespace + "." + classname, true, true);
                    object obj = Activator.CreateInstance(t);
                    System.Reflection.MethodInfo mi = t.GetMethod(methodname);
                    //Note:������Կ��ǻ������е�Web��������Լ�Web�����ϵ�MethodInfo����
                    return mi.Invoke(obj, args);
                }
                
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');

            return pps[0];
        }
        #endregion
    }
}
#endif