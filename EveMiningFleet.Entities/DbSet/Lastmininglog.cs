using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EveMiningFleet.Entities.DbSet
{
    [Table("LastMiningLogs")]
    public partial class LastMiningLog
    {
        public int CharacterId { get; set; }
        public virtual Character Character { get; set; }
        public int OreId { get; set; }
        public virtual Ore Ore { get; set; }
        public DateTime Date { get; set; }
        public long Quantity { get; set; }

    }
}
