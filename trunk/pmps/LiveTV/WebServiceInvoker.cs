/* April 8, 2009  
 * 本工具类实现对Web服务的动态调用。
 * 
 * TODO:
 * 增加对编译依赖的参数引入；
 * 增加对Web服务对象的cache；
 * 
 * 
 * */

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
        Dictionary<string, Type> availableTypes;

        /// <summary>
        /// Text description of the available services within this web service.
        /// </summary>
        public List<string> AvailableServices
        {
            get{ return this.services; }
        }

        /// <summary>
        /// Creates the service invoker using the specified web service.
        /// </summary>
        /// <param name="webServiceUri"></param>
        public WebServiceInvoker(Uri webServiceUri)
        {
            this.services = new List<string>(); // available services
            this.availableTypes = new Dictionary<string, Type>(); // available types

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
        public T InvokeMethodReturnCustomObject<T>( string serviceName, string methodName, params object[] args ) where T:new()
        {
            // create an instance of the specified service
            // and invoke the method
            object obj = this.webServiceAssembly.CreateInstance(serviceName);

            Type type = obj.GetType();

            object rc= type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, obj, args);
            Type rcType = rc.GetType();


            //这里使用反射赋值
           
            Type realType = typeof(T);
            T realObj = new T();
            foreach (PropertyInfo pi in realType.GetProperties())
            {
                pi.SetValue(realObj, rcType.GetProperty(pi.Name).GetValue(rc, null), null);
            }
            return realObj;  
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
            // create an instance of the specified service
            // and invoke the method
            object obj = this.webServiceAssembly.CreateInstance(serviceName); //应考虑缓存此对象

            Type type = obj.GetType();

            return (T)type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, obj, args);
            
        }

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
            CodeNamespace codeNamespace = new CodeNamespace("Pmps.Common");
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

            XmlTextReader xmlreader = new XmlTextReader(webServiceUri.ToString() + "?wsdl");

            ServiceDescriptionImporter descriptionImporter = BuildServiceDescriptionImporter(xmlreader);

            return CompileAssembly(descriptionImporter);
        }

        private Assembly webServiceAssembly;
        private List<string> services;
    }
}
