using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EveMiningFleet.Entities.DbSet
{
    [Table("Characters")]
    public partial class Character
    {
        public Character()
        {
            Fleetcharacters = new HashSet<FleetCharacter>();
            Fleetgroupcharacters = new HashSet<FleetGroupCharacter>();
            Fleets = new HashSet<Fleet>();
            Fleettaxes = new HashSet<FleetTaxes>();
            Lastmininglogs = new HashSet<LastMiningLog>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public int CorporationId { get; set; }
        public virtual Corporation Corporation { get; set; }

        public int AllianceId { get; set; }
        public virtual Alliance Alliance { get; set; }

        public int CharacterMainId { get; set; }
        public virtual Character CharacterMain { get; set; }
        public virtual ICollection<FleetCharacter> Fleetcharacters { get; set; }
        public virtual ICollection<FleetGroupCharacter> Fleetgroupcharacters { get; set; }
        public virtual ICollection<Fleet> Fleets { get; set; }
        public virtual ICollection<FleetTaxes> Fleettaxes { get; set; }
        public virtual ICollection<LastMiningLog> Lastmininglogs { get; set; }
    }
}
