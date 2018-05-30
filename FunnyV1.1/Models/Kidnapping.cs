using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FunnyV1.Models
{

    public class KidnappingWhom
    {
        [Key, Column(Order = 0)]
        public int? KidnappingId { get; set; }
        public Kidnapping Kidnapping { get; set; }

        [Key, Column(Order = 1)]
        public int? HumanId { get; set; }
        public Human Human { get; set; }
    }

    public class Kidnapping
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int? ShipId { get; set; }
        public SpaceShip Ship { get; set; }
        public int? AlienId { get; set; }
        public Alien Alien { get; set; }

        public ICollection<KidnappingWhom> KidnappingWhoms { get; set; }
    }
}