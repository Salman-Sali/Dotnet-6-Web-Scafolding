using FluentValidation;
using Infrastructure.Context;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Configrations;
using System.Reflection;
using WebApp.Extensions;
using WebApp.Extentions;
using WebApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



#region Services
var services = builder.Services;

services.AddMediatR(typeof(GetAllEmployeeTypesQuery).Assembly);
AssemblyScanner.FindValidatorsInAssembly(typeof(GetAllEmployeeTypesQuery).Assembly)
.ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));
services.AddMediatR(typeof(AddEmployeeTypeCommand).Assembly);
AssemblyScanner.FindValidatorsInAssembly(typeof(AddEmployeeTypeCommand).Assembly)
.ForEach(item => services.AddScoped(item.InterfaceType, item.ValidatorType));

services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MediatRValidationPipelineBehavior<,>));
services.AddMediatR(typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly);
services.AddHttpContextAccessor();

IConfiguration Configuration = builder.Configuration;

var appSettings = Configuration.Load<AppSettings>();
services.AddSingleton((provider) =>
{
    return appSettings;
});

services.AddDbContext<AppDbContext>((provider, options) =>
{
    options.UseNpgsql(appSettings.DbConnectionString);
    options.EnableSensitiveDataLogging(false);
});
services.AddScoped<IUser, User>();

var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
services.AddMvc(options =>
{
    options.Filters.Add(new AuthorizeFilter(policy));
});
#endregion



#region app
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion