﻿/* 
 * 本工具类通过动态编译，实现对Web服务的动态调用。
 * 本工具参考了来自互联网的众多代码，并经过大量修正和改进。
 * 
 * 有任何错误，请联系：samonsun@imr.ac.cn
 * ***************************************
 * 
 * 支持服务对象缓存，支持编译后程序集持久化，支持PONO对象自动转换。 
 * 
 * 当前版本不支持WSE。若要支持WSE，由于服务器端返回的XML发生了改变，需要引用WSE相关的库来编译。
 * 
 * 若使用SOAP Header，请考虑在Global.asax文件中，包含一个Application_AuthenticateRequest处理程序，
 * 集中所有验证代码。
 * 
 * 自定义的SOAP Header公共字段和属性，必须以DF_开头。本程序只设置了用户定义的公共属性，SOAP Header作为SoapHeader
 * 对象本身所继承的字段，没有做设置。可以使用仅包含具有相同的字段名的对象为自定义SOAP Header属性赋值。
 * 
 * 推荐：在Model中自定义的 HeaderInternal 的PONO对象，用它设置Web服务中定义的SOAP Header对象的属性值，
 * HeaderInternal类具有和自定义SOAP Header类相同的字段名称(公共属性以DF_下划线开头)。[2010-2-2]
 * 
 * 对于byte[] 类型的参数，可以直接在调用时传入即可，无需转换成BASE64字符串。[2010-2-8]
 * 
 * 对于具有输出参数的方法，目前仅实现了Native类型的输出参数，返回值也只能是Native类型。请遵循此简单设计。[2010-2-9 0905]
 * 
 * */


#if DEBUG
#undef ENABLE_CACHE
#else
#define ENABLE_CACHE
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Xml;

namespace DevFuture.Common
{
    class WebServiceInvoker
    {
        /// <summary>
        /// 有效的类型，用于列举服务对象上的方法,内部使用。
        /// </summary>
        Dictionary<string, Type> availableTypes;

        /// <summary>
        /// 有效的服务对象
        /// </summary>
        private List<string> services;
        /// <summary>
        /// 有效的服务对象.
        /// </summary>
        public List<string> AvailableServices
        {
            get{ return this.services; }
        }
        /// <summary>
        /// Web服务的Uri
        /// </summary>
        private Uri webServiceUri;
        /// <summary>
        /// Web服务编译后程序集缓存名称
        /// </summary>
        private readonly string wsCacheAsmName;
        /// <summary>
        /// 编译后的程序集
        /// </summary>
        private Assembly webServiceAssembly;

        private Dictionary<string, object> cachedServiceInstance;
     

        public WebServiceInvoker(string url):this(new Uri(url))
        {
          
        }

        /// <summary>
        /// Creates the service invoker using the specified web service.
        /// </summary>
        /// <param name="webServiceUri"></param>
        public WebServiceInvoker(Uri webServiceUri_)
        {
            this.services = new List<string>(); // available services
            this.availableTypes = new Dictionary<string, Type>(); // available types

            webServiceUri = webServiceUri_;

            #region 缓存设置
            string wscachedir = System.IO.Path.Combine(Environment.CurrentDirectory, @"wscache");
            if (System.IO.Directory.Exists(wscachedir) == false)
            {
                System.IO.Directory.CreateDirectory(wscachedir);
            }
            wsCacheAsmName = System.IO.Path.Combine(wscachedir,
                String.Format("ws{0}.dll", webServiceUri.GetHashCode()));
            #endregion

            cachedServiceInstance = new Dictionary<string, object>();
                       
            // create an assembly from the web service description
            this.webServiceAssembly = BuildAssemblyFromWSDL(webServiceUri);

            // see what service types are available
            Type[] types = this.webServiceAssembly.GetExportedTypes();

            // and save them
            foreach (Type type in types)
            {
                services.Add(type.FullName);
                availableTypes.Add(type.FullName, type);
            }
        }




        /// <summary>
        /// Gets a list of all methods available for the specified service.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public List<string> EnumerateServiceMethods(string serviceName)
        {
            List<string> methods = new List<string>();

            if (!this.availableTypes.ContainsKey(serviceName))
                throw new Exception("Service Not Available");
            else
            {
                Type type = this.availableTypes[serviceName];

                // only find methods of this object type (the one we generated)
                // we don't want inherited members (this type inherited from SoapHttpClientProtocol)
                foreach (MethodInfo minfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                    methods.Add(minfo.Name);

                return methods;
            }
        }


        #region 调用子

        /// <summary>
        /// 调用指定的方法，返回定制对象。定制对象必须是简单.NET对象。(Pure .NET Object)。
        /// 即：必须具有默认构造函数；必须只包含属性。
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="serviceName">The name of the service to use.
        /// 通常即web服务的类名。</param>
        /// <param name="methodName">The name of the method to call.</param>
        /// <param name="args">The arguments to the method.</param>
        /// <returns>The return value from the web service method.</returns>
        public T InvokeMethodReturnCustomObject<T>(string serviceName, string methodName, params object[] args ) where T:new()
        {
            object serviceObj = GetCachedServiceObject(serviceName);
            Type serviceObjectType = serviceObj.GetType();
            object rcObj= serviceObjectType.InvokeMember(methodName, BindingFlags.InvokeMethod, null, serviceObj, args);
            //Type rcType = rcObj.GetType();

            ////这里使用反射赋值           
            //Type realType = typeof(T);
            //T realObj = new T();
            //foreach (PropertyInfo pi in realType.GetProperties())
            //{
            //    pi.SetValue(realObj, rcType.GetProperty(pi.Name).GetValue(rcObj, null), null);
            //}
            //return realObj;  
            return TranslatePONO<T>(ref rcObj);
        }

        /// <summary>
        /// 调用指定的方法，返回定制对象的数组。定制对象必须是简单.NET对象。(Pure .NET Object)。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T[] InvokeMethodReturnCustomObjectArray<T>(string serviceName, string methodName, params object[] args) where T : new()
        {
            object serviceObj = GetCachedServiceObject(serviceName);
            Type serviceObjectType = serviceObj.GetType();

            object[]  rcObjArr = serviceObjectType.InvokeMember(methodName, BindingFlags.InvokeMethod, null, serviceObj, args) as object[];           
            if (rcObjArr != null && rcObjArr.Length > 0)
            {               
                T[] tArr = new T[rcObjArr.Length];
                return TranslatePONOArray<T>(ref rcObjArr, ref tArr);            
            }
            throw new Exception("Web Service return none object Array.");        
        }



        /// <summary>
        /// 调用带有SOAP Header的web服务方法。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soapHeaderName"></param>
        /// <param name="header"></param>
        /// <param name="serviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T InvokeMethodReturnCustomObjectWithSoapHeader<T>(string soapHeaderPropertyName, 
            object header, string serviceName, string methodName, params object[] args) where T : new()
        {

            object serviceObj = GetCachedServiceObject(serviceName);
            Type serviceObjectType = serviceObj.GetType();
            SetSoapHeaderValue(ref serviceObj, ref serviceObjectType, soapHeaderPropertyName, header);
            object rcObj = serviceObjectType.InvokeMember(methodName, BindingFlags.InvokeMethod, null, serviceObj, args);
            //Type rcType = rcObj.GetType();//它实际是Web服务的一种Export Type           
            return TranslatePONO<T>(ref rcObj);
        }


        public T[] InvokeMethodReturnCustomObjectArrayWithSoapHeader<T>(string soapHeaderPropertyName,
            object header, string serviceName, string methodName, params object[] args) where T : new()
        {

            object serviceObj = GetCachedServiceObject(serviceName);
            Type serviceObjectType = serviceObj.GetType();
            SetSoapHeaderValue(ref serviceObj, ref serviceObjectType, soapHeaderPropertyName, header);
            object[] rcObjArr = serviceObjectType.InvokeMember(methodName, BindingFlags.InvokeMethod, null, serviceObj, args) as object[]; ;

            if (rcObjArr != null && rcObjArr.Length > 0)
            {
                T[] tArr = new T[rcObjArr.Length];
                return TranslatePONOArray<T>(ref rcObjArr, ref tArr);
            }
            throw new Exception("Web Service return none object Array.");        
        }

        
        

        /// <summary>
        /// 调用指定的方法，返回.NET原生对象（即：System/System.Data等重定义的对象）。
        /// String,Int,Byte...DataSet等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T InvokeMethodReturnNativeObject<T>(string serviceName, string methodName, params object[] args)
        {
            object serviceObj = GetCachedServiceObject(serviceName);
            Type serviceObjectType = serviceObj.GetType();
            return (T)serviceObjectType.InvokeMember(methodName, BindingFlags.InvokeMethod, null, serviceObj, args);
            
        }

        /// <summary>
        /// 具有输出参数的函数调用.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public T InvokeMethodReturnNativeObject<T>(string serviceName, string methodName,ref object[] args,bool[] modifiers)
        {
            object serviceObj = GetCachedServiceObject(serviceName);
            Type serviceObjectType = serviceObj.GetType();

            ParameterModifier p = new ParameterModifier(args.Length);
            for (int i = 0; i < args.Length; i++)
            {
                p[i] = modifiers[i];
            }

            return (T)serviceObjectType.InvokeMember(methodName, BindingFlags.InvokeMethod, null, 
                serviceObj, args, new ParameterModifier[] { p } ,
                null, null);

        }



        public T[] InvokeMethodReturnNativeObjectArray<T>(string serviceName, string methodName, params object[] args)
        {
            object serviceObj = GetCachedServiceObject(serviceName);
            Type serviceObjectType = serviceObj.GetType();
            return (T[])serviceObjectType.InvokeMember(methodName, BindingFlags.InvokeMethod, null, serviceObj, args);
            
        }

        /// <summary>
        /// 调用带有自定义SOAP Header的web服务方法。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soapHeaderName"></param>
        /// <param name="header"></param>
        /// <param name="serviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T InvokeMethodReturnNativeObjectWithSoapHeader<T>(string soapHeaderPropertyName, 
            object header,string serviceName, string methodName, params object[] args)
        {
            object serviceObj = GetCachedServiceObject(serviceName);
            Type serviceObjectType = serviceObj.GetType();
            SetSoapHeaderValue(ref serviceObj, ref serviceObjectType, soapHeaderPropertyName, header);
            return (T)serviceObjectType.InvokeMember(methodName, BindingFlags.InvokeMethod, null, serviceObj, args);

        }

        public T[] InvokeMethodReturnNativeObjectArrayWithSoapHeader<T>(string soapHeaderPropertyName,
          object header, string serviceName, string methodName, params object[] args)
        {
            object serviceObj = GetCachedServiceObject(serviceName);
            Type serviceObjectType = serviceObj.GetType();
            SetSoapHeaderValue(ref serviceObj, ref serviceObjectType, soapHeaderPropertyName, header);
            return (T[])serviceObjectType.InvokeMember(methodName, BindingFlags.InvokeMethod, null, serviceObj, args);
        }

        /// <summary>
        /// 获取静态的缓存服务对象，若没有缓存，就编译它.
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        private object GetCachedServiceObject(string serviceName)
        {
            object serviceObj = null;
            lock (cachedServiceInstance)
            {
                if (cachedServiceInstance.ContainsKey(serviceName))
                {
                    serviceObj = cachedServiceInstance[serviceName];
                }
                else
                {                    
                    serviceObj = this.webServiceAssembly.CreateInstance(serviceName);
                    cachedServiceInstance.Add(serviceName, serviceObj);
                }
            }
            return serviceObj;

        }

        #region 静态辅助泛型方法，PONO对象数组之间的转换
        private static T[] TranslatePONOArray<T>(ref object[] fromArr, ref T[] toArr) where T : new()
        {
            object rcObj = null;
            for (int i = 0; i < fromArr.Length; i++)
            {
                rcObj = fromArr[i];
                toArr[i] = TranslatePONO<T>(ref rcObj);

            }
            return toArr;
        }
        #endregion

        #region 静态辅助方法 给定制的SOAP Header 赋值
        private static void SetSoapHeaderValue(ref object serviceObj, ref Type serviceObjectType, string soapHeaderPropertyName, object header)
        {

            ///SOAP Header到达客户端后，会自动转变成 [Custom Header Type]Value 名字。
            if (!soapHeaderPropertyName.EndsWith("Value"))
            {
                soapHeaderPropertyName += "Value";
            }
            //若有SOAP Header，服务中的自定义Header必须是pubulic的字段或属性。
            //Note:自定义Header的的字段必须是DF_开头的。
            //需要将传入的header转化成服务代理的[Custom Header Type]Value 属性的值。
            //由于这两个类型都不在一个名字空间中，故需要手工转化。
            Type toType = serviceObjectType.GetProperty(soapHeaderPropertyName).PropertyType;
            object toObj = Activator.CreateInstance(serviceObjectType.GetProperty(soapHeaderPropertyName).PropertyType);
            Type fromType = header.GetType();
            foreach (PropertyInfo pi in toType.GetProperties())
            {
                if (pi.Name.StartsWith("DF_"))
                    pi.SetValue(toObj, fromType.GetProperty(pi.Name).GetValue(header, null), null);
            }
            serviceObjectType.GetProperty(soapHeaderPropertyName).SetValue(serviceObj, toObj, null);

        }
        #endregion

        #region 静态辅助方法， 简单.NET对象之间的转换。
        /// <summary>
        /// 静态辅助方法， 简单.NET对象之间的转换。(Plain Old .NET Object)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fromObj"></param>
        /// <returns></returns>
        private static T TranslatePONO<T>(ref object fromObj) where T : new()
        {
            Type fromType = fromObj.GetType();
            Type toType = typeof(T);
            T realObj = new T();
            foreach (PropertyInfo pi in toType.GetProperties())
            {
                pi.SetValue(realObj, fromType.GetProperty(pi.Name).GetValue(fromObj, null), null);
            }
            return realObj;
        }
        #endregion

        /// <summary>
        /// 将本地PONO代理类(或接近)转化成Web服务Export的类型(真正的代理).
        /// </summary>
        /// <param name="stubObj"></param>
        /// <param name="serviceExportTypeName"></param>
        /// <returns></returns>
        public object TranslateStub(object stubObj, string serviceExportTypeName)
        {

            if (!availableTypes.ContainsKey(serviceExportTypeName))
            {
                throw new Exception(String.Format("找不到类型:{0}", serviceExportTypeName));
            }
            Type toType = availableTypes[serviceExportTypeName];
            object realObj = Activator.CreateInstance(toType);
            Type fromType = stubObj.GetType();
            foreach (PropertyInfo pi in toType.GetProperties())
            {
                pi.SetValue(realObj, fromType.GetProperty(pi.Name).GetValue(stubObj, null), null);
            }

            return realObj;
        }


        #endregion

        #region 编译
        /// <summary>
        /// Builds the web service description importer, which allows us to generate a proxy class based on the 
        /// content of the WSDL described by the XmlTextReader.
        /// </summary>
        /// <param name="xmlreader">The WSDL content, described by XML.</param>
        /// <returns>A ServiceDescriptionImporter that can be used to create a proxy class.</returns>
        private ServiceDescriptionImporter BuildServiceDescriptionImporter( XmlTextReader xmlreader )
        {
            // make sure xml describes a valid wsdl
            if (!ServiceDescription.CanRead(xmlreader))
                throw new Exception("Invalid Web Service Description");

            // parse wsdl
            ServiceDescription serviceDescription = ServiceDescription.Read(xmlreader);

            // build an importer, that assumes the SOAP protocol, client binding, and generates properties
            ServiceDescriptionImporter descriptionImporter = new ServiceDescriptionImporter();
            descriptionImporter.ProtocolName = "Soap";
            descriptionImporter.AddServiceDescription(serviceDescription, null, null);
            descriptionImporter.Style = ServiceDescriptionImportStyle.Client;
            descriptionImporter.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties;

            return descriptionImporter;
        }

        /// <summary>
        /// Compiles an assembly from the proxy class provided by the ServiceDescriptionImporter.
        /// </summary>
        /// <param name="descriptionImporter"></param>
        /// <returns>An assembly that can be used to execute the web service methods.</returns>
        private Assembly CompileAssembly(ServiceDescriptionImporter descriptionImporter)
        {
            // a namespace and compile unit are needed by importer
            CodeNamespace codeNamespace = new CodeNamespace();
            CodeCompileUnit codeUnit = new CodeCompileUnit();

            codeUnit.Namespaces.Add(codeNamespace);

            ServiceDescriptionImportWarnings importWarnings = descriptionImporter.Import(codeNamespace, codeUnit);

            if (importWarnings == 0) // no warnings
            {
                // create a c# compiler
                CodeDomProvider compiler = CodeDomProvider.CreateProvider("CSharp");

                // include the assembly references needed to compile
                //注意：下面列举的引用，必须都被当前项目显式的引用，否则运行将出错.
                string[] references = new string[4] { 
                    "System.dll",  //必须                    
                    "System.Data.dll",//必须
                    "System.Web.Services.dll",//必须
                    "System.Xml.dll" //必须 
                };

                CompilerParameters parameters = new CompilerParameters(references);
                parameters.GenerateExecutable = false;
#if ENABLE_CACHE
                parameters.OutputAssembly = wsCacheAsmName;
#endif

                // compile into assembly
                CompilerResults results = compiler.CompileAssemblyFromDom(parameters, codeUnit);

                foreach (CompilerError oops in results.Errors)
                {
                    // trap these errors and make them available to exception object
                    throw new Exception("Compilation Error Creating Assembly");
                }

                // all done....
                return results.CompiledAssembly;
            }
            else
            {
                // warnings issued from importers, something wrong with WSDL
                throw new Exception("Invalid WSDL");
            }
        }

        /// <summary>
        /// Builds an assembly from a web service description.
        /// The assembly can be used to execute the web service methods.
        /// </summary>
        /// <param name="webServiceUri">Location of WSDL.</param>
        /// <returns>A web service assembly.</returns>
        private Assembly BuildAssemblyFromWSDL(Uri webServiceUri)
        {
            if (String.IsNullOrEmpty(webServiceUri.ToString()))
                throw new Exception("Web Service Not Found");

            
            if (System.IO.File.Exists(wsCacheAsmName))
            {
                Console.WriteLine("从缓存文件加载.");
                return Assembly.LoadFrom(wsCacheAsmName);
            }
            else
            {
                XmlTextReader xmlreader = new XmlTextReader(webServiceUri.ToString() + "?wsdl");

                ServiceDescriptionImporter descriptionImporter = BuildServiceDescriptionImporter(xmlreader);

                return CompileAssembly(descriptionImporter);
            }
        }
        #endregion
        
    }
}
