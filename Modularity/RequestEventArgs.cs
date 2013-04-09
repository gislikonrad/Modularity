using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Modularity
{
	/// <summary>
	/// Event args for request based events
	/// </summary>
    public class RequestEventArgs : EventArgs
    {       
        internal RequestEventArgs(HttpContextBase context)
        {
            Context = context;
        }

		/// <summary>
		/// Current HttpContext
		/// </summary>
        public HttpContextBase Context { get; private set; }
    }
}
