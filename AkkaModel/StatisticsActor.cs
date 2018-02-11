using System.Collections.Generic;
using Akka.Actor;
using Akka.IO;

namespace AkkaModel
{
    public class StatisticsActor : ReceiveActor
    {

        private readonly IActorRef _databaseActor;
        
        public Dictionary<string, int> PlayCounts { get; set; }

        public StatisticsActor(IActorRef databaseActor)
        {
            _databaseActor = databaseActor;
            
            Receive<InitialStatisticsMessage>(message => HandleInitialMessage(message));
            Receive<string>(title => HandleTitleMessage(title));
        }

        public void HandleTitleMessage(string title)
        {
            if (PlayCounts.ContainsKey(title))
            {
                PlayCounts[title]++;
            }
            else
            {
                PlayCounts.Add(title, 1);
            }
        }

        public void HandleInitialMessage(InitialStatisticsMessage message)
        {
            PlayCounts = new Dictionary<string, int>(message.PlayCounts);
        }

        protected override void PreStart()
        {
            _databaseActor.Tell(new GetInitialStatisticsMessage());
        }
    }
}