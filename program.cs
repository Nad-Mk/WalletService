using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using log4net;
using log4net.Config;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using log4net.Repository.Hierarchy;
using log4net.Layout;
using log4net.Appender;
using log4net.Core;

public class Program
{
    public static void Main(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .Build();

        ConfigureLog4Net(configuration);

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });


    private static void ConfigureLog4Net(IConfiguration configuration)
    {
        var logFilePath = configuration["Log4NetCore:LogFilePath"];

        var hierarchy = (Hierarchy)LogManager.GetRepository();
        var patternLayout = new PatternLayout
        {
            ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
        };
        patternLayout.ActivateOptions();

        var roller = new RollingFileAppender
        {
            AppendToFile = true,
            File = logFilePath,
            Layout = patternLayout,
            MaxSizeRollBackups = 5,
            MaximumFileSize = "1GB",
            RollingStyle = RollingFileAppender.RollingMode.Size,
            StaticLogFileName = true
        };
        roller.ActivateOptions();
        hierarchy.Root.AddAppender(roller);

        hierarchy.Root.Level = Level.Debug;
        hierarchy.Configured = true;
    }
}
