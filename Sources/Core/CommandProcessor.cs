using Redcat.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.Core
{
    public class CommandProcessor : IRunable, IDisposable
    {
        private bool initialized = false;

        protected bool IsRunning
        {
            get { return initialized; }
        }

        public void Execute<T>(T command)
        {
            foreach (var handler in GetHandlersForCommand<T>()) handler.Handle(command);
        }

        private IEnumerable<ICommandHandler<T>> GetHandlersForCommand<T>()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            if (initialized) return;            
            OnBeforeInit();
            OnInit();
            OnAfterInit();
            initialized = true;
        }

        protected virtual void OnBeforeInit()
        { }

        protected virtual void OnInit()
        { }

        protected virtual void OnAfterInit()
        { }

        public void Dispose()
        { }
    }
}
