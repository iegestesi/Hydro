using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydro_Auth
{
    public class csSettings
    {

        public int PrimaryKey { get { return _PrimaryKey; } }
        public string ClientID { get { return _ClientID; } }
        public string ClientSecret { get { return _ClientSecret; } }
        public string ApplicationID { get { return _ApplicationID; } }
        public string Hydro_ID { get { return _Hydro_ID; } }
        public string AccesToken { get { return _AccesToken; } }
        public string RefreshToken { get { return _RefreshToken; } }
        public string UrlSandBoxToken { get { return _UrlSandBoxToken; } }
        public string UrlApiToken { get { return _UrlApiToken; } }
        public string PostSandBox { get { return _PostSandBox; } }
        public string PostApi { get { return _PostApi; } }
        public string ApiGetVerify { get { return _ApiGetVerify; } }
        public string ApiSandBoxVerify { get { return _ApiSandBoxVerify; } }
        public string Message { get { return _Message; } }
        public string ErrorMessage { get { return _ErrorMessage; } }
        public string Demo { get { return _Demo; } }

        public int _PrimaryKey { get; set; }
        public string _ClientID { get; set; }
        public string _ClientSecret { get; set; }
        public string _ApplicationID { get; set; }
        public string _Hydro_ID { get; set; }
        public string _AccesToken { get; set; }
        public string _RefreshToken { get; set; }
        public string _UrlSandBoxToken { get; set; }
        public string _UrlApiToken { get; set; }
        public string _PostSandBox { get; set; }
        public string _PostApi { get; set; }
        public string _ApiGetVerify { get; set; }
        public string _ApiSandBoxVerify { get; set; }
        public string _Message { get; set; }
        public string _ErrorMessage { get; set; }
        public string _Demo { get; set; }
    }
}
