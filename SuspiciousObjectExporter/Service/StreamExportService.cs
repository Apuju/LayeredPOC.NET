using System;
using System.Collections.Generic;
using System.Text;
using SuspiciousObjectExporter.Bussiness;
using SuspiciousObjectExporter.Frameworks;
using SuspiciousObjectExporter.ShareTypes;
using System.Xml;
using System.Xml.Xsl;
using System.IO;

namespace SuspiciousObjectExporter.Service
{
    public class StreamExportService
    {
        private ConfigDataEntity m_configDataEntiry = new ConfigDataEntity();
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

        #region Initialize the output settings
        private void InitExportSettings()
        {
            using(ConfigAgent config = new ConfigAgent())
            {
                try
                {
                    m_configDataEntiry = config.GetExportSettings();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            using (IOWriter ioWriter = new IOWriter())
            {
                try
                {
                    String exportRootFolder = Path.Combine(m_configDataEntiry.DownloadRootFloderPath, m_configDataEntiry.DownloadFloderName);
                    ioWriter.CreateDirectory(exportRootFolder);
                    String templateRootFolder = Path.Combine(m_configDataEntiry.DownloadRootFloderPath, m_configDataEntiry.TemplateFloderName);
                    ioWriter.CreateDirectory(templateRootFolder);
                    ioWriter.CopyOrReplaceFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine(m_configDataEntiry.DefaultSampleSourceRootFolder, m_configDataEntiry.DefaultSampleDataSource)), Path.Combine(templateRootFolder, m_configDataEntiry.DefaultSampleDataSource));
                    foreach (String template in m_configDataEntiry.DefaultSampleTemplates)
                    {
                        ioWriter.CopyOrReplaceFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine(m_configDataEntiry.DefaultSampleSourceRootFolder, template)), Path.Combine(templateRootFolder, template));
                    }
                    //ioWriter.CopyOrReplaceFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine(m_configDataEntiry.DefaultSampleSourceRootFolder, m_configDataEntiry.DefaultSmapleBatchRunFile)), Path.Combine(exportRootFolder, m_configDataEntiry.DefaultSmapleBatchRunFile));
                    //ioWriter.CopyOrReplaceFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine(m_configDataEntiry.DefaultSampleSourceRootFolder, m_configDataEntiry.DefaultSmaplePowerShellRunFile)), Path.Combine(exportRootFolder, m_configDataEntiry.DefaultSmaplePowerShellRunFile));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        public String ExportSuspiciosObjectList(XmlDocument soList)
        {
            String exportFile = String.Empty;
            String templateFile = String.Empty;
            String sampleDataFile = String.Empty;
            try
            {
                InitExportSettings();
                exportFile = Path.Combine(m_configDataEntiry.DownloadRootFloderPath, m_configDataEntiry.DownloadFloderName);
                templateFile = Path.Combine(m_configDataEntiry.DownloadRootFloderPath, m_configDataEntiry.TemplateFloderName);
                sampleDataFile = templateFile;
                exportFile = Path.Combine(exportFile, m_configDataEntiry.OutputFileName);
                templateFile = Path.Combine(templateFile, m_configDataEntiry.StyleSheetFileName);
                sampleDataFile = Path.Combine(sampleDataFile,m_configDataEntiry.DefaultSampleDataSource);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            using(IOWriter ioWriter = new IOWriter())
            using(TemplateHelper template = new TemplateHelper())
            {
                try
                {
                    if (m_debugModeFlag)
                    {
                        template.Transform(sampleDataFile, exportFile, templateFile);
                    }
                    else
                    {
                        template.Transform(soList, exportFile, templateFile);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return exportFile;
        }
    }
}
