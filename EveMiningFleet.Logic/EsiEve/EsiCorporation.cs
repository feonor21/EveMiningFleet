﻿using EveMiningFleet.Logic.Tools;
using ESI.NET;
using ESI.NET.Models.Corporation;

namespace EveMiningFleet.Logic.EsiEve
{
    public static class EsiCorporation
    {
        /// <summary>
        /// recupere le nom de la corp
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetName(int ID)
        {
            return Retry.Do(() =>
            {
                var eveEsiConnexion = new EveEsiConnexion();
                EsiResponse<Corporation> tmp = eveEsiConnexion.EsiClient.Corporation.Information(ID).Result;

                return tmp.Data.Name;
            }, System.TimeSpan.FromMilliseconds(0), 5);
        }
    }
}
