using Application;
using Application.Common.Interfaces;
using Core.ReportingData;
using Infra;
using Infra.ReportingData;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
//TODO complete this
builder.Configuration.AddUserSecrets(typeof(WebApp.Startup).GetTypeInfo().Assembly);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // add Basic Authentication
    var basicSecurityScheme = new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Reference = new OpenApiReference { Id = "BasicAuth", Type = ReferenceType.SecurityScheme }
    };
    c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {basicSecurityScheme, Array.Empty<string>()}
    });
});
builder.Services.AddTransient<IReportingDataService, ReportingDataService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();