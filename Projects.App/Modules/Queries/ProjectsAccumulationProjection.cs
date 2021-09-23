using System.Collections.Generic;
using System.Threading.Tasks;
using Eventuous.Projections.MongoDB;
using Eventuous.Projections.MongoDB.Tools;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Projects.Domain.Tasks;
using static MongoDB.Driver.Builders<Projects.App.Modules.Queries.ProjectAccumulatedNumbers>;

namespace Projects.App.Modules.Queries {
    public record ProjectAccumulatedNumbers : ProjectedDocument {
        public ProjectAccumulatedNumbers(string id) : base(id) { }

        public int TasksCount { get; init; }
    }

    public class ProjectAccumulationProjection : MongoProjection<ProjectAccumulatedNumbers> {
        public ProjectAccumulationProjection(
            IMongoDatabase  database,
            ILoggerFactory? loggerFactory
        ) : base(database, QuerySubscription.Id, loggerFactory) { }

        protected override ValueTask<Operation<ProjectAccumulatedNumbers>> GetUpdate(object evt, long? position) {
            return evt switch {
                TaskEvents.V1.TaskCreated e => UpdateOperationTask(
                    filter => filter.And(
                        Filter.Eq(x => x.Id, e.ProjectId),
                        Filter.Gt(x => x.Position, position)
                    ),
                    update => update.Inc(x => x.TasksCount, 1)
                ),
                _ => NoOp
            };
        }
    }
}