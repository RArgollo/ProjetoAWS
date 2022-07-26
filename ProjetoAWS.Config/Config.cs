using Amazon.Rekognition;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjetoAWS.Application.Services;
using ProjetoAWS.lib.Data;
using ProjetoAWS.lib.Data.Interfaces;
using ProjetoAWS.lib.Data.Repositorios;
using ProjetoAWS.Services;

namespace ProjetoAWS.Config;

public static class Config
{
    public static IServiceCollection AddConfig(
         this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
        services.AddScoped<IUsuarioApplication, UsuarioApplication>();
        services.AddScoped<IUsuarioServices, UsuarioServices>();
        services.AddAWSService<IAmazonS3>();
        services.AddScoped<AmazonRekognitionClient>();
        services.AddDbContext<AWSContext>(conn =>
                                          conn.UseNpgsql(config.GetConnectionString("AWSDB"))
                                              .UseSnakeCaseNamingConvention());
        
        var awsOptions = config.GetAWSOptions();
        awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
        services.AddDefaultAWSOptions(awsOptions);
        
        return services;
    }
}