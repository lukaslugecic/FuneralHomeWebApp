using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Repositories;
using FuneralHome.Repositories.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// create a configuration (app settings) from the appsettings file, depending on the ENV

IConfiguration configuration = builder.Environment.IsDevelopment()
 ? builder.Configuration.AddJsonFile("appsettings.Development.json").Build()
 : builder.Configuration.AddJsonFile("appsettings.json").Build();
// register the DbContext - EF ORM
// this allows the DbContext to be injected
builder.Services.AddDbContext<FuneralHomeContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("FuneralHomeDB")));
builder.Services.AddTransient<IVrstaUslugeRepository, VrstaUslugeRepository>();
builder.Services.AddTransient<IUslugaRepository, UslugaRepository>();
builder.Services.AddTransient<IVrstaOpremeRepository, VrstaOpremeRepository>();
builder.Services.AddTransient<IOpremaRepository, OpremaRepository>();
builder.Services.AddTransient<IKorisnikRepository, KorisnikRepository>();
builder.Services.AddTransient<IOglasRepository, OglasRepository>();
builder.Services.AddTransient<IOsiguranjeRepository, OsiguranjeRepository>();
builder.Services.AddTransient<IPogrebRepository, PogrebRepository>();
builder.Services.AddTransient<ISmrtniSlucajRepository, SmrtniSlucajRepository>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("SecretKeys:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options => options.AddPolicy(name: "PogrebnoPoduzeceOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PogrebnoPoduzeceOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
