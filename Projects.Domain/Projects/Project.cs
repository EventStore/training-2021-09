using Eventuous;
using static Projects.Domain.Projects.ProjectEvents;

namespace Projects.Domain.Projects {
    public class Project : Aggregate<ProjectState, ProjectId> {
        public void CreateProject(ProjectId projectId, string description, int estimate)
            => Apply(new ProjectCreated(projectId, description, estimate));
    }

    public record ProjectState : AggregateState<ProjectState, ProjectId> {
        public override ProjectState When(object @event) {
            return @event switch {
                ProjectRegistered evt => this with { Id = new ProjectId(evt.Id) },
                ProjectCreated evt    => this with { Id = new ProjectId(evt.Id) },
                _                     => this
            };
        }
    }

    public record ProjectId(string Id) : AggregateId(Id);

    public static class ProjectEvents {
        // Legacy event
        [EventType("ProjectRegistered")]
        public record ProjectRegistered(string Id, string Name);

        [EventType("ProjectCreated")]
        public record ProjectCreated(string Id, string Description, int EstimateInHours);
    }
}