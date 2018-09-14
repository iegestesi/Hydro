using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Hydro_Mobil.Models
{
    public class csSettings
    {
        public int PrimaryKey { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string ApplicationID { get; set; }
        public string Hydro_ID { get; set; }
        public string AccesToken { get; set; }
        public string RefreshToken { get; set; }
        public string UrlSandBoxToken { get; set; }
        public string UrlApiToken { get; set; }
        public string PostSandBox { get; set; }
        public string PostApi { get; set; }
        public string ApiGetVerify { get; set; }
        public string ApiSandBoxVerify { get; set; }
        public string Message { get; set; }
        public string Demo { get; set; }
    }
}