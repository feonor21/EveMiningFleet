using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace EveMiningFleet.Entities.DbSet
{
    [Table("FleetGroups")]
    public partial class FleetGroup
    {
        public FleetGroup()
        {
            Fleetgroupcharacters = new HashSet<FleetGroupCharacter>();
        }

        public int Id { get; set; }
        public int FleetId { get; set; }
        public virtual Fleet Fleet { get; set; }

        public virtual ICollection<FleetGroupCharacter> Fleetgroupcharacters { get; set; }
    }
}
