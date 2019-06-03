using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DetegoServer.Helpers
{
    public class NotStoredException : Exception
    {
        public NotStoredException() : base() { }
        public NotStoredException(string Message) : base(Message) { }
        public NotStoredException(string Message, Exception InnerException) : base(Message, InnerException) { }
    }
}
