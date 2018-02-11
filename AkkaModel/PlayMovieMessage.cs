namespace AkkaModel
{
    public class PlayMovieMessage
    {
        public string TitleName { get; set; }

        public PlayMovieMessage(string titleName)
        {
            TitleName = titleName;
        }
    }
}