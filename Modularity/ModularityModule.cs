using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modularity
{
    public abstract class ModularityModule
    {
		internal bool IsAsync { get; set; }

        public virtual void Initialize() { }
        internal void InternalInitialize()
        {
            MethodHandler.RunOnce(() => Initialize());
        }

		public RequestEventHandler<RequestEventArgs> OnBeginRequest;
		public RequestEventHandler<RequestEventArgs> OnAuthenticateRequest;
		public RequestEventHandler<RequestEventArgs> OnPostAuthenticateRequest;
		public RequestEventHandler<RequestEventArgs> OnAuthorizeRequest;
		public RequestEventHandler<RequestEventArgs> OnPostAuthorizeRequest;
		public RequestEventHandler<RequestEventArgs> OnResolveRequestCache;
		public RequestEventHandler<RequestEventArgs> OnPostResolveRequestCache;
		public RequestEventHandler<RequestEventArgs> OnMapRequestHandler;
		public RequestEventHandler<RequestEventArgs> OnPostMapRequestHandler;
		public RequestEventHandler<RequestEventArgs> OnAcquireRequestState;
		public RequestEventHandler<RequestEventArgs> OnPostAcquireRequestState;
		public RequestEventHandler<RequestEventArgs> OnPreRequestHandlerExecute;
		public RequestEventHandler<RequestEventArgs> OnPostRequestHandlerExecute;
		public RequestEventHandler<RequestEventArgs> OnReleaseRequestState;
		public RequestEventHandler<RequestEventArgs> OnPostReleaseRequestState;
		public RequestEventHandler<RequestEventArgs> OnUpdateRequestCache;
		public RequestEventHandler<RequestEventArgs> OnPostUpdateRequestCache;
		public RequestEventHandler<RequestEventArgs> OnLogRequest;
		public RequestEventHandler<RequestEventArgs> OnPostLogRequest;
		public RequestEventHandler<RequestEventArgs> OnEndRequest;
		public RequestEventHandler<RequestEventArgs> OnPreSendRequestHeaders;
		public RequestEventHandler<RequestEventArgs> OnPreSendRequestContent;


		public RequestEventHandler<RequestEventArgs> OnSessionStart;
		public RequestEventHandler<RequestEventArgs> OnSessionEnd;

		public RequestEventHandler<ErrorRequestEventArgs> OnError;
		public RequestEventHandler<EventArgs> OnDisposed;
    }
	public delegate void RequestEventHandler<TEventArgs>(object sender, TEventArgs e)
		where TEventArgs : EventArgs;
}
