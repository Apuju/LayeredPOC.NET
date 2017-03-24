using System;
using System.Collections.Generic;
using System.Text;
using SuspiciousObjectExporter.Data;
using SuspiciousObjectExporter.ShareTypes;

namespace SuspiciousObjectExporter.Bussiness
{
    internal class ConfigAgent : IDisposable
    {
        private Boolean m_disposed = false;
        private Config m_config = new Config();

        #region Export Settings
        internal ConfigDataEntity GetExportSettings()
        {
            try
            {
                return m_config.GetExportSettings();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Log Settings
        internal LogDataEntity GetLogSettings()
        {
            try
            {
                return m_config.GetLogSettings();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Ignore Information
        internal IgnoreCharacterDataEntity GetIgnoreCharacterList()
        {
            try
            {
                return m_config.GetIgnoreCharacterList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Suspicious Object Data Column Settings
        internal SuspiciousObjectDataColumnSettings GetSuspiciousObjectDataColumnSettings()
        {
            try
            {
                return m_config.GetSuspiciousObjectDataColumnSettings();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Replace Information
        internal ReplaceCharacterDataEntity GetReplaceCharacterList()
        {
            try
            {
                return m_config.GetReplaceCharacterList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Discrad the object
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
                m_config.Dispose();
            }
            m_disposed = true;
        }

        ~ConfigAgent()
        {
            Dispose(false);
        }
        #endregion
    }
}
