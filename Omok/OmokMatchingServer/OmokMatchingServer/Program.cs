using OmokMatchingServer.Model;
using OmokMatchingServer.Service;
using OmokMatchingServer.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddSingleton<IMatchWorker, MatchWorker>();

builder.Services.AddControllers();

WebApplication app = builder.Build();

app.MapDefaultControllerRoute();

app.Run(configuration["ServerAddress"]);