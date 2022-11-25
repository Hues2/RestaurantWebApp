using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Xunit;

namespace RestaurantWebApp.Data
{
    public class Registration
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, EmailAddress,Display(Name = "Email")]
        public string Email { get; set; }

        [Required, StringLength(100)]
        [DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
