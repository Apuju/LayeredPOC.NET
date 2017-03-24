using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SuspiciousObjectExporter.ShareTypes
{

    #region Ignore Configuration Section
    public class IgnoreCharacterDataEntity : ConfigurationSection
    {
        [ConfigurationProperty("isEnable", IsRequired = true)]
        public Boolean IsIgnoreEnable
        {
            get
            {
                return (Boolean)base["isEnable"];
            }
        }

        [ConfigurationProperty("ignoreList")]
        [ConfigurationCollection(typeof(IgnoreCharacterCollection))]
        public IgnoreCharacterCollection IgnoreCharacterList
        {
            get
            {
                return (IgnoreCharacterCollection)base["ignoreList"];
            }
        }
    }

    public class IgnoreCharacterCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IgnoreCharacter();
        }

        public IgnoreCharacter this[Int32 index]
        {
            get
            {
                return (IgnoreCharacter)BaseGet(index);
            }
        }

        public new IgnoreCharacter this[String key]
        {
            get
            {
                return (IgnoreCharacter)BaseGet(key);
            }
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IgnoreCharacter)element).Name;
        }
    }

    public class IgnoreCharacter : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            {
                return (String)base["name"];
            }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public String Value
        {
            get
            {
                return (String)base["value"];
            }
        }
    }
    #endregion

}
