using System;

namespace GamR.Backend.Core.Framework.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message, params object[] args) 
            : base(string.Format(message, args))
        {
            
        }

        public DomainException(Exception innerException, string message, params object[] args) : base(
            string.Format(message, args), innerException)
        {
            
        }
    }
}