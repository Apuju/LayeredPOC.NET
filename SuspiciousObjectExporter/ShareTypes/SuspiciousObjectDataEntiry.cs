using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SuspiciousObjectExporter.ShareTypes
{
    #region Suspicious Object Data Columns Settings
    public class SuspiciousObjectDataColumnSettings : ConfigurationSection
    {
        [ConfigurationProperty("isEnable", IsRequired = true)]
        public Boolean IsEnable
        {
            get
            {
                return (Boolean)base["isEnable"];
            }
        }

        [ConfigurationProperty("suspiciousObjectColumns")]
        [ConfigurationCollection(typeof(SuspiciousObjectDataColumnCollection))]
        public SuspiciousObjectDataColumnCollection SuspiciousObjectDataColumns
        {
            get
            {
                return (SuspiciousObjectDataColumnCollection)base["suspiciousObjectColumns"];
            }
        }
    }

    public class SuspiciousObjectDataColumnCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SuspiciousObjectDataColumn();
        }

        public SuspiciousObjectDataColumn this[Int32 index]
        {
            get
            {
                return (SuspiciousObjectDataColumn)BaseGet(index);
            }
        }

        public new SuspiciousObjectDataColumn this[String key]
        {
            get
            {
                return (SuspiciousObjectDataColumn)BaseGet(key);
            }
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SuspiciousObjectDataColumn)element).ID;
        }
    }

    public class SuspiciousObjectDataColumn : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true, IsKey = true)]
        public String ID
        {
            get
            {
                return (String)base["id"];
            }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            {
                return (String)base["name"];
            }
        }

        [ConfigurationProperty("isEnable", IsRequired = true)]
        public String IsEnable
        {
            get
            {
                return (String)base["isEnable"];
            }
        }
    }
    #endregion

    #region Suspicious Object Type Settings
    public class SuspiciousObjectTypeSettings : ConfigurationSection
    {
        [ConfigurationProperty("isEnable", IsRequired = true)]
        public Boolean IsEnable
        {
            get
            {
                return (Boolean)base["isEnable"];
            }
        }

        [ConfigurationProperty("suspiciousObjectTypeList")]
        [ConfigurationCollection(typeof(SuspiciousObjectTypeCollection))]
        public SuspiciousObjectTypeCollection SuspiciousObjectTypes
        {
            get
            {
                return (SuspiciousObjectTypeCollection)base["suspiciousObjectTypeList"];
            }
        }
    }

    public class SuspiciousObjectTypeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SuspiciousObjectType();
        }

        public SuspiciousObjectType this[Int32 index]
        {
            get
            {
                return (SuspiciousObjectType)BaseGet(index);
            }
        }

        public new SuspiciousObjectType this[String key]
        {
            get
            {
                return (SuspiciousObjectType)BaseGet(key);
            }
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SuspiciousObjectType)element).TypeValue;
        }
    }

    public class SuspiciousObjectType : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true, IsKey = true)]
        public Int32 TypeValue
        {
            get
            {
                return (Int32)base["value"];
            }
        }

        [ConfigurationProperty("description", IsRequired = true)]
        public String Description
        {
            get
            {
                return (String)base["description"];
            }
        }

        [ConfigurationProperty("isEnable", IsRequired = true)]
        public Boolean IsEnable
        {
            get
            {
                return (Boolean)base["isEnable"];
            }
        }
    }
    #endregion
}
