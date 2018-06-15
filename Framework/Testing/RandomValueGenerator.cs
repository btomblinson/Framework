using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Faker;

namespace Framework.Testing
{
    public class RandomValueGenerator
    {
        private static readonly Random random = new Random();

        #region RandomDates

        /// <summary>
        ///     Generates a random DateTime since 1/1/1900
        /// </summary>
        /// <returns>A DateTime object</returns>
        public DateTime RandomDateTime()
        {
            DateTime start = new DateTime(1900, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }

        #endregion

        /// <summary>
        ///     Generates a random string of desired length
        /// </summary>
        /// <param name="minLength">The minimum length of string desired</param>
        /// <param name="maxLength">The maximum length of string desired</param>
        /// <returns>A randomly generated string</returns>
        public string RandomString(int minLength, int maxLength)
        {
            const string allowedChars =
                "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            int length = RandomNumber(minLength, maxLength);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++) sb.Append(allowedChars[RandomNumber(0, allowedChars.Length - 1)]);

            return sb.ToString();
        }

        public int GetRandomValueIntFromDropDown(List<SelectListItem> dropdown)
        {
            string value = dropdown.Select(m => m.Value).ToList()[RandomNumber(0, dropdown.Count - 1)];
            return int.TryParse(value, out int output) ? output : 0;
        }

        public int GetRandomValueIntFromDropDownRequired(List<SelectListItem> dropdown)
        {
            string value = dropdown.Select(m => m.Value).ToList()[RandomNumber(1, dropdown.Count - 1)];
            return int.TryParse(value, out int output) ? output : 0;
        }

        public string GetRandomValueStringFromDropDown(List<SelectListItem> dropdown)
        {
            return dropdown.Select(m => m.Value).ToList()[RandomNumber(0, dropdown.Count - 1)];
        }

        public string GetRandomValueStringFromDropDownRequired(List<SelectListItem> dropdown)
        {
            return dropdown.Select(m => m.Value).ToList()[RandomNumber(1, dropdown.Count - 1)];
        }

        public List<int> GetRandomValuesIntFromDropDown(List<SelectListItem> dropdown)
        {
            //first select a random number of items to get
            int count = RandomNumber(0, dropdown.Count);
            var values = new List<int>();

            while (values.Count < count)
            {
                string value = dropdown.Select(m => m.Value).ToList()[RandomNumber(0, dropdown.Count - 1)];
                int numericValue = int.TryParse(value, out int output) ? output : 0;
                if (!values.Contains(numericValue)) values.Add(numericValue);
            }

            return values;
        }

        public List<int> GetRandomValuesIntFromDropDownRequired(List<SelectListItem> dropdown)
        {
            //first select a random number of items to get
            int count = RandomNumber(0, dropdown.Count);
            var values = new List<int>();

            while (values.Count < count)
            {
                string value = dropdown.Select(m => m.Value).ToList()[RandomNumber(1, dropdown.Count - 1)];
                int numericValue = int.TryParse(value, out int output) ? output : 0;
                if (!values.Contains(numericValue)) values.Add(numericValue);
            }

            return values;
        }

        public List<string> GetRandomValuesStringFromDropDown(List<SelectListItem> dropdown)
        {
            //first select a random number of items to get
            int count = RandomNumber(0, dropdown.Count);
            var values = new List<string>();

            while (values.Count < count)
            {
                string value = dropdown.Select(m => m.Value).ToList()[RandomNumber(0, dropdown.Count - 1)];
                if (!values.Contains(value)) values.Add(value);
            }

            return values;
        }

        public List<string> GetRandomValuesStringFromDropDownRequired(List<SelectListItem> dropdown)
        {
            //first select a random number of items to get
            int count = RandomNumber(0, dropdown.Count);
            var values = new List<string>();

            while (values.Count < count)
            {
                string value = dropdown.Select(m => m.Value).ToList()[RandomNumber(1, dropdown.Count - 1)];
                if (!values.Contains(value)) values.Add(value);
            }

            return values;
        }

        #region RandomNumbers

        /// <summary>
        ///     Used to generate a random number
        /// </summary>
        /// <returns>A random number between 0 and max int</returns>
        public int RandomNumber()
        {
            return random.Next();
        }

        /// <summary>
        ///     Used to generate a random number with a max value limit inclusive
        /// </summary>
        /// <param name="maxValue">The largest random number that can be returned</param>
        /// <returns>A random number between 0 and maxValue inclusive</returns>
        public int RandomNumber(int maxValue)
        {
            return random.Next(maxValue + 1);
        }

        /// <summary>
        ///     Used to generate a random number with a min value limit and a max value limit
        /// </summary>
        /// <param name="maxValue">The largest random number that can be returned</param>
        /// <param name="minValue">The smallest random number that can be returned</param>
        /// <returns>A random number between minValue and maxValue inclusive</returns>
        public int RandomNumber(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue + 1);
        }

        public decimal RandomDecimal()
        {
            return (decimal) (random.NextDouble() * (random.Next() % 1000000));
        }

        #endregion

        #region RandomPersonalData

        /// <summary>
        ///     Generates a random full name
        /// </summary>
        /// <returns>A string for a full name</returns>
        public string RandomFullName()
        {
            return Name.FullName();
        }

        /// <summary>
        ///     Generates a random first name
        /// </summary>
        /// <returns>A string representing a first name</returns>
        public string RandomFirstName()
        {
            return Name.First();
        }

        /// <summary>
        ///     Generates a random last name
        /// </summary>
        /// <returns>A string representing a last name</returns>
        public string RandomLastName()
        {
            return Name.Last().Replace("'", "");
        }

        /// <summary>
        ///     Generates a random address
        /// </summary>
        /// <returns>A string representing an address</returns>
        public string RandomAddress()
        {
            return Address.StreetAddress();
        }

        /// <summary>
        ///     Generates a random city
        /// </summary>
        /// <returns>A string representing a city</returns>
        public string RandomCity()
        {
            return Address.City();
        }

        /// <summary>
        ///     Generates a random state
        /// </summary>
        /// <returns>A string for a US state</returns>
        public string RandomState()
        {
            return Address.UsState();
        }

        /// <summary>
        ///     Generates a random state abbreviation
        /// </summary>
        /// <returns>An abbreviation for a US state</returns>
        public string RandomStateAbbr()
        {
            return Address.UsStateAbbr();
        }

        /// <summary>
        ///     Generates a random zip code
        /// </summary>
        /// <returns>A string for a US zip code</returns>
        public string RandomZip()
        {
            return Address.ZipCode();
        }

        /// <summary>
        ///     Generates a random phone number
        /// </summary>
        /// <returns>A string representing a random phone number</returns>
        public string RandomPhoneNumber()
        {
            const string allowedChars = "0123456789";
            int length = RandomNumber(10, 10);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++) sb.Append(allowedChars[RandomNumber(0, allowedChars.Length - 1)]);

            return sb.ToString();
        }

        /// <summary>
        ///     Generates a random email address
        /// </summary>
        /// <returns>A string representing a random email address</returns>
        public string RandomEmailAddress()
        {
            return Internet.Email();
        }

        #endregion

        #region RandomCompanyInfo

        public string RandomCompanyName()
        {
            return Company.Name();
        }

        public string RandomWebsite()
        {
            return "http://" + Internet.DomainName();
        }

        #endregion

        #region RandomText

        /// <summary>
        ///     Generates a random paragraph
        /// </summary>
        /// <returns>A string</returns>
        public string RandomParagraph()
        {
            return Lorem.Paragraph();
        }

        /// <summary>
        ///     Generates a random sentence
        /// </summary>
        /// <returns>A string</returns>
        public string RandomSentence()
        {
            return Lorem.Sentence();
        }

        #endregion
    }
}