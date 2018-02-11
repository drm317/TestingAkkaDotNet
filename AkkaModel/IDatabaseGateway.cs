using System.Collections.Generic;

namespace AkkaModel
{
    public interface IDatabaseGateway
    {
        IDictionary<string, int> GetStoredStatistics();
    }
}