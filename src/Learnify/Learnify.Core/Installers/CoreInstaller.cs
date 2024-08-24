﻿using System.Reflection;
using Azure.Storage.Blobs;
using FluentValidation;
using Learnify.Core.Installer;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Managers;
using Learnify.Core.Options;
using Learnify.Core.ServiceContracts;
using Learnify.Core.Services;
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
        
        services.AddScoped<ITokenManager, TokenManager>();
        services.AddScoped<IGoogleAuthManager, GoogleAuthManager>();
        services.AddScoped<ILessonManager, LessonManager>();
        services.AddScoped<IEncryptionHelper, EncryptionHelper>();
        services.AddScoped<IBlobStorage, BlobStorage>();
        services.AddScoped<IRedisCacheManager, RedisCacheManager>();
        services.AddScoped<IParagraphManager, ParagraphManager>();
        services.AddScoped<ICourseManager, CourseManager>();
        services.AddScoped<IPrivateFileManager, PrivateFileManager>();
        services.AddScoped<ISubtitlesManager, SubtitlesManager>();
        
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IParagraphService, ParagraphService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IFileService, FileService>();

        services.Configure<GoogleAuthOptions>(config.GetSection("GoogleAuthSettings"));
        services.Configure<JwtOptions>(config.GetSection("JwtSettings"));
        services.Configure<MailOptions>(config.GetSection("MailConfig"));
        services.Configure<EncryptionOptions>(config.GetSection("EncryptionOptions"));
        services.AddSingleton(x => new BlobServiceClient(config["BlobStorageSettings:ConnectionString"]));
    }
}