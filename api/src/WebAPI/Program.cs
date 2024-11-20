using Homemap.ApplicationCore;
using Homemap.ApplicationCore.Interfaces.Seeders;
using Homemap.Infrastructure.Data;
using Homemap.Infrastructure.Data.Contexts;
using Homemap.Infrastructure.Data.Seeds;
using Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

const string devCorsPolicy = "devNuxtCorsPolicy";

builder.Services
    .AddDatabase(builder.Configuration)
    .AddRepositories()
    .AddMessagingService(builder.Configuration)
    .AddMappers()
    .AddValidators()
    .AddApplicationServices()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCors(opts => opts.AddPolicy(devCorsPolicy, policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "http://localhost:3080")
            .AllowAnyHeader()
            .AllowAnyMethod();
    }))
    .AddControllers();

var app = builder.Build();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    // add swagger
    app.UseSwagger().UseSwaggerUI(opts =>
    {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        opts.RoutePrefix = string.Empty;
    });

    // seed database
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    new List<ISeeder>
    {
        new ProjectSeeder(context),
        new ReceiverSeeder(context),
        new DeviceSeeder(context)
    }.ForEach(async seeder => await seeder.SeedAsync());

    // enable cors
    app.UseCors(devCorsPolicy);
}

app.MapControllers();

app.Run();
