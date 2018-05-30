using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FunnyV1.Models
{
    public class Escape
    {
        public int Id { get; set; }
        public int? HumanId { get; set; }
        public Human Human { get; set; }
        public int? ShipId { get; set; }
        public SpaceShip Ship { get; set; }
        public DateTime DateTime { get; set; }
    }
}