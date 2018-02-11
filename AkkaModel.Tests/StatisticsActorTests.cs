using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using Akka.TestKit.Xunit2;
using Xunit;

namespace AkkaModel.Tests
{
    public class StatisticsActorTests : TestKit
    {
        [Fact]
        public void ShouldHaveInitialPlayCountsValue()
        {
            StatisticsActor actor = new StatisticsActor(null);
            
            Assert.Null(actor.PlayCounts);
        }

        [Fact]
        public void ShouldSetInitialPlayCounts()
        {
            StatisticsActor actor = new StatisticsActor(null);

            var initialMovieStats = new Dictionary<string, int>();
            initialMovieStats.Add("Conan the Barbarian", 12);

            actor.HandleInitialMessage(
                new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(initialMovieStats)));
            
            Assert.Equal(12, actor.PlayCounts["Conan the Barbarian"]);
        }

        [Fact]
        public void ShouldReceiveInitialStatisticsMessage()
        {
            TestActorRef<StatisticsActor> actor =
                ActorOfAsTestActorRef(() => new StatisticsActor(ActorOf(BlackHoleActor.Props)));
            
            var initialMovieStats = new Dictionary<string, int>();
            initialMovieStats.Add("Conan the Barbarian", 12);
            
            actor.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(initialMovieStats)));
            
            Assert.Equal(12, actor.UnderlyingActor.PlayCounts["Conan the Barbarian"]);
        }

        [Fact]
        public void ShouldUpdatePlayCounts()
        {
            TestActorRef<StatisticsActor> actor =
                ActorOfAsTestActorRef(() => new StatisticsActor(ActorOf(BlackHoleActor.Props)));
            
            var initialMovieStats = new Dictionary<string, int>();
            initialMovieStats.Add("Conan the Barbarian", 12);
            
            actor.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(initialMovieStats)));
            
            actor.Tell("Conan the Barbarian");
            
            Assert.Equal(13, actor.UnderlyingActor.PlayCounts["Conan the Barbarian"]);
        }

        [Fact]
        public void ShouldGetInitialStatsFromDatabase()
        {
            // TestActorRef<MockDatabaseActor> mockDb = ActorOfAsTestActorRef<MockDatabaseActor>();

            TestProbe mockDb = CreateTestProbe();

            var messageHandler = new DelegateAutoPilot((sender, message) =>
            {
                if (message is GetInitialStatisticsMessage)
                {
                    var stats = new Dictionary<string, int>
                    {
                        {"Conan the Barbarian", 42}
                    };
                
                    sender.Tell(new InitialStatisticsMessage(new ReadOnlyDictionary<string, int>(stats)));    
                }             
                
                return AutoPilot.KeepRunning;
            });
            
            mockDb.SetAutoPilot(messageHandler);
            
            
            
            TestActorRef<StatisticsActor> actor =
                ActorOfAsTestActorRef(() => new StatisticsActor(mockDb));
            
            Assert.Equal(42, actor.UnderlyingActor.PlayCounts["Conan the Barbarian"]);
        }


        [Fact]
        public void ShouldAskDatabaseForInitialStats()
        {
            TestProbe mockDb = CreateTestProbe();

            IActorRef actor = ActorOf(() => new StatisticsActor(mockDb));

            mockDb.ExpectMsg<GetInitialStatisticsMessage>();
        }
    }
}
