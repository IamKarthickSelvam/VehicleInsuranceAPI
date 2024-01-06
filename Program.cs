using API.Data;
using API.Interfaces;
using API.Services;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddFeatureManagement();
builder.Services.AddHttpClient();
builder.Services.AddSingleton(_ => new BlobServiceClient(
    builder.Configuration.GetValue<string>("AzureBlob:BlobConnection")
    ));
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("AzureRedisConnection");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder => builder.AllowAnyMethod().AllowAnyHeader().WithOrigins("https://wonderful-wave-002693203.4.azurestaticapps.net"));

app.Use(async (HttpContext, next) =>
{
    HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "https://wonderful-wave-002693203.4.azurestaticapps.net");
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
PrepDb.PrepPopulation(app);

app.Run();
