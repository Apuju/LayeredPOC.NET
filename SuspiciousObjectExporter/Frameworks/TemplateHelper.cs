using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace SuspiciousObjectExporter.Frameworks
{
    public class TemplateHelper : IDisposable
    {
        private Boolean m_disposed = false;
        private XslCompiledTransform m_templateEngine = new XslCompiledTransform();

        public void Transform(XPathDocument source, String destination, String styleSheetPath)
        {
            m_templateEngine.Load(styleSheetPath, new XsltSettings(true,true), new XmlUrlResolver());
            XmlWriter xmlWriter = XmlWriter.Create(destination, m_templateEngine.OutputSettings);
            m_templateEngine.Transform(source, xmlWriter);
            xmlWriter.Close();
        }

        public void Transform(String source, String destination, String styleSheetPath)
        {
            m_templateEngine.Load(styleSheetPath, new XsltSettings(true, true), new XmlUrlResolver());
            XmlWriter xmlWriter = XmlWriter.Create(destination, m_templateEngine.OutputSettings);
            m_templateEngine.Transform(source, xmlWriter);
            xmlWriter.Close();
        }

        public void Transform(XmlDocument source, String destination, String styleSheetPath)
        {
            m_templateEngine.Load(styleSheetPath, new XsltSettings(true, true), new XmlUrlResolver());
            XmlWriter xmlWriter = XmlWriter.Create(destination, m_templateEngine.OutputSettings);
            m_templateEngine.Transform(source, xmlWriter);
            xmlWriter.Close();
        }

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
                m_templateEngine = null;
            }

            m_disposed = true;
        }

        ~TemplateHelper()
        {
            Dispose(false);
        }
        #endregion
    }
}
