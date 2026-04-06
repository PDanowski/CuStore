using CuStore.Core.Abstractions;
using CuStore.Infrastructure.Data;
using CuStore.Infrastructure.Repositories;
using CuStore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CuStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddStoreInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StoreDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("Store") ?? "Data Source=custore.store.db"));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IShippingMethodRepository, ShippingMethodRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmailSender, NoOpEmailSender>();
        services.AddScoped<IOrderCheckoutService, OrderCheckoutService>();

        return services;
    }
}
