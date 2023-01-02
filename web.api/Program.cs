using FluentValidation;
using FluentValidation.AspNetCore;
using infrustruture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Middleware.Example;
using System.Reflection;
using web.api.Appsettings;
using web.api.Utility;
using web.api.Utility.Filter;
using web.api.Utility.HostService;
using web.api.Utility.Logging;
using web.api.Utility.Logging2;
using web.api.Utility.Logging3;
using web.api.Utility.signalR;

var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddJsonFile();


using ILoggerFactory loggerFactory =
    LoggerFactory.Create(builder =>
        builder.AddSimpleConsole(options =>
        {
            options.IncludeScopes = true;
            options.SingleLine = true;
            options.TimestampFormat = "HH:mm:ss ";
        }));

ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
using (logger.BeginScope("[scope is enabled]"))
{
    logger.LogInformation("Hello World!");
    logger.LogInformation("Logs contain timestamp and log level.");
    logger.LogInformation("Each log message is fit in a single line.");
}

//builder.Logging.ClearProviders();
//var config = new ColoredConsoleLoggerConfiguration
//{
//    LogLevel = LogLevel.Information,
//    Color = ConsoleColor.Red
//};

//builder.Logging.AddProvider(new ColoredConsoleLoggerProvider(config));

//builder.Host.ConfigureLogging(logging =>
//{
//    logging.ClearProviders();
//    logging.AddConsole();
//});

//var logger = LoggerFactory.Create(config =>
//{
//    config.AddConsole();
//}).CreateLogger("Program");

//using var loggerFactory = LoggerFactory.Create(builder =>
//{
//    builder.ClearProviders();
//    builder.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Disabled);
//});

//using var loggerFactory = LoggerFactory.Create(builder =>
//{
//    builder.AddProvider(new ColoredConsoleLoggerProvider(config));
//});

builder.Logging.ClearProviders();
builder.Logging.AddMongoDbLoggerr();

//var logger = loggerFactory.CreateLogger<Program>();

//logger.LogInformation("--------->>>>>>>>>> Program starting <<<<<<<<<<---------");

builder.Services.AddScoped<IShowUserInfo, ShowUserInfo>();

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = ApiVersion.Default;
    //options.ReportApiVersions = true;
    //options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    typeof(ApiVersions).GetEnumNames().ToList().ForEach(verison =>
    {
        c.SwaggerDoc(verison, new OpenApiInfo()
        {
            Title = $"My API {verison}",
            Version = verison,
            Description = "Testing Swagger version"
        });
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.OrderActionsBy(o => o.RelativePath);

    // This call remove version from parameter, without it we will have version as parameter 
    // for all endpoints in swagger UI
    c.OperationFilter<RemoveVersionParameterFilter>();
    // This make replacement of v{version:apiVersion} to real version of corresponding swagger doc.
    c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Please enter token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    //c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    //    {
    //        new OpenApiSecurityScheme {
    //            Reference = new OpenApiReference() {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        new string[]{}
    //    }
    //});

    c.OperationFilter<FileUploadFilter>();

});

//builder.Services.AddMvc(opts =>
//{
//    opts.Filters.Add<MyExceptonFilter2>();
//    opts.Filters.Add<MyExceptonFilter>();
//});
//builder.Services.Configure<MvcOptions>(options =>
//{
//    options.Filters.Add<MyExceptonFilter2>();
//    options.Filters.Add<MyExceptonFilter>();
//    options.Filters.Add<MyActionFilter>();
//    options.Filters.Add<MyActionFilter2>();
//});
builder.Services.AddControllers(options =>
{
    options.Filters.Add<MyExceptonFilter2>();
    options.Filters.Add<MyExceptonFilter>();
    options.Filters.Add<MyActionFilter>();
    options.Filters.Add<MyActionFilter2>();
});

builder.Services.AddSingleton<IMongoDbLoggingService, MongoDbLoggingService>();

string[] strings = { "a", "b" };
strings.Where(s => !string.IsNullOrEmpty(s)).ToList();
builder.Services.Configure<DbOptions>(
    builder.Configuration.GetSection(nameof(DbOptions))
);

builder.Services.AddHealthChecks();
builder.Services.AddMemoryCache();


builder.Services.AddHostedService<HostService1>();
builder.Services.AddHostedService<HostService2>();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddSignalR();

string[] urls = new[] { "http://localhost:3000"};
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => {
        builder.WithOrigins(urls)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});






var app = builder.Build();

app.Logger.LogInformation("********** APP STARTING **********");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsTest())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var version in typeof(ApiVersions).GetEnumNames())
        {
            options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"API {version}");
        }
    });
}

app.UseCors();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.UseResponseCaching();
app.UseHealthChecks("/health");

app.MapHub<MyHub>("/MyHub");

app.MapControllers();

app.Run();
