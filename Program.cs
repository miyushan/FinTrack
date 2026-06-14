using FinTrack.ExternalServices.AlphaVantage;
using FinTrack.ExternalServices.Interfaces;
using FinTrack.Options;
using FinTrack.Repositories;
using FinTrack.Repositories.Interfaces;
using FinTrack.Services;
using FinTrack.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Options
builder.Services.AddOptions<AlphaVantageOptions>()
    .Bind(builder.Configuration.GetSection("AlphaVantage"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.ApiKey), "API key is required")
    .ValidateOnStart();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "FinTrack",
        Version = "v1",
        Description = "API for retrieving stock movers and company information"
    });
});

// Repositories
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IMoverRepository, MoverRepository>();

// Services
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IMoverService, MoverService>();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
