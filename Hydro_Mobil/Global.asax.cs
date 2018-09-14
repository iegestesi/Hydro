using Hydro_Mobil.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hydro_Mobil
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            using (TableContext db = new TableContext())
            {
                //Aşağıda olan Method tablolarımız yoksa 
                //veritabanında onu oluşturur kapalı olan ise veritabanını oluşturur.
                db.Database.CreateIfNotExists();
                db.Member.Create();
            }

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
