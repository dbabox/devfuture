#if V1
/*
 * 动态调用Web服务的帮助类。
 * 
 * 使用方法：
            //示例1：(WCF服务调用) 
            string url = "http://www.webservicex.net/globalweather.asmx" ;
            string[] args = new string[2] ;
            args[0] = "ShenYang";
            args[1] = "China" ;
            object result = WebServiceHelper.InvokeWebService(url ,"GetWeather" ,args) ;
            return result.ToString() ;
            //示例2：(IIS Host Web Service调用)
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
        //动态调用web服务
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
                //获取WSDL
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

                    //生成客户端代理类代码
                    CodeCompileUnit ccu = new CodeCompileUnit();
                    ccu.Namespaces.Add(cn);
                    sdi.Import(cn, ccu);

                    CompilerResults cr = null;
                    using (CSharpCodeProvider csc = new CSharpCodeProvider())
                    {
                        //ICodeCompiler icc = csc.CreateCompiler();

                        //设定编译参数
                        CompilerParameters cplist = new CompilerParameters();
                        cplist.GenerateExecutable = false;
                        cplist.GenerateInMemory = true;
                        cplist.ReferencedAssemblies.Add("System.dll");
                        cplist.ReferencedAssemblies.Add("System.XML.dll");
                        cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                        cplist.ReferencedAssemblies.Add("System.Data.dll");

                        //编译代理类
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

                    //生成代理实例，并调用方法
                    System.Reflection.Assembly assembly = cr.CompiledAssembly;
                    Type t = assembly.GetType(@namespace + "." + classname, true, true);
                    object obj = Activator.CreateInstance(t);
                    System.Reflection.MethodInfo mi = t.GetMethod(methodname);
                    //Note:这里可以考虑缓存所有的Web服务对象，以及Web服务上的MethodInfo对象；
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