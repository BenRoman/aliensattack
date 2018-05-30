using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FunnyV1.Models
{
    public class Murder
    {
        [Key, Column(Order = 0)]
        public int? AlienId { get; set; }
        public Alien Alien { get; set; }

        public int? HumanId { get; set; }
        public Human Human { get; set; }
        [Key, Column(Order = 1)]
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
    }
}