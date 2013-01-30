using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Concurrent;
using System.Collections.Specialized;

namespace Modularity
{
    public abstract class ModularityApplicationObserverBase : IHttpModule
    {
        internal static readonly ConcurrentQueue<ModularityModule> Modules;
        internal static Func<HttpContextBase> GetContext;

        static ModularityApplicationObserverBase()
        {
			Modules = new ConcurrentQueue<ModularityModule>();
            AddConfiguredModules();
            GetContext = () => new HttpContextWrapper(HttpContext.Current);
        }

        public void Init(HttpApplication context)
        {
			InitializeModules(context.Modules.Cast<string>().Select(k => context.Modules[k]));
            InitializeEvents(context);
        }

        internal abstract void InitializeEvents(HttpApplication context);
        public abstract void Dispose();

		internal void InitializeModules(IEnumerable<IHttpModule> applicationModules)
		{
			AssertObserversLoaded(applicationModules);

            foreach (var module in Modules)
            {
                module.InternalInitialize();
            }
        }

        internal static void AddConfiguredModules()
        {
            if (ModularitySection.Instance != null)
            {
				var elements = ModularitySection.Instance.CustomModules.Cast<ModuleElement>();
                foreach (var element in elements)
                {
                    var type = Type.GetType(element.Type);
                    var module = Activator.CreateInstance(type) as ModularityModule;
                    if (module == null) throw new ApplicationException(string.Format("Unable to add {0}. Does it inherit the abstract class Modularity.Module?", element.Type));					
					//module.IsAsync = element.Async;
                    Modules.Enqueue(module);
                }
            }
        }

        internal void FireSynchronousEvent(Func<ModularityModule, RequestEventHandler> getEvent)
		{
			foreach (var handler in Modules.Where(m => !m.IsAsync).Select(m => getEvent(m)).Where(e => e != null))
			{
				handler(this, new RequestEventArgs(GetContext()));
			}
        }

		private static void AssertObserversLoaded(IEnumerable<IHttpModule> applicationModules)
		{			
			var hasSynchronous = Modules.Any(e => !e.IsAsync);
			var hasAsynchronous = Modules.Any(e => e.IsAsync);
			if (hasSynchronous && !applicationModules.OfType<ModularityApplicationObserver>().Any())
				throw new ApplicationException("Trying to load a synchronous Modularity module without having ModularityApplicationObserver in the http modules section of the configuration file");
			if (hasAsynchronous && !applicationModules.OfType<ModularityAsyncApplicationObserver>().Any())
				throw new ApplicationException("Trying to load an asynchronous Modularity module without having ModularityAsyncApplicationObserver in the http modules section of the configuration file");
		}
    }
}
