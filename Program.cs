using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using HotelListing.Api.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
builder.Services.AddDbContext<HotelListingDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IHotelsService, HotelsService>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<HotelMappingProfile>();
    cfg.AddProfile<CountryMappingProfile>();
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//   more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
