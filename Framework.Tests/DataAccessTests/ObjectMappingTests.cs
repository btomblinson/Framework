using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DataAccess.ObjectMapping;

namespace Framework.Tests.DataAccessTests
{
    [TestFixture]
    public class ObjectMappingTests
    {
        private DataTable TestTable;

        [OneTimeSetUp]
        public void CreateDataTable()
        {
            TestTable = new DataTable();
            TestTable.Clear();
            TestTable.Columns.Add("Column1", typeof(string));
            TestTable.Columns.Add("Column2", typeof(string));
            DataRow row = TestTable.NewRow();
            row["Column1"] = "Value1";
            row["Column2"] = "Value2";
            TestTable.Rows.Add(row);

            row = TestTable.NewRow();
            row["Column1"] = "Value21";
            row["Column2"] = "Value22";
            TestTable.Rows.Add(row);
        }

        [Test]
        public void MapRowToClass()
        {
            MappingService mapper = new MappingService();

            ExampleObjectMapClass mappedObject = mapper.MapFirstDataRow<ExampleObjectMapClass>(TestTable);

            Assert.AreEqual("Value1", mappedObject.Column1);
            Assert.AreEqual("Value2", mappedObject.Column2);
        }

        [Test]
        public void MapTable()
        {
            MappingService mapper = new MappingService();

            List<ExampleObjectMapClass> mappedObject = mapper.MapDataTable<ExampleObjectMapClass>(TestTable);

            Assert.AreEqual("Value1", mappedObject[0].Column1);
            Assert.AreEqual("Value2", mappedObject[0].Column2);

            Assert.AreEqual("Value21", mappedObject[1].Column1);
            Assert.AreEqual("Value22", mappedObject[1].Column2);
        }
    }
}