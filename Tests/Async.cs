using NUnit.Framework;
using System;
using System.Threading;
using Q.Web;

namespace Q.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    //Test for parallel execution
    public class Async
    {       
        [Test]        
        public void TestOne()
        {
            Console.WriteLine($"TestOne started on thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(4000); // Simulate some work
            Console.WriteLine($"TestOne finished on thread {Thread.CurrentThread.ManagedThreadId}");
        }

        [Test]                    
        public void TestTwo()
        {
            Console.WriteLine($"TestTwo started on thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(4000); // Simulate some work
            Console.WriteLine($"TestTwo finished on thread {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}