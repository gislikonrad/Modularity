using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections.Concurrent;
using System.Web.SessionState;

namespace Modularity
{
    public class ModularityApplicationObserver : IHttpModule
    {
        internal static readonly ConcurrentBag<Module> Modules;
        internal static Func<HttpContextBase> GetContext;

        static ModularityApplicationObserver()
        {
            Modules = new ConcurrentBag<Module>();
            AddConfiguredModules();
            GetContext = () => new HttpContextWrapper(HttpContext.Current);
        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += (o, e) => FireEvent(m => m.OnAcquireRequestState);
            context.AuthenticateRequest += (o, e) => FireEvent(m => m.OnAuthenticateRequest);
            context.AuthorizeRequest += (o, e) => FireEvent(m => m.OnAuthorizeRequest);
            context.BeginRequest += (o, e) => FireEvent(m => m.OnBeginRequest);
            context.Disposed += (o, e) => FireEvent(m => m.OnDisposed);
            context.EndRequest += (o, e) => FireEvent(m => m.OnEndRequest);
            context.Error += (o, e) => FireEvent(m => m.OnError);
            context.LogRequest += (o, e) => FireEvent(m => m.OnLogRequest);
            context.MapRequestHandler += (o, e) => FireEvent(m => m.OnMapRequestHandler);
            context.PostAcquireRequestState += (o, e) => FireEvent(m => m.OnPostAcquireRequestState);
            context.PostAuthenticateRequest += (o, e) => FireEvent(m => m.OnPostAuthenticateRequest);
            context.PostAuthorizeRequest += (o, e) => FireEvent(m => m.OnPostAuthorizeRequest);
            context.PostLogRequest += (o, e) => FireEvent(m => m.OnPostLogRequest);
            context.PostMapRequestHandler += (o, e) => FireEvent(m => m.OnPostMapRequestHandler);
            context.PostReleaseRequestState += (o, e) => FireEvent(m => m.OnPostReleaseRequestState);
            context.PostRequestHandlerExecute += (o, e) => FireEvent(m => m.OnPostRequestHandlerExecute);
            context.PostResolveRequestCache += (o, e) => FireEvent(m => m.OnPostResolveRequestCache);
            context.PostUpdateRequestCache += (o, e) => FireEvent(m => m.OnPostUpdateRequestCache);
            context.PreRequestHandlerExecute += (o, e) => FireEvent(m => m.OnPreRequestHandlerExecute);
            context.PreSendRequestContent += (o, e) => FireEvent(m => m.OnPreSendRequestContent);
            context.PreSendRequestHeaders += (o, e) => FireEvent(m => m.OnPreSendRequestHeaders);
            context.ReleaseRequestState += (o, e) => FireEvent(m => m.OnReleaseRequestState);
            context.ResolveRequestCache += (o, e) => FireEvent(m => m.OnResolveRequestCache);
            context.UpdateRequestCache += (o, e) => FireEvent(m => m.OnUpdateRequestCache);

            var session = context.Modules["Session"] as SessionStateModule;
            if (session != null)
            {
                session.Start += (o, e) => FireEvent(m => m.OnSessionStart);
                session.End += (o, e) => FireEvent(m => m.OnSessionEnd);
            }
        }

        public void Dispose()
        {
        }

        internal void FireEvent(Func<Module, RequestEventHandler> getEvent)
        {
            foreach (var eventToFire in Modules.Select(m => getEvent(m)).Where(e => e != null))
            {
                eventToFire(this, new RequestEventArgs(GetContext()));
            }
        }

        internal static void AddConfiguredModules()
        {
            if (ModularitySection.Instance != null)
            {
                foreach (ModuleElement element in ModularitySection.Instance.CustomModules)
                {
                    var type = Type.GetType(element.Type);
                    var module = Activator.CreateInstance(type) as Module;
                    if (module == null) throw new ApplicationException(string.Format("Unable to add {0}. Does it inherit the abstract class Modularity.Module?", element.Type));
                    Modules.Add(module);
                }
            }
        }
    }
}
