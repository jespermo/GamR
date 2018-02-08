using System;

namespace GamR.Client.Wpf.Models
{
    public class PlayerScore
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Score { get; set; }
    }
}