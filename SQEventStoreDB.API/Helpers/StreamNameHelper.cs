using SQEventStore.Contracts.Contracts.Domain;

namespace SQEventStoreDB.API.Helpers
{
    public class StreamNameHelper
    {
        public static string GetStreamName<T>() where T : IAggregate
        {
            return typeof(T).Name;
        }
    }
}
