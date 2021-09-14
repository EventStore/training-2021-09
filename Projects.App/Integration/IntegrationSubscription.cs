using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Eventuous;
using Eventuous.Producers.EventStoreDB;
using Eventuous.Subscriptions;
using Eventuous.Subscriptions.EventStoreDB;
using Microsoft.Extensions.Logging;
using Projects.Domain.Projects;
using static Projects.App.Integration.ProjectIntegrationEvents;

namespace Projects.App.Integration {
    public class IntegrationSubscription : AllStreamSubscription {
        public IntegrationSubscription(
            EventStoreClient           eventStoreClient,
            ICheckpointStore           checkpointStore,
            IEnumerable<IEventHandler> eventHandlers,
            ILoggerFactory?            loggerFactory
        ) : base(
            eventStoreClient,
            Id,
            checkpointStore,
            eventHandlers,
            loggerFactory: loggerFactory
        ) { }
        
        public const string Id = "IntegrationSub";
    }
    
    public class IntegrationPublisher : IEventHandler {
        readonly EventStoreProducer _producer;

        public IntegrationPublisher(EventStoreProducer producer)
            => _producer = producer;

        public async Task HandleEvent(object evt, long? position, CancellationToken cancellationToken) {
            object? intEvent = evt switch {
                ProjectEvents.ProjectRegistered e => new V1.ProjectRegistered(e.Id, e.Name),
                ProjectEvents.ProjectCreated e    => new V1.ProjectRegistered(e.Id, e.Description),
                _ => null
            };

            if (intEvent != null)
                await _producer.Produce("ProjectIntegration", intEvent, cancellationToken);
        }

        public string SubscriptionId => IntegrationSubscription.Id;
    }

    public static class ProjectIntegrationEvents {
        public static class V1 {
            [EventType("Ext.V1.ProjectRegistered")]
            public record ProjectRegistered(string Id, string Name);
        }
        
        public static class V2 {
            [EventType("Ext.V2.ProjectRegistered")]
            public record ProjectRegistered(string Id, string Name, int Estimate);
        }
    }
}