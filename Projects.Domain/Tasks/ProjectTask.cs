using System;
using Eventuous;
using Projects.Domain.Projects;
using Projects.Domain.Users;
using static Projects.Domain.Tasks.TaskEvents;

namespace Projects.Domain.Tasks {
    public class ProjectTask : Aggregate<ProjectTaskState, ProjectTaskId> {
        public void CreateTask(
            TaskName       name,
            Project        project,
            string         description,
            TimeSpan       duration,
            int            priority,
            Rate           chargeRate,
            DateTimeOffset createdAt,
            string         createdBy
        ) {
            if (!project.CanAcceptTask())
                throw new DomainException("Project doesn't accept tasks!");
            
            Apply(
                new V1.TaskCreated(
                    GetId(),
                    project.GetId(),
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

        public void UpdateDescription(string description, string by) {
            if (description == State.Description) return;

            Apply(new V1.TaskDescriptionUpdated(State.Id, description, by));
        }
    }
}