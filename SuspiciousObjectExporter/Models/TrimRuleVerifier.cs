using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SuspiciousObjectExporter.ShareTypes;
using SuspiciousObjectExporter.Service;
using System.Xml;

namespace SuspiciousObjectExporter.Models
{
    internal partial class Verifier
    {
        private ReplaceCharacterDataEntity m_trimCharacterList = null;
        
        #region Query Ignored Character Rule
        public void QueryTrimCharacterRule()
        {
            try
            {
                ConfigurationService configClient = new ConfigurationService();
                m_trimCharacterList = configClient.GetReplaceCharacterList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean IsEnableReplace
        {
            get
            {
                return m_trimCharacterList.IsReplaceEnable;
            }
        }
        #endregion
        
        #region Trim special characters of the Suspicios Object with replace Rule
        public XmlDocument TrimSuspiciousObjectCharacterWithReplaceRule(XmlDocument soList)
        {
            QueryTrimCharacterRule();
            if (IsEnableReplace)
            {
                String pattern = String.Empty;
                XmlNodeList nodes = soList.SelectNodes(soListRootName + "/" + soListNodeName);
                foreach (XmlNode node in nodes)
                {
                    foreach (ReplaceCharacter trimCharacter in m_trimCharacterList.ReplaceCharacterList)
                    {
                        pattern = String.Format("{0}{1}{2}",
                            "(",
                            trimCharacter.Value,
                            ")");
                        node.Attributes[trimCharacter.Name].Value = Regex.Replace(node.Attributes[trimCharacter.Name].Value, pattern, String.Empty, RegexOptions.IgnoreCase);
                    }
                }
            }
            return soList;
        }
        #endregion
        
    }
}
