using Institution.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddService(builder.Configuration)
    .AddMediatorConfig();

builder.Services
    .AddControllers()
    .AddJsonOptions( options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddlewares();

app.MapControllers();

app.Run();
