using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.XPath;
using System.Configuration;

namespace SuspiciousObjectExporter.Frameworks
{
    public class ConfigHelper : IDisposable
    {
        private Boolean m_disposed = false;

        #region Query AppSetting Configuration Section
        public String GetAppSettingByName(String name)
        {
            try
            {
                return ConfigurationManager.AppSettings[name].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Query Customerized Configuration Section
        public ConfigurationSection GetConfigSectionByName(String sectionName)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                return config.GetSection(sectionName);
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
            }
            m_disposed = true;
        }

        ~ConfigHelper()
        {
            Dispose(false);
        }
        #endregion
    }
}
