﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DalApi
{
    /**
     <summary>
     Class for processing config.xml file and getting from there
     information which is relevant for initialization of DalApi<br/>
     The file has to include in the 1st level:<br/>
     <list type="bullet">
        <item><b>&lt;dal&gt;</b> element with the name of the entry in the packages' list</item>
        <item><b>&lt;dal-packages&gt;</b> element which includes sub-element for each package</item>
        <item><b>&lt;</b><em>key</em><b>&gt;</b> element where the element name is the key for the package
     in packages' list, its value is the package name, as well as it has the following attributes:
     <list type="number">
     <item><c>namespace="..."</c> - name of the namespace containing the Dal implementation class, default value is "Dal"</item>
     <item><c>class="..."</c> - name of the Dal implementation class, default value is the same as the package name</item>
     </list>
     </item>
     </list>
     <example>Example of config.xml file content:
     <code>
&lt;?xml version="1.0" encoding="utf-8"?&gt;<br/>
&lt;config&gt;<br/>
    &lt;dal&gt;data&lt;/dal&gt;<br/>
    &lt;dal-packages&gt;<br/>
    &lt;data&gt;DalObject&lt;/data&gt;<br/>
    &lt;xml namespace="DAL" class="DalXml"&gt;DalXml&lt;/xml&gt;<br/>
    &lt;oracle namespace="DL" class="DalDbOracle"&gt;DalOracle&lt;/oracle&gt;<br/>
    &lt;/dal-packages&gt;<br/>
&lt;/config&gt;<br/>
     </code>
     </example>
     </summary>
    */
    static class DalConfig
    {
        public class DalPackage
        {
            public string Name;
            public string PkgName;
            public string NameSpace;
            public string ClassName;
        }
        internal static string DalName;
        internal static Dictionary<string, DalPackage> DalPackages;

        /// <summary>
        /// Static constructor extracts Dal packages list and Dal type from
        /// Dal configuration file config.xml
        /// </summary>
        static DalConfig()
        {
            XElement DalConfig = XElement.Load(@"dal-config.xml");
            DalName = DalConfig.Element("dal").Value;
            DalPackages = (from pkg in DalConfig.Element("dal-packages").Elements()
                          let tmp1 = pkg.Attribute("namespace")
                          let nameSpace = tmp1 == null ? "Dal" : tmp1.Value
                          let tmp2 = pkg.Attribute("class")
                          let className = tmp2 == null ? pkg.Value : tmp2.Value
                          select new DalPackage()
                          {
                              Name = "" + pkg.Name,
                              PkgName = pkg.Value,
                              NameSpace = nameSpace,
                              ClassName = className
                          })
                           .ToDictionary(p => "" + p.Name, p => p);
        }
    }

    /// <summary>
    /// Represents errors during DalApi initialization
    /// </summary>
    [Serializable]
    public class DalConfigException : Exception
    {
        public DalConfigException(string message) : base(message) { }
        public DalConfigException(string message, Exception inner) : base(message, inner) { }
    }
}