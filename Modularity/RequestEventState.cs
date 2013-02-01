using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Modularity
{
	internal class RequestEventState<TEventArgs>
		where TEventArgs : EventArgs
	{
		internal HttpContext Context { get; set; }
		internal object Sender { get; set; }
		internal RequestEventHandler<TEventArgs> Handler { get; set; }
		internal Func<TEventArgs> GetEventArgs { get; set; }
	}
}
