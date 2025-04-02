using SQEventStoreDB.Domain.Entities;

namespace SQEventStoreDB.API.Helpers
{
    public class StreamNameHelper
    {
        public static string GetStreamName<T>() where T : IEntity
        {
            return typeof(T).Name;
        }
    }
}
