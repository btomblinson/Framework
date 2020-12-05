using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DataAccess.ObjectMapping;

namespace Framework.Tests.DataAccessTests
{

    public class ExampleObjectMapClass
    {
        [DataBinding(FieldName = "Column1")]
        public string Column1 { get; set; }

        [DataBinding(FieldName = "Column2")]
        public string Column2 { get; set; }
    }
}
