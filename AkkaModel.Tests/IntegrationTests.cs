using System.Collections.Generic;
using System.Collections.ObjectModel;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using Akka.TestKit.Xunit2;
using Xunit;

namespace AkkaModel.Tests
{
    public class IntegrationTests : TestKit
    {
        [Fact]
        public void UserShouldUpdatePlayCounts()
        {
            TestActorRef<StatisticsActor> stats =
                ActorOfAsTestActorRef(() => new StatisticsActor(ActorOf(BlackHoleActor.Props)));
            var initialMovieStats = new Dictionary<string, int>();
            initialMovieStats.Add("Conan the Barbarian", 12);
            stats.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(initialMovieStats)));

            TestActorRef<UserActor> user = ActorOfAsTestActorRef<UserActor>(Props.Create(() => new UserActor(stats)));

            user.Tell(new PlayMovieMessage("Conan the Barbarian"));

            Assert.Equal(13, stats.UnderlyingActor.PlayCounts["Conan the Barbarian"]);
        }
    }
}