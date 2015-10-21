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
        public void Execute_Uses_Correct_CommandHandler()
        {
            CommandProcessor processor = new CommandProcessor();
            ICommandHandler<Guid> guidHandler = A.Fake<ICommandHandler<Guid>>();
            ICommandHandler<string> strHandler = A.Fake<ICommandHandler<string>>();
            processor.AddCommandHandler(guidHandler);
            processor.AddCommandHandler(strHandler);
            Guid command = Guid.NewGuid();

            processor.Execute(command);

            A.CallTo(() => guidHandler.Handle(command)).MustHaveHappened();
            A.CallTo(() => strHandler.Handle(A<string>._)).MustNotHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Execute_Throws_Exception_If_No_Handlers_For_Command()
        {
            CommandProcessor processor = new CommandProcessor();
            processor.Execute<string>("some-str");
        }

        [Test]
        public void Run_Adds_Extensions()
        {
            IServiceContainer container = A.Fake<IServiceContainer>();
            CommandProcessor processor = new TestCommandProcessor(container);
            Action<IServiceContainer> extension = A.Fake<Action<IServiceContainer>>();
            processor.AddExtension("test", extension);

            A.CallTo(() => extension.Invoke(container)).MustNotHaveHappened();
            processor.Run();

            A.CallTo(() => extension.Invoke(container)).MustHaveHappened();
        }
    }

    class TestCommandProcessor : CommandProcessor
    {
        private IServiceContainer container;
        public TestCommandProcessor(IServiceContainer container)
        {
            this.container = container;
        }

        protected override IServiceContainer CreateServiceContainer()
        {
            return container;
        }
    }
}
