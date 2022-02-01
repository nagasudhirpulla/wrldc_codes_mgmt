﻿using Application;
using Application.Common.Interfaces;
using Application.Users.Commands.SeedUsers;
using Infra;
using MediatR;
using Microsoft.AspNetCore.Mvc.Authorization;
using WebApp.Services;

namespace WebApp;
public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddApplication();
        services.AddInfrastructure(Configuration, Environment);
        services.AddRazorPages()
            .AddMvcOptions(o => o.Filters.Add(new AuthorizeFilter()))
            .AddRazorRuntimeCompilation();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMediator mediator)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // seed Data
        SeedData(mediator).Wait();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
    }
    public async Task SeedData(IMediator mediator)
    {
        _ = await mediator.Send(new SeedUsersCommand());
    }
}
