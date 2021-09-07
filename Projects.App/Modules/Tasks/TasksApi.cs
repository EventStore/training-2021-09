using System.Threading;
using System.Threading.Tasks;
using Eventuous;
using Microsoft.AspNetCore.Mvc;
using Projects.Domain.Tasks;
using static Projects.App.Modules.Tasks.TaskCommands;

namespace Projects.App.Modules.Tasks {
    [Route("/tasks")]
    public class TasksApi : ControllerBase {
        readonly TasksCommandService _service;

        public TasksApi(TasksCommandService service) => _service = service;

        [HttpPost]
        public Task<Result<ProjectTask, ProjectTaskState, ProjectTaskId>> Post([FromBody] V1.CreateTask cmd, CancellationToken cancellationToken)
            => _service.Handle(cmd, cancellationToken);

        [HttpPost("assign")]
        public Task<Result<ProjectTask, ProjectTaskState, ProjectTaskId>> AssignStaff(
            [FromBody] V1.AssignStaff cmd,
            CancellationToken         cancellationToken
        )
            => _service.Handle(cmd, cancellationToken);
    }
}