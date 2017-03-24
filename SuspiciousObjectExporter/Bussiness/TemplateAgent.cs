using System;
using System.Collections.Generic;
using System.Text;

namespace SuspiciousObjectExporter.Bussiness
{
    public class TemplateAgent : IDisposable
    {
        private Boolean m_disposed = false;

        #region Discard the Object
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_disposed)
                return;
            if (disposing)
            {
            }

            m_disposed = true;
        }

        ~TemplateAgent()
        {
            Dispose(false);
        }
        #endregion
    }
}
