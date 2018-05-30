using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FunnyV1.Models
{
    public class Transfer
    {
        [Key, Column(Order = 0)]
        public int? HumanId { get; set; }
        public Human Human { get; set; }

        public int? SpaceShipFromId { get; set; }
        public SpaceShip SpaceShipFrom { get; set; }

        public int? SpaceShipToId { get; set; }
        public SpaceShip SpaceShipTo { get; set; }
        [Key, Column(Order = 1)]
        public DateTime date { get; set; }

        public int? AlienId { get; set; }
        public Alien Alien { get; set; }

    }
}