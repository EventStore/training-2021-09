using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Projects.App;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(
        webBuilder
            => webBuilder
                .UseStartup<Startup>()
                .UseSerilog()
    )
    .Build().Run();