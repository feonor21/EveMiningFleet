using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EveMiningFleet.Entities.DbSet
{
    [Table("DataPrices")]
    public class DataPrice
    {
        [Key]
        public int TypeId { get; set; }
        public double PriceSell { get; set; }
        public double PriceBuy { get; set; }
        public string Name { get; set; }
    }
}
