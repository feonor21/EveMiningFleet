using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EveMiningFleet.Entities.DbSet
{
    [Table("AlerteMessages")]
    public class AlerteMessage
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime End { get; set; }

    }
}
