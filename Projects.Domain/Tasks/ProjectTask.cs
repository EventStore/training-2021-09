using System;
using System.Threading.Tasks;
using Eventuous;
using Projects.Domain.Projects;
using Projects.Domain.Users;
using static Projects.Domain.Tasks.TaskEvents;

namespace Projects.Domain.Tasks {
    public class ProjectTask : Aggregate<ProjectTaskState, ProjectTaskId> {
        public void CreateTask(
            TaskName       name,
            ProjectId      projectId,
            string         description,
            TimeSpan       duration,
            int            priority,
            Rate           chargeRate,
            DateTimeOffset createdAt,
            string         createdBy
        ) {
            Apply(
                new V1.TaskCreated(
                    State.Id,
                    projectId,
                    name.Value,
                    description,
                    duration,
                    priority,
                    chargeRate.Value,
                    createdAt,
                    createdBy
                )
            );
        }

        public void AssignStaff(UserId userId) {
            if (State.StaffAlreadyAssigned(userId)) return;

            Apply(new V1.StaffAssignedToTask(State.Id, userId.UserIdString));
        }
    }
}