using Microsoft.AspNetCore.Mvc;
using shared;
using worker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<WorkerBackgroundService>();
builder.Services.AddSingleton<WorkerState>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapPost("/move", ([FromBody]Location destination, ILogger<Program> logger) =>
{
    logger.LogInformation($"Trying to move to {destination}");
})
.WithName("move");

app.Run();
