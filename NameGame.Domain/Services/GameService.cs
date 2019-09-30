using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NameGame.Domain.Models;
using NameGame.Domain.Services.Interfaces;

namespace NameGame.Domain.Services
{
    public class GameService : IGameService
    {
        private readonly IProfileHttpService _profileHttpService;
        private readonly IMemoryCache _cache;

        public GameService(IProfileHttpService profileHttpService,
            IMemoryCache cache)
        {
            _profileHttpService = profileHttpService;
            _cache = cache;
        }

        public async Task<Challenge> CreateChallenge(ChallengeRequest request)
        {
            var profiles = await _cache.GetOrCreateAsync("profiles", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromDays(1);
                return await GetProfiles();
            });

            var rnd = new Random();
            var index = rnd.Next(0, profiles.Count - request.NumberOfOptions);
            var subset = profiles.GetRange(index, request.NumberOfOptions);

            return new Challenge(
                "Identify which face does this name belong to.", 
                $"{subset[0].FirstName} {subset[0].LastName}",
                subset.Select(x => x.Image.Url).ToArray());
        }

        public bool IsAnswerValid(ChallengeAnswer answer)
        {
            throw new System.NotImplementedException();
        }

        private async Task<List<Profile>> GetProfiles()
        {
            var profiles = await _profileHttpService.Get<List<Profile>>("/api/v1.0/profiles");
            return profiles.Where(x => x.Image != null && !string.IsNullOrEmpty(x.Image.Url)).ToList();
        }
    }
}
