using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Util.Store;

namespace TODOList
{
    class GoogleAuth
    {
        #region Variables
        string[] Scopes = { CalendarService.Scope.Calendar };
        private UserCredential credential;
        #endregion
        #region Constructor
        public GoogleAuth()
        {

            //Creating credentials and generate token
            using(var stream =new FileStream("client_id.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                    Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
            }
            
        }
        #endregion
        #region Methods
        /// <summary>
        /// Method to get credentials
        /// </summary>
        /// <returns></returns>
        public UserCredential GetCredential()
        {
            return credential;
        }
        #endregion
    }
}
