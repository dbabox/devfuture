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
        private static readonly string UPLOAD_PUTH = ConfigurationManager.AppSettings["upload_path"];
        /// <summary></summary>
        /// This function used to append chunk of bytes to a file name. if the offet start from 0 means file name should be created.
        ///
        /// File Name
        /// Buffer array
        /// Offset
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
               
                //foreach (String sourceFileName in request.FileContents.FileCollection)
                //{
                //    log.DebugFormat("{0} lenth={1}", sourceFileName, new FileInfo(sourceFileName).Length);
                //    if (File.Exists(destFileName))
                //        File.Delete(destFileName);
                //    File.Move(sourceFileName, destFileName);
                //    break;
                //}
                if (File.Exists(destFileName))
                    File.Delete(destFileName);
                File.Move(request.FileContents.FileName, destFileName);
                File.Delete(request.FileContents.FileName);
                rc = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);

            }
            return rc;
        }

 
    }
}
