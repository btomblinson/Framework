namespace Framework.Commons.CommonObj
{
    /// <summary>Used as return object for checking if file has passed validation or not</summary>
    public class FileValidationResult
    {
        /// <summary>Does the file pass validation?</summary>
        public bool IsValid { get; set; }

        /// <summary>Message being returned after validation process.  Could be success message or reason for failure</summary>
        public string Message { get; set; }
    }
}