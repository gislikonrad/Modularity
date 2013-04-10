Modularity
==========

Create modules for asp.net using a base class that helps you subscribe to application events easier than before and in a unit testable manner.

Sample
======

	ModularityModule
	================

	namespace YourApplication.NameSpace
	{
		public class DebugOutputModule : ModularityModule
		{
			public override void Initialize()
			{	
				OnBeginRequest += (o, e) => { Debug.WriteLine("OnBeginRequest"); };
				OnAcquireRequestState += (o, e) => { Debug.WriteLine("OnAcquireRequestState"); };
				OnAuthenticateRequest += (o, e) => { Debug.WriteLine("OnAuthenticateRequest"); };
				OnAuthorizeRequest += (o, e) => { Debug.WriteLine("OnAuthorizeRequest"); };
				OnDisposed += (o, e) => { Debug.WriteLine("OnDisposed"); };
				OnEndRequest += (o, e) => { Debug.WriteLine("OnEndRequest"); };
				OnError += (o, e) => { Debug.WriteLine("OnError"); };
				OnLogRequest += (o, e) => { Debug.WriteLine("OnLogRequest"); };
				OnMapRequestHandler += (o, e) => { Debug.WriteLine("OnMapRequestHandler"); };
				OnPostAcquireRequestState += (o, e) => { Debug.WriteLine("OnPostAcquireRequestState"); };
				OnPostAuthenticateRequest += (o, e) => { Debug.WriteLine("OnPostAuthenticateRequest"); };
				OnPostAuthorizeRequest += (o, e) => { Debug.WriteLine("OnPostAuthorizeRequest"); };
				OnPostLogRequest += (o, e) => { Debug.WriteLine("OnPostLogRequest"); };
				OnPostMapRequestHandler += (o, e) => { Debug.WriteLine("OnPostMapRequestHandler"); };
				OnPostReleaseRequestState += (o, e) => { Debug.WriteLine("OnPostReleaseRequestState"); };
				OnPostRequestHandlerExecute += (o, e) => { Debug.WriteLine("OnPostRequestHandlerExecute"); };
				OnPostResolveRequestCache += (o, e) => { Debug.WriteLine("OnPostResolveRequestCache"); };
				OnPostUpdateRequestCache += (o, e) => { Debug.WriteLine("OnPostUpdateRequestCache"); };
				OnPreRequestHandlerExecute += (o, e) => { Debug.WriteLine("OnPreRequestHandlerExecute"); };
				OnPreSendRequestContent += (o, e) => { Debug.WriteLine("OnPreSendRequestContent"); };
				OnPreSendRequestHeaders += (o, e) => { Debug.WriteLine("OnPreSendRequestHeaders"); };
				OnReleaseRequestState += (o, e) => { Debug.WriteLine("OnReleaseRequestState"); };
				OnResolveRequestCache += (o, e) => { Debug.WriteLine("OnResolveRequestCache"); };
				OnSessionEnd += (o, e) => { Debug.WriteLine("OnSessionEnd"); };
				OnSessionStart += (o, e) => { Debug.WriteLine("OnSessionStart"); };
				OnUpdateRequestCache += (o, e) => { Debug.WriteLine("OnUpdateRequestCache"); };
			}

			public override bool IsAsync
			{
				get { return true; }
			}
		}
	}
	
	Web.config
	==========
	<configuration>
	  <configSections>
		<section name="modularity" type="Modularity.ModularitySection, Modularity, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null"/>
	  </configSections>
	  <modularity xmlns="urn:Modularity"
		  		  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
		<customModules>
			<add type="YourApplication.NameSpace.DebugeOutputModule, YourApplication" />
		</customModules>
	  </modularity>
	  <system.web>
		<httpModules>
		  <add name="ModularityApplicationObserver" type="Modularity.ModularityApplicationObserver, Modularity" />
		</httpModules>
	  </system.web> 
	  <system.webServer>
		<modules runAllManagedModulesForAllRequests="true">
		  <add name="ModularityApplicationObserver" type="Modularity.ModularityApplicationObserver, Modularity" preCondition="integratedMode,runtimeVersionv4.0" />
		</modules>
	  </system.webServer>
	</configuration>