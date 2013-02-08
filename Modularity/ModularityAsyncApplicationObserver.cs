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
    public class ModularityAsyncApplicationObserver : ModularityApplicationObserverBase
	{
		protected override bool IsAsync { get { return true; } }
        internal override void InitializeEvents(HttpApplication context)
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
            
            // Synchronous only events
            context.Disposed += (o, e) => FireEventSynchronously(m => m.OnDisposed);
            context.Error += (o, e) => FireEventSynchronously(m => m.OnError);
            context.PreSendRequestContent += (o, e) => FireEventSynchronously(m => m.OnPreSendRequestContent);
            context.PreSendRequestHeaders += (o, e) => FireEventSynchronously(m => m.OnPreSendRequestHeaders);

            var session = context.Modules["Session"] as SessionStateModule;
            if (session != null)
            {
                session.Start += (o, e) => FireEventSynchronously(m => m.OnSessionStart);
                session.End += (o, e) => FireEventSynchronously(m => m.OnSessionEnd);
            }
        }

        public override void Dispose()
        {
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
					var task = new Task(s =>
					{
						var requestEventState = (RequestEventState<TEventArgs>)s;
						HttpContext.Current = requestEventState.Context;
						requestEventState.Handler(requestEventState.Sender, requestEventState.GetEventArgs());
					}, eventState);
					task.ContinueWith(t => callback(t));
					return task;
				};
		}
    }
}
