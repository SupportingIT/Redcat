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
