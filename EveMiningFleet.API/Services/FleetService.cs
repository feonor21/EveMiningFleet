using EveMiningFleet.API.Models;
using EveMiningFleet.Entities;
using EveMiningFleet.Entities.DbSet;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EveMiningFleet.API.Services
{
    public class FleetService
    {
        private EveMiningFleetContext eveMiningFleetContext;

        public FleetService(EveMiningFleetContext _eveMiningFleetContext)
        {
            eveMiningFleetContext = _eveMiningFleetContext;
        }

        public FleetModel GetById(int idfleet)
        {
            Entities.DbSet.Fleet fleet = eveMiningFleetContext.fleets.Include("Character").Include("Corporation").Include("Alliance").Include("Fleetcharacters.Character").FirstOrDefault(x=> x.Id == idfleet);
            if (fleet == null)
                return null;
            else
                return new FleetModel(fleet);
        }
        public List<FleetModel> GetAllByViewRight(typeview viewRight , Character mainCharacter)
        {
            IQueryable<Fleet> allFleet = eveMiningFleetContext.fleets.Include("Character").Include("Corporation").Include("Alliance").Include("Fleetcharacters.Character");



            var characters = eveMiningFleetContext.characters.Include("Corporation").Include("Alliance").Where(_character => _character.CharacterMainId == mainCharacter.CharacterMainId);
            switch (viewRight)
            {
                case typeview.viewPublic:
                    allFleet = allFleet.Where(_fleet => _fleet.ViewRight == 3);
                    break;
                case typeview.viewAlliance:
                    allFleet = allFleet.Where(_fleet => _fleet.ViewRight == 2 && characters.Any(_Character => _fleet.AllianceId == _Character.AllianceId));
                    break;
                case typeview.viewCorporation:
                    allFleet = allFleet.Where(_fleet => _fleet.ViewRight == 1 && characters.Any(_Character => _fleet.CorporationId == _Character.CorporationId));
                    break;
                case typeview.viewPrivate:
                    allFleet = allFleet.Where(_fleet => _fleet.Fleetcharacters.Any(_fleetCharacter => characters.Any(_Character => _fleetCharacter.CharacterId == _Character.Id)));
                    break;
                default:
                    allFleet = allFleet.Where(_fleet => 0==1);
                    break;
            }
            return allFleet.Select(_fleet => new FleetModel(_fleet)).ToList();
        }
        public bool AuthoriseToSeeFleet(int idfleet, Character mainCharacter)
        {
            var fleet = eveMiningFleetContext.fleets.Include("Character").Include("Corporation").Include("Alliance").Include("Fleetcharacters.Character").FirstOrDefault(_fleet => _fleet.Id == idfleet);
            if (fleet == null)
                return false;
            else
            {
                var characters = eveMiningFleetContext.characters.Include("Corporation").Include("Alliance").Where(_character => _character.CharacterMainId == mainCharacter.CharacterMainId);
                switch ((typeview)fleet.ViewRight)
                {
                    case typeview.viewPublic:
                        return true;
                    case typeview.viewAlliance:
                        return characters.Any(_Character => fleet.AllianceId == _Character.AllianceId);
                    case typeview.viewCorporation:
                        return characters.Any(_Character => fleet.CorporationId == _Character.CorporationId);
                    case typeview.viewPrivate:
                        return fleet.Fleetcharacters.Any(_fleetCharacter => characters.Any(_Character => _fleetCharacter.CharacterId == _Character.Id));
                    default:
                        return false;
                }
            }
        }
        public FleetModel CreateFleet(FleetModel fleetModel)
        {
            CharacterService characterService = new CharacterService(eveMiningFleetContext);
            var fleetCreator = characterService.Get(fleetModel.Character.Id);
            
            Entities.DbSet.Fleet fleet = new Entities.DbSet.Fleet();
            fleet.CharacterId = fleetCreator.Id;
            fleet.CorporationId = fleetCreator.Coorporation.Id;
            fleet.AllianceId = fleetCreator.Alliance.Id;
            
            fleet.Reprocess = fleetModel.Reprocess;
            fleet.Distribution = fleetModel.Distribution;
            fleet.ViewRight = fleetModel.ViewRight;

            eveMiningFleetContext.fleets.Add(fleet);
            eveMiningFleetContext.SaveChanges();

            var resultInsert = new FleetModel(fleet);
            return resultInsert;
        }
        
        public enum typeview
        {
            viewPrivate = 0,
            viewCorporation = 1,
            viewAlliance = 2,
            viewPublic = 3
        }

    }
}
