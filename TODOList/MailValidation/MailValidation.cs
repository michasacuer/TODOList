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
            =>  new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                          @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                          @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").IsMatch(inputEmail);
    }
}
