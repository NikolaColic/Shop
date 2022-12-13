using Data.Entities;
using Infrastructure.Repository.Interfaces;
using Infrastructure.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Vendor.Api.Profiles;
using Vendor.Api.Repository;
using Vendor.Api.Repository.EF;
using Vendor.Api.Repository.UnitOfWork;
using Vendor.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    // Setup a HTTP/2 endpoint without TLS.
    //options.ListenLocalhost(5001, o => o.Protocols =
    //    HttpProtocols.Http2);
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddGrpc();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//EF Configuration
var configuration = builder.Configuration;
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Connection")));

//Automapper configuration

builder.Services.AddAutoMapper(typeof(MappingProfile));

//UoW Registration 

builder.Services.AddScoped<IUnitOfWork<Article>, ArticleUnitOfWork>();

//Repository Registration

builder.Services.AddScoped<IRepositoryList<Article>, ArticleRepository>();

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
app.MapGrpcService<ArticleService>();

app.Run();
