using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Collections.Concurrent;
using Moq;
using Modularity.Test.Stubs;
using System.Collections.Specialized;

namespace Modularity.Test
{
    [TestClass]
    public class ModularityApplicationObserverTests
    {
        private ModularityModule _module;
        private Mock<ModularityModule> _moduleMock;
        private int _counter;
        private ModularityApplicationObserver _observer;

        [TestInitialize]
        public void Init()
        {
            _moduleMock = new Mock<ModularityModule>();
            _module = _moduleMock.Object;
            _counter = 0;
            ModularityApplicationObserver.GetContext = () => new Mock<HttpContextBase>().Object;
            ModularityApplicationObserver.Modules.Enqueue(_module);
            _observer = new ModularityApplicationObserver();
        }

        [TestCleanup]
        public void CleanUp()
        {
            var module = null as ModularityModule;
            while (ModularityApplicationObserver.Modules.Any())
            {
                ModularityApplicationObserver.Modules.TryDequeue(out module);
            }
            MethodHandler.ClearHandlers();
        }

        [TestMethod]
        public void InheritsIHttpModule()
        {
            var type = typeof(ModularityApplicationObserver);
            Assert.IsTrue(typeof(IHttpModule).IsAssignableFrom(type));
        }

        [TestMethod]
        public void ShouldCallModuleEvent()
        {
            _module.OnBeginRequest += (o, e) => _counter++;
            _observer.FireEventSynchronously(m => m.OnBeginRequest);
            Assert.AreEqual(1, _counter);
        }

        [TestMethod]
        public void ShouldNotCallModuleOnBeginRequestIfNotSubscribed()
        {
            _observer.FireEventSynchronously(m => m.OnBeginRequest);
            Assert.AreEqual(0, _counter);
        }

        [TestMethod]
        public void ShouldInitializeModulesFromConfiguration()
        {
            ModularityApplicationObserver.AddConfiguredModules();
            Assert.IsTrue(ModularityApplicationObserver.Modules.Any(m => m.GetType() == typeof(ModuleStub)));
        }

        [TestMethod]
        public void ShouldStartModules()
        {
			var applicationModules = new IHttpModule[]
			{
				new ModularityApplicationObserver(),
				new ModularityAsyncApplicationObserver()
			};
			_observer.InitializeModules(applicationModules);
            _moduleMock.Verify(m => m.Initialize());
        }
    }
}
