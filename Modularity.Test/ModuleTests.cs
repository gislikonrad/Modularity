using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modularity.Test.Stubs;

namespace Modularity.Test
{
    [TestClass]
    public class ModuleTests
    {
        [TestCleanup]
        public void CleanUp()
        {
            MethodHandler.ClearHandlers();
        }

        [TestMethod]
        public void ShouldUseThreadTrapInModuleInternalStart()
        {
            var module = new ModuleStub();
            module.InternalInitialize();
            module.InternalInitialize();

            Assert.AreEqual(1, module.Counter);
        }
    }
}
