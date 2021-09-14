using System;
using Eventuous;

namespace Projects.Domain.Tasks {
    public static class TaskEvents {
        public static class V1 {
            [EventType("V1.TaskCreated")]
            public record TaskCreated(
                string         TaskId,
                string         ProjectId,
                string         Name,
                string         Description,
                TimeSpan       Duration,
                int            Priority,
                float          ChargeRate,
                DateTimeOffset CreatedAt,
                string         CreatedBy
            );

            [EventType("V1.StaffAssignedToTask")]
            public record StaffAssignedToTask(string TaskId, string UserId);
            
            [EventType("V1.TaskDescriptionUpdated")]
            public record TaskDescriptionUpdated(string TaskId, string Description, string UpdatedBy);
        }

        public static class V2 {
            [EventType("V2.TaskDescriptionUpdated")]
            public record TaskDescriptionChanged(string TaskId, string NewDescription, string UpdatedBy);
        }
    }
}