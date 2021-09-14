using System.Collections.Generic;
using EventStore.Client;
using Eventuous.Subscriptions;
using Eventuous.Subscriptions.EventStoreDB;
using Microsoft.Extensions.Logging;

namespace Projects.App.Modules.Queries {
    public class UserQuerySubscription : AllStreamSubscription {
        public const string Id = "UserQueries";
        
        public UserQuerySubscription(
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