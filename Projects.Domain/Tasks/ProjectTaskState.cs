using Eventuous;

namespace Projects.Domain.Tasks {
    public record ProjectTaskId(string Value) : AggregateId(Value);
    
    public record ProjectTaskState : AggregateState<ProjectTaskState, ProjectTaskId> {
        
    }
}