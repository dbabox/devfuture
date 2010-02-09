/*
 * 文件上传服务。实现了：
 * 1、流式下载
 * 2、分Chunk上传
 * 3、伪流式上传（实际是客户端以流的方式构建XML，整体XML是非流的。Web服务本身不支持流式的Request）
 * 4、文件MD5码验证
 * 
 * 需要注意的是，所有文件都上传到UPLOAD_PUTH中，若配置文件中无定义，则默认上传到App_Data中。
 * 
 * 
 * 
 * */

using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.IO;
using System.Configuration;
using Common.Logging;
using Pmps.Common;

namespace PmpsWebService
{
    /// <summary>
    /// FileTransfer 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class FileTransfer : System.Web.Services.WebService
    {

        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static readonly string UPLOAD_PUTH = String.IsNullOrEmpty(ConfigurationManager.AppSettings["UPLOAD_PATH"]) ? AppDomain.CurrentDomain.BaseDirectory + @"App_Data\" : ConfigurationManager.AppSettings["UPLOAD_PATH"];

        /// <summary>
        /// 预先准备文件，以备上传.
        /// </summary>
        /// <param name="fileName">文件名，请确保文件名的唯一性，否则可能因文件名导致文件被覆盖。特别是并发时。</param>
        /// <param name="length">文件大小，字节为单位</param>
        /// <returns></returns>
        [WebMethod]
        public bool PrepareForUploadFile(string fileName, long length)
        {
            bool rc = false;

            try
            {
                using (FileStream fs = File.Create(Path.Combine(UPLOAD_PUTH, fileName)))
                {
                    fs.SetLength(length);
                }
                return true;
            }
            catch (IOException ex1)
            {
                log.Error(ex1);
            }
            return rc;
        }

        /// <summary>
        /// 准备一个文件，由服务器决定存储文件的名称和位置，返回相对位置。
        /// 生成的文件名是一个GUID名称，默认的，文件存储目录按[年月,yyyyMM]分文件夹存储。
        /// </summary>
        /// <param name="extention"></param>
        /// <param name="length"></param>
        /// <param name="relativeFilePath"></param>
        /// <returns></returns>
        [WebMethod]
        public bool PrepareForUploadFileByServer(string extention, long length,out string relativeFilePath)
        {
            bool rc = false;             
            
            relativeFilePath = DateTime.Today.ToString("yyyyMM") +"\\"+ Guid.NewGuid().ToString("N")  + extention;

            string filePath = Path.Combine(UPLOAD_PUTH, relativeFilePath);
            string dirPath= Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            try
            {
                using (FileStream fs = File.Create(filePath))
                {
                    fs.SetLength(length);
                }
                return true;
            }
            catch (IOException ex1)
            {
                log.Error(ex1);
            }
            return rc;
        }

        /// <summary>
        /// 上传数据到已准备好的文件
        /// </summary>
        /// <param name="FileName">相对UPLOAD_PUTH的文件路径名。</param>
        /// <param name="buffer">要上传的数据</param>
        /// <param name="Offset">偏移</param>
        /// <returns></returns>
        [WebMethod]
        public bool UploadDataToPreparedFile(string FileName, byte[] buffer, long Offset)
        {           
            bool retVal = false;
            try
            {
                // setting the file location to be save in the server. reading from the web.config file

                string FilePath = Path.Combine(UPLOAD_PUTH, FileName);
                if (!File.Exists(FilePath)) return false;

                //if (Offset == 0) // new file, create an empty file
                //    File.Create(FilePath).Close();
                // open a file stream and write the buffer. Don't open with FileMode.Append because the transfer may wish to start a different point
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    fs.Seek(Offset, SeekOrigin.Begin);
                    fs.Write(buffer, 0, buffer.Length);
                }
                retVal = true;

            }
            catch (Exception ex)
            {
                log.ErrorFormat("UploadFile 文件名:{0},Offset={1}异常.{2}", FileName, Offset, ex);
            }
            return retVal;
        }

        /// <summary>
        /// This function used to append chunk of bytes to a file name. if the offet start from 0 means file name should be created.
        /// </summary>
        /// <param name="FileName">File Name</param>
        /// <param name="buffer">Buffer array，不应超过4M</param>
        /// <param name="Offset">偏移，即起始位置.</param>
        /// <returns>boolean: true means append is suucessfully</returns>
        [WebMethod]
        public bool UploadFile(string FileName, byte[] buffer, long Offset)
        {
            bool retVal = false;
            try
            {
                // setting the file location to be save in the server. reading from the web.config file
                string FilePath = Path.Combine(UPLOAD_PUTH, FileName);
                
                if (Offset == 0) // new file, create an empty file
                    File.Create(FilePath).Close();
                // open a file stream and write the buffer. Don't open with FileMode.Append because the transfer may wish to start a different point
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    fs.Seek(Offset, SeekOrigin.Begin);
                    fs.Write(buffer, 0, buffer.Length);
                }
                retVal = true;
                
            }
            catch (Exception ex)
            {
                log.ErrorFormat("UploadFile 文件名:{0},Offset={1}异常.{2}", FileName, Offset, ex);                       
            }
            return retVal;
        }
        

       
        //This WebMethod returns binary data and is not secure
        [WebMethod]
        public GetFileResponse GetFile(string fileName)
        {
            GetFileResponse response = new GetFileResponse();
            response.FileName = fileName;
            String filePath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\" + fileName;
            response.FileContents = File.ReadAllBytes(filePath);
            return response;
        }

        //This WebMethod returns binary data which is not secured, however it is streamed on the
        //service side by returning a type called GetFileResponseWrapper which is IXmlSerializable
        //WriteXml streams the file to the network from the service.
        //ReadXml is called on the service to write the file to a temporary file on disk
        //The client still caches the whole file as a byte[]
        //
        //On the server ASP.NET buffering needs to be turned off by setting BufferResponse="false"
        //to ensure that streaming occurs.
        [WebMethod(BufferResponse = false)]
        public GetFileResponseStreaming GetFileStreaming(string fileName)
        {
            GetFileResponseStreaming response = new GetFileResponseStreaming();
            response.FileName = fileName;
            String filePath = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\" + fileName;
            response.FileContents = new GetFileResponseWrapper(filePath);
            return response;
        }

        [WebMethod]
        public bool PutFileStreaming(GetFileRequestStreaming request)
        {
            bool rc = false;            
            String destFileName = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\" + request.FileName;
            
            try
            {

                foreach (String sourceFileName in request.FileContents.FileCollection)
                {
                    log.DebugFormat("{0} lenth={1}", sourceFileName, new FileInfo(sourceFileName).Length);
                    if (File.Exists(destFileName))
                        File.Delete(destFileName);
                    File.Move(sourceFileName, destFileName);
                    break;
                }
                //if (File.Exists(destFileName))
                //    File.Delete(destFileName);
                //File.Move(request.FileContents.FileName, destFileName);
                //File.Delete(request.FileContents.FileName);
                rc = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);

            }
            return rc;
        }

        /// <summary>
        /// 校验文件。
        /// </summary>
        /// <param name="fileName">要校验的服务器文件名。</param>
        /// <param name="verifyCode">客户端提供的本地文件校验码。</param>
        /// <returns>与服务器上的文件MD5值相等则返回true</returns>
        [WebMethod]
        public bool VerifyFileMD5(string fileName, string verifyCode)
        {
            string FilePath = Path.Combine(UPLOAD_PUTH, fileName);
            if (!File.Exists(FilePath)) return false;
            using (System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                using (FileStream fs = File.OpenRead(FilePath))
                {
                    string hash=Convert.ToBase64String(md5.ComputeHash(fs));
                    return string.CompareOrdinal(hash, verifyCode) == 0;                    
                }
            }
        }
 
    }
}
