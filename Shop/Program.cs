using Infrastructure.AppConfig.Implementations;
using Infrastructure.AppConfig.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Constants;
using Shop.Repository.EF;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//EF Configuration

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(SettingConstants.ConnectionString)));

//LazyCache Configuration 

builder.Services.AddLazyCache();

//AppConfig

var appConfig = new AppConfig();

configuration.GetSection(SettingConstants.AppConfig).Bind(appConfig);
builder.Services.AddSingleton<IAppConfig>(appConfig);


var app = builder.Build();

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
