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
            SearchResults model)
        {
            if (string.IsNullOrWhiteSpace(model.Results.Error))
            {
                int colCounter = 0;
                int minDisplayRow = 0;
                int maxDisplayRow = 0;
                string lblCount = "";
                int maxPageRange = 0;
                int minPageRange = 1;
                string pageRow = "";
                int pageCounter = 1;
                bool nextOnly = false;
                bool previousOnly = false;
                bool bothNextAndPrevious = false;
                string pageControllerName = helper.ViewContext.RouteData.GetRequiredString("controller");
                string pageAcion = helper.ViewContext.RouteData.GetRequiredString("action");
                string result = CreateDefaultStyles();

                //Set Up Variables for paging
                if (model.TotalPages != 1 && model.CurrentPage != 1)
                {
                    minDisplayRow = model.RecordsPerPage * model.CurrentPage - model.RecordsPerPage;
                    maxDisplayRow = minDisplayRow + model.RecordsPerPage - 1;
                }
                else
                {
                    maxDisplayRow = model.RecordsPerPage - 1;
                }

                if (maxDisplayRow >= model.RecordCount)
                {
                    maxDisplayRow = model.RecordCount - 1;
                }

                //Set Up label to show what records the user is currently viewing
                if (model.RecordCount == 1)
                {
                    lblCount = "Showing 1 Record";
                }
                else if (model.RecordCount > model.RecordsPerPage)
                {
                    lblCount = "Showing records <span class='MinDisplayNum'>" + (minDisplayRow + 1) +
                               "</span> thru <span class='MaxDisplayNum'>"
                               + (maxDisplayRow + 1) + "</span> of <span class='TotalDisplayNum'>" + model.RecordCount +
                               "</span> ";
                }
                else
                {
                    lblCount = "Showing records <span class='MinDisplayNum'>1</span> thru <span class='MaxDisplayNum'>"
                               + model.RecordCount + "</span> of <span class='TotalDisplayNum'> " + model.RecordCount +
                               "</span> ";
                }

                result += "<div id='" + model.TableId + "_Wrappper'class='SmartGrid_Wrapper'> ";

                if (model.RecordCount == model.RecordLimit)
                {
                    result += "<span class='error'> Your request retrieved many results. Only " + model.RecordLimit +
                              " results were returned. Please narrow your search criteria to retrieve better results.</span>";
                }

                result += "<div id='" + model.TableId + "_GridProperties' class='SmartGridProperties' >";
                result += "<div id='" + model.TableId + "_RecordMessage' class='RecordMessage'><span>" + lblCount +
                          "</span></div> ";

                if (!string.IsNullOrWhiteSpace(model.TableCaption))
                {
                    result += "<span class='TableCaption'>" + model.TableCaption + "</span> ";
                }

                //Display Paging Algorithm 
                pageRow = "<span class='GridPager'><table><tbody><tr><td><span class='PagerCaption'>Page</span></td> ";
                maxPageRange = (int) (Math.Ceiling((decimal) model.CurrentPage / 10) * 10);

                //When Page One
                if (maxPageRange <= 10 && model.TotalPages <= 10)
                {
                    maxPageRange = model.TotalPages;
                }

                if (maxPageRange >= model.TotalPages && maxPageRange > 11)
                {
                    maxPageRange = model.TotalPages;
                    previousOnly = true;
                }

                if (maxPageRange > 11)
                {
                    minPageRange = maxPageRange - 10;
                }
                else if (maxPageRange != model.TotalPages)
                {
                    maxPageRange++;
                    nextOnly = true;
                }

                if (!nextOnly && !previousOnly && maxPageRange > 11)
                {
                    bothNextAndPrevious = true;
                    maxPageRange++;
                }

                while (minPageRange <= maxPageRange)
                {
                    if (pageCounter == 1 && (previousOnly || bothNextAndPrevious) ||
                        minPageRange == maxPageRange && (nextOnly || bothNextAndPrevious))
                    {
                        pageRow += "<td class='PagerDot'>" + helper.ActionLink("...",
                                       pageAcion, pageControllerName, new {PageNumber = minPageRange.ToString()},
                                       null) + "</td> ";
                    }
                    else
                    {
                        //Bold CurrentPage
                        if (minPageRange == model.CurrentPage)
                        {
                            pageRow += "<td class='PagerCurrentPage'>" + minPageRange + "</td> ";
                        }
                        else
                        {
                            pageRow += "<td class='PagerPageNum'>" + helper.ActionLink(minPageRange.ToString(),
                                           pageAcion, pageControllerName,
                                           new {PageNumber = minPageRange.ToString()}, null) + "</td> ";
                        }
                    }

                    minPageRange++;
                    pageCounter++;
                }

                if (model.PagerPosition == PagerPosition.Top || model.PagerPosition == PagerPosition.Both)
                {
                    result += pageRow + "</tr></tbody></table></span></div> ";
                }

                result += "<table id='" + model.TableId +
                          "' class='SmartGrid' role='grid'><tbody><tr data-sortable='true'> ";
                foreach (DataColumn col in model.Results.Ds.Tables[model.TableIndex].Columns)
                {
                    //If programmer has defined aliases and alias exist for this column use alias programmer specified
                    string specialColumnValue = "";
                    if (model.SpecialColumnAliases != null &&
                        model.SpecialColumnAliases.TryGetValue(col.Caption, out specialColumnValue))
                    {
                        result += "<th class='" + col.Caption + "'>" + specialColumnValue + "</th> ";
                    }
                    else
                    {
                        result += "<th class='" + col.Caption + "'>" + col.Caption.ToProperCase() + "</th> ";
                    }
                }

                result += "</tr> ";

                while (minDisplayRow <= maxDisplayRow)
                {
                    result += "<tr> ";
                    foreach (object cell in model.Results.Ds.Tables[model.TableIndex].Rows[minDisplayRow].ItemArray)
                    {
                        result += "<td class='" +
                                  model.Results.Ds.Tables[model.TableIndex].Columns[colCounter].Caption + "'>" +
                                  cell + "</td> ";
                        colCounter++;
                    }

                    result += "</tr ";
                    colCounter = 0;
                    minDisplayRow++;
                }


                result += "</tbody></table>";

                if (model.PagerPosition == PagerPosition.Bottom || model.PagerPosition == PagerPosition.Both)
                {
                    result += pageRow + "</tr></tbody></table></span>";
                }

                result += "</div>";
                return helper.Raw(result);
            }

            return helper.Raw("<span class='error'>" + model.Results.Error + "</span>");
        }

        /// <summary>
        ///     This is a helper control for constructing a DataTable, all this does is generate a table and add the required
        ///     information
        /// </summary>
        public static IHtmlString CreateDataTable<TModel>(this HtmlHelper<TModel> helper,
            DataTableGridView model)
        {
            StringBuilder sb = new StringBuilder();

            //first off check and see if we have to create the jQuery and DataTable script and link tags

            //jQuery
            if (!model.DisableJqueryScript)
            {
                sb.Append("<script src='" + model.JqueryScriptUrl + "'></script>");
            }

            //jQuery DataTables JS
            if (!model.DisableDataTablesScript)
            {
                sb.Append("<script src='" + model.JqueryDataTableScriptUrl + "'></script>");
            }

            //jQuery DataTables CSS
            if (!model.DisableDataTablesStyles)
            {
                sb.Append("<link rel='styleheet' type='text/css' href='" + model.JqueryDataTableCssUrl + "' />");
            }

            //now build up the JQuery script that will turn the table into a DataTable
            sb.Append("<script>");
            sb.Append("$(function(){");
            sb.Append("$('#").Append(model.TableId).Append("').DataTable(").Append(model.JqueryDataTableOptions)
                .Append(");");

            sb.Append("$('#").Append(model.TableId).Append("_filter').css('margin-bottom', '10px');");

            sb.Append("});");
            sb.Append("</script>");
            //now, we can create the actual table
            sb.Append("<table id='" + model.TableId + "'>");


            //create the columns and abide by SpecialAliases
            sb.Append("<thead>");
            foreach (DataColumn col in model.Results.Ds.Tables[model.TableIndex].Columns)
            {
                //If programmer has defined aliases and alias exist for this column use alias programmer specified
                string specialColumnValue = "";
                if (model.SpecialColumnAliases != null &&
                    model.SpecialColumnAliases.TryGetValue(col.Caption, out specialColumnValue))
                {
                    sb.Append("<th class='" + col.Caption + "'>" + specialColumnValue + "</th> ");
                }
                else if (!model.ProperCase)
                {
                    sb.Append("<th class='" + col.Caption + "'>" + col.Caption + "</th> ");
                }
                else
                {
                    sb.Append("<th class='" + col.Caption + "'>" + col.Caption.ToProperCase() + "</th> ");
                }
            }

            sb.Append("</tr>");
            sb.Append("</thead>");

            //now create the body
            sb.Append("<tbody>");
            int colCounter = 0;
            for (int i = 0; i < model.Results.Ds.Tables[model.TableIndex].Rows.Count; i++)
            {
                sb.Append("<tr>");
                foreach (object cell in model.Results.Ds.Tables[model.TableIndex].Rows[i].ItemArray)
                {
                    sb.Append("<td class='" + model.Results.Ds.Tables[model.TableIndex].Columns[colCounter].Caption +
                              "'>" + cell + "</td> ");
                    colCounter++;
                }

                colCounter = 0;
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