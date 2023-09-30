﻿using System.ComponentModel.DataAnnotations;

namespace ProjetoPortfolio.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
