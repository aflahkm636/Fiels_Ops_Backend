using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Field_Ops.WebApi.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddFieldOpsAuthentication(
            this IServiceCollection services, IConfiguration config)
        {
            var jwt = config.GetSection("Jwt");
            var key = jwt["Key"] ?? throw new Exception("Jwt:Key is missing.");

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwt["Issuer"],
                        ValidAudience = jwt["Audience"],
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }

        public static IServiceCollection AddFieldOpsAuthorization(
            this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
                options.AddPolicy("StaffOnly", p => p.RequireRole("Staff"));
                options.AddPolicy("TechnicianOnly", p => p.RequireRole("Technician"));
                options.AddPolicy("CustomerOnly", p => p.RequireRole("Customer"));
            });

            return services;
        }
    }
}
