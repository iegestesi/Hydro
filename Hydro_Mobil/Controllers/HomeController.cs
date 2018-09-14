using Hydro_Mobil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Hydro_Mobil.Controllers
{
    public class HomeController : Controller
    {
        LoginProfil users = new LoginProfil();
        HttpCookie LoginCookie = new HttpCookie("HydroMobil");
        string strApiSandBox = "";
        string strMessage = "";

        public ActionResult Index()
        {
            LoginCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(LoginCookie);
            return View();
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
                return RedirectToAction("MobilAdd");
            }
        }
        [HttpPost]
        public ActionResult Login(FormCollection frm, LoginProfil user)
        {
            if (ModelState.IsValid)
            {
                var Kontrol = Information.LoginKontrol(frm["UserName"].ToString(), frm["Password"].ToString()).ToList();

                if (Kontrol != null)
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
                        return RedirectToAction("MobilAuth");
                    }
                    else
                    {
                        return RedirectToAction("MobilAdd");
                    }
                }
                else
                {
                    ViewBag.Title = "Hydro Mobil Login";
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult MobilAdd()
        {
            LoginCookie = Request.Cookies["HydroMobil"];

            if (LoginCookie == null)
            {
                ViewBag.Title = "Login";
                return RedirectToAction("Index");
            }
            else
            {
                return View(Information.cSettings);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MobilAdd(FormCollection frm)
        {
            strApiSandBox = frm["ApiSandBox"].ToString();
            Information.cSettings[0].Demo = strApiSandBox;
            
            if (ModelState.IsValid)
            {
                if (Information.GetAccesToken(strApiSandBox, out strMessage))
                {
                    Information.cSettings[0].Hydro_ID = frm["HydroID"].ToString();
                    if (Information.AutAdd(frm["HydroID"].ToString(), strApiSandBox, out strMessage))
                    {
                        Information.cSettings[0].Message = strMessage;
                        ViewBag.Message = Information.cSettings[0].Message;

                        return RedirectToAction("MobilAuth");
                    }
                    else
                    {
                        return RedirectToAction("MobilAdd");
                    }
                }
                else
                {
                    return View(Information.cSettings);
                }
            }
            return View(Information.cSettings);
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
                ViewBag.Message = Information.cSettings[0].Message;
                return View(Information.cSettings);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MobilAuth(FormCollection frm)
        {
            strApiSandBox = Information.cSettings[0].Demo.ToString();
            LoginCookie = Request.Cookies["HydroMobil"];

            if (ModelState.IsValid)
            {
                if (Information.GetVerifyMessage(Information.cSettings[0].Hydro_ID, strApiSandBox, Information.cSettings[0].Message, out strMessage))
                {
                    Information.cSettings[0].Message = strMessage;
                    LoginCookie["Auth"] = "True";

                    Information.cMembers[0].MembersID = 1;
                    Information.cMembers[0].UserName = LoginCookie["UserName"].ToString();
                    Information.cMembers[0].PassWord = LoginCookie["PassWord"].ToString();
                    Information.cMembers[0].HydroID = Information.cSettings[0].Hydro_ID.ToString();
                    Information.cMembers[0].Auth = Convert.ToBoolean(LoginCookie["Auth"].ToString());

                    return RedirectToAction("MobilAutList", Information.cMembers);
                }
                else
                {
                    return RedirectToAction("MobilAdd", Information.cSettings);
                }
            }
            return View(Information.cSettings);
        }

        public ActionResult MobilAutList()
        {
            LoginCookie = Request.Cookies["HydroMobil"];

            if (LoginCookie == null)
            {
                ViewBag.Title = "Login";
                return View("Login");
            }
            else
            {
                return View(Information.cMembers);
            }
        }
        public ActionResult MobilAutListDelete(int ID)
        {
            LoginCookie = Request.Cookies["HydroMobil"];
            strApiSandBox = Information.cSettings[0].Demo.ToString();

            if (LoginCookie == null)
            {
                ViewBag.Title = "Login";
                return View("Login");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (Information.GetAccesToken(strApiSandBox, out strMessage))
                    {
                        if (Information.GetDeleteApi(Information.cSettings[0].Hydro_ID, strApiSandBox, out strMessage))
                        {
                            Information.cSettings[0].Message = strMessage;
                            LoginCookie["Auth"] = "False";
                            Information.cMembers[0].Auth = false;
                            return RedirectToAction("MobilAdd");
                        }
                        else
                        {
                            return RedirectToAction("MobilAutList");
                        }
                    }
                    else
                    {
                        return RedirectToAction("MobilAutList");
                    }
                }
            }
            return RedirectToAction("MobilAutList");
        }
    }
}