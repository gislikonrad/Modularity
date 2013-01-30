using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Modularity.Test
{
    [TestClass]
    public class MethodHandlerTests
    {
        [TestCleanup]
        public void CleanUp()
        {
            MethodHandler.ClearHandlers();
        }

        [TestMethod]
        public void ShouldRunOnce()
        {
            var counter = 0;
            Action @delegate = () =>
                {
                    counter++;
                };

            MethodHandler.RunOnce(@delegate);
            MethodHandler.RunOnce(@delegate);

            Assert.AreEqual(1, counter);
        }

        [TestMethod]
        public void ShouldRunTwoMethodsOnceEach()
        {
            var counter1 = 0;
            Action @delegate1 = () =>
            {
                counter1++;
            };
            MethodHandler.RunOnce(@delegate1);

            var counter2 = 0;
            Action @delegate2 = () =>
            {
                counter2++;
            };
            MethodHandler.RunOnce(@delegate2);


            MethodHandler.RunOnce(@delegate1);
            MethodHandler.RunOnce(@delegate2);

            Assert.AreEqual(1, counter1);
            Assert.AreEqual(1, counter2);
        }

        [TestMethod]
        public void ShouldRunTwoMethodsOnceEachOnSeperateThreads()
        {
            var counter1 = 0;
            var counter2 = 0;
            var random = new Random();
            Action @delegate1 = () =>
            {
                counter1++;
                Thread.Sleep(random.Next(500, 2500));
            };
            Action @delegate2 = () =>
            {
                counter2++;
                Thread.Sleep(random.Next(500, 2500));
            };

            var thread1 = new Thread(() => MethodHandler.RunOnce(@delegate1));
            var thread2 = new Thread(() => MethodHandler.RunOnce(@delegate2));
            var thread3 = new Thread(() => MethodHandler.RunOnce(@delegate1));
            var thread4 = new Thread(() => MethodHandler.RunOnce(@delegate2));

            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            
            thread1.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();

            Assert.AreEqual(1, counter1);
            Assert.AreEqual(1, counter2);
        }
    }
}
