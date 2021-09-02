using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Projects.App.Modules.Tasks.TaskCommands;

namespace Projects.App.Modules.Tasks {
    [Route("/tasks")]
    public class TasksApi : ControllerBase {
        readonly TasksCommandService _service;
        
        public TasksApi(TasksCommandService service) => _service = service;

        [HttpPost]
        public Task Post([FromBody] V1.CreateTask cmd, CancellationToken cancellationToken)
            => _service.Handle(cmd, cancellationToken);
    }
}