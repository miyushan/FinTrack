using FinTrack.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddOptions<AlphaVantageOptions>()
    .Bind(builder.Configuration.GetSection("AlphaVantage"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "API key is required")
    .Validate(o => !string.IsNullOrWhiteSpace(o.BaseUrl), "BaseUrl is required")
    .ValidateOnStart();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
