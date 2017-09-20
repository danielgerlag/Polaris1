﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S1.API.Models.Account
{
    public class RegisterRequest
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string PropertyName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}