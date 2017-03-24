using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SuspiciousObjectExporter.ShareTypes
{

    #region Replace Configuration Section
    public class ReplaceCharacterDataEntity : ConfigurationSection
    {
        [ConfigurationProperty("isEnable", IsRequired = true)]
        public Boolean IsReplaceEnable
        {
            get
            {
                return (Boolean)base["isEnable"];
            }
        }

        [ConfigurationProperty("replaceList")]
        [ConfigurationCollection(typeof(ReplaceCharacterCollection))]
        public ReplaceCharacterCollection ReplaceCharacterList
        {
            get
            {
                return (ReplaceCharacterCollection)base["replaceList"];
            }
        }
    }

    public class ReplaceCharacterCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ReplaceCharacter();
        }

        public ReplaceCharacter this[Int32 index]
        {
            get
            {
                return (ReplaceCharacter)BaseGet(index);
            }
        }

        public new ReplaceCharacter this[String key]
        {
            get
            {
                return (ReplaceCharacter)BaseGet(key);
            }
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ReplaceCharacter)element).Name;
        }
    }

    public class ReplaceCharacter : ConfigurationElement
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
