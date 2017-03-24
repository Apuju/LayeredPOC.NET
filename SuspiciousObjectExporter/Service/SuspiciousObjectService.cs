using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SuspiciousObjectExporter.Bussiness;

namespace SuspiciousObjectExporter.Service
{
    public class SuspiciousObjectService
    {
        #region Get the Suspicious Object List
        public XmlDocument GetSuspiciousObject(Int32 startID, Int32 endID)
        {
            using (SuspiciousObjectAgent soAgent = new SuspiciousObjectAgent())
            {
                try
                {
                    return soAgent.GetSuspiciousObjectList(startID, endID);
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
