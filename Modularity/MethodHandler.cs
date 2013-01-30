using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.Concurrent;
using System.Threading;

namespace Modularity
{
    internal class MethodHandler
    {
        private static ConcurrentBag<MethodHandler> handlers;

        static MethodHandler()
        {
            handlers = new ConcurrentBag<MethodHandler>();
        }

        internal MethodHandler(Action action)
        {
            Delegate = action;
            Status = 0;
        }

        public static void RunOnce(Action action)
        {
            var methodName = GetMethodName(action);

            var handler = handlers.SingleOrDefault(h => h.MethodName == methodName);
            if (handler != null && handler.Status == 2) return;

            if (handler == null)
            {
                handler = new MethodHandler(action);
                handlers.Add(handler);
            }

            if (Interlocked.Exchange(ref handler.Status, 1) == 0)
            {
                action();
                handler.Status = 2;
            }
            while (handler.Status == 1)
            {
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// For testing purposes only...
        /// </summary>
        internal static void ClearHandlers()
        {
            handlers = new ConcurrentBag<MethodHandler>();
        }

		private static string GetMethodName(Action action)
		{
			return action.Target.GetType().FullName + "." + action.Method.Name;
		}

        /// <summary>        
        /// NotStarted = 0,
        /// Executing = 1,
        /// Ended = 2
        /// </summary>
        internal int Status;
        internal Action Delegate { get; private set; }
		internal string MethodName { get { return GetMethodName(Delegate); } }
    }
}
