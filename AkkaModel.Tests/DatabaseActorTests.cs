using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Moq;
using Xunit;

namespace AkkaModel.Tests
{
    public class DatabaseActorTests : TestKit
    {
        [Fact]
        public void ShouldReadStatsFromDatabase()
        {
            var statsData = new Dictionary<string, int>
            {
                {"Boolean Lies", 42},
                {"Conan the Barbarian", 200}
            };

            var mockDb = new Mock<IDatabaseGateway>();
            mockDb.Setup(x => x.GetStoredStatistics()).Returns(statsData);

            IActorRef actor = ActorOf(Props.Create(() => new DatabaseActor(mockDb.Object)));
            
            actor.Tell((new GetInitialStatisticsMessage()));

            var received = ExpectMsg<InitialStatisticsMessage>();
            
            Assert.Equal(42, received.PlayCounts["Boolean Lies"]);
            Assert.Equal(200, received.PlayCounts["Conan the Barbarian"]);
        }
    }
}