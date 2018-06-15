using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Framework.Security
{
    /// <summary>
    ///     Validate field values in web application
    /// </summary>
    public static class HtmlValidator
    {
        /// <summary>
        ///     Tells what validations to call in ValidateAll method
        /// </summary>
        public enum ValidateCheck
        {
            /// <summary>
            ///     Tells ValidateAll to call the CheckRequired method
            /// </summary>
            Required,

            /// <summary>
            ///     Tells ValidateAll to call the CheckValidText method
            /// </summary>
            ValidText,

            /// <summary>
            ///     Tells ValidateAll to call the CheckValidYear method
            /// </summary>
            ValidYear,

            /// <summary>
            ///     Tells ValidateAll to call the CheckValidNumber method
            /// </summary>
            ValidNumber,

            /// <summary>
            ///     Tells ValidateAll to call the CheckValidUrl Method
            /// </summary>
            ValidUrl,

            /// <summary>
            ///     Tells ValidateAll to call the CheckValidNumberMoney Method
            /// </summary>
            ValidNumberMoney,

            /// <summary>
            ///     Tells ValidateAll to call the CheckValidNumberMoneyAllowNegatives Method
            /// </summary>
            ValidNumberMoneyAllowNegatives
        }

        /// <summary>
        ///     Checks to see if field's text is filled
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool CheckRequired(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Checks to see if field's text matches a regular expression
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <param name="regExpression">Regular Expression to check against</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool CheckAgainstRegularExpression(string text, string regExpression)
        {
            if (!new Regex(regExpression).IsMatch(text))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Checks to see if one of the values passed in is not blank or -1 (for dropdownlists)
        /// </summary>
        /// <param name="values">Values to check</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool CheckOneValueNotBlank(params string[] values)
        {
            foreach (string value in values)
            {
                if (!string.IsNullOrWhiteSpace(value) && value != "-1")
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Checks to see how many values passed in are not blank or -1 (for dropdownlists)
        /// </summary>
        /// <param name="numNotBlank">Number of non-blank fields in order to pass validation</param>
        /// <param name="values">Values to check</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool CheckValuesNotBlank(int numNotBlank, params string[] values)
        {
            int countNotBlank = 0;
            foreach (string value in values)
            {
                if (!string.IsNullOrWhiteSpace(value) && value != "-1")
                {
                    countNotBlank++;
                }
            }

            if (countNotBlank >= numNotBlank)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Checks to see if passed in value is alphanumeric, ampersand, dash, comma, period, single quote, space, forward
        ///     slash, dollar sign, and/or question mark if passed in value is not blank or just whitespace.
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool CheckValidText(string text)
        {
            // Blank text will cause regular expression to fail automatically.
            if (!string.IsNullOrWhiteSpace(text))
            {
                return CheckAgainstRegularExpression(text, @"^[a-zA-Z0-9 &\-,.'\/\s\$\?]*$");
            }

            return true;
        }

        /// <summary>
        ///     Checks to see if the string is a valid url
        /// </summary>
        /// <param name="url">String to validate</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool CheckValidUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                return CheckAgainstRegularExpression(url, @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$");
            }

            return true;
        }

        /// <summary>
        ///     Checks to see if passed in value consists of 4 numbers if passed in value is not blank or just whitespace.
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool CheckValidYear(string text)
        {
            // Blank text will cause regular expression to fail automatically.
            if (!string.IsNullOrWhiteSpace(text))
            {
                return CheckAgainstRegularExpression(text, @"^\d{4}");
            }

            return true;
        }


        /// <summary>
        ///     Checks to see if passed in value is a numberic field
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool CheckValidNumber(string text)
        {
            if (!new Regex("^[0-9]*$").IsMatch(text))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Checks to see if passed in value is a valid money field.  Doesn't allow for negatives.
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool CheckValidNumberMoney(string text)
        {
            if (!new Regex(@"^(?:\$)?(?:\d+|\d{1,3}(?:,\d{3})*)(?:\.\d{1,2})?$").IsMatch(text))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Checks to see if passed in value is a valid money field.  Allows for negatives.
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool CheckValidNumberMoneyAllowNegatives(string text)
        {
            if (!new Regex(@"^(?:\$)?(?:-)?(?:\d+|\d{1,3}(?:,\d{3})*)(?:\.\d{1,2})?$").IsMatch(text))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Calls different validations based on passed in ValidateChecks
        /// </summary>
        /// <param name="error">Error message that will have message added too if validation fails.</param>
        /// <param name="textToValidate">Text to check</param>
        /// <param name="fieldName">The field name that will be used in error message if validation fails.</param>
        /// <param name="validateChecks">Listing of ValidateCheck value that tells what validation rules to check.</param>
        /// <returns>true if valid and false if not valid.  An error message is included if validation fails.</returns>
        public static bool ValidateAll(ref string error, string textToValidate, string fieldName,
            params ValidateCheck[] validateChecks)
        {
            bool isValidated = true;
            foreach (ValidateCheck validateCheck in validateChecks)
            {
                switch (validateCheck)
                {
                    case ValidateCheck.Required:
                        if (!CheckRequired(textToValidate))
                        {
                            error += "Please enter a value in the '" + fieldName + "' field.<br/>";
                            isValidated = false;
                        }

                        break;
                    case ValidateCheck.ValidText:
                        if (!CheckValidText(textToValidate))
                        {
                            error += "Please enter a valid " + fieldName + ".<br/>";
                            isValidated = false;
                        }

                        break;
                    case ValidateCheck.ValidYear:
                        if (!CheckValidYear(textToValidate))
                        {
                            error += "Please enter a valid year in the '" + fieldName + "' field.<br/>";
                            isValidated = false;
                        }

                        break;
                    case ValidateCheck.ValidNumber:
                        if (!CheckValidNumber(textToValidate))
                        {
                            error += "Please enter a valid number in the '" + fieldName + "' field.<br/>";
                            isValidated = false;
                        }

                        break;
                    case ValidateCheck.ValidUrl:
                        if (!CheckValidUrl(textToValidate))
                        {
                            error += "Please enter a valid url in the '" + fieldName + "' field.<br/>";
                            isValidated = false;
                        }

                        break;
                    case ValidateCheck.ValidNumberMoney:
                        if (!CheckValidNumberMoney(textToValidate))
                        {
                            error += "Please enter a valid money value in the '" + fieldName + "' field.<br/>";
                            isValidated = false;
                        }

                        break;
                    case ValidateCheck.ValidNumberMoneyAllowNegatives:
                        if (!CheckValidNumberMoneyAllowNegatives(textToValidate))
                        {
                            error += "Please enter a valid money value in the '" + fieldName + "' field.<br/>";
                            isValidated = false;
                        }

                        break;
                }
            }

            return isValidated;
        }

        /// <summary>
        ///     Calls different validations based on passed in ValidateChecks
        /// </summary>
        /// <param name="textToValidate">Text to check</param>
        /// <param name="validateChecks">Listing of ValidateCheck value that tells what validation rules to check.</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool ValidateAll(string textToValidate, params ValidateCheck[] validateChecks)
        {
            bool isValidated = true;
            foreach (ValidateCheck validateCheck in validateChecks)
            {
                switch (validateCheck)
                {
                    case ValidateCheck.Required:
                        isValidated = CheckRequired(textToValidate);
                        break;
                    case ValidateCheck.ValidText:
                        isValidated = CheckValidText(textToValidate);
                        break;
                    case ValidateCheck.ValidYear:
                        isValidated = CheckValidYear(textToValidate);
                        break;
                    case ValidateCheck.ValidNumber:
                        isValidated = CheckValidNumber(textToValidate);
                        break;
                    case ValidateCheck.ValidNumberMoney:
                        isValidated = CheckValidNumberMoney(textToValidate);
                        break;
                    case ValidateCheck.ValidNumberMoneyAllowNegatives:
                        isValidated = CheckValidNumberMoneyAllowNegatives(textToValidate);
                        break;
                }

                if (!isValidated)
                {
                    return false;
                }
            }

            return isValidated;
        }

        /// <summary>
        ///     Checks to see if value of file path passed in has a valid extension of given value
        /// </summary>
        /// <param name="Filename">Full path of file to be validated</param>
        /// <param name="ValidFileTypes">Listing of valid file type(s) that the File Name is checked against</param>
        /// <returns>true if valid file type(s) and false if not valid filetype(s)</returns>
        public static bool CheckFileType(string Filename, params string[] ValidFileTypes)
        {
            string Extension = Path.GetExtension(Filename);
            foreach (string value in ValidFileTypes)
            {
                if (Extension.ToLower() == value)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks to see if passed in value is alphanumeric, dash, or underscore only.
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>true if valid and false if not valid</returns>
        //public static bool CheckURLText(String text)
        //{
        //    return CheckAgainstRegularExpression(text, @"^[a-zA-Z0-9 \-\_");
        //}

        /// <summary>
        ///     Checks to see if passed in value is alphanumeric, dash, or underscore only.
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>true if valid and false if not valid</returns>
        public static bool CheckURLText(string text)
        {
            return CheckAgainstRegularExpression(text, @"^[a-zA-Z0-9 \-\_");
        }

        /// <summary>
        ///     Checks to see if the datetime passed in is as old as the age_required.
        /// </summary>
        /// <param name="date">Date you are checking against</param>
        /// <param name="age_Required">The age that is required</param>
        /// <returns>true if the date is as old as age_Required, false if not</returns>
        public static bool CheckValidBirthday(string date, double age_Required)
        {
            var values = age_Required.ToString().Split('.');
            int year = int.Parse(values[0]);
            int months = int.Parse(values[1]);
            DateTime required_age_as_date = DateTime.Now.AddYears(-year).AddMonths(-months);
            return DateTime.Parse(date).Date <= required_age_as_date.Date;
        }
    }
}