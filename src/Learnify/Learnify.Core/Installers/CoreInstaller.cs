using System.Reflection;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs;
using FluentValidation;
using Learnify.Core.Installer;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Managers;
using Learnify.Core.Options;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Services;
using MassTransit;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace Learnify.Core.Installers;

public class CoreInstaller: IInstaller
{        
    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    public void InstallServices(IServiceCollection services, IConfiguration config)
    {
        services.AddEndpointsApiExplorer();
        services.AddControllers();
        services.AddHttpClient();
        services.AddValidatorsFromAssembly(Assembly);
        services.AddFluentValidationAutoValidation();
        services.AddAutoMapper(Assembly);
        services.AddSignalR();

        services.Configure<FormOptions>(opts =>
        {
            opts.MultipartBodyLengthLimit = (long)10 * 1024 * 1024 * 1024;
        });
        
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Learnify API",
                Description = "Learnify API"
            }); 
        });
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetConnectionString("RedisCache");
        });
        
        services.AddMassTransit(x =>
        {
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("MainService", false));

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.UseMessageRetry(r =>
                {
                    r.Handle<RabbitMqConnectionException>();
                    r.Interval(5, TimeSpan.FromSeconds(10));
                });

                cfg.Host(config["RabbitMq:Host"], "/", host =>
                {
                    host.Username(config.GetSection("RabbitMq:Username").Value ?? "guest");
                    host.Password(config.GetSection("RabbitMq:Password").Value ?? "guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        
        services.AddScoped<ITokenManager, TokenManager>();
        services.AddScoped<IGoogleAuthManager, GoogleAuthManager>();
        services.AddScoped<IBlobStorage, BlobStorage>();
        services.AddScoped<IRedisCacheManager, RedisCacheManager>();
        services.AddScoped<PrivateFileService, PrivateFileService>();
        services.AddScoped<ISubtitlesManager, SubtitlesManager>();
        services.AddScoped<IUserValidatorManager, UserValidatorManager>();
        
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IParagraphService, ParagraphService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IPrivateFileService, PrivateFileService>();
        services.AddScoped<IQuizService, QuizService>();

        services.Configure<GoogleAuthOptions>(config.GetSection("GoogleAuthSettings"));
        services.Configure<JwtOptions>(config.GetSection("JwtSettings"));
        services.Configure<MailOptions>(config.GetSection("MailConfig"));
        services.Configure<EncryptionOptions>(config.GetSection("EncryptionOptions"));
        services.AddSingleton(x => new BlobServiceClient(config["BlobStorageSettings:ConnectionString"]));
    }
}