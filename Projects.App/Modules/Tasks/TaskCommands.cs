using System;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Projects.App.Modules.Tasks {
    public static class TaskCommands {
        public static class V1 {
            public record CreateTask(
                string         TaskId,
                string         ProjectId,
                string         Name,
                string         Description,
                TimeSpan       Duration,
                int            Priority,
                float          ChargeRate,
                string[]?      AssignedStuff,
                DateTimeOffset CreatedAt,
                string         CreatedBy
            );

            public record AssignStaff(string TaskId, string UserId);
        }
    }
}