using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using EveMiningFleet.API.Models;
using EveMiningFleet.Entities.DbSet;
using EveMiningFleet.Entities;
using EveMiningFleet.API.Services;
using static EveMiningFleet.API.Services.FleetService;

namespace EveMiningFleet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FleetController : OriginController
    {
        public FleetController(EveMiningFleetContext context) : base(context)
        {

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FleetModel>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get([FromQuery] typeview? type = null,[FromQuery] int? id = null)
        {
            if (!id.HasValue && !type.HasValue)
                return BadRequest("At least one parameter");
            if(id.HasValue && type.HasValue)            
                return BadRequest("Just one parameter at same time");
                
            var tokenCharacter = TokenService.GetCharacterFromToken(eveMiningFleetContext, HttpContext);
            if (tokenCharacter == null && ((type.HasValue && !(type.Value==typeview.viewPublic)) || id.HasValue))
                return Unauthorized("Problem for get you character link to your token.");

            FleetService fleetService = new FleetService(eveMiningFleetContext);

            List<FleetModel> allFleet = null;



            if (id.HasValue)
            {
                allFleet = new List<FleetModel>();
                allFleet.Add(fleetService.GetById(id.Value));
                if (allFleet.Count==0) return NotFound();
                if(!fleetService.AuthoriseToSeeFleet(id.Value, tokenCharacter)) return Unauthorized("You are not authorise to see this fleet");
            }
            
            if (type.HasValue)
            {
                allFleet = fleetService.GetAllByViewRight(type.Value, tokenCharacter);
            }

            if (allFleet.Count() > 0)
                return Ok(allFleet);
            else
                return NoContent();
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FleetModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult CreateFleet([FromBody]FleetModel fleet)
        {
            var tokenCharacter = TokenService.GetCharacterFromToken(eveMiningFleetContext, HttpContext);
            if (tokenCharacter == null)
                return Unauthorized("Problem for get you character link to your token.");


            if (fleet.isValidToCreate())
            {
                CharacterService characterService = new CharacterService(eveMiningFleetContext);
                if (!characterService.GetByMainId(tokenCharacter.Id).Any(_x => _x.Id==fleet.Character.Id))
                    return Unauthorized("You try to create a fleet with a character not link to your account");

                FleetService fleetService = new FleetService(eveMiningFleetContext);
                var result = fleetService.CreateFleet(fleet);
                if (result != null)
                    return Ok(result);
                else
                    return BadRequest("Problem for create fleet, if problem persist contact administrator");                
            }
            else
                return BadRequest("Invalid fleetmodel");
        }  
     
    
    
    
    
    }

}
