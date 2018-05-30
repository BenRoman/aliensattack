
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunnyV1.Models
{
    public class Alien
    {

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Display(Name = "Iмя")]
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string FourthName { get; set; }
        public int Age { get; set; }
        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Kidnapping> Kidnappings{ get; set; }
        public ICollection<Excursion> Excursions { get; set; }
        public ICollection<Murder> Murders { get; set; }

        //public List<Excursion> Excursions { get; set; }
    }
}