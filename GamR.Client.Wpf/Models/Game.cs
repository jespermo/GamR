using System;
using System.Collections.Generic;

namespace GamR.Client.Wpf.Models
{
    public class Game
    {
        public List<string> MeldingPlayers { get; set; }
        public string Melding { get; set; }
        public int NumberOfTrics { get; set; }
        public string NumberOfVips { get; set; }
        public int ActualNumberOfTricks { get; set; }
    }
}