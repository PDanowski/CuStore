using CuStore.Crm.GrpcService.Services;
using CuStore.Infrastructure.Crm;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<CrmDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Crm") ?? "Data Source=custore.crm.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CrmDbContext>();
    db.Database.EnsureCreated();
}

app.MapGrpcService<CrmApiService>();
app.MapGet("/", () => "Use a gRPC client to communicate with CRM endpoints.");

app.Run();
