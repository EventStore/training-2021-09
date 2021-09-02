using Eventuous;
using Projects.Domain.Tasks;
using static Projects.App.Modules.Tasks.TaskCommands;

namespace Projects.App.Modules.Tasks {
    public class TasksCommandService : ApplicationService<ProjectTask, ProjectTaskState, ProjectTaskId> {
        public TasksCommandService(IAggregateStore store) : base(store) {
            OnNew<V1.CreateTask>(
                cmd => new ProjectTaskId(cmd.TaskId),
                (task, cmd) => task.CreateTask(
                    new TaskName(cmd.Name),
                    cmd.Description,
                    cmd.Duration,
                    cmd.Priority,
                    new Rate(cmd.ChargeRate),
                    cmd.AssignedStuff,
                    cmd.CreatedAt,
                    cmd.CreatedBy
                )
            );
        }
    }
}