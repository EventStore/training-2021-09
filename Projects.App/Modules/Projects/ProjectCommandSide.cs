using System.Threading;
using System.Threading.Tasks;
using Eventuous;
using Microsoft.AspNetCore.Mvc;
using Projects.Domain.Projects;

namespace Projects.App.Modules.Projects {
    public class ProjectService : ApplicationService<Project, ProjectState, ProjectId> {
        public ProjectService(IAggregateStore store) : base(store) {
            OnNew<CreateProject>(
                cmd => new ProjectId(cmd.Id),
                (project, cmd) => project.CreateProject(new ProjectId(cmd.Id), cmd.Description, cmd.Estimate)
            );
        }
    }

    public record CreateProject(string Id, string Description, int Estimate);

    [Route("projects")]
    public class ProjectApi : ControllerBase {
        readonly ProjectService _service;

        public ProjectApi(ProjectService service) {
            _service = service;
        }

        [HttpPost]
        public Task Register([FromBody] CreateProject cmd, CancellationToken cancellationToken)
            => _service.Handle(cmd, cancellationToken);
    }
}