using System.Collections.ObjectModel;

namespace AkkaModel
{
    public class InitialStatisticsMessage
    {
        public ReadOnlyDictionary<string, int> PlayCounts { get; set; }

        public InitialStatisticsMessage(ReadOnlyDictionary<string, int> playCounts)
        {
            PlayCounts = playCounts;
        }
    }
}