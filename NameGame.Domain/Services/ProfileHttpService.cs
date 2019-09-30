﻿using NameGame.Domain.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NameGame.Domain.Services
{
    public class ProfileHttpService : IProfileHttpService
    {
        private readonly HttpClient _httpClient;

        public ProfileHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> Get<T>(string uri)
        {
            var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"External API returned error. {response.ReasonPhrase}. {await response.Content.ReadAsStringAsync()}");

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
