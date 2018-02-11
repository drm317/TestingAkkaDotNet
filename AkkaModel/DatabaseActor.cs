using System.Collections.ObjectModel;
using Akka.Actor;

namespace AkkaModel
{
    public class DatabaseActor : ReceiveActor
    {
        private readonly IDatabaseGateway _databaseGateway;

        public DatabaseActor(IDatabaseGateway databaseGateway)
        {
            _databaseGateway = databaseGateway;

            Receive<GetInitialStatisticsMessage>(
                message =>
                {
                    var storedStats = _databaseGateway.GetStoredStatistics();
                    
                    Sender.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(storedStats)));
                }
            );
        }
    }
}