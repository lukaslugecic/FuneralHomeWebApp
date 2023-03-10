using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using FuneralHome.Repositories;
using FuneralHome.Repositories.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// create a configuration (app settings) from the appsettings file, depending on the ENV

IConfiguration configuration = builder.Environment.IsDevelopment()
 ? builder.Configuration.AddJsonFile("appsettings.Development.json").Build()
 : builder.Configuration.AddJsonFile("appsettings.json").Build();
// register the DbContext - EF ORM
// this allows the DbContext to be injected
builder.Services.AddDbContext<FuneralHomeContext>(options =>
options.UseSqlServer(configuration.GetConnectionString("FuneralHomeDB")));
builder.Services.AddTransient<ICvijeceRepository<int, Cvijece>, CvijeceRepository>();
builder.Services.AddTransient<IGlazbaRepository<int, Glazba>, GlazbaRepository>();
builder.Services.AddTransient<IKorisnikRepository<int, Korisnik>, KorisnikRepository>();
builder.Services.AddTransient<ILijesRepository<int, Lijes>, LijesRepository>();
builder.Services.AddTransient<INadgrobniZnakRepository<int, NadgrobniZnak>, NadgrobniZnakRepository>();
builder.Services.AddTransient<IOglasRepository<int, Oglas>, OglasRepository>();
builder.Services.AddTransient<IOsiguranjeRepository<int, Osiguranje>, OsiguranjeRepository>();
builder.Services.AddTransient<IPogrebRepository<int, Pogreb>, PogrebRepository>();
builder.Services.AddTransient<ISmrtniSlucajRepository<int, SmrtniSlucaj>, SmrtniSlucajRepository>();
builder.Services.AddTransient<IUrnaRepository<int, Urna>, UrnaRepository>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
