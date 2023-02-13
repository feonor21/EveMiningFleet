using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EveMiningFleet.Entities.DbSet
{
    [Table("FleetCharacters")]
    public partial class FleetCharacter
    {
        public FleetCharacter()
        {
            Mininglogs = new HashSet<MiningLog>();
        }

        public int Id { get; set; }
        public int FleetId { get; set; }
        public virtual Fleet Fleet { get; set; }
        public int CharacterId { get; set; }
        public virtual Character Character { get; set; }
        public DateTime Join { get; set; }
        public DateTime? Quit { get; set; }
        public DateTime? LastRefresh { get; set; }

        public virtual ICollection<MiningLog> Mininglogs { get; set; }
    }
}
