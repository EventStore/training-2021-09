using System.Threading.Tasks;
using Eventuous.Projections.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using static Projects.Domain.Tasks.TaskEvents;

namespace Projects.App.Modules.Queries {
    public class TasksHandler : MongoProjection<TaskDocument> {
        public TasksHandler(IMongoDatabase database, ILoggerFactory loggerFactory)
            : base(database, QuerySubscription.Id, loggerFactory) { }

        protected override ValueTask<Operation<TaskDocument>> GetUpdate(object evt, long? position) {
            return evt switch {
                V1.TaskCreated e => UpdateOperationTask(
                    e.TaskId,
                    update => update
                        .SetOnInsert(x => x.Id, e.TaskId)
                        .SetOnInsert(x => x.Description, e.Description)
                ),
                _ => NoOp
            };
        }
    }
}