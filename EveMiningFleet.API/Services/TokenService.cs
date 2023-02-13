using EveMiningFleet.Entities;
using EveMiningFleet.Entities.DbSet;
using EveMiningFleet.Logic.Token.jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveMiningFleet.API.Services
{
    public static class TokenService
    {
        public static readonly string characterIdClaimKey = "CharacterIdClaimKey";
        public static Character GetCharacterFromToken(EveMiningFleetContext DbContext, HttpContext httpContext)
        {
            if(httpContext==null) return null;
            var httpcontextuserIdentity = httpContext.User.Identity;

            var identity = httpcontextuserIdentity as System.Security.Claims.ClaimsIdentity;

            var claimObject = identity?.FindFirst(TokenService.characterIdClaimKey);
            if (claimObject == null) return null;

            int tokenCharacterId;
            if (!int.TryParse(claimObject.Value, out tokenCharacterId)) return null;
            
            Character response = DbContext.characters.FirstOrDefault(x => x.Id == tokenCharacterId);
            return response;
        }

        
        public static string Createtoken(int characterId)
        {
            var tokenKey = Encoding.UTF8.GetBytes(System.Environment.GetEnvironmentVariable("JWTToken"));
            var token = new JwtTokenBuilder()
                    .AddSecurityKey(new SymmetricSecurityKey(tokenKey))
                    .AddClaim(TokenService.characterIdClaimKey, characterId.ToString())
                    .AddExpiry(4320)
                    .Build();

            return token.Value;
        }
    }

    public class TokenServiceCharacters{
        public Character main { get; set; }
        public List<Character> all { get; set; } = new List<Character>();

        public TokenServiceCharacters(Character character)
                {
                    main = character;
                    all.Add(character);
                }
        public void addCharacter(Character character)
        {
            if (!all.Any(x => x.Id == character.Id))
                all.Add(character);
        }
    }

}
