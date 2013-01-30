using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Web.SessionState;

namespace Modularity
{
	// TODO: Finish this
    public class ModularityAsyncApplicationObserver : ModularityApplicationObserverBase
    {
        internal override void InitializeEvents(HttpApplication context)
        {
            // Async events
            AddAsyncEventHandlers(context.AddOnAcquireRequestStateAsync, m => m.OnAcquireRequestState);
            AddAsyncEventHandlers(context.AddOnAuthenticateRequestAsync, m => m.OnAuthenticateRequest);
            AddAsyncEventHandlers(context.AddOnAuthorizeRequestAsync, m => m.OnAuthorizeRequest);
            AddAsyncEventHandlers(context.AddOnBeginRequestAsync, m => m.OnBeginRequest);
            AddAsyncEventHandlers(context.AddOnEndRequestAsync, m => m.OnEndRequest);
            AddAsyncEventHandlers(context.AddOnLogRequestAsync, m => m.OnLogRequest);
            AddAsyncEventHandlers(context.AddOnMapRequestHandlerAsync, m => m.OnMapRequestHandler);
            AddAsyncEventHandlers(context.AddOnPostAcquireRequestStateAsync, m => m.OnPostAcquireRequestState);
            AddAsyncEventHandlers(context.AddOnPostAuthenticateRequestAsync, m => m.OnPostAuthenticateRequest);
            AddAsyncEventHandlers(context.AddOnPostAuthorizeRequestAsync, m => m.OnPostAuthorizeRequest);
            AddAsyncEventHandlers(context.AddOnPostLogRequestAsync, m => m.OnPostLogRequest);
            AddAsyncEventHandlers(context.AddOnPostMapRequestHandlerAsync, m => m.OnPostMapRequestHandler);
            AddAsyncEventHandlers(context.AddOnPostReleaseRequestStateAsync, m => m.OnPostReleaseRequestState);
            AddAsyncEventHandlers(context.AddOnPostRequestHandlerExecuteAsync, m => m.OnPostRequestHandlerExecute);
            AddAsyncEventHandlers(context.AddOnPostResolveRequestCacheAsync, m => m.OnPostResolveRequestCache);
            AddAsyncEventHandlers(context.AddOnPostUpdateRequestCacheAsync, m => m.OnPostUpdateRequestCache);
            AddAsyncEventHandlers(context.AddOnPreRequestHandlerExecuteAsync, m => m.OnPreRequestHandlerExecute);
            AddAsyncEventHandlers(context.AddOnReleaseRequestStateAsync, m => m.OnReleaseRequestState);
            AddAsyncEventHandlers(context.AddOnResolveRequestCacheAsync, m => m.OnResolveRequestCache);
            AddAsyncEventHandlers(context.AddOnUpdateRequestCacheAsync, m => m.OnUpdateRequestCache);
            
            // Synchronous only events
            context.Disposed += (o, e) => FireSynchronousEvent(m => m.OnDisposed);
            context.Error += (o, e) => FireSynchronousEvent(m => m.OnError);
            context.PreSendRequestContent += (o, e) => FireSynchronousEvent(m => m.OnPreSendRequestContent);
            context.PreSendRequestHeaders += (o, e) => FireSynchronousEvent(m => m.OnPreSendRequestHeaders);

            var session = context.Modules["Session"] as SessionStateModule;
            if (session != null)
            {
                session.Start += (o, e) => FireSynchronousEvent(m => m.OnSessionStart);
                session.End += (o, e) => FireSynchronousEvent(m => m.OnSessionEnd);
            }
        }

        public override void Dispose()
        {
        }

        private void AddAsyncEventHandlers(Action<BeginEventHandler, EndEventHandler, object> addAsyncEvent, Func<ModularityModule, RequestEventHandler> getEvent)
		{
			foreach (var handler in Modules.Where(m => m.IsAsync).Select(m => getEvent(m)).Where(e => e != null))
			{
				BeginEventHandler begin = (sender, args, callback, state) =>
				{
					System.Diagnostics.Debug.WriteLine(string.Format("Async event start. ThreadId: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));
					var application = (HttpApplication)sender;
					var s = new RequestEventState
					{
						Context = application.Context,
						Sender = sender,
						Handler = (RequestEventHandler)state
					};

					return Run(s);
				};
				EndEventHandler end = result => 
				{				
					System.Diagnostics.Debug.WriteLine(string.Format("Async event end. ThreadId: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));
				};
				addAsyncEvent(begin, end, handler);
			}
        }

		private Task Run(RequestEventState state)
		{
			var task = Task.Factory.StartNew(s =>
				{
					var requestEventState = (RequestEventState)s;
					HttpContext.Current = requestEventState.Context;
					requestEventState.Handler(requestEventState.Sender, new RequestEventArgs(GetContext()));
				}, state);
			task.Wait();
			return task;
		}
    }
}
