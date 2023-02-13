using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EveMiningFleet.Entities.DbSet
{
    [Table("Fleets")]
    public partial class Fleet
    {
        public Fleet()
        {
            Fleetcharacters = new HashSet<FleetCharacter>();
            Fleetgroups = new HashSet<FleetGroup>();
            Fleettaxes = new HashSet<FleetTaxes>();
        }

        public int Id { get; set; }
        public DateTime Begin { get; set; }
        public DateTime? End { get; set; }
        public string JoinToken { get; set; }
        public DateTime? LastFullRefresh { get; set; }
        public int? ViewRight { get; set; }
        public int? Distribution { get; set; }
        public double? Reprocess { get; set; }

        public int CharacterId { get; set; }
        public virtual Character Character { get; set; }

        public int CorporationId { get; set; }
        public virtual Corporation Corporation { get; set; }

        public int AllianceId { get; set; }
        public virtual Alliance Alliance { get; set; }

        public virtual ICollection<FleetCharacter> Fleetcharacters { get; set; }
        public virtual ICollection<FleetGroup> Fleetgroups { get; set; }
        public virtual ICollection<FleetTaxes> Fleettaxes { get; set; }
    }
}
