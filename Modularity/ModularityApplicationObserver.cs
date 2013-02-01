using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Concurrent;
using System.Web.SessionState;

namespace Modularity
{
    public class ModularityApplicationObserver : ModularityApplicationObserverBase
	{
		protected override bool IsAsync { get { return false; } }
        internal override void InitializeEvents(HttpApplication context)
        {
            context.AcquireRequestState += (o, e) => FireEventSynchronously(m => m.OnAcquireRequestState);
            context.AuthenticateRequest += (o, e) => FireEventSynchronously(m => m.OnAuthenticateRequest);
            context.AuthorizeRequest += (o, e) => FireEventSynchronously(m => m.OnAuthorizeRequest);
            context.BeginRequest += (o, e) => FireEventSynchronously(m => m.OnBeginRequest);
            context.EndRequest += (o, e) => FireEventSynchronously(m => m.OnEndRequest);
            context.LogRequest += (o, e) => FireEventSynchronously(m => m.OnLogRequest);
            context.MapRequestHandler += (o, e) => FireEventSynchronously(m => m.OnMapRequestHandler);
            context.PostAcquireRequestState += (o, e) => FireEventSynchronously(m => m.OnPostAcquireRequestState);
            context.PostAuthenticateRequest += (o, e) => FireEventSynchronously(m => m.OnPostAuthenticateRequest);
            context.PostAuthorizeRequest += (o, e) => FireEventSynchronously(m => m.OnPostAuthorizeRequest);
            context.PostLogRequest += (o, e) => FireEventSynchronously(m => m.OnPostLogRequest);
            context.PostMapRequestHandler += (o, e) => FireEventSynchronously(m => m.OnPostMapRequestHandler);
            context.PostReleaseRequestState += (o, e) => FireEventSynchronously(m => m.OnPostReleaseRequestState);
            context.PostRequestHandlerExecute += (o, e) => FireEventSynchronously(m => m.OnPostRequestHandlerExecute);
            context.PostResolveRequestCache += (o, e) => FireEventSynchronously(m => m.OnPostResolveRequestCache);
            context.PostUpdateRequestCache += (o, e) => FireEventSynchronously(m => m.OnPostUpdateRequestCache);
            context.PreRequestHandlerExecute += (o, e) => FireEventSynchronously(m => m.OnPreRequestHandlerExecute);
            context.PreSendRequestContent += (o, e) => FireEventSynchronously(m => m.OnPreSendRequestContent);
            context.PreSendRequestHeaders += (o, e) => FireEventSynchronously(m => m.OnPreSendRequestHeaders);
            context.ReleaseRequestState += (o, e) => FireEventSynchronously(m => m.OnReleaseRequestState);
            context.ResolveRequestCache += (o, e) => FireEventSynchronously(m => m.OnResolveRequestCache);
            context.UpdateRequestCache += (o, e) => FireEventSynchronously(m => m.OnUpdateRequestCache);

            context.Disposed += (o, e) => FireEventSynchronously(m => m.OnDisposed);
            context.Error += (o, e) => FireEventSynchronously(m => m.OnError);

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
	}
}
