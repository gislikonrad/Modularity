using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modularity
{
	/// <summary>
	/// Base class to create a strongly typed ASP.Net http module
	/// </summary>
    public abstract class ModularityModule
    {
		/// <summary>
		/// Gets whether the ModularityModule should use Async application events if available.
		/// </summary>
		public abstract bool IsAsync { get; }

		/// <summary>
		/// Initialized the ModularityModule. This is where you hook up the module events.
		/// </summary>
		public abstract void Initialize();
        internal void InternalInitialize()
        {
            MethodHandler.RunOnce(() => Initialize());
        }

		/// <summary>
		/// First event in lifecycle. Happens before OnAuthenticateRequest.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnBeginRequest;

		/// <summary>
		/// Lifecycle event. Happens after OnBeginRequest and before OnPostAuthenticateRequest.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnAuthenticateRequest;

		/// <summary>
		/// Lifecycle event. Happens after OnAuthenticateRequest and before OnAuthorizeRequest.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnPostAuthenticateRequest;

		/// <summary>
		/// Lifecycle event. Happens after OnPostAuthenticateRequest and before OnPostAuthorizeRequest.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnAuthorizeRequest;

		/// <summary>
		/// Lifecycle event. Happens after OnAuthorizeRequest and before OnResolveRequestCache.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnPostAuthorizeRequest;

		/// <summary>
		/// Lifecycle event. Happens after OnPostAuthorizeRequest and before OnPostResolveRequestCache.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnResolveRequestCache;

		/// <summary>
		/// Lifecycle event. Happens after OnResolveRequestCache and before OnMapRequestHandler.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnPostResolveRequestCache;

		/// <summary>
		/// Lifecycle event. Happens after OnPostResolveRequestCache and before OnPostMapRequestHandler. This event is only fired in integrated pipeline mode.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnMapRequestHandler;

		/// <summary>
		/// Lifecycle event. Happens after OnMapRequestHandler and before OnAcquireRequestState.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnPostMapRequestHandler;

		/// <summary>
		/// Lifecycle event. Happens after OnPostMapRequestHandler and before OnPostAcquireRequestState.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnAcquireRequestState;

		/// <summary>
		/// Lifecycle event. Happens after OnAcquireRequestState and before OnPreRequestHandlerExecute.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnPostAcquireRequestState;

		/// <summary>
		/// Lifecycle event. Happens after OnPostAcquireRequestState and before OnPostRequestHandlerExecute.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnPreRequestHandlerExecute;

		/// <summary>
		/// Lifecycle event. Happens after OnPreRequestHandlerExecute and before OnReleaseRequestState.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnPostRequestHandlerExecute;

		/// <summary>
		/// Lifecycle event. Happens after OnPostRequestHandlerExecute and before OnPostReleaseRequestState.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnReleaseRequestState;

		/// <summary>
		/// Lifecycle event. Happens after OnReleaseRequestState and before OnUpdateRequestCache.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnPostReleaseRequestState;

		/// <summary>
		/// Lifecycle event. Happens after OnPostReleaseRequestState and before OnPostUpdateRequestCache.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnUpdateRequestCache;

		/// <summary>
		/// Lifecycle event. Happens after OnUpdateRequestCache and before OnLogRequest.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnPostUpdateRequestCache;

		/// <summary>
		/// Lifecycle event. Happens after OnPostUpdateRequestCache and before OnPostLogRequest. This event is only fired in integrated pipeline mode.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnLogRequest;

		/// <summary>
		/// Lifecycle event. Happens after OnLogRequest and before OnEndRequest. This event is only fired in integrated pipeline mode.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnPostLogRequest;

		/// <summary>
		/// Lifecycle event. Happens after OnPostLogRequest and before OnPreSendRequestHeaders.
		/// </summary>
		public RequestEventHandler<RequestEventArgs> OnEndRequest;

		/// <summary>
		/// Lifecycle event. Happens after OnEndRequest and before OnPreSendRequestContent. This event can only be fired synchronously.
		/// </summary>
		[SynchronousOnlyEvent]
		public RequestEventHandler<RequestEventArgs> OnPreSendRequestHeaders;

		/// <summary>
		/// Last event in lifecycle. Happens after OnPreSendRequestHeaders. This event can only be fired synchronously.
		/// </summary>
		[SynchronousOnlyEvent]
		public RequestEventHandler<RequestEventArgs> OnPreSendRequestContent;

		/// <summary>
		/// On session start. This event can only be fired synchronously.
		/// </summary>
		[SynchronousOnlyEvent]
		public RequestEventHandler<RequestEventArgs> OnSessionStart;

		/// <summary>
		/// On session end. This event can only be fired synchronously.
		/// </summary>
		[SynchronousOnlyEvent]
		public EventHandler OnSessionEnd;

		/// <summary>
		/// Global error handler. This event can only be fired synchronously.
		/// </summary>
		[SynchronousOnlyEvent]
		public RequestEventHandler<ErrorRequestEventArgs> OnError;

		/// <summary>
		/// On application end. This event can only be fired synchronously.
		/// </summary>
		[SynchronousOnlyEvent]
		public EventHandler OnDisposed;
    }

	/// <summary>
	/// Event handler definition for ModularityModule events
	/// </summary>
	/// <typeparam name="TEventArgs">Type of event args</typeparam>
	/// <param name="sender">The event sender.</param>
	/// <param name="e">Event args</param>
	public delegate void RequestEventHandler<TEventArgs>(object sender, TEventArgs e)
		where TEventArgs : EventArgs;
}
