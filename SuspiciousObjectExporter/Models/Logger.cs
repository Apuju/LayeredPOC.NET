using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SuspiciousObjectExporter.Frameworks;
using SuspiciousObjectExporter.Service;
using SuspiciousObjectExporter.ShareTypes;

namespace SuspiciousObjectExporter.Models
{
    internal class Logger
    {
        private ConfigDataEntity m_configDataEntity = new ConfigDataEntity();
        private LogDataEntity m_logSettings = new LogDataEntity();

        #region Export Settings
        private void GetExportSettings()
        {
            ConfigurationService configClient = new ConfigurationService();
            try
            {
                m_configDataEntity = configClient.GetExportSettings();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Log Settings
        private void GetLogSettings()
        {
            ConfigurationService configClient = new ConfigurationService();
            try
            {
                m_logSettings = configClient.GetLogSettings();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Create Log file and wirte the log content
        public void LogIt(String content)
        {
            try
            {
                GetExportSettings();
                GetLogSettings();
                String logRootFloder = Path.Combine(Path.Combine(m_configDataEntity.DownloadRootFloderPath, m_configDataEntity.DownloadFloderName), m_logSettings.DefaultLogRecordRootFloder);
                Log(content, logRootFloder, Path.Combine(logRootFloder, m_logSettings.DefaultExportRecordLogFile));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Log Exception
        public void LogException(String excpetion)
        {
            try
            {
                GetExportSettings();
                GetLogSettings();
                String logRootFloder = Path.Combine(Path.Combine(m_configDataEntity.DownloadRootFloderPath, m_configDataEntity.DownloadFloderName), m_logSettings.DefaultLogRecordRootFloder);
                Log(excpetion, logRootFloder, Path.Combine(logRootFloder, m_logSettings.DefaultExceptionLogFile));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Log something
        public void Log(String content, String logRootFloder, String logFile)
        {
            using (IOWriter log = new IOWriter())
            {
                try
                {
                    log.CreateDirectory(logRootFloder);
                    log.HelloLogWriter(Path.Combine(logRootFloder, logFile));
                    log.WriteLineLog(content);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

    }
}
