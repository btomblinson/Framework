using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Framework.Commons.Utilities;
using Framework.UI.BusinessObj;

namespace Framework.UI.Web.MvcHtmlHelpers
{
    /// <summary>
    ///     Html Helper Class for Constructing GridViews.
    /// </summary>
    public static class GridViews
    {
        /// <summary>
        ///     Helper Control for constructing a serverSide Gridview for MVC Views using the SearcResults model.
        /// </summary>
        public static IHtmlString CreateSmartGrid<TModel>(this HtmlHelper<TModel> helper,
            SearchResults Model)
        {
            if (string.IsNullOrWhiteSpace(Model.Results.Error))
            {
                int ColCounter = 0;
                int MinDisplayRow = 0;
                int MaxDisplayRow = 0;
                string lblCount = "";
                int MaxPageRange = 0;
                int MinPageRange = 1;
                string PageRow = "";
                int PageCounter = 1;
                bool NextOnly = false;
                bool PreviousOnly = false;
                bool BothNextAndPrevious = false;
                string PageControllerName = helper.ViewContext.RouteData.GetRequiredString("controller");
                string PageAcion = helper.ViewContext.RouteData.GetRequiredString("action");
                string Result = CreateDefaultStyles();

                //Set Up Variables for paging
                if (Model.TotalPages != 1 && Model.CurrentPage != 1)
                {
                    MinDisplayRow = Model.RecordsPerPage * Model.CurrentPage - Model.RecordsPerPage;
                    MaxDisplayRow = MinDisplayRow + Model.RecordsPerPage - 1;
                }
                else
                {
                    MaxDisplayRow = Model.RecordsPerPage - 1;
                }

                if (MaxDisplayRow >= Model.RecordCount) MaxDisplayRow = Model.RecordCount - 1;

                //Set Up label to show what records the user is currently viewing
                if (Model.RecordCount == 1)
                    lblCount = "Showing 1 Record";
                else if (Model.RecordCount > Model.RecordsPerPage)
                    lblCount = "Showing records <span class='MinDisplayNum'>" + (MinDisplayRow + 1) +
                               "</span> thru <span class='MaxDisplayNum'>"
                               + (MaxDisplayRow + 1) + "</span> of <span class='TotalDisplayNum'>" + Model.RecordCount +
                               "</span> ";
                else
                    lblCount = "Showing records <span class='MinDisplayNum'>1</span> thru <span class='MaxDisplayNum'>"
                               + Model.RecordCount + "</span> of <span class='TotalDisplayNum'> " + Model.RecordCount +
                               "</span> ";

                Result += "<div id='" + Model.TableId + "_Wrappper'class='SmartGrid_Wrapper'> ";

                if (Model.RecordCount == Model.RecordLimit)
                    Result += "<span class='error'> Your request retrieved many results. Only " + Model.RecordLimit +
                              " results were returned. Please narrow your search criteria to retrieve better results.</span>";

                Result += "<div id='" + Model.TableId + "_GridProperties' class='SmartGridProperties' >";
                Result += "<div id='" + Model.TableId + "_RecordMessage' class='RecordMessage'><span>" + lblCount +
                          "</span></div> ";

                if (!string.IsNullOrWhiteSpace(Model.TableCaption))
                    Result += "<span class='TableCaption'>" + Model.TableCaption + "</span> ";

                //Display Paging Algorithm 
                PageRow = "<span class='GridPager'><table><tbody><tr><td><span class='PagerCaption'>Page</span></td> ";
                MaxPageRange = (int) (Math.Ceiling((decimal) Model.CurrentPage / 10) * 10);

                //When Page One
                if (MaxPageRange <= 10 && Model.TotalPages <= 10) MaxPageRange = Model.TotalPages;

                if (MaxPageRange >= Model.TotalPages && MaxPageRange > 11)
                {
                    MaxPageRange = Model.TotalPages;
                    PreviousOnly = true;
                }

                if (MaxPageRange > 11)
                {
                    MinPageRange = MaxPageRange - 10;
                }
                else if (MaxPageRange != Model.TotalPages)
                {
                    MaxPageRange++;
                    NextOnly = true;
                }

                if (!NextOnly && !PreviousOnly && MaxPageRange > 11)
                {
                    BothNextAndPrevious = true;
                    MaxPageRange++;
                }

                while (MinPageRange <= MaxPageRange)
                {
                    if (PageCounter == 1 && (PreviousOnly || BothNextAndPrevious) ||
                        MinPageRange == MaxPageRange && (NextOnly || BothNextAndPrevious))
                    {
                        PageRow += "<td class='PagerDot'>" + helper.ActionLink("...",
                                       PageAcion, PageControllerName, new {PageNumber = MinPageRange.ToString()},
                                       null) + "</td> ";
                    }
                    else
                    {
                        //Bold CurrentPage
                        if (MinPageRange == Model.CurrentPage)
                            PageRow += "<td class='PagerCurrentPage'>" + MinPageRange + "</td> ";
                        else
                            PageRow += "<td class='PagerPageNum'>" + helper.ActionLink(MinPageRange.ToString(),
                                           PageAcion, PageControllerName,
                                           new {PageNumber = MinPageRange.ToString()}, null) + "</td> ";
                    }

                    MinPageRange++;
                    PageCounter++;
                }

                if (Model.pagerPosition == PagerPosition.Top || Model.pagerPosition == PagerPosition.Both)
                    Result += PageRow + "</tr></tbody></table></span></div> ";

                Result += "<table id='" + Model.TableId +
                          "' class='SmartGrid' role='grid'><tbody><tr data-sortable='true'> ";
                foreach (DataColumn col in Model.Results.DS.Tables[Model.TableIndex].Columns)
                {
                    //If programmer has defined aliases and alias exist for this column use alias programmer specified
                    string SpecialColumnValue = "";
                    if (Model.SpecialColumnAliases != null &&
                        Model.SpecialColumnAliases.TryGetValue(col.Caption, out SpecialColumnValue))
                        Result += "<th class='" + col.Caption + "'>" + SpecialColumnValue + "</th> ";
                    else
                        Result += "<th class='" + col.Caption + "'>" + col.Caption.ToProperCase() + "</th> ";
                }

                Result += "</tr> ";

                while (MinDisplayRow <= MaxDisplayRow)
                {
                    Result += "<tr> ";
                    foreach (object cell in Model.Results.DS.Tables[Model.TableIndex].Rows[MinDisplayRow].ItemArray)
                    {
                        Result += "<td class='" +
                                  Model.Results.DS.Tables[Model.TableIndex].Columns[ColCounter].Caption + "'>" +
                                  cell + "</td> ";
                        ColCounter++;
                    }

                    Result += "</tr ";
                    ColCounter = 0;
                    MinDisplayRow++;
                }


                Result += "</tbody></table>";

                if (Model.pagerPosition == PagerPosition.Bottom || Model.pagerPosition == PagerPosition.Both)
                    Result += PageRow + "</tr></tbody></table></span>";

                Result += "</div>";
                return helper.Raw(Result);
            }

            return helper.Raw("<span class='error'>" + Model.Results.Error + "</span>");
        }

        /// <summary>
        ///     This is a helper control for constructing a DataTable, all this does is generate a table and add the required
        ///     information
        /// </summary>
        public static IHtmlString CreateDataTable<TModel>(this HtmlHelper<TModel> helper,
            DataTableGridView Model)
        {
            StringBuilder sb = new StringBuilder();

            //first off check and see if we have to create the jQuery and DataTable script and link tags

            //jQuery
            if (!Model.DisableJqueryScript) sb.Append("<script src='" + Model.JqueryScriptUrl + "'></script>");

            //jQuery DataTables JS
            if (!Model.DisableDataTablesScript)
                sb.Append("<script src='" + Model.JqueryDataTableScriptUrl + "'></script>");

            //jQuery DataTables CSS
            if (!Model.DisableDataTablesStyles)
                sb.Append("<link rel='styleheet' type='text/css' href='" + Model.JqueryDataTableCssUrl + "' />");

            //now build up the JQuery script that will turn the table into a DataTable
            sb.Append("<script>");
            sb.Append("$(function(){");
            sb.Append("$('#").Append(Model.TableId).Append("').DataTable(").Append(Model.JqueryDataTableOptions)
                .Append(");");

            sb.Append("$('#").Append(Model.TableId).Append("_filter').css('margin-bottom', '10px');");

            sb.Append("});");
            sb.Append("</script>");
            //now, we can create the actual table
            sb.Append("<table id='" + Model.TableId + "'>");


            //create the columns and abide by SpecialAliases
            sb.Append("<thead>");
            foreach (DataColumn col in Model.Results.DS.Tables[Model.TableIndex].Columns)
            {
                //If programmer has defined aliases and alias exist for this column use alias programmer specified
                string SpecialColumnValue = "";
                if (Model.SpecialColumnAliases != null &&
                    Model.SpecialColumnAliases.TryGetValue(col.Caption, out SpecialColumnValue))
                    sb.Append("<th class='" + col.Caption + "'>" + SpecialColumnValue + "</th> ");
                else if (!Model.ProperCase)
                    sb.Append("<th class='" + col.Caption + "'>" + col.Caption + "</th> ");
                else
                    sb.Append("<th class='" + col.Caption + "'>" + col.Caption.ToProperCase() + "</th> ");
            }

            sb.Append("</tr>");
            sb.Append("</thead>");

            //now create the body
            sb.Append("<tbody>");
            int ColCounter = 0;
            for (int i = 0; i < Model.Results.DS.Tables[Model.TableIndex].Rows.Count; i++)
            {
                sb.Append("<tr>");
                foreach (object cell in Model.Results.DS.Tables[Model.TableIndex].Rows[i].ItemArray)
                {
                    sb.Append("<td class='" + Model.Results.DS.Tables[Model.TableIndex].Columns[ColCounter].Caption +
                              "'>" + cell + "</td> ");
                    ColCounter++;
                }

                ColCounter = 0;
                sb.Append("</tr>");
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            return new HtmlString(sb.ToString());
        }


        private static string CreateDefaultStyles()
        {
            StringBuilder style = new StringBuilder();
            style.Append("<style> ");
            style.Append(".SmartGrid_Wrapper {width: 100%;} ");
            style.Append(".SmartGrid{color: #000;border-collapse: collapse;width:100%;} ");
            style.Append(
                ".SmartGrid th{color:#fff;background-color:rgb(58,102,102); font-weight:bold;font-size:medium;padding:3px;} ");
            style.Append(".SmartGrid tr td{border:1px solid gray;font-size:medium; padding:4px;} ");
            style.Append(
                ".SmartGridProperties .RecordMessage{float: right; text-align: right;width: 40%;display: inline-block;} ");
            style.Append(
                ".SmartGridProperties .TableCaption{color: black;font-weight: bold;font-size: 1.2em;padding: 10px 0 5px 0;text-align: left;float:left;width:60%;margin-bottom:-12px} ");
            style.Append(
                ".SmartGridProperties .GridPager, .GridPager{width:auto;float:right;text-align:right;margin:auto;font-size:1.2em;} ");
            style.Append(".GridPager.PagerCurrentPage, .RecordMessage .MinDisplayNum, ");
            style.Append(".RecordMessage .MaxDisplayNum, .RecordMessage .TotalDisplayNum, ");
            style.Append(".GridPager .PagerCaption{font-weight:bold;} ");
            style.Append(".GridPager .PagerPageNum a, .GridPager .PagerDot a {color:grey; font-weight:bold;}");
            style.Append(".GridPager .PagerCurrentPage {font-weight:bold;}");
            style.Append("</style> ");

            return style.ToString();
        }
    }
}