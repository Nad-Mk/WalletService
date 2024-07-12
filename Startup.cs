using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using WalletService.Repositories;
using WalletService.Repositories.Interfaces;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<WalletContext>(opt => opt.UseInMemoryDatabase("WalletDB"));
        services.AddScoped<IWalletRepo, WalletRepo>();
        services.AddControllers();

        // register versioning service
        services.AddApiVersioning(o =>
        {
            o.ReportApiVersions = true;                    // add the API versions 
            o.AssumeDefaultVersionWhenUnspecified = true;  // assume default if not specified 
            o.DefaultApiVersion = new ApiVersion(1, 0);    // default version 
        });

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                    .AddConfiguration(Configuration).Build();
         
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage(); 
        }

        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
