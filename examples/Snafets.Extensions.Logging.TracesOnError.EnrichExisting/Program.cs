using Snafets.Extensions.Logging.TracesOnError;

var builder = WebApplication.CreateBuilder(args);

// Add logging to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddTracesOnErrorWithoutLogSink();

// Add services to the container.
builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
