using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Modularity
{
	public class ErrorRequestEventArgs : RequestEventArgs
	{
		public ErrorRequestEventArgs(Exception exception, HttpContextBase context)
			: base(context)
		{

		}

		public Exception LastError { private set; get; }
	}
}
