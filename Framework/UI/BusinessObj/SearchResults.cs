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
            pagerPosition = PagerPosition.Top;
            SpecialColumnAliases = null;
        }

        public SearchResults(DtoDataSet _Results, string _TableId = "", int _RecordsPerPage = 50,
            int _RecordLimit = int.MaxValue, int _CurrentPage = 1, int _TableIndex = 0,
            string _TableCaption = "Search Results", PagerPosition _pagerPosition = PagerPosition.Top,
            Dictionary<string, string> _SpecialColumnAliases = null) : this()
        {
            //OverWrite Default Values From Default Constructor Value If Need To
            TableIndex = _TableIndex;
            RecordsPerPage = _RecordsPerPage;
            RecordLimit = _RecordLimit;
            TableCaption = _TableCaption;
            pagerPosition = _pagerPosition;
            SpecialColumnAliases = _SpecialColumnAliases;
            //If user doesn't specify an Id field stick with the random generated Id if its provided.  Else generate another unique id
            if (!string.IsNullOrWhiteSpace(_TableId))
                TableId = _TableId;


            //If its null want to set it to new object to display error messages
            if (_Results == null)
                Results = new DtoDataSet();
            else
                Results = _Results;


            if (string.IsNullOrWhiteSpace(Results.Error) && Results.DS.Tables != null
                                                         && Results.DS.Tables.Count != 0 &&
                                                         Results.DS.Tables[TableIndex].Rows.Count != 0)
            {
                if (Results.DS.Tables[TableIndex].Rows.Count > RecordLimit)
                    Results.DS = ReplaceTable(Results.DS, TableIndex,
                        GetNRows(Results.DS.Tables[TableIndex], RecordLimit));

                RecordCount = Results.DS.Tables[TableIndex].Rows.Count;
                TotalPages = CountPages(RecordCount, RecordsPerPage);
            }

            if (CurrentPage != _CurrentPage && _CurrentPage > 0 && _CurrentPage <= TotalPages)
                CurrentPage = _CurrentPage;
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

        public PagerPosition pagerPosition { get; set; }

        public Dictionary<string, string> SpecialColumnAliases { get; set; }

        private DataTable GetNRows(DataTable CurrentTable, int Limit)
        {
            DataTable temp = new DataTable();
            foreach (DataColumn collumn in CurrentTable.Columns) temp.Columns.Add(collumn.ColumnName, collumn.DataType);

            for (int i = 0; i < Limit; i++)
            {
                DataRow row = temp.Rows.Add();
                foreach (DataColumn collumn in CurrentTable.Columns)
                    row[collumn.ColumnName] = CurrentTable.Rows[i][collumn.ColumnName];
            }

            return temp;
        }

        private DataSet ReplaceTable(DataSet CurrentDataset, int Index, DataTable AdditionalTable)
        {
            DataSet temp = new DataSet();

            for (int i = 0; i < CurrentDataset.Tables.Count; i++)
                if (i != Index)
                    temp.Tables.Add(CurrentDataset.Tables[i]);
                else
                    temp.Tables.Add(AdditionalTable);

            return temp;
        }

        private int CountPages(int totalRecords, int recordsPerPage)
        {
            if (totalRecords == 0 || totalRecords <= recordsPerPage)
                return 1;

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