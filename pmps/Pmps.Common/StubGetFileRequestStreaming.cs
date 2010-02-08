using System;
using System.Collections.Generic;
using System.Text;

namespace Pmps.Common
{
    /// <remarks/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class StubGetFileRequestStreaming
    {

        private string fileNameField;

        private byte[] fileDataField;

        /// <remarks/>
        public string fileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] fileData
        {
            get
            {
                return this.fileDataField;
            }
            set
            {
                this.fileDataField = value;
            }
        }
    }
}
