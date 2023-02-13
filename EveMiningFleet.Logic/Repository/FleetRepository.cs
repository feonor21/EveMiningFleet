using EveMiningFleet.Entities;
using EveMiningFleet.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EveMiningFleet.Logic.Repository
{
    public class FleetRepository : BaseRepository
    {
        public FleetRepository(EveMiningFleetContext _eveMiningFleetContext) : base(_eveMiningFleetContext)
        {

        }

        public IQueryable<Fleet> GetSimple()
        {
            return eveMiningFleetContext.fleets.Include("Character").Include("Corporation").Include("Alliance").Include("Fleetcharacters.Character");
        }
        public IQueryable<Fleet> GetDetails()
        {
            return eveMiningFleetContext.fleets.Include("Character").Include("Corporation").Include("Alliance").Include("Fleetcharacters.Character");
        }
    }
}
