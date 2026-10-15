namespace Mangala.API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerWithAuth(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo { Title = "Mangala" });

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.ParameterLocation.Header,
                    Description = "Paste your JWT token here (no need to type 'Bearer ' in front)"
                });

                c.AddSecurityRequirement(document => new Microsoft.OpenApi.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document),
                        new List<string>()
                    }
                });
            });

            return services;
        }
    }
}