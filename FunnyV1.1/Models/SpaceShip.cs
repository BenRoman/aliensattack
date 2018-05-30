using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunnyV1.Models
{

    public class ShipType
    {
        [Key]
        public string Type { get; set; }
        public string Description { get; set; }
        public ICollection<SpaceShip> SpaceShips { get; set; }

    }
    public class SpaceShip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ShipId { get; set; }

        public string ShipTypeType { get; set; }
        public virtual ShipType ShipType { get; set; }

        public ICollection<Human> Humen { get; set; }
        public ICollection<Escape> Escapes { get; set; }
        public ICollection<Kidnapping> Kidnappings { get; set; }
        public ICollection<Excursion> Excursions { get; set; }


        //public virtual List<Escape> Escapes { get; set; }
    }
}