using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SuspiciousObjectExporter.Models
{
    internal partial class Verifier
    {
        //private ShareTypes.ReplaceCharacterDataEntity m_replaceCharacterList;
        /*
        #region Query Ignored Character Rule
        public void QueryReplaceCharacterRule()
        {
            try
            {
                ConfigHelper config = new ConfigHelper();
                m_replaceCharacterList = config.ReplaceCharacterList;
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
                return m_replaceCharacterList.IsReplaceEnable;
            }
        }
        #endregion

        #region Replace special character in the Suspicios Object by replace Rule
        public String ReplaceSuspiciousObjectCharacterByReplaceRule(String inputString)
        {
            String str = inputString.ToLower();
            if (IsEnableReplace)
            {
                String pattern = String.Empty;
                foreach (ShareTypes.ReplaceCharacter replaceCharacter in m_replaceCharacterList.ReplaceCharacterList)
                {
                    pattern = String.Format("{0}{1}{2}",
                        "(",
                        replaceCharacter.Value.ToLower(),
                        ")");
                    str = Regex.Replace(str, pattern, String.Empty);
                }
            }
            return str;
        }
        #endregion
        */
    }
}
