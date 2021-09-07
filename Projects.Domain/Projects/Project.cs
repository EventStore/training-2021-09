using Eventuous;
using static Projects.Domain.Projects.ProjectEvents;

namespace Projects.Domain.Projects {
    public class Project : Aggregate<ProjectState, ProjectId> {
        public void RegisterProject(ProjectId projectId, string projectName)
            => Apply(new ProjectRegistered(projectId, projectName));
    }

    public record ProjectState : AggregateState<ProjectState, ProjectId> {
        public override ProjectState When(object @event) {
            return @event switch {
                ProjectRegistered evt => this with { Id = new ProjectId(evt.Id) },
                _                     => this
            };
        }
    }

    public record ProjectId(string Id) : AggregateId(Id);

    public static class ProjectEvents {
        [EventType("ProjectRegistered")]
        public record ProjectRegistered(string Id, string Name);
    }
}