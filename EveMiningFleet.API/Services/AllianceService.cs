using EveMiningFleet.Entities;
using EveMiningFleet.Entities.DbSet;
using EveMiningFleet.Logic.EsiEve;
using System.Linq;

namespace EveMiningFleet.API.Services
{
    public class AllianceService
    {
        private EveMiningFleetContext eveMiningFleetContext;

        public AllianceService(EveMiningFleetContext _eveMiningFleetContext)
        {
            eveMiningFleetContext = _eveMiningFleetContext;
        }

        public Alliance GetOrCreate(int id)
        {
            Alliance alliance = eveMiningFleetContext.alliances.FirstOrDefault(x => x.Id == id);
            if (alliance == null)
            {
                alliance = new Alliance();
                alliance.Id = id;
                alliance.Name = EsiAlliance.GetName(alliance.Id);
                eveMiningFleetContext.alliances.Add(alliance);
                eveMiningFleetContext.SaveChanges();
            }
            return alliance;
        }
 

    }
}
