using Snafets.Extensions.Logging.TracesOnError;
using Snafets.Extensions.Logging.TracesOnError.JsonConsoleSink;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddTracesOnError<JsonConsoleSink>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
