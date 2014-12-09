using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using XSockets.Core.Common.Socket;
using XSockets.Core.Common.Utility.Logging;
using XSockets.Plugin.Framework;
using XSocketsWorker.ConfigurationHelper;

namespace XSocketsWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private IXSocketServerContainer _container;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {            
            try
            {
                this.RunAsync(this._cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this._runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {         
            bool result = base.OnStart();

            // Use the XSockets.NET Plugin Framwork to get an IXSocketServerContainer
            _container = Composable.GetExport<IXSocketServerContainer>();

            //Start server by using extension (see ConfigurationHelper)
            _container.StartOnAzure();
            
            return result;
        }

        public override void OnStop()
        {
            _container.Stop();

            this._cancellationTokenSource.Cancel();
            this._runCompleteEvent.WaitOne();

            base.OnStop();
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(3000);
            }
        }
    }
}
