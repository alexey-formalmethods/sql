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
        
    }
}
