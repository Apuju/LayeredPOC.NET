using System;
using System.Collections.Generic;
using System.Text;
using SuspiciousObjectExporter.Frameworks;
using SuspiciousObjectExporter.ShareTypes;
using System.Xml;

namespace SuspiciousObjectExporter.Data
{
    internal class SuspiciousObject :IDisposable
    {
        private Boolean m_disposed = false;
        private String m_sqlServer = String.Empty;
        private String m_dbName = String.Empty;
        private String m_accountUserName = String.Empty;
        private String m_accountPassword = String.Empty;
        private Dictionary<Int32, String> m_soColumnMappingSettings = new Dictionary<Int32, String>();

        public SuspiciousObject(ConfigDataEntity sqlServerSettings)
        {
            m_sqlServer = sqlServerSettings.SQLServer;
            m_dbName = sqlServerSettings.DBname;
            m_accountUserName = sqlServerSettings.UserID;
            m_accountPassword = sqlServerSettings.UserPassword;
        }

        #region Query Suspicious Object by SQL Function
        private String GetSqlFunction4QuerySuspicousObject(Int32 type, String searchKeyWord, Int32 queryStartID, Int32 queryEndID, Int32 queryOrderBy, Int32 queryOrderAsc, Int32 sourceType, Int32 status)
        {
            return String.Format("dbo.fn_UI_QueryBlacklistInfo({0}, '{1}', {2}, {3}, {4}, {5}, {6}, {7})",
                                          type,
                                          searchKeyWord,
                                          queryStartID,
                                          queryEndID,
                                          queryOrderBy,
                                          queryOrderAsc,
                                          sourceType,
                                          status);
        }
        #endregion

        #region Suspicious Object Data Column Mapping Settings
        private void InitSOColumnMappingSettings()
        {
            m_soColumnMappingSettings.Add(1, "SeqID");
            m_soColumnMappingSettings.Add(2, "MD5Key");
            m_soColumnMappingSettings.Add(3, "Entity");
            m_soColumnMappingSettings.Add(4, "Severity");
            m_soColumnMappingSettings.Add(5, "Type");
            m_soColumnMappingSettings.Add(6, "ReportID");
            m_soColumnMappingSettings.Add(7, "Expiration");
            m_soColumnMappingSettings.Add(8, "IsNeverExpired");
            m_soColumnMappingSettings.Add(9, "HasAssessed");
            m_soColumnMappingSettings.Add(10, "RiskEndPoints");
            m_soColumnMappingSettings.Add(11, "ScanAction");
            m_soColumnMappingSettings.Add(12, "ScanActionDesc");
            m_soColumnMappingSettings.Add(13, "SourceType");
            m_soColumnMappingSettings.Add(14, "Status");
            m_soColumnMappingSettings.Add(15, "LastAssessedTime");
            m_soColumnMappingSettings.Add(16, "LastUpdateTime");
            m_soColumnMappingSettings.Add(17, "UserDefineTime");
            m_soColumnMappingSettings.Add(18, "UserDefineNotes");
            m_soColumnMappingSettings.Add(19, "RecordCount");
            m_soColumnMappingSettings.Add(20, "ExportTotalCount");
            m_soColumnMappingSettings.Add(21, "SYS_SystemID_Guid");
        }
        private String GetSODataColumnQueryString(Int32 id, String name)
        {
            String queryString = String.Empty;
            if (m_soColumnMappingSettings.ContainsKey(id))
            {
                queryString = String.Format(@"{0} AS {1}", m_soColumnMappingSettings[id], name);
            }
            return queryString;
        }
        #endregion

        #region Query Suspicious Object List
        public XmlDocument GetSuspiciousObjectList(Int32 startID, Int32 endID, SuspiciousObjectDataColumnSettings soDataColumnSettings, SuspiciousObjectTypeSettings soTypeSettings)
        {
            using (DBHelper db = new DBHelper(m_sqlServer, m_accountUserName, m_accountPassword, m_dbName))
            {
                try
                {
                    #region Define Variable
                    Int32 type = 99; // 99 : means all type
                    String searchKeyWord = String.Empty; //unenable to search  by keyword
                    Int32 queryStartID = startID;
                    Int32 queryEndID = endID;
                    Int32 queryOrderBy = 4;
                    Int32 queryOrderAsc = 1;
                    /*
                     * ORDER BY
                     * CASE WHEN @nOrderBy=0 AND @nOrderAsc=1 THEN  b.SLF_ExpireDateTimeStamp END ASC,
                     * CASE WHEN @nOrderBy=0 AND @nOrderAsc=0 THEN  b.SLF_ExpireDateTimeStamp END DESC,
                     * CASE WHEN @nOrderBy=1 AND @nOrderAsc=1 THEN  b.SLF_RiskLevel END ASC,
                     * CASE WHEN @nOrderBy=1 AND @nOrderAsc=0 THEN  b.SLF_RiskLevel END DESC,
                     * CASE WHEN @nOrderBy=2 AND @nOrderAsc=1 THEN  b.AtRiskEndpoints END ASC,
                     * CASE WHEN @nOrderBy=2 AND @nOrderAsc=0 THEN  b.AtRiskEndpoints END DESC,
                     * CASE WHEN @nOrderBy=3 AND @nOrderAsc=1 THEN  UPPER(b.ScanActionDesc) COLLATE Latin1_General_BIN END ASC,
                     * CASE WHEN @nOrderBy=3 AND @nOrderAsc=0 THEN  UPPER(b.ScanActionDesc) COLLATE Latin1_General_BIN END DESC,
                     * CASE WHEN @nOrderBy=4 AND @nOrderAsc=1 THEN  b.UserDefinedTime END ASC,
                     * CASE WHEN @nOrderBy=4 AND @nOrderAsc=0 THEN  b.UserDefinedTime END DESC
                     */
                    Int32 sourceType = 0; //For VA page (unenable)
                    Int32 status = 1; //For exception list, the source type might be 0 or 1
                    #endregion

                    #region Define Query String
                    StringBuilder queryString = new StringBuilder();

                    #region Data Column Settings
                    InitSOColumnMappingSettings();
                    String dataColumnQueryString = String.Empty;
                    if (soDataColumnSettings.IsEnable)
                    {
                        foreach (SuspiciousObjectDataColumn columnSetting in soDataColumnSettings.SuspiciousObjectDataColumns)
                        {
                            if (Convert.ToBoolean(columnSetting.IsEnable))
                            {
                                dataColumnQueryString = GetSODataColumnQueryString(Convert.ToInt32(columnSetting.ID), columnSetting.Name);
                                if (!String.IsNullOrEmpty(dataColumnQueryString))
                                {
                                    queryString.Append(" " + dataColumnQueryString + ",");
                                    dataColumnQueryString = String.Empty;
                                }
                            }
                        }
                    }
                    if (queryString.Length > 0)
                    {
                        dataColumnQueryString = queryString.ToString();
                        dataColumnQueryString = dataColumnQueryString.Substring(0, dataColumnQueryString.Length - 1);
                    }
                    else
                    {
                        dataColumnQueryString = "*";
                    }
                    #endregion

                    #region Type Settings
                    queryString = new StringBuilder();
                    String typeQueryString = String.Empty;
                    if (soTypeSettings.IsEnable)
                    {
                        foreach (SuspiciousObjectType soType in soTypeSettings.SuspiciousObjectTypes)
                        {
                            if (soType.IsEnable)
                            {
                                typeQueryString = soType.TypeValue.ToString();
                                if (!String.IsNullOrEmpty(typeQueryString))
                                {
                                    queryString.Append(typeQueryString + ",");
                                    typeQueryString = String.Empty;
                                }
                            }
                        }
                    }
                    if (queryString.Length > 0)
                    {
                        typeQueryString = queryString.ToString();
                        typeQueryString = String.Format("AND Type IN ({0})", typeQueryString.Substring(0, typeQueryString.Length - 1));
                    }
                    #endregion

                    String sqlQueryCommand = String.Format("SELECT {0} FROM {1} LEFT OUTER JOIN (SELECT SYS_SystemID_Guid FROM dbo.tb_SystemInfo WITH (NOLOCK)) AS SystemInfo ON 1=1 WHERE 1=1 {2} ORDER BY SeqID ASC FOR XML RAW('SuspiciousObject'), ROOT('SuspiciousObjectList');",
                        dataColumnQueryString,
                        GetSqlFunction4QuerySuspicousObject(type, searchKeyWord, queryStartID, queryEndID, queryOrderBy, queryOrderAsc, sourceType, status),
                        typeQueryString);
                    #endregion

                    db.OpenConnection();
                    return db.QueryMssqlDbToXmlDocument(sqlQueryCommand);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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
                m_sqlServer = String.Empty;
                m_dbName = String.Empty;
                m_accountUserName = String.Empty;
                m_accountPassword = String.Empty;
            }
            m_disposed = true;
        }

        ~SuspiciousObject()
        {
            Dispose(false);
        }
        #endregion
    }
}
