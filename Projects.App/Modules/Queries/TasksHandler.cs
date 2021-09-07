using System.Threading.Tasks;
using Eventuous.Projections.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Projects.App.Modules.Queries {
    public class TasksHandler : MongoProjection<TaskDocument> {
        public TasksHandler(IMongoDatabase database, ILoggerFactory loggerFactory)
            : base(database, QuerySubscription.Id, loggerFactory) { }

        protected override ValueTask<Operation<TaskDocument>> GetUpdate(object evt, long? position) {
            return NoOp;
        }
    }
}