using FinTrack.ExternalServices.AlphaVantage;
using FinTrack.ExternalServices.Interfaces;
using FinTrack.Options;
using FinTrack.Repositories;
using FinTrack.Repositories.Interfaces;
using FinTrack.Services;
using FinTrack.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Options
builder.Services.AddOptions<AlphaVantageOptions>()
    .Bind(builder.Configuration.GetSection("AlphaVantage"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "API key is required")
    .ValidateOnStart();

// Repositories
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

// Services
builder.Services.AddScoped<ICompanyService, CompanyService>();

// HttpClient
builder.Services.AddHttpClient<IStockApiClient, AlphaVantageClient>(client =>
{
    var baseUrl = builder.Configuration["AlphaVantage:BaseUrl"]
              ?? throw new InvalidOperationException("AlphaVantage BaseUrl not configured.");

    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
