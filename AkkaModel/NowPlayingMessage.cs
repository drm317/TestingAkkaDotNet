namespace AkkaModel
{
    public class NowPlayingMessage
    {
        public string CurrentlyPlaying { get; set; }

        public NowPlayingMessage(string currentlyPlaying)
        {
            CurrentlyPlaying = currentlyPlaying;
        }
    }
}