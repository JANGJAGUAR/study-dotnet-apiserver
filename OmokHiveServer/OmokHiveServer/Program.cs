using OmokHiveServer.Model;
using OmokHiveServer.Repository;
using OmokHiveServer.Repository.Interface;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddTransient<IRedisHiveDb, RedisHiveDb>();
builder.Services.AddTransient<IUserAccountRdb, UserAccountRdb>();

builder.Services.AddControllers();

builder.Services.AddLogging(); 
builder.Logging.ClearProviders();
builder.Logging.AddZLoggerConsole();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
}
app.MapDefaultControllerRoute();

app.Run();