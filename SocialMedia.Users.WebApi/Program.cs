using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SocialMedia.Users.Application;
using SocialMedia.Users.Infrastructure;
using SocialMedia.Users.Infrastructure.Persistence.Context;
using SocialMedia.Users.Presentation.Modules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Users API",
        Version = "v1",
        Description = "API para la gestión de usuarios."
    });
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("TestDataBase")
        ?? throw new InvalidOperationException("Connection string 'TestDataBase' was not found.");
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API V1");
    options.RoutePrefix = "swagger";
});

ModulesConfiguration.Configure(app);

app.UseHttpsRedirection();

app.Run();
