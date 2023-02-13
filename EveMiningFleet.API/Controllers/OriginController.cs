using EveMiningFleet.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EveMiningFleet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class OriginController : ControllerBase
    {
        protected readonly EveMiningFleetContext eveMiningFleetContext;
        public OriginController(EveMiningFleetContext _eveMiningFleetContext)
        {
            eveMiningFleetContext = _eveMiningFleetContext;
        }

    }
}
