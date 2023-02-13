using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EveMiningFleet.Entities.DbSet
{
    [Table("FleetGroupCharacters")]
    public partial class FleetGroupCharacter
    {
        public int Id { get; set; }
        public int FleetgroupId { get; set; }
        public virtual FleetGroup Fleetgroup { get; set; }
        public int CharacterId { get; set; }
        public virtual Character Character { get; set; }

    }
}
