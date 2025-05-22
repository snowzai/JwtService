using JwtAuthorization.Models.Configuration;
using System.Text;
using JwtAuthorization.Services.Implement;
using JwtAuthorization.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using JwtAuthorization.Repositories.Interfaces;
using JwtAuthorization.Repositories.Implement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserTokenRepository, UserTokenRepository>();
builder.Services.AddSingleton<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddSingleton<IJwtService, JwtService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region JWT
//將"class JwtConfig"中的"Secret"賦值為"appsettings.json"中的"JwtConfig"
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

// 設定key
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);

TokenValidationParameters tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    //ValidIssuer = jwtSettings.Issuer,
    //ValidAudience = jwtSettings.Audience,
    IssuerSigningKey = new SymmetricSecurityKey(key)
};

//註冊tokenValidationParams，後續可以注入使用。
builder.Services.AddSingleton(tokenValidationParams);

// 添加認證服務
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParams;
});

// 添加授權服務
builder.Services.AddAuthorization();
#endregion

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
