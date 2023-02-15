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
    public class CharacterService
    {
        private EveMiningFleetContext eveMiningFleetContext;

        public CharacterService(EveMiningFleetContext _eveMiningFleetContext)
        {
            eveMiningFleetContext = _eveMiningFleetContext;
        }

        /// <summary>
        /// Create and update character by esi login.
        /// </summary>
        /// <param name="authorizedCharacterData"></param>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public CharacterModel GetAndUpdateByauthorizedCharacterData(AuthorizedCharacterData authorizedCharacterData, SsoToken accessToken)
        {
            Character characterConnexion = eveMiningFleetContext.characters.FirstOrDefault((x) => x.Id == authorizedCharacterData.CharacterID);
            if (characterConnexion == null)
            {
                characterConnexion = new Character();

                characterConnexion.Id = authorizedCharacterData.CharacterID;
                characterConnexion.CharacterMainId = authorizedCharacterData.CharacterID;
                eveMiningFleetContext.characters.Add(characterConnexion);
            }
            characterConnexion.AllianceId = authorizedCharacterData.AllianceID;
            characterConnexion.CorporationId = authorizedCharacterData.CorporationID;
            //on met a jours les informations du player.
            characterConnexion.Name = authorizedCharacterData.CharacterName;
            characterConnexion.Token = accessToken.AccessToken;
            characterConnexion.RefreshToken = accessToken.RefreshToken;
            eveMiningFleetContext.SaveChanges();
            return new CharacterModel(characterConnexion);
        }

        /// <summary>
        /// Get a character by his id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CharacterModel Get(int id)
        {
            Entities.DbSet.Character character = eveMiningFleetContext.characters.Include("Corporation").Include("Alliance").FirstOrDefault(x => x.Id == id);
            if (character == null)
                return null;
            else
                return new CharacterModel(character);
        }
        /// <summary>
        /// Get all characters with the same main id.
        /// </summary>
        /// <param name="mainId"></param>
        /// <returns></returns>
        public List<CharacterModel> GetByMainId(int mainId)
        {
            return eveMiningFleetContext.characters.Include("Corporation").Include("Alliance").Where(x => x.CharacterMainId == mainId).Select(_Character => new CharacterModel(_Character)).ToList();
        }
        
        /// <summary>        
        /// Get all characters.
        /// </summary>
        /// <returns></returns>
        public List<CharacterModel> GetAll()
        {
            return eveMiningFleetContext.characters.Include("Corporation").Include("Alliance").Select(_Character => new CharacterModel(_Character)).ToList();
        }
        /// <summary>
        /// Get a character by his name. The name is not case sensitive and the space are not important.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<CharacterModel> GetByName(string name)
        {
            return eveMiningFleetContext.characters.Include("Corporation").Include("Alliance").Where(x => x.Name.ToLower().Replace(" ", "") == name.ToLower().Replace(" ", "")).Select(_Character => new CharacterModel(_Character)).ToList();
        }
        
        /// <summary>
        /// Update the main id of all characters with the old main id to the new main id.
        /// </summary>
        /// <param name="oldMainId"></param>
        /// <param name="mainId"></param>
        public async void UpdateMainId(int oldMainId, int mainId)
        {
            var characters =  eveMiningFleetContext.characters.Include("Corporation").Include("Alliance").Where(x => x.CharacterMainId == oldMainId);

            await characters.ForEachAsync(x => x.CharacterMainId = mainId);
            eveMiningFleetContext.SaveChanges();
        }
        /// <summary>
        /// Set the main id of a character.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mainID"></param>
        public void SetMain(int id, int mainID)
        {
            Character character = eveMiningFleetContext.characters.FirstOrDefault(x => x.Id == id);
            if (character != null)
            {
                character.CharacterMainId = mainID;
                eveMiningFleetContext.SaveChanges();
            }
        }   
    }
}
