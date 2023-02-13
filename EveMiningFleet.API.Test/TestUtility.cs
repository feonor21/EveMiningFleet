using EveMiningFleet.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using EveMiningFleet.API.Services;
using EveMiningFleet.API.Controllers;

namespace EveMiningFleet.API.Test.Controllers
{
    public static class TestUtility
    {
        public static void CorrectToken(OriginController controllerContext, int characterId){
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(TokenService.characterIdClaimKey, characterId.ToString())
            };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            controllerContext.ControllerContext.HttpContext = httpContext;
        }

        public static int johndoeId = 1;
        public static int CharacterIdTestCoorpoId = 5;
        public static int CharacterIdTestAllianceId = 6;
        public static int CharacterIdTestPrivateId = 7;
        public static string johndoename= "John Doe";
        public static void populateCharacter(EveMiningFleetContext context)
        {
            context.alliances.Add(new Entities.DbSet.Alliance() { Id = 1, Name = "alliance1" });
            context.alliances.Add(new Entities.DbSet.Alliance() { Id = 2, Name = "alliance2" });
            context.alliances.Add(new Entities.DbSet.Alliance() { Id = 3, Name = "alliance3" });

            context.corporations.Add(new Entities.DbSet.Corporation() { Id = 1, Name = "All1coorp1" });
            context.corporations.Add(new Entities.DbSet.Corporation() { Id = 2, Name = "All1coorp2" });
            context.corporations.Add(new Entities.DbSet.Corporation() { Id = 3, Name = "All2coorp3" });

            context.characters.Add(new Entities.DbSet.Character() { Id = johndoeId, Name = johndoename, AllianceId = 1, CorporationId = 1, CharacterMainId = johndoeId });
            context.characters.Add(new Entities.DbSet.Character() { Id = 2, Name = "John Alt1", AllianceId = 1, CorporationId = 1, CharacterMainId = johndoeId });
            context.characters.Add(new Entities.DbSet.Character() { Id = 3, Name = "John Alt2", AllianceId = 1, CorporationId = 1, CharacterMainId = johndoeId });
            context.characters.Add(new Entities.DbSet.Character() { Id = 4, Name = "John Alt3", AllianceId = 1, CorporationId = 1, CharacterMainId = johndoeId });

            context.characters.Add(new Entities.DbSet.Character() { Id = CharacterIdTestCoorpoId, Name = "John coorpo", AllianceId = 1, CorporationId = 1, CharacterMainId = CharacterIdTestCoorpoId });
            context.characters.Add(new Entities.DbSet.Character() { Id = CharacterIdTestAllianceId, Name = "John alliance", AllianceId = 1, CorporationId = 2, CharacterMainId = CharacterIdTestAllianceId });
            context.characters.Add(new Entities.DbSet.Character() { Id = CharacterIdTestPrivateId, Name = "John private", AllianceId = 2, CorporationId = 3, CharacterMainId = CharacterIdTestPrivateId });

            context.SaveChanges();
        }
        
        public static int arkonorId = 1;
        public static string arkonorName= "Arkonor";
        public static void populateoreEtDataprice(EveMiningFleetContext context)
        {
            context.ores.Add(new Entities.DbSet.Ore() { Id = arkonorId, Name = arkonorName });
            context.ores.Add(new Entities.DbSet.Ore() { Id = 2, Name = "Veldspar"});
            context.ores.Add(new Entities.DbSet.Ore() { Id = 3, Name = "Jaspet" });
            context.ores.Add(new Entities.DbSet.Ore() { Id = 4, Name = "Spodumain"});

            context.dataPrices.Add(new Entities.DbSet.DataPrice() { TypeId = arkonorId, Name = arkonorName, PriceBuy = 1, PriceSell = 1 });
            context.dataPrices.Add(new Entities.DbSet.DataPrice() { TypeId = 2, Name = "Veldspar", PriceBuy = 1, PriceSell = 1 });
            context.dataPrices.Add(new Entities.DbSet.DataPrice() { TypeId = 3, Name = "Jaspet", PriceBuy = 1, PriceSell = 1 });
            context.dataPrices.Add(new Entities.DbSet.DataPrice() { TypeId = 4, Name = "Spodumain", PriceBuy = 1, PriceSell = 1 });

            context.SaveChanges();
        }
        
        public static int fleetid = 1;
        public static int fleetIdTestCoorpoId = 2;
        public static int fleetIdTestAllianceId = 3;
        public static int fleetIdTestPublicId = 4;
        public static int fleetIdTestPrivateId = 5;
        public static void populateFleet(EveMiningFleetContext context)
        {
            populateCharacter(context);

            //private
            context.fleets.Add(new Entities.DbSet.Fleet() { Id=fleetid, CharacterId=johndoeId,CorporationId=1,AllianceId=1,Distribution=0,ViewRight=(int)FleetService.typeview.viewPrivate,End=new DateTime(2023, 02, 15),Begin=new DateTime(2023, 02, 16)});
            context.fleetCharacters.Add(new Entities.DbSet.FleetCharacter() { FleetId=fleetid, CharacterId=johndoeId,Join=new DateTime(2023, 02, 15)});

            //coorpo
            context.fleets.Add(new Entities.DbSet.Fleet() { Id=fleetIdTestCoorpoId, CharacterId=5,CorporationId=1,AllianceId=2,Distribution=0,ViewRight=(int)FleetService.typeview.viewCorporation,End=new DateTime(2023, 02, 17),Begin=new DateTime(2023, 02, 18)});
            context.fleetCharacters.Add(new Entities.DbSet.FleetCharacter() { FleetId=fleetIdTestCoorpoId, CharacterId=5,Join=new DateTime(2023, 02, 17)});

            //alliance
            context.fleets.Add(new Entities.DbSet.Fleet() { Id=fleetIdTestAllianceId, CharacterId=6,CorporationId=2,AllianceId=1,Distribution=0,ViewRight=(int)FleetService.typeview.viewAlliance,End=new DateTime(2023, 02, 19),Begin=new DateTime(2023, 02, 20)});
            context.fleetCharacters.Add(new Entities.DbSet.FleetCharacter() { FleetId=fleetIdTestAllianceId, CharacterId=6,Join=new DateTime(2023, 02, 19)});

            //public
            context.fleets.Add(new Entities.DbSet.Fleet() { Id=fleetIdTestPublicId, CharacterId=7,CorporationId=3,AllianceId=2,Distribution=0,ViewRight=(int)FleetService.typeview.viewPublic,End=new DateTime(2023, 02, 21),Begin=new DateTime(2023, 02, 22)});
            context.fleetCharacters.Add(new Entities.DbSet.FleetCharacter() { FleetId=fleetIdTestPublicId, CharacterId=7,Join=new DateTime(2023, 02, 17)});
            //private
            context.fleets.Add(new Entities.DbSet.Fleet() { Id=fleetIdTestPrivateId, CharacterId=7,CorporationId=3,AllianceId=2,Distribution=0,ViewRight=(int)FleetService.typeview.viewPrivate,End=new DateTime(2023, 02, 21),Begin=new DateTime(2023, 02, 22)});
            context.fleetCharacters.Add(new Entities.DbSet.FleetCharacter() { FleetId=fleetIdTestPrivateId, CharacterId=7,Join=new DateTime(2023, 02, 17)});


            context.SaveChanges();
        }
  

    }
}
