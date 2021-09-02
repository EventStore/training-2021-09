using System;
using Eventuous;
using static Projects.Domain.Tasks.TaskEvents;

namespace Projects.Domain.Tasks {
    public class ProjectTask : Aggregate<ProjectTaskState, ProjectTaskId> {
        public void CreateTask(
            TaskName       name,
            string         description,
            TimeSpan       duration,
            int            priority,
            Rate           chargeRate,
            string[]?      assignedStuff,
            DateTimeOffset createdAt,
            string         createdBy
        ) {
            Apply(
                new V1.TaskCreated(
                    State.Id,
                    name.Value,
                    description,
                    duration,
                    priority,
                    chargeRate.Value,
                    assignedStuff,
                    createdAt,
                    createdBy
                )
            );
        }
    }
}