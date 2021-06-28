using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace NameGame.Api.Setup {
    public static class Swagger {
        public static void AddSwagger(this IServiceCollection services) {
            services.AddSwaggerGen(options => {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Swagger).GetTypeInfo().Assembly.GetName().Name + ".xml";
                options.IncludeXmlComments(Path.Combine(basePath, fileName));

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "NameGame", Version = "v1" });
            });
        }

        public static void AddSwagger(this IApplicationBuilder app) {
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "NameGame v1");
                options.DocExpansion(DocExpansion.None);
                options.DisplayRequestDuration();
            });
        }
    }
}
