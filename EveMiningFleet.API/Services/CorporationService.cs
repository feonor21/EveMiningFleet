using ESI.NET.Models.SSO;
using EveMiningFleet.API.Models;
using EveMiningFleet.Entities;
using EveMiningFleet.Entities.DbSet;
using EveMiningFleet.Logic.EsiEve;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EveMiningFleet.API.Services
{
    public class CorporationService
    {
        private EveMiningFleetContext eveMiningFleetContext;

        public CorporationService(EveMiningFleetContext _eveMiningFleetContext)
        {
            eveMiningFleetContext = _eveMiningFleetContext;
        }
        public Corporation GetOrCreate(int id)
        {
            Corporation corporation = eveMiningFleetContext.corporations.FirstOrDefault(x => x.Id == id);
            if (corporation == null)
            {
                corporation = new Corporation();
                corporation.Id = id;
                corporation.Name = EsiCorporation.GetName(corporation.Id);
                eveMiningFleetContext.corporations.Add(corporation);
                eveMiningFleetContext.SaveChanges();
            }
            return corporation;
        }

    }
}
