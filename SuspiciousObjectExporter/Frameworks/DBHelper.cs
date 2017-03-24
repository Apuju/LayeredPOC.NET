using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace SuspiciousObjectExporter.Frameworks
{
    public class DBHelper : IDisposable
    {
        private Boolean m_disposed = false;
        private SqlConnection m_connection = null;
        private List<SqlParameter> m_parameters = new List<SqlParameter>();

        public DBHelper(String SqlServer, String UserID, String UserPassword, String DbName)
        {
            SqlConnectionStringBuilder connectionString = new SqlConnectionStringBuilder();
            connectionString.DataSource = SqlServer;
            connectionString.UserID = UserID;
            connectionString.Password = UserPassword;
            connectionString.InitialCatalog = DbName;
            m_connection = new SqlConnection(connectionString.ConnectionString);
        }

        #region Access MSSQL connection
         public void OpenConnection()
        {
            try
            {
                m_connection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CloseConnection()
        {
            try
            {
                m_connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Query Something in MSSQL
        public void AddParemeter(String variableReplacedName, SqlDbType dbType, Int32 fieldSize, Object paramterValue)
        {
            SqlParameter parameter = new SqlParameter(variableReplacedName, dbType, fieldSize);
            parameter.Value = paramterValue;
            m_parameters.Add(parameter);
        }

        public void ClearParameters()
        {
            m_parameters.Clear();
        }

        public JArray QueryMasqlDbToJSON(String queryString)
        {
            JArray jsonQuery = new JArray();
            SqlCommand sqlCommand = new SqlCommand(queryString, m_connection);
            sqlCommand.Parameters.AddRange(m_parameters.ToArray());
            try
            {
                SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                Int32 filedcount = Int32.MinValue;
                while (sqlReader.Read())
                {
                    JObject jsonRow = new JObject();
                    for (filedcount = 0; filedcount < sqlReader.FieldCount; filedcount++)
                    {
                        if (sqlReader.GetValue(filedcount)!=null || sqlReader.GetValue(filedcount)!=DBNull.Value)
                        {
                            jsonRow.Add(sqlReader.GetName(filedcount), sqlReader.GetValue(filedcount).ToString());
                        }
                        else
                        {
                            jsonRow.Add(sqlReader.GetName(filedcount), String.Empty);
                        }
                    }
                    jsonQuery.Add(jsonRow);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jsonQuery;
        }

        public DataSet QueryMssqlDbToDataSet(String queryString)
        {
            DataSet ds = new DataSet();
            SqlCommand sqlCommand = new SqlCommand(queryString, m_connection);
            sqlCommand.Parameters.AddRange(m_parameters.ToArray());
            try
            {
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public XmlDocument QueryMssqlDbToXmlDocument(String queryString)
        {
            XmlDocument xmlDocument = new XmlDocument();
            SqlCommand sqlCommand = new SqlCommand(queryString, m_connection);
            sqlCommand.Parameters.AddRange(m_parameters.ToArray());
            try
            {
                xmlDocument.Load(sqlCommand.ExecuteXmlReader());
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return xmlDocument;
        }

        public XPathDocument QueryMssqlDbToXPathDocument(String queryString)
        {
            SqlCommand sqlCommand = new SqlCommand(queryString, m_connection);
            sqlCommand.Parameters.AddRange(m_parameters.ToArray());
            try
            {
                return new XPathDocument(sqlCommand.ExecuteXmlReader());
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                CloseConnection();
            }

            m_disposed = true;
        }

        ~DBHelper()
        {
            Dispose(false);
        }
        #endregion

    }
}
