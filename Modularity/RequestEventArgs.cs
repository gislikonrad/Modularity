using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Modularity
{
    public class RequestEventArgs : EventArgs
    {        
        public RequestEventArgs(HttpContextBase context)
        {
            Context = context;
        }
        public HttpContextBase Context { get; private set; }
    }
}
