using EveMiningFleet.Entities;
using EveMiningFleet.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EveMiningFleet.Logic.Repository
{
    public class CharacterRepository : BaseRepository
    {
        public CharacterRepository(EveMiningFleetContext _eveMiningFleetContext) : base(_eveMiningFleetContext)
        {

        }

        public IQueryable<Character> GetSimple()
        {
            return eveMiningFleetContext.characters.Include("Corporation").Include("Alliance");
        }
    }
}
