using System.Collections.Generic;
using EventStore.Client;
using Eventuous.Subscriptions;
using Eventuous.Subscriptions.EventStoreDB;
using Microsoft.Extensions.Logging;

namespace Projects.App.Modules.Queries {
    public class QuerySubscription : AllStreamSubscription {
        public const string Id = "Queries";
        
        public QuerySubscription(
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
    }
}