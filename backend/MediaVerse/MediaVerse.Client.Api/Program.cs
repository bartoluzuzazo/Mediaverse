using MediaVerse.Client.Application.Extensions.MediatR;
using MediaVerse.Client.Application.Queries.Test;

const string defaultpolicy = "default";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.RegisterMediatR(typeof(TestQuery).Assembly);
builder.Services.AddAutoMapper(typeof(TestQuery).Assembly);
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

app.Run();