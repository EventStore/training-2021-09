using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Projects.App;

Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build().Run();