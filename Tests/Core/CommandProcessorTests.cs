using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Services;
using System;

namespace Redcat.Core.Tests
{
    [TestFixture]
    public class CommandProcessorTests
    {
        [Test]
        public void Run_Attaches_All_Extensions()
        {
            CommandProcessor processor = new CommandProcessor();
            var extensions = A.CollectionOfFake<IKernelExtension>(4);
            processor.AddExtensions(extensions);

            processor.Run();

            foreach (var extension in extensions) A.CallTo(() => extension.Attach(A<IKernel>._)).MustHaveHappened();
        }

        [Test]
        public void Run_Attaches_Extensions_Only_Once()
        {
            CommandProcessor processor = new CommandProcessor();
            var extension = A.Fake<IKernelExtension>();
            processor.AddExtension(extension);

            processor.Run();
            processor.Run();

            A.CallTo(() => extension.Attach(A<IKernel>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Execute_Calls_Correct_CommandHandler()
        {
            CommandProcessor processor = new CommandProcessor();
            ICommandHandler<string> handler = A.Fake<ICommandHandler<string>>();
            IKernelExtension extension = CreateExtension(handler);
            processor.AddExtension(extension);
            processor.Run();

            processor.Execute("my-command");

            A.CallTo(() => handler.Handle("my-command")).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Execute_Throws_Exception_If_No_CommandHandler_For_Command()
        {
            CommandProcessor processor = new CommandProcessor();
            ICommandHandler<string> handler = A.Fake<ICommandHandler<string>>();
            IKernelExtension extension = CreateExtension(handler);
            processor.AddExtension(extension);
            processor.Run();

            processor.Execute(1);
        }

        private IKernelExtension CreateExtension<T>(ICommandHandler<T> handler)
        {
            IServiceProvider provider = A.Fake<IServiceProvider>();
            A.CallTo(() => provider.GetService(typeof(ICommandHandler<T>))).Returns(handler);
            IKernelExtension extension = A.Fake<IKernelExtension>();
            A.CallTo(() => extension.Attach(A<Kernel>._)).Invokes(c => ((Kernel)c.Arguments[0]).AddServiceProvider(provider));
            return extension;
        }
    }
}
