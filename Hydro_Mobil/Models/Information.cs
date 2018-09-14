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
    public static class Information
    {
        public static List<Members> LoginKontrol(string strUserName, string strPassword)
        {
            var admList = from adm in cMembers
                          where adm.UserName == strUserName && adm.PassWord == strPassword
                          select adm;

            return admList.ToList();
        }

        public static bool AutAddSandBox(string strHydroID,out string strMessage)
        {
            strMessage = "";
            bool blnResult = false;
            try
            {
                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(cSettings[0].PostSandBox);
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
        public static bool AutAddApi(string strHydroID, out string strMessage)
        {
            strMessage = "";
            bool blnResult = false;
            try
            {
                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(cSettings[0].PostApi);
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
        public static bool GetAccesTokenSandBox(out string strMessage)
        {
            bool blnResult = false;
            try
            {
                string strClientSecretID = cSettings[0].ClientID + ":" + cSettings[0].ClientSecret;
                strMessage = "";

                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(cSettings[0].UrlSandBoxToken);
                request.Method = "POST";
                request.ContentType = "application/json; charset=UTF-8";

                CredentialCache mycache = new CredentialCache();
                mycache.Add(new Uri(cSettings[0].UrlSandBoxToken), "Basic", new NetworkCredential(cSettings[0].ClientID, cSettings[0].ClientSecret));
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
        public static bool GetAccesTokenApi(out string strMessage)
        {
            bool blnResult = false;
            try
            {
                string strClientSecretID = cSettings[0].ClientID + ":" + cSettings[0].ClientSecret;
                strMessage = "";

                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(cSettings[0].UrlApiToken);
                request.Method = "POST";
                request.ContentType = "application/json; charset=UTF-8";

                CredentialCache mycache = new CredentialCache();
                mycache.Add(new Uri(cSettings[0].UrlApiToken), "Basic", new NetworkCredential(cSettings[0].ClientID, cSettings[0].ClientSecret));
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
        public static bool GetVerifyMessageSandBox(string strHydroID,string strMessage,out string strNotMessage)
        {
            bool blnResult = false;
            strNotMessage = "";

            try
            {
                string strUrl = cSettings[0].ApiSandBoxVerify + "?message=" + strMessage + "&hydro_id=" + strHydroID + "&application_id=" + cSettings[0].ApplicationID;
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
                        cMembers[0].HydroID = strHydroID;
                        cMembers[0].Auth = true;
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
        public static bool GetVerifyMessageApi(string strHydroID, string strMessage, out string strNotMessage)
        {
            bool blnResult = false;
            strNotMessage = "";

            try
            {
                string strUrl = cSettings[0].ApiGetVerify + "?message=" + strMessage + "&hydro_id=" + strHydroID + "&application_id=" + cSettings[0].ApplicationID;
                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(strUrl);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + cSettings[0].AccesToken);

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        strNotMessage = streamReader.ReadToEnd();
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
        //public static bool GetDeleteApi()
        //{
        //    var client = new RestClient(strUrlSandbox);// + "?hydro_id=" + teHydroUserName.Text + "&application_id=" + strApplicationID);

        //    //ParaMeters p = new ParaMeters { hydro_id = teHydroUserName.Text, application_id = strApplicationID };

        //    var request = new RestRequest(Method.DELETE);
        //    request.AddHeader("authorization", "Bearer " + strAccesToken + "");

        //    //var body = new { hydro_id = teHydroUserName.Text, application_id = strApplicationID };
        //    //request.AddParameter("application/json", body, ParameterType.RequestBody);
        //    request.AddParameter("hydro_id", teHydroUserName.Text);
        //    request.AddParameter("application_id", strApplicationID);
        //    //request.AddJsonBody(body);
        //    IRestResponse response = client.Execute(request);

        //    if (string.IsNullOrEmpty(response.Content.ToString()))
        //    {
        //        lblError.Text = "Silme Başarılı";
        //    }
        //    else
        //    {
        //        lblError.Text = response.Content.ToString();
        //    }
        //}

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
                    Demo = ""
                }
            };

        public static List<Members> cMembers = new List<Members>
            {
                new Models.Members {
                    MembersID = 1,
                    UserName = "Mehmet",
                    PassWord = "1",
                    HydroID = ""
                    Auth = false
                }
            };
    }
    public class RefreshTokenResultJSON
    {
        public string access_token { get; set; }
    }
}