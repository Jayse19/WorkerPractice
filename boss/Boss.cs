using shared;
using boss;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<BossLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();


app.MapPost("/enlist", (EnlistRequest enlistRequest, ILogger<Program> logger) =>
{
    logger.LogInformation($"Received {enlistRequest}");
    return "This is your boss, you're FIRED";
})
.WithName("enlist");


app.MapGet("/start", (string password, BossLogic bossLogic) => bossLogic.StartRunning(password))
.WithName("start");


// app.MapGet("/start", (StartWorking startWorking, ILogger<Program> logger) =>
// {
//     logger.LogInformation($"Received {startWorking}");
// })
// .WithName("start");


// app.MapGet("/status", (Status status, ILogger<Program> logger) =>
// {
//     logger.LogInformation($"Received {status}");
// })
// .WithName("status");

app.Run();
