using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Web.SessionState;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Modularity
{
	/// <summary>
	/// The application observer IHttpModule which handles all ModularityModules that are specified in configuration
	/// </summary>
    public sealed class ModularityApplicationObserver : IHttpModule
    {
        internal static readonly ConcurrentQueue<ModularityModule> Modules;
        internal static Func<HttpContextBase> GetContext;

        static ModularityApplicationObserver()
        {
			Modules = new ConcurrentQueue<ModularityModule>();
            AddConfiguredModules();
            GetContext = () => new HttpContextWrapper(HttpContext.Current);
        }

		/// <summary>
		/// Initializes the ModularityModules specified in the configuration file
		/// </summary>
		/// <param name="context"></param>
        public void Init(HttpApplication context)
        {
			InitializeModules(context.Modules.Cast<string>().Select(k => context.Modules[k]));
			InitializeSynchronousEvents(context);
			InitializeAsynchronousEvents(context);
        }

		private void InitializeAsynchronousEvents(HttpApplication context)
		{            
			// Async events
			AddAsyncEventHandlers(context.AddOnBeginRequestAsync, m => m.OnBeginRequest, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnAuthenticateRequestAsync, m => m.OnAuthenticateRequest, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnPostAuthenticateRequestAsync, m => m.OnPostAuthenticateRequest, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnAuthorizeRequestAsync, m => m.OnAuthorizeRequest, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnPostAuthorizeRequestAsync, m => m.OnPostAuthorizeRequest, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnResolveRequestCacheAsync, m => m.OnResolveRequestCache, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnPostResolveRequestCacheAsync, m => m.OnPostResolveRequestCache, () => new RequestEventArgs(GetContext()));
			if (HttpRuntime.UsingIntegratedPipeline)
			{
				AddAsyncEventHandlers(context.AddOnMapRequestHandlerAsync, m => m.OnMapRequestHandler, () => new RequestEventArgs(GetContext()));
			}
			AddAsyncEventHandlers(context.AddOnPostMapRequestHandlerAsync, m => m.OnPostMapRequestHandler, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnAcquireRequestStateAsync, m => m.OnAcquireRequestState, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnPostAcquireRequestStateAsync, m => m.OnPostAcquireRequestState, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnPreRequestHandlerExecuteAsync, m => m.OnPreRequestHandlerExecute, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnPostRequestHandlerExecuteAsync, m => m.OnPostRequestHandlerExecute, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnReleaseRequestStateAsync, m => m.OnReleaseRequestState, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnPostReleaseRequestStateAsync, m => m.OnPostReleaseRequestState, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnUpdateRequestCacheAsync, m => m.OnUpdateRequestCache, () => new RequestEventArgs(GetContext()));
			AddAsyncEventHandlers(context.AddOnPostUpdateRequestCacheAsync, m => m.OnPostUpdateRequestCache, () => new RequestEventArgs(GetContext()));
			if (HttpRuntime.UsingIntegratedPipeline)
			{
				AddAsyncEventHandlers(context.AddOnLogRequestAsync, m => m.OnLogRequest, () => new RequestEventArgs(GetContext()));
				AddAsyncEventHandlers(context.AddOnPostLogRequestAsync, m => m.OnPostLogRequest, () => new RequestEventArgs(GetContext()));
			}
			AddAsyncEventHandlers(context.AddOnEndRequestAsync, m => m.OnEndRequest, () => new RequestEventArgs(GetContext()));
		}

		private void InitializeSynchronousEvents(HttpApplication context)
		{
			context.BeginRequest += (o, e) => FireEventSynchronously(m => m.OnBeginRequest);
			context.AuthenticateRequest += (o, e) => FireEventSynchronously(m => m.OnAuthenticateRequest);
			context.PostAuthenticateRequest += (o, e) => FireEventSynchronously(m => m.OnPostAuthenticateRequest);
			context.AuthorizeRequest += (o, e) => FireEventSynchronously(m => m.OnAuthorizeRequest);
			context.PostAuthorizeRequest += (o, e) => FireEventSynchronously(m => m.OnPostAuthorizeRequest);
			context.ResolveRequestCache += (o, e) => FireEventSynchronously(m => m.OnResolveRequestCache);
			context.PostResolveRequestCache += (o, e) => FireEventSynchronously(m => m.OnPostResolveRequestCache);
			if (HttpRuntime.UsingIntegratedPipeline)
			{
				context.MapRequestHandler += (o, e) => FireEventSynchronously(m => m.OnMapRequestHandler);
			}
			context.PostMapRequestHandler += (o, e) => FireEventSynchronously(m => m.OnPostMapRequestHandler);
			context.AcquireRequestState += (o, e) => FireEventSynchronously(m => m.OnAcquireRequestState);
			context.PostAcquireRequestState += (o, e) => FireEventSynchronously(m => m.OnPostAcquireRequestState);
			context.PreRequestHandlerExecute += (o, e) => FireEventSynchronously(m => m.OnPreRequestHandlerExecute);
			context.PostRequestHandlerExecute += (o, e) => FireEventSynchronously(m => m.OnPostRequestHandlerExecute);
			context.ReleaseRequestState += (o, e) => FireEventSynchronously(m => m.OnReleaseRequestState);
			context.PostReleaseRequestState += (o, e) => FireEventSynchronously(m => m.OnPostReleaseRequestState);
			context.UpdateRequestCache += (o, e) => FireEventSynchronously(m => m.OnUpdateRequestCache);
			context.PostUpdateRequestCache += (o, e) => FireEventSynchronously(m => m.OnPostUpdateRequestCache);
			if (HttpRuntime.UsingIntegratedPipeline)
			{
				context.LogRequest += (o, e) => FireEventSynchronously(m => m.OnLogRequest);
				context.PostLogRequest += (o, e) => FireEventSynchronously(m => m.OnPostLogRequest);
			}
			context.EndRequest += (o, e) => FireEventSynchronously(m => m.OnEndRequest);

			// Synchronous only events
			context.PreSendRequestHeaders += (o, e) => FireEventSynchronously(m => m.OnPreSendRequestHeaders);
			context.PreSendRequestContent += (o, e) => FireEventSynchronously(m => m.OnPreSendRequestContent);

			context.Disposed += (o, e) => FireEventSynchronously(m => m.OnDisposed);
			context.Error += (o, e) => FireEventSynchronously(m => m.OnError);

			var session = context.Modules["Session"] as SessionStateModule;
			if (session != null)
			{
				session.Start += (o, e) => FireEventSynchronously(m => m.OnSessionStart);
				session.End += (o, e) => FireEventSynchronously(m => m.OnSessionEnd);
			}
		}

		/// <summary>
		/// Empty dispose function
		/// </summary>
		public void Dispose()
		{

		}

		internal void InitializeModules(IEnumerable<IHttpModule> applicationModules)
		{
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
                    if (module == null) throw new ApplicationException(string.Format("Unable to add {0}. Does it inherit the abstract class Modularity.ModularityModule?", element.Type));										
                    Modules.Enqueue(module);
                }
            }
        }

		private void FireEventSynchronously(Expression<Func<ModularityModule, EventHandler>> getEvent)
		{
			var iterator = Modules
					   .Where(m => !m.IsAsync || IsSynchronousOnlyEvent(getEvent))
					   .Select(m => getEvent.Compile()(m))
					   .Where(e => e != null);

			foreach (var handler in iterator)
			{
				handler(null, EventArgs.Empty);
			}
		}

		internal void FireEventSynchronously(Expression<Func<ModularityModule, RequestEventHandler<RequestEventArgs>>> getEvent)
		{
			var context = GetContext();
			FireEventSynchronously<RequestEventArgs>(getEvent, () => new RequestEventArgs(context));
		}

		internal void FireEventSynchronously(Expression<Func<ModularityModule, RequestEventHandler<EventArgs>>> getEvent)
		{
			FireEventSynchronously<EventArgs>(getEvent, () => EventArgs.Empty);
		}

		internal void FireEventSynchronously(Expression<Func<ModularityModule, RequestEventHandler<ErrorRequestEventArgs>>> getEvent)
		{
			var context = GetContext();
			var exception = context.Server.GetLastError();
			//context.Server.ClearError();
			FireEventSynchronously<ErrorRequestEventArgs>(getEvent, () => new ErrorRequestEventArgs(exception, context));
		}

		private void FireEventSynchronously<TEventArgs>(Expression<Func<ModularityModule, RequestEventHandler<TEventArgs>>> getEvent, Func<TEventArgs> getArgs)
			where TEventArgs : EventArgs
		{
			var context = GetContext();
			var iterator = Modules
					.Where(m => !m.IsAsync || IsSynchronousOnlyEvent(getEvent))
					.Select(m => getEvent.Compile()(m))
					.Where(e => e != null);

			foreach (var handler in iterator)
			{
				var sender = context.ApplicationInstance;
				var args = getArgs();
				handler(sender, args);
			}
		}

		private void AddAsyncEventHandlers<TEventArgs>(Action<BeginEventHandler, EndEventHandler, object> addAsyncEvent, Func<ModularityModule, RequestEventHandler<TEventArgs>> getEvent, Func<TEventArgs> getArgs)
			where TEventArgs : EventArgs
		{
			foreach (var handler in Modules.Where(m => m.IsAsync).Select(m => getEvent(m)).Where(e => e != null))
			{
				BeginEventHandler begin = CreateBeginEventHandler<TEventArgs>(handler, getArgs);
				EndEventHandler end = result =>
				{
				};
				addAsyncEvent(begin, end, handler);
			}
		}

		private BeginEventHandler CreateBeginEventHandler<TEventArgs>(RequestEventHandler<TEventArgs> handler, Func<TEventArgs> getArgs)
			where TEventArgs : EventArgs
		{
			return (sender, args, callback, state) =>
			{
				var application = (HttpApplication)sender;
				var eventState = new RequestEventState<TEventArgs>
				{
					Context = application.Context,
					Sender = sender,
					Handler = (RequestEventHandler<TEventArgs>)state,
					GetEventArgs = getArgs
				};
				var task = Task.Factory.StartNew(s =>
				{
					var requestEventState = (RequestEventState<TEventArgs>)s;
					HttpContext.Current = requestEventState.Context;
					requestEventState.Handler(requestEventState.Sender, requestEventState.GetEventArgs());
				}, eventState);
				task.ContinueWith(t => callback(t));
				return task;
			};
		}

		private bool IsSynchronousOnlyEvent(Expression getEventExpression)
		{
			using (var visitor = new GetEventExpressionVisitor(getEventExpression))
			{
				return visitor.IsSynchronousOnly;
			}
		}
    }
}
