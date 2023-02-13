using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EveMiningFleet.Entities.DbSet
{
    [Table("FleetTaxes")]
    public partial class FleetTaxes
    {
        public int Id { get; set; }
        public int FleetId { get; set; }
        public virtual Fleet Fleet { get; set; }
        public int? CharacterId { get; set; }
        public virtual Character Character { get; set; }
        public float Taxe { get; set; }
        public string Name { get; set; }

    }
}
