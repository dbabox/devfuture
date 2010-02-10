using System;
using System.Data;
using System.Configuration;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.CodeDom.Compiler;
using Common.Logging;
 

namespace PmpsWebService
{
    // Shared Web Method Return types for the BinaryDataMTOMService and BinaryDataMTOMSecureService.
    [XmlRoot("getFileResponse")]
    public class GetFileResponse
    {
        [XmlElement("fileName", IsNullable = false)]
        public string FileName;

        [XmlElement("fileData", IsNullable = false)]
        public byte[] FileContents;
    }

    //The type to stream a response
    [XmlRoot("getFileResponseStreaming")]
    public class GetFileResponseStreaming
    {
        [XmlElement("fileName", IsNullable = false)]
        public string FileName;

        [XmlElement("fileData", IsNullable = false)]
        public GetFileResponseWrapper FileContents;
    }

    //The type to stream a request
    [XmlRoot("getFileResponseStreaming")]
    public class GetFileRequestStreaming
    {
        [XmlElement("fileName", IsNullable = false)]
        public string FileName;

        [XmlElement("fileData", IsNullable = false)]
        public GetFileResponseWrapper FileContents;
    }


    // This attribute tells the schema machinery to use the GetMysSchema
    // method to get the schema for this class.
    [XmlSchemaProvider("GetMySchema")]
    public class GetFileResponseWrapper : IXmlSerializable, IDisposable
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static readonly int Buff_Size = 4096;//和磁盘格式化的块大小一致最好
        private string _fileName;
        private TempFileCollection _tfc = null;

        public string FileName { get { return _fileName; } }

        public TempFileCollection FileCollection
        {
            get { return _tfc; }
        }

      
        public GetFileResponseWrapper()
            : this(null)
        {
          
        }

        public GetFileResponseWrapper(string fileName)
        {
            _fileName = fileName;
            _tfc = new TempFileCollection();
            _tfc.KeepFiles = false;
        }

        #region Dispose logic
        ~GetFileResponseWrapper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        // Dispose this wrapper to clean up temp files.
        void Dispose(bool isDisposing)
        {
            if (isDisposing && _tfc != null)
            {
                _tfc.KeepFiles = false;//这样才能真正清除所有文件 WIN32 Bug fix szj.2010-2-8
                _tfc.Delete();

            }
            _tfc = null;
        }
        #endregion

        /// <summary>
        /// The schema for the file contents node is actually just
        /// base 64 binary data so return the qname of the schema
        /// type directly.
        /// </summary>
        public static XmlQualifiedName GetMySchema(XmlSchemaSet xss)
        {
            return new XmlQualifiedName("base64Binary", "http://www.w3.org/2001/XMLSchema");
        }

        /// <summary>
        /// Always return null.
        /// </summary>
        public XmlSchema GetSchema() { return null; }

        /// <summary>
        /// Deserializes state out of an XmlReader 
        /// 反序列化.
        /// </summary>
        public void ReadXml(XmlReader r)
        {
            // Read the open tag of the encapsulating element
            r.ReadStartElement();
           
            // Read the binary data that represents the file contents
            // into a temp file.
           
            _fileName =_tfc.AddExtension("fileContents", false);       
            if (log.IsDebugEnabled) log.DebugFormat("从XML中读取内容到临时文件{0}", _fileName);
            ReadContentsIntoFile(r, _fileName);     
            // Read the close tag of the encapsulating element
            r.ReadEndElement();
        }

        /// <summary>
        /// Serializes state into an XmlWriter
        /// </summary>
        public void WriteXml(XmlWriter w)
        {
            using (FileStream fs = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
            {
                byte[] buf = new byte[Buff_Size];
                int numRead = 0;
                while ((numRead = fs.Read(buf, 0, Buff_Size)) > 0)
                {
                    w.WriteBase64(buf, 0, numRead);
                }
            }
        }

        /// <summary>
        /// 从XML中读取二进制数据到文件中，反序列化XML时使用。
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fileName"></param>
        void ReadContentsIntoFile(XmlReader reader, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.CreateNew))
            {
                if (log.IsDebugEnabled) log.DebugFormat("fileName={1},XmlReader: reader.CanReadBinaryContent={0}", reader.CanReadBinaryContent, fileName);
                // call ReadElementContentAsBase64
                if (reader.CanReadBinaryContent)
                {
                    byte[] buf = new byte[Buff_Size];
                    int numRead = 0;
                    while ((numRead = reader.ReadContentAsBase64(buf, 0, Buff_Size)) > 0)
                    {
                        fs.Write(buf, 0, numRead);                      
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }
    }

}
