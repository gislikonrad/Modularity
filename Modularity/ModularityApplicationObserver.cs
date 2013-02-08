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

        public override void Dispose()
        {
        }
	}
}
