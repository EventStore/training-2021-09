using Eventuous;
using static Projects.Domain.Tasks.TaskEvents;

namespace Projects.Domain.Tasks {
    public record ProjectTaskId(string Value) : AggregateId(Value);

    public record ProjectTaskState : AggregateState<ProjectTaskState, ProjectTaskId> {
        public ProjectTaskState() {
            On<V1.TaskCreated>(Handle);
        }

        ProjectTaskState Handle(V1.TaskCreated evt)
            => this with { Id = new ProjectTaskId(evt.TaskId) };
    }
}