using System;

namespace GamR.Backend.Web.ApiModels
{
    public class Match
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public string Location { get; set; }
    }
}