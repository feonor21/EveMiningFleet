using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EveMiningFleet.Entities.DbSet
{
    [Table("UsageHistory")]
    public partial class UsageHistory
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        public int? fleetactive { get; set; }
        public int? characteractif { get; set; }
    }
}
