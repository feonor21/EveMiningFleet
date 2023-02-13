using EveMiningFleet.Logic.Tools;
using ESI.NET;
using ESI.NET.Models.Alliance;

namespace EveMiningFleet.Logic.EsiEve
{
    public static class EsiAlliance
    {
        /// <summary>
        /// Recupere le nom de l'allaince
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetName(int ID)
        {
            return Retry.Do(() =>
            {
                var eveEsiConnexion = new EveEsiConnexion();
                EsiResponse<Alliance> tmp = eveEsiConnexion.EsiClient.Alliance.Information(ID).Result;

                return tmp.Data.Name;
            }, System.TimeSpan.FromMilliseconds(0), 5);
        }
    }
}
