using Microsoft.EntityFrameworkCore;
using SocialMedia.Users.Application;
using SocialMedia.Users.Infrastructure;
using SocialMedia.Users.Infrastructure.Persistence.Context;
using SocialMedia.Users.Application.Abstractions;
using SocialMedia.Users.Presentation.Modules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string? connectionString = builder.Configuration.GetConnectionString("TestDataBase");
    options.UseSqlServer(connectionString ?? "");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
    });
}

ModulesConfiguration.Configure(app);

app.UseHttpsRedirection();

app.Run();
