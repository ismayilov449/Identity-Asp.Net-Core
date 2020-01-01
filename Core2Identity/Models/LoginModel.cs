using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core2Identity.Models
{
    public class LoginModel
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }


        [Required]
        [UIHint("password")]
        public string Password { get; set; }


    }
}
