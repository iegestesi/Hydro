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
    }
}