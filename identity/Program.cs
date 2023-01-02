using identity.classes;
using identity.classes.Postgresql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<PostgresqlSettings>(builder.Configuration.GetSection(nameof(PostgresqlSettings)));
builder.Services.AddTransient<IDbPostgresql, DbPostgresql>();


var jwtSettings = new JwtSettings();
builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);
//OR
//builder.Configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);

builder.Services.AddSingleton<JwtSettings>(jwtSettings);

//builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
//builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<JwtSettings>>().Value);

//var jwtOptins = builder.Services.BuildServiceProvider().GetService(typeof(JwtSettings));

//var jwtKey = builder.Configuration.GetValue<string>($"{nameof(JwtSettings)}:{nameof(JwtSettings.Key)}");

builder.Services.AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),    // 加密解密Token的密钥

            // 是否验证发布者
            ValidateIssuer = true,
            // 发布者名称
            ValidIssuer = jwtSettings.Issuer,

            // 是否验证订阅者
            // 订阅者名称
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            // 是否验证令牌有效期
            ValidateLifetime = true,
            // 每次颁发令牌，令牌有效时间
            //ClockSkew = TimeSpan.FromMinutes(120),
            AudienceValidator = (m, n, z) =>
            {
                return true;
            },
            LifetimeValidator = (notBefore, expires, SecurityKey, validationParam) =>
            {
                return true;
            }
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Headers["Authorization"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                },
            OnAuthenticationFailed = context =>
            {
                return Task.CompletedTask;
            }
        };

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
