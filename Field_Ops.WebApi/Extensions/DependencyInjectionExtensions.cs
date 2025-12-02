using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.Services;
using Field_Ops.Application.Settings;
using Field_Ops.Infrastructure.Repository;
using System.Data;
using Microsoft.Data.SqlClient;
using Field_Ops.Application.Service;

namespace Field_Ops.WebApi.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddFieldOpsDependencies(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<JwtSettings>(config.GetSection("Jwt"));

            services.AddScoped<IDbConnection>(sp =>
            {
                var connString = config.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string not found");

                var conn = new SqlConnection(connString);
                conn.Open();
                return conn;
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IEmployeesRepository, EmployeesRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeesService, EmployeesService>();


            return services;
        }
    }
}
