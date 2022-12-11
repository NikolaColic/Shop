using Data.Entities;
using Infrastructure.AppConfig.Implementations;
using Infrastructure.AppConfig.Interfaces;
using Infrastructure.Repository.Interfaces;
using Infrastructure.Service.Interfaces;
using Infrastructure.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shop.Api.Constants;
using Shop.Repository.EF;
using Shop.Repository.Repository.Implementation;
using Shop.Repository.UnitOfWork;
using Shop.Service.Implementations;
using Shop.Service.Implementations.Proxy;
using Shop.Service.Interfaces;
using Shop.Service.Interfaces.Proxy;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//Services Registration 

builder.Services.AddScoped<IArticleProxyService, ArticleProxyService>();
builder.Services.AddScoped<IProxyService<User>, UserProxyService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IGenericService<User>, UserService>();
builder.Services.AddScoped<IAuthService, UserService>();

//UoW Registration 

builder.Services.AddScoped<IUnitOfWork<Article>, ArticleUnitOfWork>();
builder.Services.AddScoped<IUnitOfWork<User>, UserUnitOfWork>();

//Repository Registration

builder.Services.AddScoped<IRepositoryList<Article>, ArticleRepository>();
builder.Services.AddScoped<IUserRepository<User>, UserRepository>();

//Authentication Configuration

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RoleClaimType = ClaimTypes.Role,
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
        };
    });

//Swagger Configuration

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 5 Web API",
    });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}

        }
    });
});

//Authorization Configuration

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Customer", policy => policy.RequireRole("Customer"));

});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
