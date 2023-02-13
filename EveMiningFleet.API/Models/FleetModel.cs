using EveMiningFleet.Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveMiningFleet.API.Models
{
    public class FleetModel
    {
        public FleetModel()
        {

        }

        public FleetModel(Fleet target)
        {
            this.Id = target.Id;
            this.Begin = target.Begin;
            this.End = target.End;
            //this.JoinToken = target.JoinToken;
            this.LastFullRefresh = target.LastFullRefresh;
            this.ViewRight = target.ViewRight;
            this.Distribution = target.Distribution;
            this.Reprocess = target.Reprocess;
            this.Character = new CharacterModel(target.Character);
            this.Corporation = target.Corporation;
            this.Alliance = target.Alliance;
        }

        public int Id { get; set; }
        public DateTime Begin { get; set; }
        public DateTime? End { get; set; }
        //public string JoinToken { get; set; }
        public DateTime? LastFullRefresh { get; set; }
        public int? ViewRight { get; set; }
        public int? Distribution { get; set; }
        public double? Reprocess { get; set; }

        public CharacterModel Character { get; set; }
        public Corporation Corporation { get; set; }
        public Alliance Alliance { get; set; }

        public bool isValidToCreate()
        {
            if (this.Character == null || this.Character.Id == 0) return false;
            return true;
        }



    }
}
