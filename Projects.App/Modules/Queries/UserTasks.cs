using System.Collections.Generic;
using System.Threading.Tasks;
using Eventuous.Projections.MongoDB;
using Eventuous.Projections.MongoDB.Tools;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using static Projects.Domain.Tasks.TaskEvents;

namespace Projects.App.Modules.Queries {
    public class UserTasksProjection : MongoProjection<UserTasksDocument> {
        readonly IMongoCollection<TaskDocument> _tasksCollection;

        public UserTasksProjection(IMongoDatabase database, ILoggerFactory? loggerFactory) : base(
            database,
            UserQuerySubscription.Id,
            loggerFactory
        ) {
            _tasksCollection = database.GetDocumentCollection<TaskDocument>();
        }

        protected override ValueTask<Operation<UserTasksDocument>> GetUpdate(object evt, long? position) {
            return evt switch {
                V1.StaffAssignedToTask e => Handle(e),
                V1.TaskDescriptionUpdated e => UpdateOperationTask(
                    filter => filter.ElemMatch(x => x.Tasks, x => x.TaskId == e.TaskId),
                    update => update.Set(x => x.Tasks[-1].Description, e.Description)
                ),
                _ => NoOp
            };

            async ValueTask<Operation<UserTasksDocument>> Handle(V1.StaffAssignedToTask e) {
                TaskDocument task = (await _tasksCollection.LoadDocument(e.TaskId))!;

                return UpdateOperation(
                    e.UserId,
                    update => update
                        .SetOnInsert(x => x.UserId, e.UserId)
                        .AddToSet(x => x.Tasks, new UserTasksDocument.UserTask(e.TaskId, task.Description))
                );
            }
        }
    }

    public record UserTasksDocument : ProjectedDocument {
        public UserTasksDocument(string id) : base(id) { }

        public string         UserId { get; set; }
        public List<UserTask> Tasks  { get; set; } = new();

        public record UserTask(string TaskId, string Description);
    }
}