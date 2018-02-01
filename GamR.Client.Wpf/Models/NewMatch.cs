using System;

namespace GamR.Client.Wpf.Models
{
    [Serializable]
    public class NewMatch
    {
        public DateTime Date { get; set; }
        public string Location { get; set; }
    }
}