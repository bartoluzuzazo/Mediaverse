using System.Text;
using MediaVerse.Client.Application.Extensions.MediatR;
using MediaVerse.Client.Application.Queries.Test;
using MediaVerse.Client.Application.Services.Transformer;
using MediaVerse.Client.Application.Services.UserAccessor;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Host.Extensions;
using MediaVerse.Infrastructure.Database;
using MediaVerse.Infrastructure.Repositories;
using MediaVerse.Infrastructure.UserAccessor;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

const string defaultpolicy = "default";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MediaVerse_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
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
            new string[] { }
        }
    });
});
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});
builder.Services.AddDbContext<Context>(options =>
    options.UseNpgsql(builder.Configuration["ConnectionStrings.DefaultConnection"]));
builder.Services.RegisterMediatR(typeof(TestQuery).Assembly);
builder.Services.AddAutoMapper(typeof(TestQuery).Assembly);
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserAccessor, HttpUserAccessor>();

builder.AddLogging();


builder.Services.AddCors(o =>
{
    o.AddPolicy(name: defaultpolicy, policy =>
    {
        policy.WithOrigins(builder.Configuration["CORS:http"], builder.Configuration["CORS:https"])
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication().AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
    };
});

builder.Services.AddAuthorization(o => o.AddPolicy("Admin", p => p.RequireRole("Administrator")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(defaultpolicy);
app.MapControllers();
app.UsePathBase("/api");
app.UseAuthentication();
app.UseAuthorization();
app.ExecuteDbMigrations();
app.Run();