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
                        user.Auth = item.Auth;
                    }

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
            if (ModelState.IsValid)
            {
                if (strApiSandBox == "SandBox")
                {
                    if (Information.GetAccesTokenSandBox(out strMessage))
                    {
                        Information.cSettings[0].Hydro_ID = frm["HydroID"].ToString();
                        Information.cSettings[0].Demo = strApiSandBox;

                        if (Information.AutAddSandBox(frm["HydroID"].ToString(), out strMessage))
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
                else
                {
                    if (Information.GetAccesTokenApi(out strMessage))
                    {
                        if (Information.AutAddApi(frm["HydroID"].ToString(), out strMessage))
                        {
                            Information.cSettings[0].Message = strMessage;
                            ViewBag.Message = Information.cSettings[0].Message;

                            return RedirectToAction("MobilAuth", Information.cSettings);
                        }
                        else
                        {
                            return RedirectToAction("MobilAdd", Information.cSettings);
                        }
                    }
                    else
                    {
                        return View();
                    }
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
                if (strApiSandBox == "SandBox")
                {
                    if (Information.GetVerifyMessageSandBox(Information.cSettings[0].Hydro_ID, Information.cSettings[0].Message, out strMessage))
                    {
                        Information.cSettings[0].Message = strMessage;
                        LoginCookie["Auth"] = "True";
                        return RedirectToAction("MobilAuth", Information.cSettings);
                    }
                    else
                    {
                        return RedirectToAction("MobilAdd", Information.cSettings);
                    }
                }
                else
                {
                    if (Information.AutAddApi(frm["HydroID"].ToString(), out strMessage))
                    {
                        Information.cSettings[0].Message = strMessage;
                        LoginCookie["Auth"] = "True";
                        return RedirectToAction("MobilAuth", Information.cSettings);
                    }
                    else
                    {
                        return RedirectToAction("MobilAdd", Information.cSettings);
                    }
                }
            }
            return View(Information.cSettings);
        }

    }
}