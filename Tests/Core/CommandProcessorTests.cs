using FakeItEasy;
using NUnit.Framework;
using Redcat.Core.Communication;
using Redcat.Core.Service;
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
            processor.AddExtension("test", c => {
                c.TryAddSingleton(guidHandler);
                c.TryAddSingleton(strHandler);
            });
            Guid command = Guid.NewGuid();

            processor.Run();
            processor.Execute(command);

            A.CallTo(() => guidHandler.Handle(command)).MustHaveHappened();
            A.CallTo(() => strHandler.Handle(A<string>._)).MustNotHaveHappened();
        }

        [Test]
        public void Execute_Calls_All_Registered_Handlers_For_Specified_Command()
        {
            CommandProcessor processor = new CommandProcessor();
            var handlers = A.CollectionOfFake<ICommandHandler<string>>(2);
            processor.AddExtension("test", c =>
            {
                c.TryAddSingleton(handlers[0]);
                c.TryAddSingleton(handlers[1]);
            });
            string command = "Command";
            processor.Run();

            processor.Execute(command);

            A.CallTo(() => handlers[0].Handle(command)).MustHaveHappened();
            A.CallTo(() => handlers[1].Handle(command)).MustHaveHappened();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Execute_Throws_Exception_If_No_Handlers_For_Command()
        {
            CommandProcessor processor = new CommandProcessor();
            processor.Run();
            processor.Execute<string>("some-str");
        }

        [Test]
        public void Run_Adds_Extensions()
        {
            CommandProcessor processor = new CommandProcessor();
            Action<IServiceCollection> extension = A.Fake<Action<IServiceCollection>>();
            processor.AddExtension("test", extension);

            A.CallTo(() => extension.Invoke(A<IServiceCollection>._)).MustNotHaveHappened();
            processor.Run();

            A.CallTo(() => extension.Invoke(A<IServiceCollection>._)).MustHaveHappened();
        }

        [Test]
        public void Run_Initializes_Extensions_Only_Once()
        {
            CommandProcessor processor = new CommandProcessor();
            Action<IServiceCollection> extension = A.Fake<Action<IServiceCollection>>();
            processor.AddExtension("test", extension);

            processor.Run();
            processor.Run();

            A.CallTo(() => extension.Invoke(A<IServiceCollection>._)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
