using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Collections.Concurrent;
using Moq;

namespace Modularity.Test
{
    [TestClass]
    public class ModularityApplicationObserverTests
    {
        private Module _module;
        private Mock<Module> _moduleMock;
        private int _counter;
        private ModularityApplicationObserver _observer;

        [TestInitialize]
        public void Init()
        {
            _moduleMock = new Mock<Module>();
            _module = _moduleMock.Object;
            _counter = 0;
            ModularityApplicationObserver.GetContext = () => new Mock<HttpContextBase>().Object;
            ModularityApplicationObserver.Modules.Add(_module);
            _observer = new ModularityApplicationObserver();
        }

        [TestCleanup]
        public void CleanUp()
        {
            var module = null as Module;
            while (ModularityApplicationObserver.Modules.Any())
            {
                ModularityApplicationObserver.Modules.TryTake(out module);
            }
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
            _observer.FireEvent(m => m.OnBeginRequest);
            Assert.AreEqual(1, _counter);
        }

        [TestMethod]
        public void ShouldNotCallModuleOnBeginRequestIfNotSubscribed()
        {
            _observer.FireEvent(m => m.OnBeginRequest);
            Assert.AreEqual(0, _counter);
        }

        [TestMethod]
        public void ShouldInitializeModulesFromConfiguration()
        {
            ModularityApplicationObserver.AddConfiguredModules();
            Assert.IsTrue(ModularityApplicationObserver.Modules.Any(m => m.GetType() == typeof(ModuleStub)));
        }
    }

    public class ModuleStub : Module { }
}
