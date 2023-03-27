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
builder.Services.AddTransient<ICvijeceRepository, CvijeceRepository>();
builder.Services.AddTransient<IGlazbaRepository, GlazbaRepository>();
builder.Services.AddTransient<IKorisnikRepository, KorisnikRepository>();
builder.Services.AddTransient<ILijesRepository, LijesRepository>();
builder.Services.AddTransient<INadgrobniZnakRepository, NadgrobniZnakRepository>();
builder.Services.AddTransient<IOglasRepository, OglasRepository>();
builder.Services.AddTransient<IOsiguranjeRepository, OsiguranjeRepository>();
builder.Services.AddTransient<IPogrebRepository, PogrebRepository>();
builder.Services.AddTransient<ISmrtniSlucajRepository, SmrtniSlucajRepository>();
builder.Services.AddTransient<IUrnaRepository, UrnaRepository>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
