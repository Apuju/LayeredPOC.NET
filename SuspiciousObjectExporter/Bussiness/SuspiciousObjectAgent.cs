using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuspiciousObjectExporter.Data;
using SuspiciousObjectExporter.ShareTypes;
using System.Xml;

namespace SuspiciousObjectExporter.Bussiness
{
    internal class SuspiciousObjectAgent : IDisposable
    {
        private Boolean disposed = false;

        internal XmlDocument GetSuspiciousObjectList(Int32 startID, Int32 endID)
        {
            using (Config config = new Config())
            {
                ConfigDataEntity configSettings = new ConfigDataEntity();
                Boolean debugModeFlag = false;
#if DEBUG
                debugModeFlag = true;
                configSettings.SQLServer = @"tw-aaronsu-rs08\sqlexpress";
                configSettings.DBname = @"db_ControlManager";
                configSettings.UserID = @"sa";
                configSettings.UserPassword = @"P@ssw0rd";
#endif
                if (!debugModeFlag)
                {
                    try
                    {
                        configSettings = config.GetSQLServerInfo();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                using (SuspiciousObject so = new SuspiciousObject(configSettings))
                {
                    try
                    {
                        return so.GetSuspiciousObjectList(startID, endID, config.GetSuspiciousObjectDataColumnSettings(), config.GetSuspiciousObjectTypeSettings());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        #region Discard the Object
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
            }

            disposed = true;
        }

        ~SuspiciousObjectAgent()
        {
            Dispose(false);
        }
        #endregion

    }
}
