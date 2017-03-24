using System;
using System.Collections.Generic;
using System.Text;

namespace SuspiciousObjectExporter.ShareTypes
{
    public class ConfigDataEntity
    {
        #region TMCM Server Settings
        private String m_tmcmHomeDirectory = String.Empty;
        private String m_cplFileURL = String.Empty;
        public String TMCMRootDirectory
        {
            get
            {
                return m_tmcmHomeDirectory;
            }
            set
            {
                m_tmcmHomeDirectory = value;
            }
        }
        public String CPLFileURL
        {
            get
            {
                return m_cplFileURL;
            }
            set
            {
                m_cplFileURL = value;
            }
        }
        #endregion

        #region SQL Server Settings
        private String m_sqlServer = String.Empty;
        private String m_dbName = String.Empty;
        private String m_userID = String.Empty;
        private String m_userPassword = String.Empty;

        public String SQLServer
        {
            get
            {
                return m_sqlServer;
            }
            set
            {
                m_sqlServer = value;
            }
        }

        public String DBname
        {
            get
            {
                return m_dbName;
            }
            set
            {
                m_dbName = value;
            }
        }

        public String UserID
        {
            get
            {
                return m_userID;
            }
            set
            {
                m_userID = value;
            }
        }

        public String UserPassword
        {
            get
            {
                return m_userPassword;
            }
            set
            {
                m_userPassword = value;
            }
        }
        #endregion

        #region Export Settings
        private String m_downloadRootFloderPath = String.Empty;
        private String m_downloadFloderName = String.Empty;
        private String m_outputFileName = String.Empty;
        private String m_stylesheetFile = String.Empty;
        private String m_defaultSampleSourceRootFloder = String.Empty;
        private String m_defaultSampleDataSource = String.Empty;
        private List<String> m_defaultSmapleTemplates = new List<String>();
        private String m_defaultSampleBatchRunFile = String.Empty;
        private String m_defaultSamplePowerShellRunFile = String.Empty;
        private string m_templateFloderName = string.Empty;
        public String DownloadRootFloderPath
        {
            get
            {
                return m_downloadRootFloderPath;
            }
            set
            {
                m_downloadRootFloderPath = value;
            }
        }
        public String DownloadFloderName
        {
            get
            {
                return m_downloadFloderName;
            }
            set
            {
                m_downloadFloderName = value;
            }
        }
        public String OutputFileName
        {
            get
            {
                return m_outputFileName;
            }
            set
            {
                m_outputFileName = value;
            }
        }
        public String StyleSheetFileName
        {
            get
            {
                return m_stylesheetFile;
            }
            set
            {
                m_stylesheetFile = value;
            }
        }
        public String DefaultSampleSourceRootFolder
        {
            get
            {
                return m_defaultSampleSourceRootFloder;
            }
            set
            {
                m_defaultSampleSourceRootFloder = value;
            }
        }
        public String DefaultSampleDataSource
        {
            get
            {
                return m_defaultSampleDataSource;
            }
            set
            {
                m_defaultSampleDataSource = value;
            }
        }
        public List<String> DefaultSampleTemplates
        {
            get
            {
                return m_defaultSmapleTemplates;
            }
            set
            {
                m_defaultSmapleTemplates = value;
            }
        }
        public String DefaultSmapleBatchRunFile
        {
            get
            {
                return m_defaultSampleBatchRunFile;
            }
            set
            {
                m_defaultSampleBatchRunFile = value;
            }
        }
        public String DefaultSmaplePowerShellRunFile
        {
            get
            {
                return m_defaultSamplePowerShellRunFile;
            }
            set
            {
                m_defaultSamplePowerShellRunFile = value;
            }
        }
        public String TemplateFloderName
        {
            get
            {
                return m_templateFloderName;
            }
            set
            {
                m_templateFloderName = value;
            }
        }
        #endregion
    }
}
