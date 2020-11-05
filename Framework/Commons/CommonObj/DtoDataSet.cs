using System.Data;
using System.Runtime.Serialization;

namespace Framework.Commons.CommonObj
{
    /// <summary>
    ///     Object that returns a Dataset and and a string for error messages.
    /// </summary>
    [DataContract]
    public class DtoDataSet
    {
        /// <summary>
        ///     Error Message
        /// </summary>
        [DataMember]
        public string Error { get; set; }

        /// <summary>
        ///     Results from the database call
        /// </summary>
        [DataMember]
        public DataSet Ds { get; set; }
    }
}