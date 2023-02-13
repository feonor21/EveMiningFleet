using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace EveMiningFleet.Entities.DbSet
{
    [Table("Invtypematerials")]
    public partial class InvTypeMaterial
    {
        public int TypeId { get; set; }
        public int MaterialTypeId { get; set; }
        public int Quantity { get; set; }
    }
}
