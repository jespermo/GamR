
using System;

namespace GamR.Backend.Web.ApiModels
{
    [Serializable]
    public class NewMatch
    {
        public DateTime Date { get; set; }
        public string Location { get; set; }
    }

    public class MatchCreated
    {
        public Guid Id;
    }
    public class Match
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public string Location { get; set; }
    }
    public class CreateMatch
    {
        public DateTime Date { get; set; }

        public string Location { get; set; }
    }
}
