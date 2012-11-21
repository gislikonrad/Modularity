using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modularity
{
    public abstract class Module
    {
        public RequestEventHandler OnAcquireRequestState;
        public RequestEventHandler OnAuthenticateRequest;
        public RequestEventHandler OnAuthorizeRequest;
        public RequestEventHandler OnBeginRequest;
        public RequestEventHandler OnDisposed;
        public RequestEventHandler OnEndRequest;
        public RequestEventHandler OnError;
        public RequestEventHandler OnLogRequest;
        public RequestEventHandler OnMapRequestHandler;
        public RequestEventHandler OnPostAcquireRequestState;
        public RequestEventHandler OnPostAuthenticateRequest;
        public RequestEventHandler OnPostAuthorizeRequest;
        public RequestEventHandler OnPostLogRequest;
        public RequestEventHandler OnPostMapRequestHandler;
        public RequestEventHandler OnPostReleaseRequestState;
        public RequestEventHandler OnPostRequestHandlerExecute;
        public RequestEventHandler OnPostResolveRequestCache;
        public RequestEventHandler OnPostUpdateRequestCache;
        public RequestEventHandler OnPreRequestHandlerExecute;
        public RequestEventHandler OnPreSendRequestContent;
        public RequestEventHandler OnPreSendRequestHeaders;
        public RequestEventHandler OnReleaseRequestState;
        public RequestEventHandler OnResolveRequestCache;
        public RequestEventHandler OnUpdateRequestCache;

        public RequestEventHandler OnSessionStart;
        public RequestEventHandler OnSessionEnd;
    }    
    public delegate void RequestEventHandler(object sender, RequestEventArgs e);
}
