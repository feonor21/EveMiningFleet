using EveMiningFleet.API.Models;
using EveMiningFleet.Entities;
using EveMiningFleet.Logic.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EveMiningFleet.API.Services
{
    public class CharacterService
    {
        private EveMiningFleetContext eveMiningFleetContext;
        private CharacterRepository characterRepository;

        public CharacterService(EveMiningFleetContext _eveMiningFleetContext)
        {
            eveMiningFleetContext = _eveMiningFleetContext;
            characterRepository = new CharacterRepository(eveMiningFleetContext);
        }

        public List<CharacterModel> GetAll()
        {
            return characterRepository.GetSimple().Select(_Character => new CharacterModel(_Character)).ToList();
        }

        public CharacterModel GetById(int id)
        {
            Entities.DbSet.Character character = characterRepository.GetSimple().FirstOrDefault(x => x.Id == id);
            if (character == null)
                return null;
            else
                return new CharacterModel(character);
        }

        public List<CharacterModel> GetByName(string name)
        {
            return characterRepository.GetSimple().Where(x => x.Name.ToLower().Replace(" ", "") == name.ToLower().Replace(" ", "")).Select(_Character => new CharacterModel(_Character)).ToList();
        }

        public List<CharacterModel> GetByMainId(int mainId)
        {
            return characterRepository.GetSimple().Where(x => x.CharacterMainId == mainId).Select(_Character => new CharacterModel(_Character)).ToList();
        }

        public async void SetMainId(int oldMainId, int mainId)
        {
            var characters =  characterRepository.GetSimple().Where(x => x.CharacterMainId == oldMainId);

            await characters.ForEachAsync(x => x.CharacterMainId = mainId);
            eveMiningFleetContext.SaveChanges();
        }



    }
}
