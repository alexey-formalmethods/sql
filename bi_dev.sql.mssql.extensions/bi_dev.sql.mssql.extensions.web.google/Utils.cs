using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bi_dev.sql.mssql.extensions.web.google
{
    public static class Auth
    {
        public static GoogleCredential GetUserCredential(string accessToken)
        {
            return GoogleCredential.FromAccessToken(accessToken);
        }
        public static UserCredential GetUserCredential(string credentialsPath, string[] scopes)
        {
            UserCredential credential = null;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = new FileInfo(credentialsPath).DirectoryName + "\\token.json";

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                
            }
            return credential;
        }
        
        public static void Do(string credentialsPath, string[] scopes)
        {
            GetUserCredential(credentialsPath, scopes);
        }
    }
}
