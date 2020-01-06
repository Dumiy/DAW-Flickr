using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public String RoleName { get; set; }
    }
}