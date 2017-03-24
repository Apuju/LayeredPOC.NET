using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace SuspiciousObjectExporter.Frameworks
{
    public class RegHelper : IDisposable
    {
        private Boolean m_disposed = false;
        private RegistryKey m_reg = null;

        #region Access Windows Register Table
        public Boolean OpenRegReader(String regPath)
        {
            Boolean opend = false;
            try
            {
                m_reg = Registry.LocalMachine.OpenSubKey(regPath);
                if (m_reg != null)
                {
                    opend = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return opend;
        }

        public String QueryRegValueByName(String name)
        {
            try
            {
                return m_reg.GetValue(name) as String;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CloseRegReader()
        {
            try
            {
                m_reg.Close();
                m_reg = null;
            }
            catch (Exception ex)
            {
                throw ex;
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
                CloseRegReader();
            }
            m_disposed = true;
        }

        ~RegHelper()
        {
            Dispose(false);
        }
        #endregion
    }
}
