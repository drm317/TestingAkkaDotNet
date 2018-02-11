using Akka.Actor;
using Akka.IO;

namespace AkkaModel
{
    public class UserActor : ReceiveActor
    {
        public string CurrentlyPlaying { get; set; }

        private readonly IActorRef _stats;

        public UserActor(IActorRef stats)
        {
            _stats = stats;
            Receive<PlayMovieMessage>(message =>
            {
                CurrentlyPlaying = message.TitleName;
                Sender.Tell(new NowPlayingMessage(CurrentlyPlaying));
                _stats.Tell(message.TitleName);
            });
        }
    }
}