using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hydro_Mobil.Models
{
    public class Members
    {
        public int MembersID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string HydroID { get; set; }
        public bool Auth { get; set; }
    }
}