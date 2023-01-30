using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RssManager.DataAccess;
using RssManager.Interfaces;
using RssManager.Services;
using RssManager.Services.Repositories;
using System.Security.Claims;

namespace RssManager.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<RssManagerContext>();

            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IFeedReposiory, FeedRepository>();

            builder.Services.AddAuthentication(builder.Configuration);
            builder.Services.AddAuthorization(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwagger(builder.Configuration);

            builder.Services.AddHostedService<WorkerService>();
        }
        public static void AddAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://{configuration["Auth0:Domain"]}/";
                    options.Audience = configuration["Auth0:Audience"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });
        }

        public static void AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "access:admin-data", policy =>
                        policy.RequireAssertion(context =>
                            context.User.HasClaim(claim =>
                                claim.Type == "permissions" &&
                                claim.Value == "access:admin-data" &&
                                claim.Issuer == "https://" + configuration["Auth0:Domain"] + "/"
                            )
                        )
                );
            });
        }

        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Documentation",
                    Version = "v1.0",
                    Description = ""
                });
                options.ResolveConflictingActions(x => x.First());
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    BearerFormat = "JWT",
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri($"https://{configuration["Auth0:Domain"]}/oauth/token"),
                            AuthorizationUrl =
                                new Uri(
                                    $"https://{configuration["Auth0:Domain"]}/authorize?audience={configuration["Auth0:Audience"]}")
                        }
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}
                        },
                        new[] {"openid"}
                    }
                });
            });
        }
    }
}
