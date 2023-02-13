using EveMiningFleet.Entities;
using EveMiningFleet.Entities.DbSet;
using EveMiningFleet.Logic.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EveMiningFleet.API.Models
{
    public class CharacterModel
    {
        public CharacterModel(Character target)
        {
            this.Id = target.Id;
            this.Name = target.Name;
            this.Coorporation = target.Corporation;
            this.Alliance = target.Alliance;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Corporation Coorporation { get; set; }
        public Alliance Alliance { get; set; }

    }
}
