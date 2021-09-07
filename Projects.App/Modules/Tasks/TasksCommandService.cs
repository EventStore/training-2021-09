using System;
using Eventuous;
using Projects.Domain.Tasks;
using Projects.Domain.Users;
using static Projects.App.Modules.Tasks.TaskCommands;

namespace Projects.App.Modules.Tasks {
    public class TasksCommandService : ApplicationService<ProjectTask, ProjectTaskState, ProjectTaskId> {
        public TasksCommandService(IsUserValid isUserValid, IAggregateStore store) : base(store) {
            OnNewAsync<V1.CreateTask>(
                cmd => new ProjectTaskId(cmd.TaskId),
                async (task, cmd, _) => {
                    task.CreateTask(
                        new TaskName(cmd.Name),
                        cmd.Description,
                        cmd.Duration,
                        cmd.Priority,
                        new Rate(cmd.ChargeRate),
                        cmd.CreatedAt,
                        cmd.CreatedBy
                    );

                    if (cmd.AssignedStuff != null) {
                        foreach (var user in cmd.AssignedStuff) {
                            task.AssignStaff(await UserId.FromString(user, isUserValid));
                        }
                    }
                }
            );

            OnExistingAsync<V1.AssignStaff>(
                cmd => new ProjectTaskId(cmd.TaskId),
                async (task, cmd, _) => { task.AssignStaff(await UserId.FromString(cmd.UserId, isUserValid)); }
            );
        }
    }
}