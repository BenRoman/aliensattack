using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunnyV1.Models
{
    public class RegisterHumanModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Name { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        [Range(0, 150, ErrorMessage = "Invalid age")]
        public int Age { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Different passwords")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterAlienModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Name { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        public string ThirdName { get; set; }

        [Required]
        public string FourthName { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid age")]
        public int Age { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Diff pass")]
        public string ConfirmPassword { get; set; }
    }
    public class LoginModel
    {
        [Required]
        [Display(Name ="Email")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}