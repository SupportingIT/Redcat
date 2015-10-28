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
    }
}
