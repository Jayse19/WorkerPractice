using Microsoft.AspNetCore.Mvc;
using shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

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

app.MapGet("/wakeup", (IConfiguration config, HttpClient httpClient) =>
{
    var enlistRequest = new EnlistRequest("http://localhost", 5178);
    httpClient.PostAsJsonAsync($"{config["BOSS"]}/enlist", enlistRequest);
})
.WithName("wakeup");
app.Run();
