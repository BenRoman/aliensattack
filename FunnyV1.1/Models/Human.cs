using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FunnyV1.Models
{

    public class Human
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int Age { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int? SpaceShipId { get; set; }
        public virtual SpaceShip SpaceShip { get; set; }

        public ICollection<ExOn> ExOns { get; set; }
        public ICollection<Escape> Escapes{ get; set; }
        public ICollection<KidnappingWhom> KidnappingWhoms { get; set; }
        public ICollection<ExcursionFor> ExcursionFors{ get; set; }
        public ICollection<Murder> Murders{ get; set; }


        //public virtual List<Escape> Escapes { get; set; }
        //public virtual List<ExcursionFor> ExcursionFors{ get; set; }

    }

}