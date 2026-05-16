using FluentValidation;
using Outbox.Api.Minimal.User.Validation;
using Outbox.Application;
using Outbox.Application.Interfaces;

namespace Outbox.Api.Minimal.User;

public static class ServiceRegistration
{
    public static IServiceCollection AddUserServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        services.Configure<UserValidationConfiguration>(configuration.GetSection("UserValidationConfiguration"));
        
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}