using CuStore.Crm;
using CuStore.Core.Abstractions;
using CuStore.Infrastructure;
using CuStore.Infrastructure.Data;
using CuStore.WebApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddStoreInfrastructure(builder.Configuration);

builder.Services.AddGrpcClient<CrmApi.CrmApiClient>(options =>
{
    options.Address = new Uri(builder.Configuration["Grpc:CrmUrl"] ?? "https://localhost:7179");
});

builder.Services.AddScoped<ICrmClient, GrpcCrmClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
    db.Database.EnsureCreated();
    StoreDbSeed.Seed(db);
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
