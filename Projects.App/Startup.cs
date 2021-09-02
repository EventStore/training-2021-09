using EventStore.Client;
using Eventuous;
using Eventuous.EventStoreDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Projects.App.Modules.Tasks;
using Projects.Domain.Tasks;

namespace Projects.App {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            TypeMap.RegisterKnownEventTypes(typeof(TaskEvents.V1.TaskCreated).Assembly);
            services.AddSingleton(ConfigureEventStore("esdb://localhost:2113?tls=false"));
            services.AddSingleton<IEventStore, EsdbEventStore>();
            services.AddSingleton<IAggregateStore, AggregateStore>();

            services.AddSingleton<TasksCommandService>();
            
            services.AddControllers();

            services.AddSwaggerGen(
                c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Projects.App", Version = "v1" })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Projects.App v1"));
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
        
        public static EventStoreClient ConfigureEventStore(string connectionString) {
            var settings = EventStoreClientSettings.Create(connectionString);
            return new EventStoreClient(settings);
        }
    }
}