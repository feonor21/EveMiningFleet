using EveMiningFleet.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveMiningFleet.Logic.Repository
{
    public class BaseRepository
    {
        protected readonly EveMiningFleetContext eveMiningFleetContext;
        public BaseRepository(EveMiningFleetContext _eveMiningFleetContext)
        {
            eveMiningFleetContext = _eveMiningFleetContext;
        }
    }
}
