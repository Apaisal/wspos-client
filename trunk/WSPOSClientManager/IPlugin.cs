using System;
namespace WSPOS.Client
{
    public interface IPlugin : IDisposable
    {
        protected void Start();

        protected void Stop();
    }
}
