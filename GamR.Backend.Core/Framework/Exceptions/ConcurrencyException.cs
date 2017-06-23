using System;

namespace GamR.Backend.Core.Framework.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(string message, params object[] args) 
            : base(string.Format(message, args))
        {

        }

        public ConcurrencyException(Exception innerException, string message, params object[] args) : base(
            string.Format(message, args), innerException)
        {

        }
    }
}