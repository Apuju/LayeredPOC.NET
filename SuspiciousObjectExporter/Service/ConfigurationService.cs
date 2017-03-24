using System;
using System.Collections.Generic;
using System.Text;
using SuspiciousObjectExporter.Bussiness;
using SuspiciousObjectExporter.ShareTypes;

namespace SuspiciousObjectExporter.Service
{
    public class ConfigurationService
    {
        #region Export Settings
        internal ConfigDataEntity GetExportSettings()
        {
            using (ConfigAgent configAgnet = new ConfigAgent())
            {
                try
                {
                    return configAgnet.GetExportSettings();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Log Settings
        internal LogDataEntity GetLogSettings()
        {
            using (ConfigAgent configAgnet = new ConfigAgent())
            {
                try
                {
                    return configAgnet.GetLogSettings();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Ignore Information
        public IgnoreCharacterDataEntity GetIgnoreCharacterList()
        {
            using (ConfigAgent configAgnet = new ConfigAgent())
            {
                try
                {
                    return configAgnet.GetIgnoreCharacterList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Replace Information
        public ReplaceCharacterDataEntity GetReplaceCharacterList()
        {
            using (ConfigAgent configAgnet = new ConfigAgent())
            {
                try
                {
                    return configAgnet.GetReplaceCharacterList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Suspicious Object Data Column Settings
        internal SuspiciousObjectDataColumnSettings GetSuspiciousObjectDataColumnSettings()
        {
            using (ConfigAgent configAgent = new ConfigAgent())
            {
                try
                {
                    return configAgent.GetSuspiciousObjectDataColumnSettings();
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
