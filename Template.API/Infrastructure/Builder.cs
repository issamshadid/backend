using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Template.API.ErrorHandling;
using Template.Business.Infrastructure;
using Template.Configurations.ConfigurationReader;
using Template.DataAccess;

namespace Template.API.Infrastructure;

/// <summary>
///     Builder
/// </summary>
public static class Builder
{
    /// <summary>
    /// </summary>
    public static IConfigurationReader? ConfigurationReader { get; set; }

    /// <summary>
    ///     Get Builder
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static WebApplicationBuilder GetBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(
            containerBuilder =>
                containerBuilder.ConfigureDependencyInjection(ConfigurationReader!));
        builder.Services.AddHttpContextAccessor(); // Add HttpContextAccessor
        builder.Services.AddScoped<AuditInterceptor>(); // Register the interceptor

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                corsPolicyBuilder => corsPolicyBuilder.WithOrigins(
                        "http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("Authorization")
                    .AllowCredentials());
        });

        // Add JWT Bearer for the default scheme
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = ConfigurationReader!.GetJwtSettingsIssuer(),
                    ValidAudience = ConfigurationReader.GetJwtSettingsAudience(),
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationReader.GetJwtSettingsKey()))
                };
            });

        //Controller Services
        builder.Services.AddControllers
            (options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
                options.MaxModelValidationErrors = 20;
                // Note: this particular order executes logger before handler
                options.Filters.Add<GlobalExceptionHandler>();
                options.Filters.Add<GlobalExceptionLogger>();
                var readerFactory = builder.Services.BuildServiceProvider()
                    .GetRequiredService<IHttpRequestStreamReaderFactory>();
                options.ModelBinderProviders.Insert(0,
                    new HybridModelBinderProvider(options.InputFormatters, readerFactory));
            })
            .ConfigureApiBehaviorOptions(options =>
                options.InvalidModelStateResponseFactory = HttpErrorHandlingDefaults
                    .CreateInvalidModelStateResponse
            )
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true)
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        // Configure Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tempalte API", Version = "v1" });

            // Define the security scheme for JWT Bearer token
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            // Add a global security requirement
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        return builder;
    }

    /// <summary>
    ///     Configure Dependency Injection
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configReader"></param>
    public static void ConfigureDependencyInjection(
        this ContainerBuilder builder,
        IConfigurationReader configReader)
    {
        builder.RegisterInstance(configReader)
            .As<IConfigurationReader>()
            .SingleInstance();
        // Register business layer.
        builder.RegisterModule<DependencyInjectionModule>();
    }
}