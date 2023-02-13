using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using EveMiningFleet.Entities.DbSet;
using EveMiningFleet.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EveMiningFleet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatapriceController : OriginController
    {
        public DatapriceController(EveMiningFleetContext context) : base(context)
        {

        }

        // GET: api/<DatapriceController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DataPrice>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromQuery] int? typeid = null)
        {
            if (!typeid.HasValue)
            {
                var allDataprice = eveMiningFleetContext.dataPrices.ToList();
                return allDataprice.Any() ? (IActionResult)Ok(allDataprice) : NoContent();
            }
            else
            {
                var Datapricetarget = eveMiningFleetContext.dataPrices.FirstOrDefault(x => x.TypeId == typeid);
                if (Datapricetarget != null)
                    return Ok(new List<DataPrice> { Datapricetarget });
                else
                    return NotFound();
            }
        }


    }
}
