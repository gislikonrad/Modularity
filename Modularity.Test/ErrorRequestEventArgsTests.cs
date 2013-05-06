using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Modularity.Test
{
	[TestClass]
	public class ErrorRequestEventArgsTests
	{
		[TestMethod]
		public void ShouldPopulateLastError()
		{
			var exception = new Exception("Some message");
			var args = new ErrorRequestEventArgs(exception, null);
			Assert.AreEqual(exception, args.LastError);
		}
	}
}
