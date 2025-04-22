using FIAP.FCG.Crosscutting;
using FIAP.FCG.Domain.Entities;
using FIAP.FCG.Presentation.JwtConfig;
using Keycloak.Net;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<KeycloakOptions>(
    builder.Configuration.GetSection("Keycloak"));

builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<KeycloakOptions>>().Value;
    var keycloakClient = new KeycloakClient(options.ServerUrl, options.ClientId);
    return keycloakClient;
});

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddInfrastructureSwagger();

var app = builder.Build();

app.UseSwaggerConfiguration();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
