using Microsoft.EntityFrameworkCore;
using Minio;
using OcrWebService;
using OcrWebService.Data;
using OcrWebService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<FileManagementService>();
builder.Services.AddTransient<OcrService>();
builder.Services.AddScoped<OcrWebService.Minio.GgitMinio>();

///MINIO
String endpoint = "localhost:9000";
String accessKey = "minio";
String secretKey = "minio123";
// Add Minio using the custom endpoint and configure additional settings for default MinioClient initialization
builder.Services.AddMinio(configureClient => configureClient.WithSSL(false)
    .WithEndpoint(endpoint)
    .WithCredentials(accessKey, secretKey));
///
var Configuration = builder.Configuration;
builder.Services.AddDbContext<DbFileContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();