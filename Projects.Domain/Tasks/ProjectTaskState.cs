using System.Collections.Immutable;
using System.Linq;
using Eventuous;
using Projects.Domain.Users;
using static Projects.Domain.Tasks.TaskEvents;

namespace Projects.Domain.Tasks {
    public record ProjectTaskId(string Value) : AggregateId(Value);

    public record ProjectTaskState : AggregateState<ProjectTaskState, ProjectTaskId> {
        public override ProjectTaskState When(object @event) {
            return @event switch {
                V1.TaskCreated e         => Handle(e),
                V1.StaffAssignedToTask e => Handle(e),
                V1.TaskDescriptionUpdated e => this,
                V2.TaskDescriptionChanged e => this,
            _                        => this
            };
        }

        ProjectTaskState Handle(V1.StaffAssignedToTask evt)
            => this with { AssignedStaff = AssignedStaff.Add(new UserId(evt.UserId)) };

        ProjectTaskState Handle(V1.TaskCreated evt)
            => this with {
                Id = new ProjectTaskId(evt.TaskId),
                Description = evt.Description
                // AssignedStaff = AssignSeveralPeople(evt.AssignedStuff)
            };

        ProjectTaskState Handle(V1.TaskDescriptionUpdated evt)
            => this with { Description = evt.Description };

        internal ImmutableList<UserId> AssignedStaff { get; init; } = ImmutableList<UserId>.Empty;
        internal string Description { get; set; }

        internal bool StaffAlreadyAssigned(UserId user) => AssignedStaff.Contains(user);

        // ImmutableList<UserId> AssignSeveralPeople(string[]? staff) {
        //     return staff == null ? AssignedStaff : AssignedStaff.AddRange(staff.Select(x => new UserId(x)));
    }
}