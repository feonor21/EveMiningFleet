using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EveMiningFleet.Entities.DbSet
{
    [Table("MiningLogs")]
    public partial class MiningLog
    {
        public int FleetCharacterId { get; set; }
        public virtual FleetCharacter FleetCharacter { get; set; }
        public int OreId { get; set; }
        public virtual Ore Ore { get; set; }
        public int Quantity { get; set; }

    }
}
