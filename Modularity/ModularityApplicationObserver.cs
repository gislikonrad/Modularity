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
        internal override void InitializeEvents(HttpApplication context)
        {
            context.AcquireRequestState += (o, e) => FireSynchronousEvent(m => m.OnAcquireRequestState);
            context.AuthenticateRequest += (o, e) => FireSynchronousEvent(m => m.OnAuthenticateRequest);
            context.AuthorizeRequest += (o, e) => FireSynchronousEvent(m => m.OnAuthorizeRequest);
            context.BeginRequest += (o, e) => FireSynchronousEvent(m => m.OnBeginRequest);
            context.Disposed += (o, e) => FireSynchronousEvent(m => m.OnDisposed);
            context.EndRequest += (o, e) => FireSynchronousEvent(m => m.OnEndRequest);
            context.Error += (o, e) => FireSynchronousEvent(m => m.OnError);
            context.LogRequest += (o, e) => FireSynchronousEvent(m => m.OnLogRequest);
            context.MapRequestHandler += (o, e) => FireSynchronousEvent(m => m.OnMapRequestHandler);
            context.PostAcquireRequestState += (o, e) => FireSynchronousEvent(m => m.OnPostAcquireRequestState);
            context.PostAuthenticateRequest += (o, e) => FireSynchronousEvent(m => m.OnPostAuthenticateRequest);
            context.PostAuthorizeRequest += (o, e) => FireSynchronousEvent(m => m.OnPostAuthorizeRequest);
            context.PostLogRequest += (o, e) => FireSynchronousEvent(m => m.OnPostLogRequest);
            context.PostMapRequestHandler += (o, e) => FireSynchronousEvent(m => m.OnPostMapRequestHandler);
            context.PostReleaseRequestState += (o, e) => FireSynchronousEvent(m => m.OnPostReleaseRequestState);
            context.PostRequestHandlerExecute += (o, e) => FireSynchronousEvent(m => m.OnPostRequestHandlerExecute);
            context.PostResolveRequestCache += (o, e) => FireSynchronousEvent(m => m.OnPostResolveRequestCache);
            context.PostUpdateRequestCache += (o, e) => FireSynchronousEvent(m => m.OnPostUpdateRequestCache);
            context.PreRequestHandlerExecute += (o, e) => FireSynchronousEvent(m => m.OnPreRequestHandlerExecute);
            context.PreSendRequestContent += (o, e) => FireSynchronousEvent(m => m.OnPreSendRequestContent);
            context.PreSendRequestHeaders += (o, e) => FireSynchronousEvent(m => m.OnPreSendRequestHeaders);
            context.ReleaseRequestState += (o, e) => FireSynchronousEvent(m => m.OnReleaseRequestState);
            context.ResolveRequestCache += (o, e) => FireSynchronousEvent(m => m.OnResolveRequestCache);
            context.UpdateRequestCache += (o, e) => FireSynchronousEvent(m => m.OnUpdateRequestCache);

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
    }
}
