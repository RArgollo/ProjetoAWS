using Amazon.Rekognition;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using ProjetoAWS.Application.Services;
using ProjetoAWS.lib.Data;
using ProjetoAWS.lib.Data.Interfaces;
using ProjetoAWS.lib.Data.Repositorios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<AmazonRekognitionClient>();

builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioApplication, UsuarioApplication>();

builder.Services.AddDbContext<AWSContext>(conn =>
conn.UseNpgsql(builder.Configuration.GetConnectionString("AWSDB"))
.UseSnakeCaseNamingConvention());

var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
builder.Services.AddDefaultAWSOptions(awsOptions);

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
