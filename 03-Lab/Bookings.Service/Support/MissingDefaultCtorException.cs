using System;

namespace Bookings.Service.Support
{
    public class MissingDefaultCtorException : Exception
    {
        public MissingDefaultCtorException(Type t)
            : base(string.Format("Type {0} has no default constructor. Add protected {1}(){{}}", t.FullName, t.Name))
        {
        }
    }
}