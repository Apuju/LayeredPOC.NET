using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SuspiciousObjectExporter.Service;
using SuspiciousObjectExporter.ShareTypes;
using System.Xml;

namespace SuspiciousObjectExporter.Models
{
    internal  partial class Verifier : IDisposable
    {
        private Boolean m_disposed = false;
        private IgnoreCharacterDataEntity m_ignoreCharacterList = null;
        private SuspiciousObjectDataColumnSettings m_soDataColumnSettings = null;
        private String soListRootName = "SuspiciousObjectList";
        private String soListNodeName = "SuspiciousObject";

        #region Query Ignored Character Rule
        public void QueryIgnoreCharacterRule()
        {
            try
            {
                ConfigurationService configClient = new ConfigurationService();
                m_soDataColumnSettings = configClient.GetSuspiciousObjectDataColumnSettings();
                m_ignoreCharacterList = configClient.GetIgnoreCharacterList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean IsEnableIgnore
        {
            get
            {
                return m_ignoreCharacterList.IsIgnoreEnable;
            }
        }
        #endregion

        

        #region Check the Suspicios Object with Ignore Rule
        public XmlDocument CheckSuspiciousObjectWithIgnoreRule(XmlDocument soList)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement(soListRootName);
            String pattern = String.Empty;
            QueryIgnoreCharacterRule();
            if (m_ignoreCharacterList != null && m_ignoreCharacterList.IgnoreCharacterList.Count > 0)
            {
                Boolean pass = true;
                XmlNodeList subTree = soList.SelectNodes(soListRootName + "/" + soListNodeName);
                foreach (XmlNode node in subTree)
                {
                    foreach (IgnoreCharacter ignoreCharacter in m_ignoreCharacterList.IgnoreCharacterList)
                    {
                        pattern = String.Format("{0}{1}{2}",
                            "(",
                            ignoreCharacter.Value,
                            ")");
                        if (Regex.IsMatch(node.Attributes[ignoreCharacter.Name].Value, pattern, RegexOptions.IgnoreCase))
                        {
                            pass = false;
                            break;
                        }
                    }
                    if (pass)
                    {
                        XmlNode childNode = doc.ImportNode(node, true);
                        root.AppendChild(childNode);
                    }
                    pass = true;
                }
                doc.AppendChild(root);
            }
            return doc;
        }
        #endregion

        #region Check the Suspicios Object by Ignore Rule
        public Boolean CheckSuspiciousObjectByIgnoreRule(String suspicousObject)
        {
            Boolean pass = true;
            String pattern = String.Empty;
            if (m_ignoreCharacterList != null && m_ignoreCharacterList.IgnoreCharacterList.Count > 0)
            {
                foreach (IgnoreCharacter ignoreCharacter in m_ignoreCharacterList.IgnoreCharacterList)
                {
                    pattern = String.Format("{0}{1}{2}",
                        "(",
                        ignoreCharacter.Value.ToLower(),
                        ")");
                    if (Regex.IsMatch(suspicousObject.ToLower(), pattern))
                    {
                        pass = false;
                    }
                }
            }
            return pass;
        }
        #endregion
        
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
                if (m_ignoreCharacterList != null)
                {
                    m_ignoreCharacterList = null;
                }
            }

            m_disposed = true;
        }

        ~Verifier()
        {
            Dispose(false);
        }
        #endregion
        
    }
}
