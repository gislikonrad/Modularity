using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Modularity
{
	/// <summary>
	/// Event args for error based events
	/// </summary>
	public class ErrorRequestEventArgs : RequestEventArgs
	{
		internal ErrorRequestEventArgs(Exception exception, HttpContextBase context)
			: base(context)
		{
			LastError = exception;
		}

		/// <summary>
		/// The exception that was thrown which triggered the event handler
		/// </summary>
		public Exception LastError { private set; get; }
	}
}
