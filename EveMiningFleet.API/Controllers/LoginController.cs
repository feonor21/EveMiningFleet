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
            bool requestcreatetoken = true;
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

            Character characterConnexion;
           
            //on recupere la corp du joueur qui viens de ce connecter si on le connais pas on l'insert dans la table coorp
            Corporation corporation = eveMiningFleetContext.corporations.FirstOrDefault((x) => x.Id == _eveEsiConnexion.authorizedCharacterData.CorporationID);
            if (corporation == null)
            {
                corporation = new Corporation();
                corporation.Id = _eveEsiConnexion.authorizedCharacterData.CorporationID;
                corporation.Name = EsiCorporation.GetName(corporation.Id);
                eveMiningFleetContext.corporations.Add(corporation);
            }

            //on recupere l'alliance du joueur qui viens de ce connecter si on le connais pas on l'insert dans la table alliance
            Alliance alliance = eveMiningFleetContext.alliances.FirstOrDefault((x) => x.Id == _eveEsiConnexion.authorizedCharacterData.AllianceID);
            if (alliance == null)
            {
                alliance = new Alliance();
                alliance.Id = _eveEsiConnexion.authorizedCharacterData.AllianceID;
                alliance.Name = EsiAlliance.GetName(alliance.Id);
                eveMiningFleetContext.alliances.Add(alliance);
            }

            //on recupere enfin le joueur et si jamais on le connais pas on l'insert
            characterConnexion = eveMiningFleetContext.characters.FirstOrDefault((x) => x.Id == _eveEsiConnexion.authorizedCharacterData.CharacterID);
            if (characterConnexion == null)
            {
                characterConnexion = new Character();

                characterConnexion.Id = _eveEsiConnexion.authorizedCharacterData.CharacterID;
                characterConnexion.CharacterMainId = _eveEsiConnexion.authorizedCharacterData.CharacterID;
                characterConnexion.CharacterMain = characterConnexion;
                eveMiningFleetContext.characters.Add(characterConnexion);
            }

            characterConnexion.AllianceId = _eveEsiConnexion.authorizedCharacterData.AllianceID;
            characterConnexion.CorporationId = _eveEsiConnexion.authorizedCharacterData.CorporationID;
            //on met a jours les informations du player.
            characterConnexion.Name = _eveEsiConnexion.authorizedCharacterData.CharacterName;
            characterConnexion.Token = _eveEsiConnexion.ssoToken.AccessToken;
            characterConnexion.RefreshToken = _eveEsiConnexion.ssoToken.RefreshToken;


            characterConnexion.CharacterMainId = _eveEsiConnexion.authorizedCharacterData.CharacterID;
            //ici on met a jour le mainid
            if (state!= null)
            {
                int TokenCharacterId = -1;
                if (int.TryParse(state, out TokenCharacterId))
                {
                    // on a bien un characterid dans le state
                    var TokenCharacter = eveMiningFleetContext.characters.Find(TokenCharacterId);
                    if (TokenCharacter != null)
                    {
                        characterConnexion.CharacterMainId = TokenCharacter.CharacterMainId;
                        requestcreatetoken = false;
                    }
                }
            }

            eveMiningFleetContext.SaveChanges();

            if (requestcreatetoken)
                return Ok(TokenService.Createtoken(characterConnexion.Id));
            else
                return Ok("Token Always Available.");
        }


    }
}