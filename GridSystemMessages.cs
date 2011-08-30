﻿/*
Copyright ©  Olav Christian Botterli.

Dual licensed under the MIT or GPL Version 2 licenses.

Date: 30.08.2011, Norway.

http://www.webgrid.com
*/


#region Header

/*
Copyright ©  Olav Christian Botterli. 

Dual licensed under the MIT or GPL Version 2 licenses.

Date: 30.08.2011, Norway.

http://www.webgrid.com
*/

#endregion Header

namespace WebGrid
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Xml;
    using System.Xml.XPath;

    using Collections;
    using Config;
    using Design;
    using Enums;
    using Sentences;

    public partial class Grid
    {
        #region Fields

        internal readonly SystemMessageCollection m_SystemMessages;

        private SystemMessages m_GridSystemMessages;
        private string m_SystemMessageCritical;
        private string m_SystemMessageDelete;
        private string m_SystemMessageInsert;
        private string m_SystemMessageUpdate;
        private string m_SystemMessageUpdateRows;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a collections of all the errors generated by the grid.
        /// </summary>
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Always),
        ]
        public SystemMessageCollection SystemMessage
        {
            get { return m_SystemMessages; }
        }

        /// <summary>
        /// Gets or sets the error you want to be displayed when a critical event occur.
        /// As default WebGrid will render a userfriendly critical message on the web page, this message
        /// can by replaced by this property. If you need to debug your web application please see 
        /// <see cref="WebGrid.Grid.Debug">WebGrid.Grid.Debug</see> and for further debug information
        /// you can set the property "trace" at your aspx page equal true.
        /// Web.Config key for this property is "WG" + property name.
        /// </summary>
        [Category("SystemMessage"),
        Description(
                @"Gets or sets the error you want to be displayed when a critical event occur. Default WebGrid will render a userfriendly critical message on the web page, this message can by replaced by this property."
                )]
        public string SystemMessageCritical
        {
            get
            {
                if (m_SystemMessageCritical == null && GridConfig.Get("wgsystemmessageCritical") != null)
                {
                    return GridConfig.Get("wgsystemmessageCritical");
                }
                return m_SystemMessageCritical;
            }
            set { m_SystemMessageCritical = value; }
        }

        /// <summary>
        /// Gets or sets the error you want to be displayed when a record is deleted from your data source.
        /// This message is displayed as error for the instance of the <see cref="WebGrid.Grid">WebGrid.Grid</see>
        /// Web.Config key for this property is "WG" + property name.
        /// </summary>
        /// <remarks>
        /// By default the message for this property is empty and will not be displayed, and the message is only displayed if there are no other errors being generated.
        /// </remarks>
        /// <value>The message you want to be displayed when a record is deleted from your data source.</value>
        [Category("SystemMessage"),
        Description(
                @"Gets or sets the error you want to be displayed when a record is deleted from your datasource. Web.Config key for this property is ""WG"" + property name"
                )]
        public string SystemMessageDelete
        {
            get
            {
                if (m_SystemMessageDelete == null && GridConfig.Get("wgsystemmessageDelete") != null)
                {
                    return GridConfig.Get("wgsystemmessageDelete");
                }
                return m_SystemMessageDelete;
            }
            set { m_SystemMessageDelete = value; }
        }

        /// <summary>
        /// Gets or sets the error you want to be displayed when a new record is inserted into your data source.
        /// This message is displayed as error for the instance of the <see cref="WebGrid.Grid">WebGrid.Grid</see>
        /// Web.Config key for this property is "WG" + property name.
        /// </summary>
        /// <remarks>
        /// By default the message for this property is empty and will not be displayed, and the message is only displayed if there are no other errors being generated.
        /// </remarks>
        /// <value>The message you want to be displayed when a new record is inserted into the data source.</value>
        [Category("SystemMessage"),
        Description(
                @"Gets or sets the error you want to be displayed when a new record is inserted into your datasource. Web.Config key for this property is ""WG"" + property name"
                )]
        public string SystemMessageInsert
        {
            get
            {
                if (m_SystemMessageInsert == null && GridConfig.Get("wgsystemmessageInsert") != null)
                {
                    return GridConfig.Get("wgsystemmessageInsert");
                }
                return m_SystemMessageInsert;
            }
            set { m_SystemMessageInsert = value; }
        }

        /// <summary>
        /// Gets or sets the error you want to be displayed when a record is updated in your data source.
        /// This message is displayed as error for the instance of the <see cref="WebGrid.Grid">WebGrid.Grid</see>
        /// Web.Config key for this property is "WG" + property name.
        /// </summary>
        /// <remarks>
        /// By default the message for this property is empty and will not be displayed, and the message is only displayed if there are no other errors being generated.
        /// </remarks>
        /// <value>The message you want to be displayed when a record is updated in your data source.</value>
        [Category("SystemMessage"),
        Description(
                @"Gets or sets the error you want to be displayed when a record is updated in your datasource. Web.Config key for this property is ""WG"" + property name"
                )]
        public string SystemMessageUpdate
        {
            get
            {
                if (m_SystemMessageUpdate == null && GridConfig.Get("wgsystemmessageUpdate") != null)
                {
                    return GridConfig.Get("wgsystemmessageUpdate");
                }
                return m_SystemMessageUpdate;
            }
            set { m_SystemMessageUpdate = value; }
        }

        /// <summary>
        /// Gets or sets the error you want to be displayed when records has been updated in grid view.
        /// This message is displayed as error for the instance of the <see cref="WebGrid.Grid">WebGrid.Grid</see>
        /// Web.Config key for this property is "WG" + property name.
        /// </summary>
        /// <remarks>
        /// By default the message for this property is empty and will not be displayed, and the message is only displayed if there are no other errors being generated.
        /// </remarks>
        /// <value>The message you want to be displayed when records is updated in grid view.</value>
        [Category("SystemMessage"),
        Description(
                @"Gets or sets the error you want to be displayed when records has been updated in grid view. Web.Config key for this property is ""WG"" + property name"
                )]
        public string SystemMessageUpdateRows
        {
            get
            {
                if (m_SystemMessageUpdateRows == null && GridConfig.Get("wgsystemmessageUpdateRows") != null)
                {
                    return GridConfig.Get("wgsystemmessageUpdateRows");
                }
                return m_SystemMessageUpdateRows;
            }
            set { m_SystemMessageUpdateRows = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets grid system message display text.
        /// </summary>
        /// <param name="systemMessageID">systemMessageID for the system message</param>
        /// <returns>display text for the system message</returns>
        public string GetSystemMessage(string systemMessageID)
        {
            if (m_GridSystemMessages == null)
            {
                LoadLanguage();
                if (m_GridSystemMessages == null)
                    return systemMessageID;
            }
            return m_GridSystemMessages.GetSystemMessage(systemMessageID);
        }

        /// <summary>
        /// Gets grid system message display text.
        /// </summary>
        /// <param name="systemMessageID">systemMessageID for the system message</param>
        /// <param name="defaultCssClass">The default CSS class.</param>
        /// <returns>display text for the system message</returns>
        public string GetSystemMessageClass(string systemMessageID, string defaultCssClass)
        {
            if (m_GridSystemMessages == null)
            {
                LoadLanguage();
                if (m_GridSystemMessages == null)
                    return systemMessageID;
            }
            return m_GridSystemMessages.GetSystemMessageClass(systemMessageID, defaultCssClass);
        }

        /// <summary>
        /// The system messages for each language can be found in the file "WebGridMessages.xml" that follows WebGrid starter kit.
        /// </summary>
        /// <param name="systemMessageID">The message you want to add or modify</param>
        /// <param name="displayText">The message itself.</param>
        /// <param name="cssClass">The css class.</param>
        public void SetSystemMessage(string systemMessageID, string displayText, string cssClass)
        {
            if (m_GridSystemMessages == null)
            {
                LoadLanguage();
                if (m_GridSystemMessages == null)
                    return;
            }
            m_GridSystemMessages.SetSystemMessage(systemMessageID, displayText, cssClass);
        }

        private void LoadLanguage()
        {
            if (Language == SystemLanguage.Undefined)
                Language = (SystemLanguage)Enum.Parse(typeof(SystemLanguage),
                                                       GridConfig.Get("WGLanguage", SystemLanguage.English.ToString()),
                                                       true);

            string cacheobjectSystemMessages = string.Format("{0}_{1}_{2}_SystemMessages_{3}", ClientID, Trace.ClientID, DataSourceId,Language);
            if (m_GridSystemMessages != null)
                return;
            if (DesignMode == false && HttpRuntime.Cache != null && Equals(CacheGridStructure, true) &&
                HttpRuntime.Cache.Get(cacheobjectSystemMessages) != null)
            {
                m_GridSystemMessages = HttpRuntime.Cache.Get(cacheobjectSystemMessages) as SystemMessages;

                if (Debug)
                    m_DebugString.AppendFormat("<b>Cache</b> - Loading system messages from cache object: {0}<br/>",
                                               cacheobjectSystemMessages);
                return;
            }

            if (Trace.IsTracing)
                Trace.Trace("{0} : Started LoadLanguage()", ID);
            // What language should we load?

            string systemmessagefile = SystemMessageDataFile;

            try
            {
                if (systemmessagefile != null &&
                    systemmessagefile.StartsWith("http://", StringComparison.OrdinalIgnoreCase) == false)
                {
                    if (Equals(DesignMode, true) && Site != null)
                        systemmessagefile = GridConfig.LoadSystemLanguages(systemmessagefile, Site);
                    else
                        systemmessagefile = GridConfig.LoadSystemLanguages(systemmessagefile);
                }
                if (systemmessagefile != null && System.IO.File.Exists(systemmessagefile) == false)
                {
                    throw new GridException(
                        string.Format("The system message file '{0}' does not exists.", systemmessagefile));
                }
                if (string.IsNullOrEmpty(systemmessagefile))
                // Load from resources if not available.
                {
                    m_GridSystemMessages = new SystemMessages();
                    Assembly a = Assembly.GetExecutingAssembly(); //Assembly.Load(GetType().Assembly.GetName().Name);
                 
                    Stream str = a.GetManifestResourceStream("WebGrid.Resources.WebGridMessages.xml");
                    if (str == null)
                        throw new GridException("WebGrid messages is not found in resources.");
                    XmlTextReader tr = new XmlTextReader(str);
                    XmlDocument xml = new XmlDocument();
                    xml.Load(tr);
                    XPathNavigator nav = xml.CreateNavigator();

                    if (nav == null)
                        throw new GridException("Unable to get a XpathNavigator for WebGrid Messages (in resources).");
                    XPathNodeIterator it = nav.Select(string.Format(@"//{0}/*", Language));

                    while (it.MoveNext())
                    {
                        if (string.IsNullOrEmpty(it.Current.Name) || string.IsNullOrEmpty(it.Current.Value))
                            continue;
                        m_GridSystemMessages.SetSystemMessage(it.Current.Name, it.Current.Value, null);
                    }
                }
                else
                    m_GridSystemMessages = new SystemMessages(Language, systemmessagefile, this);
            }
            catch (Exception ee)
            {
                throw new GridException(
                    string.Format("Error loading WebGrid system message file '{0}'.", systemmessagefile), ee);
            }

            if (HttpRuntime.Cache != null && Equals(CacheGridStructure, true))
                HttpRuntime.Cache[cacheobjectSystemMessages] = m_GridSystemMessages;
            if (Trace.IsTracing)
                Trace.Trace(string.Format("{0} : Stopped LoadLanguage()", ID));
        }

        /// <summary>
        /// </summary>
        /// <exclude/>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool ShouldSerializeSystemMessageCritical()
        {
            return m_SystemMessageCritical != null;
        }

        /// <summary>
        /// </summary>
        /// <exclude/>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool ShouldSerializeSystemMessageDelete()
        {
            return m_SystemMessageDelete != null;
        }

        /// <summary>
        /// </summary>
        /// <exclude/>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool ShouldSerializeSystemMessageInsert()
        {
            return m_SystemMessageInsert != null;
        }

        /// <summary>
        /// </summary>
        /// <exclude/>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool ShouldSerializeSystemMessageUpdate()
        {
            return m_SystemMessageUpdate != null;
        }

        /// <summary>
        /// </summary>
        /// <exclude/>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private bool ShouldSerializeSystemMessageUpdateRows()
        {
            return m_SystemMessageUpdateRows != null;
        }

        #endregion Methods
    }
}