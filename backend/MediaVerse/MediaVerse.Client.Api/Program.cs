using System.Text;
using System.Text.RegularExpressions;
using MediaVerse.Client.Application.Extensions.MediatR;
using MediaVerse.Client.Application.Queries.Test;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Infrastructure.Database;
using MediaVerse.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

const string defaultpolicy = "default";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});
builder.Services.AddDbContext<Context>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("testdb_conn_str")));
builder.Services.RegisterMediatR(typeof(TestQuery).Assembly);
builder.Services.AddAutoMapper(typeof(TestQuery).Assembly);
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

builder.Services.AddCors(o =>
{
    o.AddPolicy(name: defaultpolicy,policy =>
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

app.Run();


public partial class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object value)
    {
        return value == null ? null : MyRegex().Replace(value.ToString(), "$1-$2").ToLower();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex MyRegex();
}