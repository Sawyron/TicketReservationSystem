using Microsoft.AspNetCore.Authentication;
using TicketReservation.Persistence;
using TicketReservation.WebApi.Auth;
using TicketReservation.WebApi.Infrastucture;
using TicketReservation.WebApi.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<SwaggerOptionsSetup>();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseCors(builder => builder
    .SetIsOriginAllowed((_) => true)
    .AllowAnyHeader()
    .AllowCredentials());

if (app.Services.EnsureDbCreated())
{
    app.Services.PolulateDb();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
