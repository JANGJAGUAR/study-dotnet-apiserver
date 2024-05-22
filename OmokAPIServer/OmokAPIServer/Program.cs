using OmokAPIServer.Repository;
using OmokAPIServer.Repository.Interface;
using OmokAPIServer.Service;
using OmokAPIServer.Service.Interface;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddTransient<IUserInfoRdb, UserInfoRdb>();
builder.Services.AddSingleton<IRedisApiDb, RedisApiDb>();
builder.Services.AddTransient<IAuthService, AuthService>();

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