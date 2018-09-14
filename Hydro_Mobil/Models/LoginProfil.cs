using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hydro_Mobil.Models
{
    public class LoginProfil
    {
        [Required(ErrorMessage = "Please Enter Your UserName.")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Your Password.")]
        [Display(Name = "PassWord")]
        public string Password { get; set; }

        public bool Auth { get; set; }

    }
}