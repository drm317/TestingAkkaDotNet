using Akka.Actor;
using Akka.Event;
using Akka.IO;

namespace AkkaModel
{
    public class UserActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();
        
        public string CurrentlyPlaying { get; set; }

        private readonly IActorRef _stats;

        public UserActor(IActorRef stats)
        {
            _stats = stats;
            Receive<PlayMovieMessage>(message =>
            {
                _log.Info("Started playing {0}", message.TitleName);
                CurrentlyPlaying = message.TitleName;
                
                _log.Info("Replying to sender");
                Sender.Tell(new NowPlayingMessage(CurrentlyPlaying));
                _stats.Tell(message.TitleName);
                
                Context.ActorSelection("/user/noexistent").Tell(message);
            });
        }
    }
}