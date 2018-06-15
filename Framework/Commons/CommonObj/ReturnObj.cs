namespace Framework.Commons.CommonObj
{
    /// <summary>
    ///     Object that contains a long primative and a string for error messages.
    /// </summary>
    public class ReturnObj
    {
        /// <summary>
        ///     Error message
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        ///     Results returned from database call
        /// </summary>
        public long Result { get; set; }
    }
}