using shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
})
.WithName("enlist");


app.MapGet("/done", (DoneWorking doneWorking, ILogger<Program> logger) =>
{
    logger.LogInformation($"Received {doneWorking}");
})
.WithName("done");


app.MapGet("/start", (StartWorking startWorking, ILogger<Program> logger) =>
{
    logger.LogInformation($"Received {startWorking}");
})
.WithName("start");


app.MapGet("/status", (Status status, ILogger<Program> logger) =>
{
    logger.LogInformation($"Received {status}");
})
.WithName("status");

app.Run();
