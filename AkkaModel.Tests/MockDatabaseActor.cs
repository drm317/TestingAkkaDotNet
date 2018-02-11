using System.Collections.Generic;
using System.Collections.ObjectModel;
using Akka.Actor;

namespace AkkaModel.Tests
{
    public class MockDatabaseActor : ReceiveActor
    {
        public MockDatabaseActor()
        {
            Receive<GetInitialStatisticsMessage>(message =>
            {
                var stats = new Dictionary<string, int>
                {
                    {"Conan the Barbarian", 12}
                };
                
                Sender.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(stats)));
            });
        }
    }
}