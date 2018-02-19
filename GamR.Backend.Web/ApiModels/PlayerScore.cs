using System;

namespace GamR.Backend.Web.ApiModels
{
    public class PlayerScore
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Score { get; set; }
    }
}