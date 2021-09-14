using System;
using System.Threading.Tasks;
using Eventuous;
using Eventuous.EventStoreDB;
using Eventuous.Projections.MongoDB;
using Eventuous.Subscriptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Projects.App.Modules.Projects;
using Projects.App.Modules.Queries;
using Projects.App.Modules.Tasks;
using Projects.Domain.Tasks;
using Projects.Domain.Users;
using MongoDefaults = Eventuous.Projections.MongoDB.Tools.MongoDefaults;

namespace Projects.App {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            TypeMap.RegisterKnownEventTypes(typeof(TaskEvents.V1.TaskCreated).Assembly);

            var settings = Settings.Load(Configuration);
            
            services.AddEventStoreClient(settings.EventStore.ConnectionString);
            services.AddSingleton(ConfigureMongo(settings.Mongo));
            services.AddSingleton<IEventStore, EsdbEventStore>();
            services.AddSingleton<IAggregateStore, AggregateStore>();
            services.AddSingleton<ICheckpointStore, MongoCheckpointStore>();

            services.AddSingleton<TasksCommandService>();
            services.AddSingleton<ProjectService>();

            services.AddSubscription<QuerySubscription>()
                .AddEventHandler<TasksProjection>()
                .AddEventHandler<ProjectWithTasksProjection>();
            
            services.AddSubscription<UserQuerySubscription>()
                .AddEventHandler<UserTasksProjection>();
            
            services.AddControllers();

            Task<bool> ValidateUser(UserId userId) => Task.FromResult(userId.UserIdString.Contains("someone"));
            services.AddSingleton<IsUserValid>(ValidateUser);

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
        
        public static IMongoDatabase ConfigureMongo(Mongo mongoSettings) {
            MongoDefaults.RegisterConventions();

            var settings = MongoClientSettings.FromConnectionString(mongoSettings.ConnectionString);
            settings.ConnectTimeout = TimeSpan.FromSeconds(10);
            return new MongoClient(settings).GetDatabase(mongoSettings.Database);
        }
    }
}