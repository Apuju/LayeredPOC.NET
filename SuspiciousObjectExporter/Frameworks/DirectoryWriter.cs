using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SuspiciousObjectExporter.Frameworks
{
    internal partial class IOWriter : IDisposable
    {
        private Boolean m_disposed = false;

        #region Create a new directory by physical path
        public void CreateDirectory(String folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Discard the object
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (m_disposed)
            {
                return;
            }
            if (disposing)
            {
                if (CheckStreamWriterExistedOrNot())
                {
                    CloseStreamWriter();
                }
            }
            m_disposed = true;
        }

        ~IOWriter()
        {
            Dispose(false);
        }
        #endregion

    }
}
