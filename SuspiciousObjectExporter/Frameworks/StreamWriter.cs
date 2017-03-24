using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SuspiciousObjectExporter.Frameworks
{
    internal partial class IOWriter
    {
        private String datetimePattern = "yyyyMMddHHmmss";
        private StreamWriter m_fileStreamWirter = null;

        #region Check the file is existed or not
        public Boolean IsFileExisted(String filePath)
        {
            try
            {
                return File.Exists(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region
        public void CopyOrReplaceFile(String sourcePath, String destinationPath)
        {
            if (!IsFileExisted(destinationPath))
            {
                File.Copy(sourcePath, destinationPath);
            }
            else
            {
                if (File.GetLastWriteTimeUtc(sourcePath) > File.GetLastWriteTimeUtc(destinationPath))
                {
                    File.Delete(destinationPath);
                    File.Copy(sourcePath, destinationPath);
                }
            }
        }
        #endregion

        #region Check the file is existed or not. If it is yes, rename the file
        public String CheckFileExistOrNot(String filePath)
        {
            String destFilePath = String.Empty;
            if (File.Exists(filePath))
            {
                try
                {
                    String destFileName = String.Format("{0}{1}{2}",
                        Path.GetFileNameWithoutExtension(filePath),
                        File.GetLastWriteTime(filePath).ToString(datetimePattern),
                        Path.GetExtension(filePath));
                    destFilePath = Path.Combine(Path.GetDirectoryName(filePath), destFileName);
                    File.Move(filePath, destFilePath);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return destFilePath;
        }
        #endregion

        #region Access File Stream
        public void OpenStreamWriter(String filePath, FileMode writerMode)
        {
            try
            {
                FileStream fs = new FileStream(filePath, writerMode);
                m_fileStreamWirter = new StreamWriter(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Boolean CheckStreamWriterExistedOrNot()
        {
            if (m_fileStreamWirter != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void WriteLineByStreamWriter(String content)
        {
            m_fileStreamWirter.WriteLine(content);
        }
        internal void CloseStreamWriter()
        {
            m_fileStreamWirter.Close();
            m_fileStreamWirter.Dispose();
        }
        #endregion

    }
}
