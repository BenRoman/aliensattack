using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FunnyV1.Models
{
    public class ExType
    {
        [Key]
        public string Type { get; set; }
        public string Description { get; set; }
        public ICollection<Experiment> Experiments { get; set; }

    }

    public class Experiment
    {
        public int Id { get; set; }

        public int? AlienId { get; set; }
        public Alien Alien { get; set; }

        public string ExTypeType { get; set; }
        public ExType ExType { get; set; }
        public DateTime DateTime { get; set; }
        public int? ShipId { get; set; }
        public SpaceShip Ship { get; set; }

        public ICollection<ExOn> ExOns { get; set; }

    }
    public class ExOn
    {
        [Key, Column(Order = 0)]
        public int? ExperimentId { get; set; }
        public Experiment Experiment { get; set; }
        [Key, Column(Order = 1)]
        public int? HumanId { get; set; }
        public Human Human { get; set; }

        //public int ExperimentId { get; set; }
    }
}