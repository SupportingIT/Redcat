using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class ObserverExtensionTest
    {
        [Test]
        public void Subscribe_Adds_Observer_To_Collection()
        {
            List<IObserver<int>> observers = new List<IObserver<int>>();
            IObserver<int> observer = A.Fake<IObserver<int>>();

            observers.Subscribe(observer);

            CollectionAssert.Contains(observers, observer);
        }

        [Test]
        public void Subscribe_Creates_Disposable_Which_Can_Unsubscribe_Observer()
        {
            List<IObserver<int>> observers = new List<IObserver<int>>();
            IObserver<int> observer = A.Fake<IObserver<int>>();

            IDisposable subscription = observers.Subscribe(observer);
            Assert.That(subscription, Is.Not.Null);

            subscription.Dispose();
            CollectionAssert.DoesNotContain(observers, observer);
        }

        [Test]
        public void OnCompleted_Retransmits_Call_For_Each_Observer()
        {
            var observers = A.CollectionOfFake<IObserver<int>>(4);

            observers.OnCompleted();

            foreach (var observer in observers) A.CallTo(() => observer.OnCompleted()).MustHaveHappened();
        }

        [Test]
        public void OnError_Retransmits_Call_For_Each_Observer()
        {
            var observers = A.CollectionOfFake<IObserver<int>>(3);
            Exception error = new Exception("Test exception");

            observers.OnError(error);

            foreach (var observer in observers) A.CallTo(() => observer.OnError(error)).MustHaveHappened();
        }

        [Test]
        public void OnNext_Retransmits_Call_For_Each_Observer()
        {
            var observers = A.CollectionOfFake<IObserver<string>>(4);
            string value = "some-kind-of-value";

            observers.OnNext(value);

            foreach (var observer in observers) A.CallTo(() => observer.OnNext(value)).MustHaveHappened();
        }
    }
}
