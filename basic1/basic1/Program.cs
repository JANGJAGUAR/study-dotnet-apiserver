using Microsoft.AspNetCore.Builder;
using basic1;
using basic1.Repository;
using ZLogger;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.Configure<DbConfig>(configuration.GetSection(nameof(DbConfig)));

builder.Services.AddTransient<IAccountDb, AccountDb>();
builder.Services.AddSingleton<IMemoryDb, MemoryDb>();

builder.Services.AddLogging(); 
builder.Services.AddControllers();
    
builder.Logging.ClearProviders();
builder.Logging.AddZLoggerConsole();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
}

app.MapDefaultControllerRoute();

// IConfiguration configuration = app.Configuration;
// DBManager.Init(configuration);


app.Run();
// configuration["ServerAddress"]

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// Configure the HTTP request pipeline.
// app.UseSwagger();
// app.UseSwaggerUI();
// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
//     {
//         var forecast = Enumerable.Range(1, 5).Select(index =>
//                 new WeatherForecast
//                 (
//                     DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                     Random.Shared.Next(-20, 55),
//                     summaries[Random.Shared.Next(summaries.Length)]
//                 ))
//             .ToArray();
//         return forecast;
//     })
//     .WithName("GetWeatherForecast")
//     .WithOpenApi();