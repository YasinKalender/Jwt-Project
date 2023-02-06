using FluentValidation.AspNetCore;
using JwtTokenProject.Common.DTOs.Model;
using JwtTokenProject.Common.Helpers.SecurityKeyHelpers;
using JwtTokenProject.Common.JWT;
using JwtTokenProject.DAL.Context;
using JwtTokenProject.DAL.Repositories;
using JwtTokenProject.DAL.Uow;
using JwtTokenProject.Entities;
using JwtTokenProject.Service.Interfaces;
using JwtTokenProject.Service.Services;
using JwtTokenProject.Service.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JwtTokenProject.Common.Extensions;
using System.Reflection;
using JwtTokenProject.Api.ClaimsRequired;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(opt => { opt.RegisterValidatorsFromAssemblyContaining<RegisterUserDtoValidator>(); opt.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()); });
builder.Services.AddCustomValidationResponse();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAutService, AutService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IService<,>), typeof(Service<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthorizationHandler, BirthDayClaimsHandler>();



builder.Services.AddDbContext<ProjectContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<UserApp, IdentityRole>(opt =>
{

    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireNonAlphanumeric = false;

}).AddEntityFrameworkStores<ProjectContext>().AddDefaultTokenProviders();

builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Client"));

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    //Yukarýda ki deðeri aþaðýda ki deðeri tanýmladýk
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();

    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudiences = tokenOptions.Audience,
        IssuerSigningKey = SecurityKeyHelper.GetSecurityKey(tokenOptions.SecurityKey),

        //doðrulama iþlemþ
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

});

builder.Services.AddAuthorization(conf =>
{
    conf.AddPolicy("KalenderPolicy", policy =>
    {
        policy.RequireClaim("LastName", "Kalender");

    });

    conf.AddPolicy("AgePolicy", policy =>
    {
        policy.Requirements.Add(new BirthDayClaims(18));

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
