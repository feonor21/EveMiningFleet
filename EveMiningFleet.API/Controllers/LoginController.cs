using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using EveMiningFleet.Logic.EsiEve;
using EveMiningFleet.Entities.DbSet;
using EveMiningFleet.Entities;
using EveMiningFleet.API.Services;

namespace EveMiningFleet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : OriginController
    {
        public LoginController(EveMiningFleetContext context) : base(context)
        {

        }

        [HttpGet("geturllogin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult geturllogin()
        {
            var tokenCharacter = TokenService.GetCharacterFromToken(eveMiningFleetContext, HttpContext);
            var _eveEsiConnexion = new EveEsiConnexion();
            string response;

            if (tokenCharacter == null)
                response = _eveEsiConnexion.GetUrlConnection();
            else
                response = _eveEsiConnexion.GetUrlConnection(tokenCharacter.CharacterMainId.ToString());


            return Ok(response);
        }

        [HttpGet("callbackccp")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CallbackCCP([FromQuery] string code, [FromQuery] string state = null)
        {
            if (code == "")
                return Unauthorized();

            var _eveEsiConnexion = new EveEsiConnexion();
            try
            {
                await _eveEsiConnexion.GetToken(code);
                await _eveEsiConnexion.ConnectCharCCP();
            }
            catch (System.Exception)
            {
                return Unauthorized();
            }

            var characterService = new CharacterService(eveMiningFleetContext);
            var corporationService = new CorporationService(eveMiningFleetContext);
            var allianceService = new AllianceService(eveMiningFleetContext);

            allianceService.GetOrCreate(_eveEsiConnexion.authorizedCharacterData.AllianceID);
            corporationService.GetOrCreate(_eveEsiConnexion.authorizedCharacterData.CorporationID);

            var characterAuthorized = characterService.GetAndUpdateByauthorizedCharacterData(_eveEsiConnexion.authorizedCharacterData, _eveEsiConnexion.ssoToken);

            int mainCharacterId = characterAuthorized.Id;
           
            //ici on met a jour le mainid
            if (state!= null && int.TryParse(state, out mainCharacterId))
                characterService.SetMain(characterAuthorized.Id, mainCharacterId);

            return Ok(TokenService.Createtoken(mainCharacterId));
        }


    }
}