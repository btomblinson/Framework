using System;
using System.Collections.Generic;
using System.Data;
using Framework.Commons.CommonObj;

namespace Framework.UI.BusinessObj
{
    public class SearchResults
    {
        public SearchResults()
        {
            //Random Number for Table Id
            Random randomNum = new Random();
            Results = new DtoDataSet();
            RecordsPerPage = 50;
            RecordLimit = int.MaxValue;
            CurrentPage = 1;
            TableIndex = 0;
            TotalPages = 1;
            RecordCount = 0;
            TableCaption = "Search Results";
            TableId = "SmartGrid" + randomNum.Next(0, 1000);
            PagerPosition = PagerPosition.Top;
            SpecialColumnAliases = null;
        }

        public SearchResults(DtoDataSet results, string tableId = "", int recordsPerPage = 50,
            int recordLimit = int.MaxValue, int currentPage = 1, int tableIndex = 0,
            string tableCaption = "Search Results", PagerPosition pagerPosition = PagerPosition.Top,
            Dictionary<string, string> specialColumnAliases = null) : this()
        {
            //OverWrite Default Values From Default Constructor Value If Need To
            TableIndex = tableIndex;
            RecordsPerPage = recordsPerPage;
            RecordLimit = recordLimit;
            TableCaption = tableCaption;
            PagerPosition = pagerPosition;
            SpecialColumnAliases = specialColumnAliases;
            //If user doesn't specify an Id field stick with the random generated Id if its provided.  Else generate another unique id
            if (!string.IsNullOrWhiteSpace(tableId))
            {
                TableId = tableId;
            }


            //If its null want to set it to new object to display error messages
            if (results == null)
            {
                Results = new DtoDataSet();
            }
            else
            {
                Results = results;
            }


            if (string.IsNullOrWhiteSpace(Results.Error) && Results.Ds.Tables != null
                                                         && Results.Ds.Tables.Count != 0 &&
                                                         Results.Ds.Tables[TableIndex].Rows.Count != 0)
            {
                if (Results.Ds.Tables[TableIndex].Rows.Count > RecordLimit)
                {
                    Results.Ds = ReplaceTable(Results.Ds, TableIndex,
                        GetNRows(Results.Ds.Tables[TableIndex], RecordLimit));
                }

                RecordCount = Results.Ds.Tables[TableIndex].Rows.Count;
                TotalPages = CountPages(RecordCount, RecordsPerPage);
            }

            if (CurrentPage != currentPage && currentPage > 0 && currentPage <= TotalPages)
            {
                CurrentPage = currentPage;
            }
        }

        public DtoDataSet Results { get; set; }

        public int RecordCount { get; set; }

        public int TotalPages { get; set; }

        public int RecordsPerPage { get; set; }

        public int RecordLimit { get; set; }

        public int CurrentPage { get; set; }

        public int TableIndex { get; set; }

        public string TableCaption { get; set; }

        public string TableId { get; set; }

        public PagerPosition PagerPosition { get; set; }

        public Dictionary<string, string> SpecialColumnAliases { get; set; }

        private DataTable GetNRows(DataTable currentTable, int limit)
        {
            DataTable temp = new DataTable();
            foreach (DataColumn collumn in currentTable.Columns)
            {
                temp.Columns.Add(collumn.ColumnName, collumn.DataType);
            }

            for (int i = 0; i < limit; i++)
            {
                DataRow row = temp.Rows.Add();
                foreach (DataColumn collumn in currentTable.Columns)
                {
                    row[collumn.ColumnName] = currentTable.Rows[i][collumn.ColumnName];
                }
            }

            return temp;
        }

        private DataSet ReplaceTable(DataSet currentDataset, int index, DataTable additionalTable)
        {
            DataSet temp = new DataSet();

            for (int i = 0; i < currentDataset.Tables.Count; i++)
            {
                if (i != index)
                {
                    temp.Tables.Add(currentDataset.Tables[i]);
                }
                else
                {
                    temp.Tables.Add(additionalTable);
                }
            }

            return temp;
        }

        private int CountPages(int totalRecords, int recordsPerPage)
        {
            if (totalRecords == 0 || totalRecords <= recordsPerPage)
            {
                return 1;
            }

            return totalRecords % recordsPerPage == 0
                ? totalRecords / recordsPerPage
                : totalRecords / recordsPerPage + 1;
        }
    }

    public enum PagerPosition
    {
        Top = 1,
        Bottom = 2,
        Both = 3
    }
}