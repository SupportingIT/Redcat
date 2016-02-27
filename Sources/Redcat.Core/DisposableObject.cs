using System;

namespace Redcat.Core
{
    public class DisposableObject : IDisposable
    {
        private bool disposed;

        protected DisposableObject()
        {
            disposed = false;
        }

        public bool IsDisposed => disposed;        

        protected void ThrowIfDisposed(string objectName)
        {
            if (disposed) throw new ObjectDisposedException(objectName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                DisposeManagedResources();
            }

            DisposeUnmanagedResources();

            disposed = true;
        }

        protected virtual void DisposeManagedResources()
        { }

        protected virtual void DisposeUnmanagedResources()
        { }
    }

    public static class DisposableExtensions
    {
        public static void DisposeIfDisposable(this object obj)
        {
            if (obj is IDisposable) ((IDisposable)obj).Dispose();
        }
    }
}
