using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using Framework.Commons.CommonObj;

namespace Framework.UI.BusinessObj
{
    public class DataTableGridView
    {
        /// <summary>
        ///     Set the url to the jquery.min.js file.
        /// </summary>
        [Bindable(true)]
        [Category("SOS Custom")]
        [DefaultValue("//ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js")]
        [Localizable(true)]
        [Description("The url to the jquery.min.js file.")]
        public string JqueryScriptUrl
        {
            get
            {
                string s = (string) HttpContext.Current.Session["JqueryScriptUrl"];
                return s ?? "//ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js";
            }

            set => HttpContext.Current.Session["JqueryScriptUrl"] = value;
        }

        /// <summary>
        ///     Disable Adding of Jquery Script file to prevent overwriting of JQuery when it already exists in a
        ///     project solution.
        /// </summary>
        [Bindable(true)]
        [Category("SOS Custom")]
        [DefaultValue(false)]
        [Localizable(true)]
        [Description("Default value is false")]
        public bool DisableJqueryScript
        {
            get
            {
                if (HttpContext.Current.Session["DisableJqueryScript"] == null) return false;

                bool s = (bool) HttpContext.Current.Session["DisableJqueryScript"];
                return s;
            }
            set => HttpContext.Current.Session["DisableJqueryScript"] = value;
        }

        /// <summary>
        ///     Set the url to the jquery.dataTables.min.js file.
        /// </summary>
        [Bindable(true)]
        [Category("SOS Custom")]
        [DefaultValue("https://cdn.datatables.net/1.10.15/js/jquery.dataTables.min.js")]
        [Localizable(true)]
        [Description("The url to the jquery.dataTables.min.js file.")]
        public string JqueryDataTableScriptUrl
        {
            get
            {
                string s = (string) HttpContext.Current.Session["JqueryDataTableScriptUrl"];
                return s ?? "https://cdn.datatables.net/1.10.15/js/jquery.dataTables.min.js";
            }

            set => HttpContext.Current.Session["JqueryDataTableScriptUrl"] = value;
        }

        /// <summary>
        ///     Disable Adding of Jquery DataTables Plugin Script file to prevent overwriting of the Plugin when it already exists
        ///     in a
        ///     project solution.
        /// </summary>
        [Bindable(true)]
        [Category("SOS Custom")]
        [DefaultValue(false)]
        [Localizable(true)]
        [Description("Default value is false")]
        public bool DisableDataTablesScript
        {
            get
            {
                if (HttpContext.Current.Session["DisableDataTablesScript"] == null) return false;

                bool s = (bool) HttpContext.Current.Session["DisableDataTablesScript"];
                return s;
            }
            set => HttpContext.Current.Session["DisableDataTablesScript"] = value;
        }


        /// <summary>
        ///     Disable Adding of Jquery DataTables Plugin Script file to prevent overwriting of the Plugin when it already exists
        ///     in a
        ///     project solution.
        /// </summary>
        [Bindable(true)]
        [Category("SOS Custom")]
        [DefaultValue(false)]
        [Localizable(true)]
        [Description("Default value is false")]
        public bool DisableDataTablesStyles
        {
            get
            {
                if (HttpContext.Current.Session["DisableDataTablesStyles"] == null) return false;

                bool s = (bool) HttpContext.Current.Session["DisableDataTablesStyles"];
                return s;
            }
            set => HttpContext.Current.Session["DisableDataTablesStyles"] = value;
        }


        /// <summary>
        ///     Disable the automatic save state option in the Jquery DataTables Plugin.  Default value is false.
        /// </summary>
        [Bindable(true)]
        [Category("SOS Custom")]
        [DefaultValue(false)]
        [Localizable(true)]
        [Description(
            "The default value is false. This will disable any of the default options we have specified to set for the DataTables Plugin.")]
        public bool DisableDataTablesDefaultOptions
        {
            get
            {
                if (HttpContext.Current.Session["DisableDataTablesDefaultOptions"] == null)
                {
                    return false;
                }

                bool s = (bool) HttpContext.Current.Session["DisableDataTablesDefaultOptions"];
                return s;
            }
            set => HttpContext.Current.Session["DisableDataTablesDefaultOptions"] = value;
        }


        /// <summary>
        ///     Set the url to the jquery.dataTables.min.css file.
        /// </summary>
        [Bindable(true)]
        [Category("SOS Custom")]
        [DefaultValue("https://cdn.datatables.net/1.10.15/css/jquery.dataTables.min.css")]
        [Localizable(true)]
        [Description("The url to the jquery.dataTables.min.css file.")]
        public string JqueryDataTableCssUrl
        {
            get
            {
                string s = (string) HttpContext.Current.Session["JqueryDataTableCssUrl"];
                return s ?? "https://cdn.datatables.net/1.10.15/css/jquery.dataTables.min.css";
            }

            set => HttpContext.Current.Session["JqueryDataTableCssUrl"] = value;
        }

        /// <summary>
        ///     Set jQuery DataTable options.
        /// </summary>
        [Bindable(true)]
        [Category("SOS Custom")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("The options for the Jquery DataTables")]
        public string JqueryDataTableOptions
        {
            get
            {
                string s = (string) HttpContext.Current.Session["JqueryDataTableOptions"];
                return s ?? "";
            }

            set => HttpContext.Current.Session["JqueryDataTableOptions"] = value;
        }

        /// <summary>
        ///     Whether to use ProperCase when setting header values, first prioity will go to aliases
        /// </summary>
        [Bindable(true)]
        [Category("SOS Custom")]
        [DefaultValue(true)]
        [Localizable(true)]
        [Description("Whether to use ProperCase when setting header values")]
        public bool ProperCase
        {
            get
            {
                if (HttpContext.Current.Session["ProperCase"] == null) return false;

                bool s = (bool) HttpContext.Current.Session["ProperCase"];
                return s;
            }

            set => HttpContext.Current.Session["ProperCase"] = value;
        }

        [DefaultValue(0)] public int TableIndex { get; set; }

        public Dictionary<string, string> SpecialColumnAliases { get; set; }

        public string TableId { get; set; }

        public DtoDataSet Results { get; set; }
    }
}