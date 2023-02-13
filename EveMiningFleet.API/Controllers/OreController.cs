using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EveMiningFleet.Entities.DbSet;
using EveMiningFleet.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EveMiningFleet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OreController : OriginController
    {
        public OreController(EveMiningFleetContext context) : base(context)
        {

        }

        // GET: api/<DatapriceController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Ore>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromQuery] int? id = null)
        {
            //get all
            if (!id.HasValue)
            {
                var allOres = eveMiningFleetContext.ores.ToList();
                return allOres.Any() ? (IActionResult)Ok(allOres) : NoContent();
            }
            else
            {

                var oreTarget = eveMiningFleetContext.ores.FirstOrDefault(x => x.Id == id);
                if (oreTarget != null)
                    return Ok(new List<Ore> { oreTarget });
                else
                    return NotFound();
            }
        }
    }
}
