using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NameGame.Domain.Constants;
using NameGame.Domain.Models;
using NameGame.Domain.Models.Dto;
using NameGame.Domain.Services.Interfaces;

namespace NameGame.Domain.Services
{
    public class GameService : IGameService
    {
        private readonly IProfileHttpService _profileHttpService;
        private readonly IMemoryCache _cache;

        public GameService(IProfileHttpService profileHttpService, IMemoryCache cache)
        {
            _profileHttpService = profileHttpService;
            _cache = cache;
        }

        public async Task<Challenge> CreateNameToFacesChallengeAsync(ChallengeRequest request)
        {
            var allProfiles = await GetProfiles().ConfigureAwait(false);
            var (options, selectedProfile) = RandomizeSelection(allProfiles, request.NumberOfOptions);

            return CreateNameToFacesChallenge(options, selectedProfile);
        }

        public async Task<bool> IsAnswerValidAsync(ChallengeAnswer answer)
        {
            var dic = _cache.Get<Dictionary<string, string>>(CacheKeys.NameToFacesAnswers);
            if (dic == null)
                await GetProfiles().ConfigureAwait(false);

            return dic[answer.SelectedImageId] == answer.GivenUserId;
        }

        private async Task<List<Profile>> GetProfiles()
        {
            return await _cache.GetOrCreateAsync(CacheKeys.Profiles, async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirations.MinutesToKeepProfiles));

                var profiles = await FetchProfilesWithImages().ConfigureAwait(false);
                BuildAnswersMap(profiles);

                return profiles;
            }).ConfigureAwait(false);

            void BuildAnswersMap(List<Profile> profiles)
            {
                var dic = new Dictionary<string, string>();
                profiles.ForEach(p => dic[p.Image.Id] = p.Id);
                _cache.Set(CacheKeys.NameToFacesAnswers, dic, TimeSpan.FromMinutes(CacheExpirations.MinutesToKeepProfiles + 1));
            }
        }

        private async Task<List<Profile>> FetchProfilesWithImages()
        {
            var profiles = await _profileHttpService.Get<List<Profile>>("/api/v1.0/profiles").ConfigureAwait(false);
            return profiles.Where(x => x.Image != null && !string.IsNullOrEmpty(x.Image.Url)).ToList();
        }

        private (List<Profile>, Profile) RandomizeSelection(List<Profile> allProfiles, int numberOfOptions)
        {
            var rnd = new Random();
            var index = rnd.Next(0, allProfiles.Count - numberOfOptions);
            var subset = allProfiles.GetRange(index, numberOfOptions);

            index = rnd.Next(0, subset.Count - 1);
            var selectedProfile = subset[index];

            return (subset, selectedProfile);
        }

        private Challenge CreateNameToFacesChallenge(List<Profile> profiles, Profile selectedProfile)
        {
            const string description = "Identify which face does this name belong to.";
            var employee = new Employee(selectedProfile.Id, selectedProfile.FirstName, selectedProfile.LastName);
            var faces = profiles.Select(x => new Face(x.Image.Id, x.Image.Url)).ToArray();

            return new Challenge(description, employee, faces);
        }
    }
}
