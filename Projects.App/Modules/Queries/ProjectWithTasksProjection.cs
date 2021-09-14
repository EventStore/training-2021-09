using System.Threading.Tasks;
using Eventuous.Projections.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Projects.Domain.Projects;
using Projects.Domain.Tasks;

namespace Projects.App.Modules.Queries {
    public class ProjectWithTasksProjection : MongoProjection<ProjectWithTasks> {
        public ProjectWithTasksProjection(
            IMongoDatabase  database,
            ILoggerFactory? loggerFactory
        ) : base(database, QuerySubscription.Id, loggerFactory) { }

        protected override ValueTask<Operation<ProjectWithTasks>> GetUpdate(object evt, long? position) {
            return evt switch {
                ProjectEvents.ProjectRegistered e => UpdateOperationTask(
                    e.Id,
                    update => update.SetOnInsert(x => x.ProjectName, e.Name)
                ),
                ProjectEvents.ProjectCreated e => UpdateOperationTask(
                    e.Id,
                    update => update.SetOnInsert(x => x.ProjectName, e.Description)
                ),
                TaskEvents.V1.TaskCreated e => UpdateOperationTask(
                    string.IsNullOrWhiteSpace(e.ProjectId) ? "unknown" : e.ProjectId,
                    update => update.AddToSet(
                        x => x.Tasks,
                        new ProjectTaskRecord(e.TaskId, e.Description)
                    )
                ),
                _ => NoOp
            };
        }
    }
}