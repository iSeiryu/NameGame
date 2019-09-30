using NameGame.Domain.Services;
using NameGame.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NameGame.API.Infrastructure
{
    public static class DependencyInjectionServiceCollectionExtensions
    {
        public static IServiceCollection SetupDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IGameService, GameService>();
            services.AddSingleton<IProfileHttpService>(new ProfileHttpService(CreateHttpClient("WillowTreeUrl", configuration)));

            return services;
        }

        private static HttpClient CreateHttpClient(string endPointName, IConfiguration configuration)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetSection(endPointName).Value)
            };

            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
