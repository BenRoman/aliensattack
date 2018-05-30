using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FunnyV1.Models
{
    public class Excursion
    {
        [Key]
        public int Id { get; set; }

        public int? SpaceShipId { get; set; }
        public SpaceShip SpaceShip { get; set; }

        public int? AlienId { get; set; }
        public Alien Alien { get; set; }

        public DateTime date { get; set; }

        public ICollection<ExcursionFor> excursionFors { get; set; }
    }

    public class ExcursionFor
    {
        [Key, Column(Order = 0)]
        public int? HumanId { get; set; }
        public Human Human { get; set; }
        [Key, Column(Order = 1)]
        public int? ExcursionId { get; set; }
        public Excursion Excursion { get; set; }


    }
}