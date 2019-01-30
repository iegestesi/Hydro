using Hydro_Mobil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Hydro_Auth;

namespace Hydro_Mobil.Controllers
{
    public class HomeController : Controller
    {
        LoginProfil users = new LoginProfil();
        Information dbs = new Information();
        TablolarContext db = new TablolarContext();
        HttpCookie LoginCookie = new HttpCookie("HydroMobil");
        string strApiSandBox = "";
        string strMessage = "";

        public ActionResult Index()
        {
            LoginCookie = Request.Cookies["HydroMobil"];

            if (LoginCookie == null)
            {
                ViewBag.Title = "Login";
                return View("Login");
            }
            else
            {
                return RedirectToAction("MobilAutList");
            }
        }

        public ActionResult Login()
        {
            LoginCookie = Request.Cookies["HydroMobil"];

            if (LoginCookie == null)
            {
                ViewBag.Title = "Login";
                return View();
            }
            else
            {
                return RedirectToAction("MobilAutList");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Login(LoginProfil user)
        {
            if (ModelState.IsValid)
            {
                var Kontrol = dbs.LoginKontrol(user.UserName, user.Password).ToList();

                if (Kontrol.Count > 0)
                {
                    foreach (var item in Kontrol)
                    {
                        user.MembersID = item.MembersID;
                        user.UserName = item.UserName;
                        user.Password = item.PassWord;
                        user.HydroID = item.HydroID;
                        user.Auth = item.Auth;
                    }

                    LoginCookie["MembersID"] = user.MembersID.ToString();
                    LoginCookie["UserName"] = user.UserName.ToString();
                    LoginCookie["Password"] = user.Password.ToString();
                    LoginCookie["HydroID"] = user.HydroID.ToString();
                    LoginCookie["Auth"] = user.Auth.ToString();
                    LoginCookie.Expires = DateTime.Now.AddDays(2);
                    Response.Cookies.Add(LoginCookie);

                    ViewBag.Title = "Hydro Mobil";

                    if (Convert.ToBoolean(user.Auth.ToString()))
                    {
                        return RedirectToAction("MobilAuthControl");
                    }
                    else
                    {
                        return RedirectToAction("MobilAutList");
                    }
                }
                else
                {
                    return View();
                }
            }
            return View(user);
        }

        public ActionResult MobilAuthControl()
        {
            if (string.IsNullOrEmpty(Auth.cSettings[0].Message.ToString()))
            {
                Auth.cSettings[0]._Message = Auth.GenerateMessage();
                Auth.cSettings[0]._ErrorMessage = "";
            }
            ViewBag.Message = Auth.cSettings[0].Message;
            ViewBag.ErrorMessage = Auth.cSettings[0].ErrorMessage;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MobilAuthControl(FormCollection frm)
        {
            strApiSandBox = Auth.cSettings[0].Demo.ToString();
            LoginCookie = Request.Cookies["HydroMobil"];

            if (ModelState.IsValid)
            {
                var Kontrol = dbs.LoginKontrol(LoginCookie["UserName"].ToString(), LoginCookie["Password"].ToString()).ToList();
                if (Kontrol.Count > 0)
                {
                    Auth.cSettings[0]._Hydro_ID = Kontrol[0].HydroID;
                }

                if (string.IsNullOrEmpty(Auth.cSettings[0].AccesToken))
                {
                    Auth.GetAccesToken(strApiSandBox, out strMessage);
                }

                if (Auth.GetVerifyMessage(Auth.cSettings[0].Hydro_ID, strApiSandBox, Auth.cSettings[0].Message, out strMessage))
                {
                    Auth.cSettings[0]._Message = strMessage;
                    return RedirectToAction("MobilAutList");
                }
                else
                {
                    if (Convert.ToBoolean(LoginCookie["Auth"].ToString()))
                    {
                        Auth.cSettings[0]._ErrorMessage = " We were unable to verify the code entered on the blockchain. Please enter again on the Hydro mobile app and re-authenticate below.";
                        return RedirectToAction("MobilAuthControl");
                    }
                    else
                    {
                        return RedirectToAction("MobilAutList");
                    }
                }
            }
            return View();
        }

        public ActionResult MobilAdd()
        {
            LoginCookie = Request.Cookies["HydroMobil"];

            if (LoginCookie == null)
            {
                ViewBag.Title = "Login";
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MobilAdd(FormCollection frm)
        {
            strApiSandBox = frm["ApiSandBox"].ToString();
            Auth.cSettings[0]._Demo = strApiSandBox;
            
            if (ModelState.IsValid)
            {
                if (Auth.GetAccesToken(strApiSandBox, out strMessage))
                {
                    Auth.cSettings[0]._Hydro_ID = frm["HydroID"].ToString();

                    if (Auth.AutAdd(Auth.cSettings[0].Hydro_ID, strApiSandBox, out strMessage))
                    {
                        Auth.cSettings[0]._Message = strMessage;
                        ViewBag.Message = Auth.cSettings[0].Message;

                        return RedirectToAction("MobilAuth");
                    }
                    else
                    {
                        Auth.cSettings[0]._ErrorMessage = strMessage;
                        return RedirectToAction("MobilAdd");
                    }
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

        public ActionResult MobilAuth()
        {
            LoginCookie = Request.Cookies["HydroMobil"];

            if (LoginCookie == null)
            {
                ViewBag.Title = "Login";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = Auth.cSettings[0].Message;
                if (!string.IsNullOrEmpty(Auth.cSettings[0].ErrorMessage) && Auth.cSettings[0].ErrorMessage != "Succes")
                {
                    ViewBag.ErrorMessage = Auth.cSettings[0].ErrorMessage.ToString();
                }
                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MobilAuth(FormCollection frm)
        {
            strApiSandBox = Auth.cSettings[0].Demo.ToString();
            LoginCookie = Request.Cookies["HydroMobil"];

            if (ModelState.IsValid)
            {
                var Kontrol = dbs.LoginKontrol(LoginCookie["UserName"].ToString(), LoginCookie["Password"].ToString()).ToList();
                if (Kontrol.Count > 0)
                {
                    LoginCookie["Auth"] = Kontrol[0].Auth.ToString();
                    if (Auth.cSettings[0].Hydro_ID.ToString() == "-" || string.IsNullOrEmpty(Auth.cSettings[0].Hydro_ID.ToString()))
                    {
                        Auth.cSettings[0]._Hydro_ID = Kontrol[0].HydroID;
                    }
                }

                if(string.IsNullOrEmpty(Auth.cSettings[0].AccesToken))
                {
                    Auth.GetAccesToken(strApiSandBox, out strMessage);
                }

                if (Auth.GetVerifyMessage(Auth.cSettings[0].Hydro_ID, strApiSandBox, Auth.cSettings[0].Message, out strMessage))
                {
                    Auth.cSettings[0]._Message = strMessage;

                    if(!Convert.ToBoolean(LoginCookie["Auth"].ToString()))
                    {
                        dbs.MembersHydroEdit(Kontrol[0].MembersID, Auth.cSettings[0].Hydro_ID, true);
                    }
                    
                    return RedirectToAction("MobilAutList");
                }
                else
                {
                    Auth.cSettings[0]._ErrorMessage = "We were unable to verify the code entered on the blockchain. Please enter again on the Hydro mobile app and re-authenticate below.";
                    ViewBag.ErrorMessage = Auth.cSettings[0].ErrorMessage.ToString();
                    return RedirectToAction("MobilAuth");
                }
            }
            return View();
        }
        public ActionResult MobilAutList()
        {
            LoginCookie = Request.Cookies["HydroMobil"];

            if (LoginCookie == null)
            {
                ViewBag.Title = "Login";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Message = Auth.cSettings[0].Message;
                return View(db.Member);
            }

        }
        public ActionResult MobilAutListDelete(int ID)
        {
            LoginCookie = Request.Cookies["HydroMobil"];

            strApiSandBox = Auth.cSettings[0].Demo.ToString();

            if (LoginCookie == null)
            {
                ViewBag.Title = "Login";
                return View("Login");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (Auth.GetAccesToken(strApiSandBox, out strMessage))
                    {
                        var Kontrol = dbs.LoginKontrol(LoginCookie["UserName"].ToString(), LoginCookie["Password"].ToString()).ToList();
                        if (Kontrol.Count > 0)
                        {
                            Auth.cSettings[0]._Hydro_ID = Kontrol[0].HydroID;
                            users.MembersID = Kontrol[0].MembersID;
                        }

                        if (Auth.GetDeleteApi(Auth.cSettings[0].Hydro_ID, strApiSandBox, out strMessage))
                        {
                            Auth.cSettings[0]._ErrorMessage = strMessage;
                            ViewBag.ErrorMessage = Auth.cSettings[0].ErrorMessage;

                            dbs.MembersHydroEdit(users.MembersID, "-", false);
                            return RedirectToAction("MobilAutList");
                        }
                        else
                        {
                            Auth.cSettings[0]._ErrorMessage = strMessage;
                            ViewBag.ErrorMessage = Auth.cSettings[0].ErrorMessage;
                            return RedirectToAction("MobilAutList");
                        }
                    }
                    else
                    {
                        Auth.cSettings[0]._ErrorMessage = strMessage;
                        ViewBag.ErrorMessage = Auth.cSettings[0].ErrorMessage;

                        return RedirectToAction("MobilAutList");
                    }
                }
            }
            return RedirectToAction("MobilAutList");
        }
        public ActionResult LogOff()
        {
            LoginCookie["MembersID"] = "0";
            LoginCookie["UserName"] = "";
            LoginCookie["Password"] = "";
            LoginCookie["HydroID"] = "";
            LoginCookie["Auth"] = "";
            LoginCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(LoginCookie);
            ViewBag.Title = "Hydro Auth";
            Auth.cSettings[0]._Message = "";
            return RedirectToAction("Login");
        }
    }
}