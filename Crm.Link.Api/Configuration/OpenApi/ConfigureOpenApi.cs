using Microsoft.OpenApi.Models;

namespace Crm.Link.Api
{
    public static class ConfigureOpenApi
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Crm.Link.Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", Schema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme{ Reference =
                            new OpenApiReference { Id = "jwt_auth", Type = ReferenceType.SecurityScheme } }, new string[] { } }
                });
            });
            return services;
        }

        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder builder)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI();
            return builder;
        }

        private static OpenApiSecurityScheme Schema
            => new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            };
    }
}
