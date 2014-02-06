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
        private static ConcurrentBag<string> runMethods;
        private static object locker = new object();

        static MethodHandler()
        {
            runMethods = new ConcurrentBag<string>();
        }

        public static void RunOnce(Action action)
        {
            var methodName = GetMethodName(action);

            var method = runMethods.FirstOrDefault(h => h == methodName);
            if (method != null) return;

            // lock to 1 thread at once.
            lock(locker)
            {   // recheck (stop race)
                method = runMethods.SingleOrDefault(h => h == methodName);

                if (method != null)
                    return;

                runMethods.Add(methodName);
            }

            action();
        }

        /// <summary>
        /// For testing purposes only...
        /// </summary>
        internal static void ClearHandlers()
        {
            runMethods = new ConcurrentBag<string>();
        }

		private static string GetMethodName(Action action)
		{
			return action.Target.GetType().FullName + "." + action.Method.Name;
		}

    }
}
