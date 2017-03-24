using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SuspiciousObjectExporter.Frameworks
{
    internal partial class IOWriter
    {
        #region Access Log file
        public void HelloLogWriter(String filePath)
        {
            try
            {
                OpenStreamWriter(filePath,FileMode.Append);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteLineLog(String content)
        {
            try
            {
                WriteLineByStreamWriter(content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
