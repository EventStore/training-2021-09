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
                _                        => this
            };
        }

        ProjectTaskState Handle(V1.StaffAssignedToTask evt)
            => this with { AssignedStaff = AssignedStaff.Add(new UserId(evt.UserId)) };

        ProjectTaskState Handle(V1.TaskCreated evt)
            => this with {
                Id = new ProjectTaskId(evt.TaskId),
                // AssignedStaff = AssignSeveralPeople(evt.AssignedStuff)
            };
        
        ImmutableList<UserId> AssignedStaff { get; init; } = ImmutableList<UserId>.Empty;

        internal bool StaffAlreadyAssigned(UserId user) => AssignedStaff.Contains(user);

        // ImmutableList<UserId> AssignSeveralPeople(string[]? staff) {
        //     return staff == null ? AssignedStaff : AssignedStaff.AddRange(staff.Select(x => new UserId(x)));
    }
}