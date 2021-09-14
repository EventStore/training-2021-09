using Eventuous.Projections.MongoDB.Tools;

namespace Projects.App.Modules.Queries {
    public record TaskDocument(string Id) : ProjectedDocument(Id) {
        public string Description { get; init; }
        public int    Time        { get; init; }
    }
}