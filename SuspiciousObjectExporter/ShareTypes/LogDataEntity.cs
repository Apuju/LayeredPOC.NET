using System;
using System.Collections.Generic;
using System.Text;

namespace SuspiciousObjectExporter.ShareTypes
{
    public class LogDataEntity
    {
        #region Export Record Log
        private String m_defaultLogRootFloder = String.Empty;
        private String m_deafultExportRecordLogFile = String.Empty;
        public String DefaultLogRecordRootFloder
        {
            get
            {
                return m_defaultLogRootFloder;
            }
            set
            {
                m_defaultLogRootFloder = value;
            }
        }
        public String DefaultExportRecordLogFile
        {
            get
            {
                return m_deafultExportRecordLogFile;
            }
            set
            {
                m_deafultExportRecordLogFile = value;
            }
        }
        #endregion

        #region Exception Log
        private String m_defaultExceptionLogFile = String.Empty;
        public String DefaultExceptionLogFile
        {
            get
            {
                return m_defaultExceptionLogFile;
            }
            set
            {
                m_defaultExceptionLogFile = value;
            }
        }
        #endregion
    }
}
