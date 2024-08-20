using Microsoft.Extensions.DependencyInjection;
using Persons.Write.AddAccountUseCase;
using Persons.Write.AddPaymentUseCase;
using Persons.Write.CreatePersonUseCase;

namespace Persons;

public static class ServiceExtension
{
    public static IServiceCollection AddPersonServices(this IServiceCollection services)
    {
        services.AddScoped<ICreatePersonService, CreatePersonService>();
        services.AddScoped<IAddAccountService, AddAccountService>();
        services.AddScoped<IAddPaymentService, AddPaymentService>();

        return services;
    }
}
