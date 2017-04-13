using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.XPath;
using EncryptDecryptUtility.NET;
using SuspiciousObjectExporter.Frameworks;
using SuspiciousObjectExporter.ShareTypes;

namespace SuspiciousObjectExporter.Data
{
    internal class Config : IDisposable
    {
        private Boolean m_disposed = false;
        private ConfigDataEntity m_configDataEntity = new ConfigDataEntity();
        private ConfigHelper m_config = new ConfigHelper();
        private const String m_regPath = @"SOFTWARE\Custom\TVCS";
        private const String m_HomeDirectoryRegName = "HomeDirectory";
        private const String m_SqlServerRegName = "SQLServer";
        private const String m_DbNameRegString = "DBName";
        private const String m_downloadRootFloderPathString = "outputRootFloderPath";
        private const String m_downloadFloderNameString = "outputFloderName";
        private const String m_outputFileNameString = "outputFile";
        private const String m_stylesheetFileNameString = "styleSheetFile";
        private const String m_defaultSampleSourceRootFolderString = "defaultSampleSourceRootFolderName";
        private const String m_defaultSampleDataSourceString = "defaultSampleDataSource";
        private const String m_defaultSampleTemplatesString = "defaultSampleTemplates";
        private const String m_defaultLogRootFolderNameString = "defaultLogRootFolderName";
        private const String m_defaultExportRecordLogFileString = "defaultExportRecordLogFile";
        private const String m_defaultExceptionLogFileString = "defaultExceptionLogFile";
        private const String m_ignoreCharacterListString = "ignoreCharacterList";
        private const String m_replaceCharacterListString = "trimCharacterList";
        private const String m_soDataColumnSettingsString = "soDataColumnSettings";
        private const String m_soTypeSettingsString = "soTypeSettings";
        private const String m_downlaodFloderDefaultName = "SOList";
        private const String m_outputFileDefaultName = "SOList.txt";
        private const String m_stylesheetFileDefaultName = "template.xsl";
        private const String m_DatasourceSettingFile = "DataSource.xml";
        private const String m_defaultSampleBatchRunFileString = "defaultSampleBatchRunFile";
        private const String m_defaultSamplePowerShellRunFileString = "defaultSamplePowerShellRunFile";
        private const string m_templateFloderName = "templateFloderName";
        private const String m_templateFloderDefaultName = "Template";

        #region Server Information
        internal ConfigDataEntity GetServerInfo()
        {
            String regPath = m_regPath;
            using (RegHelper reg = new RegHelper())
            {
                try
                {
                    reg.OpenRegReader(regPath);
                    m_configDataEntity.RootDirectory = reg.QueryRegValueByName(m_HomeDirectoryRegName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return m_configDataEntity;
        }
        #endregion

        #region  SQL Server Information
        internal ConfigDataEntity GetSQLServerInfo()
        {
            if (String.IsNullOrEmpty(m_configDataEntity.RootDirectory))
            {
                GetServerInfo();
            }

            String regPath = m_regPath;
            using (RegHelper reg = new RegHelper())
            {
                try
                {
                    reg.OpenRegReader(regPath);
                    m_configDataEntity.SQLServer = reg.QueryRegValueByName(m_SqlServerRegName);
                    m_configDataEntity.DBname = reg.QueryRegValueByName(m_DbNameRegString);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            using (StreamReader dataSourceReader = new StreamReader(new FileStream(Path.Combine(m_configDataEntity.RootDirectory, m_DatasourceSettingFile), FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                try
                {
                    XPathDocument xpdDataSourceDocument = new XPathDocument(dataSourceReader);
                    XPathNodeIterator xpniSourceNode = xpdDataSourceDocument.CreateNavigator().Select("/DataSource/Source");
                    if (xpniSourceNode != null)
                    {
                        xpniSourceNode.MoveNext();
                        m_configDataEntity.UserID = xpniSourceNode.Current.GetAttribute("ID", "");
                        m_configDataEntity.UserPassword = new EncryptDecrypt().NewDecryptStr(xpniSourceNode.Current.GetAttribute("Password", ""));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return m_configDataEntity;
        }
        #endregion

        #region Export Settings
        internal ConfigDataEntity GetExportSettings()
        {
#if DEBUG
            m_configDataEntity.RootDirectory = @"C:\SuspiciousObjectExporter";
#endif

            if (String.IsNullOrEmpty(m_configDataEntity.RootDirectory))
            {
                try
                {
                    GetServerInfo();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            try
            {
                #region The floder for output
                String downloadRootFloderPath = m_config.GetAppSettingByName(m_downloadRootFloderPathString);
                if (String.IsNullOrEmpty(downloadRootFloderPath))
                {
                    downloadRootFloderPath = m_configDataEntity.RootDirectory;
                }
                m_configDataEntity.DownloadRootFloderPath = downloadRootFloderPath;

                String downloadFloderName = m_config.GetAppSettingByName(m_downloadFloderNameString);
                if (String.IsNullOrEmpty(downloadFloderName))
                {
                    downloadFloderName = m_downlaodFloderDefaultName;
                }
                m_configDataEntity.DownloadFloderName = downloadFloderName;
                #endregion

                #region The File Name for output
                String outputFileName = m_config.GetAppSettingByName(m_outputFileNameString);
                if (String.IsNullOrEmpty(outputFileName))
                {
                    outputFileName = m_outputFileDefaultName;
                }
                m_configDataEntity.OutputFileName = outputFileName;
                #endregion

                #region The stylesheet file for template
                String styelsheetFileName = m_config.GetAppSettingByName(m_stylesheetFileNameString);
                if (String.IsNullOrEmpty(styelsheetFileName))
                {
                    styelsheetFileName = m_stylesheetFileDefaultName;
                }
                m_configDataEntity.StyleSheetFileName = styelsheetFileName;

                string templateFloderName = m_config.GetAppSettingByName(m_templateFloderName);
                if (String.IsNullOrEmpty(templateFloderName))
                {
                    templateFloderName = m_templateFloderDefaultName;
                }
                m_configDataEntity.TemplateFloderName = templateFloderName;
                #endregion

                #region Default Data Source and Templates
                m_configDataEntity.DefaultSampleSourceRootFolder = m_config.GetAppSettingByName(m_defaultSampleSourceRootFolderString);
                m_configDataEntity.DefaultSampleDataSource = m_config.GetAppSettingByName(m_defaultSampleDataSourceString);
                String defaultSampleTemplates = m_config.GetAppSettingByName(m_defaultSampleTemplatesString);
                m_configDataEntity.DefaultSampleTemplates.AddRange(defaultSampleTemplates.Split('|'));
                #endregion

                #region Default Run Files For Schedule jobs
                m_configDataEntity.DefaultSmapleBatchRunFile = m_config.GetAppSettingByName(m_defaultSampleBatchRunFileString);
                m_configDataEntity.DefaultSmaplePowerShellRunFile = m_config.GetAppSettingByName(m_defaultSamplePowerShellRunFileString);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return m_configDataEntity;
        }
        #endregion

        #region Log Settings
        internal LogDataEntity GetLogSettings()
        {
            LogDataEntity logSettings = new LogDataEntity();
            try
            {
                logSettings.DefaultLogRecordRootFloder = m_config.GetAppSettingByName(m_defaultLogRootFolderNameString);
                logSettings.DefaultExportRecordLogFile = m_config.GetAppSettingByName(m_defaultExportRecordLogFileString);
                logSettings.DefaultExceptionLogFile = m_config.GetAppSettingByName(m_defaultExceptionLogFileString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return logSettings;
        }
        #endregion

        #region Ignore Information
        internal IgnoreCharacterDataEntity GetIgnoreCharacterList()
        {
            try
            {
                return m_config.GetConfigSectionByName(m_ignoreCharacterListString) as IgnoreCharacterDataEntity;
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
                return m_config.GetConfigSectionByName(m_replaceCharacterListString) as ReplaceCharacterDataEntity;
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
                return m_config.GetConfigSectionByName(m_soDataColumnSettingsString) as SuspiciousObjectDataColumnSettings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Suspicious Object Type Settings
        internal SuspiciousObjectTypeSettings GetSuspiciousObjectTypeSettings()
        {
            try
            {
                return m_config.GetConfigSectionByName(m_soTypeSettingsString) as SuspiciousObjectTypeSettings;
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
                m_configDataEntity = null;
            }
            m_disposed = true;
        }

        ~Config()
        {
            Dispose(false);
        }
        #endregion
    }
}
