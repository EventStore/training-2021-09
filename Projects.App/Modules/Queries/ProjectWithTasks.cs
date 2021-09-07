using System.Collections.Generic;
using Eventuous.Projections.MongoDB.Tools;

namespace Projects.App.Modules.Queries {
    public record ProjectWithTasks : ProjectedDocument {
        public ProjectWithTasks(string id) : base(id) { }
        
        public string                  ProjectName { get; set; }
        public List<ProjectTaskRecord> Tasks       { get; set; } = new();
    }

    public record ProjectTaskRecord(string Id, string Description);
}