using System;
using Eventuous;

namespace Projects.Domain.Tasks {
    public static class TaskEvents {
        public static class V1 {
            [EventType("V1.TaskCreated")]
            public record TaskCreated(
                string         TaskId,
                string         Name,
                string         Description,
                TimeSpan       Duration,
                int            Priority,
                float          ChargeRate,
                string[]?      AssignedStuff,
                DateTimeOffset CreatedAt,
                string         CreatedBy
            );
        }
    }
}