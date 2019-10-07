using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NameGame.Persistence.Repositories.Interfaces;
using NameGame.Service.Constants;
using NameGame.Service.Models;
using NameGame.Service.Models.Dto;
using NameGame.Service.Services.Interfaces;

namespace NameGame.Service.Services
{
    public class GameService : IGameService
    {
        private readonly IProfileHttpService _profileHttpService;
        private readonly IMemoryCache _cache;
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;

        public GameService(
            IProfileHttpService profileHttpService,
            IUserRepository userRepository,
            IGameRepository gameRepository,
            IMemoryCache cache)
        {
            _profileHttpService = profileHttpService;
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _cache = cache;
        }

        public async Task<Challenge> CreateNameToFacesChallengeAsync(ChallengeRequest request)
        {
            var allProfiles = await GetProfiles().ConfigureAwait(false);
            var (options, selectedProfile) = RandomizeSelection(allProfiles, request.NumberOfOptions);

            var userId = await _userRepository.GetOrCreateUser(request.UserName).ConfigureAwait(false);
            return await CreateNameToFacesChallenge(options, selectedProfile, userId).ConfigureAwait(false);
        }

        public async Task<bool> IsAnswerValidAsync(ChallengeAnswer answer)
        {
            var challenge = await _gameRepository.GetChallenge(answer.ChallengeId).ConfigureAwait(false);
            var result = challenge.CorrectAnswer == answer.GivenAnswer;

            challenge.Attempts++;
            await _gameRepository.UpdateChallenge(challenge).ConfigureAwait(false);

            return result;
        }

        private async Task<List<Profile>> GetProfiles()
        {
            return await _cache.GetOrCreateAsync(CacheKeys.Profiles, async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheExpirations.MinutesToKeepProfiles));
                return await FetchProfilesWithImages().ConfigureAwait(false);
            }).ConfigureAwait(false);
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

        private async Task<Challenge> CreateNameToFacesChallenge(List<Profile> profiles, Profile selectedProfile, int userId)
        {
            var employee = new Employee(selectedProfile.Id, selectedProfile.FirstName, selectedProfile.LastName);
            var faces = profiles.Select(x => new Face(x.Image.Id, x.Image.Url)).ToArray();
            var challengeId = await _gameRepository.CreateChallenge(selectedProfile.Image.Id, userId).ConfigureAwait(false);

            return new Challenge(challengeId, GameConstants.NameToFacesChallengeDescription, employee, faces);
        }
    }
}
