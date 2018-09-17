using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Net;
using System.Text;
using System.IO;
using System.Web.Configuration;

namespace Hydro_Mobil.Models
{
    public class Information
    {
        TablolarContext db = new TablolarContext();
        public List<Members> LoginKontrol(string strUserName, string strPassword)
        {
            var memList = from mem in db.Member
                          where mem.UserName == strUserName && mem.PassWord == strPassword
                          select mem;

            return memList.ToList();
        }
        public void MembersHydroEdit(int dMembersID, string strHydroID, bool blnAuth)
        {
            Members memEdit = (from g in db.Member where g.MembersID == dMembersID select g).FirstOrDefault();
            memEdit.HydroID = strHydroID;
            memEdit.Auth = blnAuth;
            db.SaveChanges();
        }
        public static bool GetAccesToken(string strDemo, out string strMessage)
        {
            bool blnResult = false;
            try
            {
                string strClientSecretID = cSettings[0].ClientID + ":" + cSettings[0].ClientSecret;
                strMessage = "";
                string strUrl = "";

                if (strDemo == "SandBox")
                {
                    strUrl = cSettings[0].UrlSandBoxToken;
                }
                else
                {
                    strUrl = cSettings[0].UrlApiToken;
                }

                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(strUrl);
                request.Method = "POST";
                request.ContentType = "application/json; charset=UTF-8";

                CredentialCache mycache = new CredentialCache();
                mycache.Add(new Uri(strUrl), "Basic", new NetworkCredential(cSettings[0].ClientID, cSettings[0].ClientSecret));
                request.Credentials = mycache;
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(strClientSecretID)));

                Stream postStream = request.GetRequestStream();
                postStream.Flush();
                postStream.Close();

                using (System.Net.WebResponse response = request.GetResponse())
                {
                    using (System.IO.StreamReader streamReader = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        dynamic jsonResponseText = streamReader.ReadToEnd();
                        RefreshTokenResultJSON jsonResult = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize(jsonResponseText, typeof(RefreshTokenResultJSON));
                        cSettings[0].AccesToken = jsonResult.access_token;
                    }
                }
                blnResult = true;
            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                blnResult = false;
            }

            return blnResult;
        }
        public static bool AutAdd(string strHydroID, string strDemo, out string strMessage)
        {
            strMessage = "";
            bool blnResult = false;
            try
            {
                string strUrl = "";

                if (strDemo == "SandBox")
                {
                    strUrl = cSettings[0].PostSandBox;
                }
                else
                {
                    strUrl = cSettings[0].PostApi;
                }

                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(strUrl);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + cSettings[0].AccesToken);

                using (System.IO.StreamWriter tStreamWriter = new System.IO.StreamWriter(request.GetRequestStream()))
                {
                    tStreamWriter.Write(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(new
                    {
                        hydro_id = strHydroID,
                        application_id = cSettings[0].ApplicationID
                    }));
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        strMessage = GenerateMessage();
                    }
                }
                blnResult = true;
            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                blnResult = false;
            }

            return blnResult;
        }
        public static string GenerateMessage()
        {
            string strResult = "";
            Random rnd = new Random();

            for (int i = 0; i < 6; i++)
            {
                int sayi = rnd.Next(1, 9);
                strResult += sayi.ToString();
            }

            return strResult;
        }
        public bool GetVerifyMessage(string strHydroID, string strDemo, string strMessage, out string strNotMessage)
        {
            bool blnResult = false;
            strNotMessage = "";

            string strUrlMessage = "";

            if (strDemo == "SandBox")
            {
                strUrlMessage = cSettings[0].ApiSandBoxVerify;
            }
            else
            {
                strUrlMessage = cSettings[0].ApiGetVerify;
            }

            try
            {
                string strUrl = strUrlMessage + "?message=" + strMessage + "&hydro_id=" + strHydroID + "&application_id=" + cSettings[0].ApplicationID;
                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(strUrl);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + cSettings[0].AccesToken);

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        strNotMessage = streamReader.ReadToEnd();
                        string[] strSplit = strNotMessage.Split(',');
                        strNotMessage = strSplit[0].ToString();
                        strNotMessage = strNotMessage.Replace("{", "").Replace("\"", "").Replace(":", " ");
                    }
                }

                blnResult = true;
            }
            catch (Exception ex)
            {
                strNotMessage = ex.Message.ToString();
                blnResult = false;
            }

            return blnResult;
        }
        public static bool GetDeleteApi(string strHydroID, string strDemo, out string strMessage)
        {
            strMessage = "";
            bool blnResult = false;
            try
            {
                string strUrlMessage = "";

                if (strDemo == "SandBox")
                {
                    strUrlMessage = cSettings[0].PostSandBox;
                }
                else
                {
                    strUrlMessage = cSettings[0].PostApi;
                }
                string strUrl = strUrlMessage + "?message=" + strMessage + "&hydro_id=" + strHydroID + "&application_id=" + cSettings[0].ApplicationID;
                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(strUrl);
                request.Method = "DELETE";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + cSettings[0].AccesToken);

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        strMessage = "Succes";
                    }
                }
                blnResult = true;
            }
            catch (Exception ex)
            {
                strMessage = ex.Message.ToString();
                blnResult = false;
            }

            return blnResult;
        }

        public static List<csSettings> cSettings = new List<csSettings>
            {
                new Models.csSettings {
                    PrimaryKey = 1,
                    ClientID = WebConfigurationManager.AppSettings["ClientID"].ToString(),
                    ClientSecret = WebConfigurationManager.AppSettings["ClientSecret"].ToString(),
                    ApplicationID = WebConfigurationManager.AppSettings["ApplicationID"].ToString(),
                    Hydro_ID = "",
                    AccesToken = "",
                    RefreshToken = "",
                    UrlSandBoxToken = "https://sandbox.hydrogenplatform.com/authorization/v1/oauth/token?grant_type=client_credentials",
                    UrlApiToken = "https://api.hydrogenplatform.com/authorization/v1/oauth/token?grant_type=client_credentials",
                    PostSandBox = "https://sandbox.hydrogenplatform.com/hydro/v1/application/client",
                    PostApi = "https://api.hydrogenplatform.com/hydro/v1/application/client",
                    ApiGetVerify = "https://api.hydrogenplatform.com/hydro/v1/verify_signature",
                    ApiSandBoxVerify = "https://sandbox.hydrogenplatform.com/hydro/v1/verify_signature",
                    Message = "",
                    Demo = "SandBox"
                }
            };
    }
    public class RefreshTokenResultJSON
    {
        public string access_token { get; set; }
    }
}