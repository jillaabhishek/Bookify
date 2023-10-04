using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Exceptions
{
    public sealed class ConcurrencyException : Exception
    {
        public ConcurrencyException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
