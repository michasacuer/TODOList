using System.Text.RegularExpressions;

namespace TODOList
{
    public static class MailValidation
    {
        /// <summary>
        /// Checking if mail is valid
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns>True if mail is valid otherwise false</returns>
        public static bool IsEmailValid(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
    }
}
