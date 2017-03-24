using System;
using System.Collections.Generic;
using System.Text;
using SuspiciousObjectExporter.Service;
using System.Xml;

namespace SuspiciousObjectExporter.Models
{
    public class Exporter : IDisposable
    {
        private Boolean m_disposed = false;
        private Boolean m_debugModeFlag = false;
        public Boolean DebugMode
        {
            get
            {
                return m_debugModeFlag;
            }
            set
            {
                m_debugModeFlag = value;
            }
        }

        public String WriteSuspiciosObjectList(Int32 startID, Int32 endID)
        {
            try
            {
                SuspiciousObjectService soClient = new SuspiciousObjectService();
                StreamExportService exportClient = new StreamExportService();
                exportClient.DebugMode = m_debugModeFlag;
                XmlDocument soList = soClient.GetSuspiciousObject(startID, endID);
                using (Verifier verifier = new Verifier())
                {
                    soList = verifier.CheckSuspiciousObjectWithIgnoreRule(soList);
                    soList = verifier.TrimSuspiciousObjectCharacterWithReplaceRule(soList);
                }
                return exportClient.ExportSuspiciosObjectList(soList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

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
            }
            m_disposed = true;
        }

        ~Exporter()
        {
            Dispose(false);
        }
        #endregion
    }
}
