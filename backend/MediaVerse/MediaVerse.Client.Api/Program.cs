using MediaVerse.Client.Application.Extensions.MediatR;
using MediaVerse.Client.Application.Queries.Test;
using MediaVerse.Domain.Interfaces;
using MediaVerse.Infrastructure.Database;
using MediaVerse.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

const string defaultpolicy = "default";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<Context>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("testdb_conn_str")));
builder.Services.RegisterMediatR(typeof(TestQuery).Assembly);
builder.Services.AddAutoMapper(typeof(TestQuery).Assembly);
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: defaultpolicy,policy =>
    {
        policy.WithOrigins(builder.Configuration["CORS:http"], builder.Configuration["CORS:https"])
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

app.Run();