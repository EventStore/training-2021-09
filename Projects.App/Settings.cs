using Microsoft.Extensions.Configuration;

namespace Projects.App {
    public class Settings {
        public EventStore EventStore { get; set; }
        public Mongo      Mongo      { get; set; }
        
        public static Settings Load(IConfiguration configuration) {
            var settings = new Settings();
            configuration.Bind(settings);
            return settings;
        }
    }
    
    public record EventStore {
        public string ConnectionString { get; init; } = null!;
    }

    public record Mongo {
        public string ConnectionString { get; init; } = null!;
        public string Database         { get; init; } = null!;
    }
}