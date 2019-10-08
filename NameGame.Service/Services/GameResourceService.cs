using Microsoft.Extensions.Caching.Memory;
using NameGame.Persistence.Models;
using NameGame.Persistence.Repositories.Interfaces;
using NameGame.Service.Constants;
using NameGame.Service.Models;
using NameGame.Service.Models.Dto;
using NameGame.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NameGame.Service.Services
{
    public class GameResourceService : IGameResourceService
    {
        private readonly IProfileHttpService _profileHttpService;
        private readonly IMemoryCache _cache;
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;

        public GameResourceService(
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

        public async Task<NameToFacesChallenge> CreateNameToFacesChallenge(ChallengeRequest request)
        {
            var allProfiles = await GetProfiles().ConfigureAwait(false);
            var (options, selectedProfile) = RandomizeSelection(allProfiles, request.NumberOfOptions);

            var userId = await _userRepository.GetOrCreateUser(request.UserName).ConfigureAwait(false);
            return await CreateNameToFacesChallenge(options, selectedProfile, userId).ConfigureAwait(false);
        }

        public async Task<FaceToNamesChallenge> CreateFaceToNamesChallenge(ChallengeRequest request)
        {
            var allProfiles = await GetProfiles().ConfigureAwait(false);
            var (options, selectedProfile) = RandomizeSelection(allProfiles, request.NumberOfOptions);

            var userId = await _userRepository.GetOrCreateUser(request.UserName).ConfigureAwait(false);
            return await CreateFaceToNamesChallenge(options, selectedProfile, userId).ConfigureAwait(false);
        }

        public async Task<ChallengeAnswerValidationResult> IsAnswerValid(ChallengeAnswer answer)
        {
            var challenge = await _gameRepository.GetChallenge(answer.ChallengeId).ConfigureAwait(false);
            if (challenge == null) return new ChallengeAnswerValidationResult(GameConstants.NoChallengeValidationResult);
            if (challenge.Solved) return new ChallengeAnswerValidationResult(GameConstants.ChallengeAlreadySolvedValidationResult);

            var result = challenge.CorrectAnswer == answer.GivenAnswer;

            challenge.Attempts++;
            challenge.Solved = result;
            await _gameRepository.UpdateChallenge(challenge).ConfigureAwait(false);

            return result ? new ChallengeAnswerValidationResult() : new ChallengeAnswerValidationResult(GameConstants.WrongAnswer);
        }

        public async Task<Challenge> GetChallenge(int id)
        {
            return await _gameRepository.GetChallenge(id).ConfigureAwait(false);
        }

        public async Task<List<Challenge>> GetChallenges()
        {
            return await _gameRepository.GetChallenges().ConfigureAwait(false);
        }

        public async Task<bool> DeleteChallenge(Challenge challenge)
        {
            return await _gameRepository.DeleteChallenge(challenge).ConfigureAwait(false);
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

        private async Task<NameToFacesChallenge> CreateNameToFacesChallenge(List<Profile> profiles, Profile selectedProfile, int userId)
        {
            var employee = new Employee(selectedProfile.Id, selectedProfile.FirstName, selectedProfile.LastName);
            var faces = profiles.Select(x => new Face(x.Image.Id, x.Image.Url)).ToArray();
            var challengeId = await _gameRepository.CreateChallenge(selectedProfile.Image.Id, userId).ConfigureAwait(false);

            return new NameToFacesChallenge(challengeId, GameConstants.NameToFacesChallengeDescription, employee, faces);
        }

        private async Task<FaceToNamesChallenge> CreateFaceToNamesChallenge(List<Profile> profiles, Profile selectedProfile, int userId)
        {
            var employees = profiles.Select(x => new Employee(x.Id, x.FirstName, x.LastName)).ToArray();
            var face = new Face(selectedProfile.Image.Id, selectedProfile.Image.Url);
            var challengeId = await _gameRepository.CreateChallenge(selectedProfile.Id, userId).ConfigureAwait(false);

            return new FaceToNamesChallenge(challengeId, GameConstants.FaceToNamesChallengeDescription, employees, face);
        }
    }
}
