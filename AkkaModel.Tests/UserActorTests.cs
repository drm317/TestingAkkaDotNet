using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using Akka.TestKit.Xunit2;
using Xunit;

namespace AkkaModel.Tests
{
    public class UserActorTests : TestKit
    {
        [Fact]
        public void ShouldHaveInitialState()
        {
            TestActorRef<UserActor> actor = ActorOfAsTestActorRef<UserActor>(
                Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            Assert.Null(actor.UnderlyingActor.CurrentlyPlaying);
        }

        [Fact]
        public void ShouldUpdateCurrentlyPlayingState()
        {
            TestActorRef<UserActor> actor = ActorOfAsTestActorRef<UserActor>(
                Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            actor.Tell(new PlayMovieMessage("Conan the Barbarian"));

            Assert.Equal("Conan the Barbarian", actor.UnderlyingActor.CurrentlyPlaying);
        }

        [Fact]
        public void ShouldPlayMovie()
        {
            TestActorRef<UserActor> actor = ActorOfAsTestActorRef<UserActor>(
                Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            actor.Tell(new PlayMovieMessage("Conan the Barbarian"));

            NowPlayingMessage received = ExpectMsg<NowPlayingMessage>(TimeSpan.FromSeconds(10));

            Assert.Equal("Conan the Barbarian", received.CurrentlyPlaying);
        }

        [Fact]
        public void ShoudLogPlayMovie()
        {
            IActorRef actor = ActorOf(Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            EventFilter.Info("Started playing Boolean Lies").And.Info("Replying to sender")
                .Expect(2, () => actor.Tell(new PlayMovieMessage("Boolean Lies")));
        }

        [Fact]
        public void ShouldSendToDeadLettersForUndeliveredMessage()
        {
            IActorRef actor = ActorOf(Props.Create(() => new UserActor(ActorOf(BlackHoleActor.Props))));

            EventFilter.DeadLetter<PlayMovieMessage>(
                    message => message.TitleName == "Boolean Lies")
                .ExpectOne(() => actor.Tell(new PlayMovieMessage("Boolean Lies")));

        }
    }
}

    