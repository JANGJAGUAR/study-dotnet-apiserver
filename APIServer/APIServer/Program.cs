using APIServer.Repository;
using APIServer.Service;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddTransient<IUserDb, UserDb>();
builder.Services.AddSingleton<IMemRdb, MemRdb>();
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