using Field_Ops.API.Middleware;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.Settings;
using Field_Ops.WebApi.Extensions;
using Hangfire;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFieldOpsDependencies(builder.Configuration);

builder.Services.AddFieldOpsAuthentication(builder.Configuration);

builder.Services.AddFieldOpsAuthorization();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHangfireServer();

// Run Expiry every day at midnight
RecurringJob.AddOrUpdate<IAutomationService>(
    "auto-expire",
    service => service.RunAutoExpire(),
    Cron.Daily);

// Run auto-renew every day at 00:05
RecurringJob.AddOrUpdate<IAutomationService>(
    "auto-renew",
    service => service.RunAutoRenew(),
    "5 0 * * *");

// Run Auto Service Due every hour
RecurringJob.AddOrUpdate<IAutomationService>(
    "auto-service-due",
    service => service.RunAutoServiceDue(),
    Cron.Hourly);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SmartServeERP API",
        Version = "v1",
        Description = "SmartServe ERP Backend API with JWT Authentication"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your JWT token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/automation-dashboard");

app.MapControllers();

app.Run();
