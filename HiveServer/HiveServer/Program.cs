using HiveServer.Repository;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddTransient<IAccountDb, AccountDb>();
builder.Services.AddSingleton<IMemoryRdb, MemoryRdb>();

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