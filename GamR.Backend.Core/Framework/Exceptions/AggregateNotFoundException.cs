using System;

namespace GamR.Backend.Core.Framework.Exceptions
{
    public class AggregateNotFoundException : Exception
    {
        public AggregateNotFoundException(string message, params object[] args) 
            : base(string.Format(message, args))
        {

        }

        public AggregateNotFoundException(Exception innerException, string message, params object[] args) : base(
            string.Format(message, args), innerException)
        {

        }
    }
}