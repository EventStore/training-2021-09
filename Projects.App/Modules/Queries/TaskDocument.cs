using Eventuous.Projections.MongoDB.Tools;

namespace Projects.App.Modules.Queries {
    public record TaskDocument(string Id) : ProjectedDocument(Id) {
    }
}