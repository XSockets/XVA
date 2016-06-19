using System;
using System.Linq;
using System.Timers;
using Ninject;
using PollingDbForUpdates.Core.Interfaces.Service;
using PollingDbForUpdates.Core.ViewModel;
using PollingDbForUpdates.NinjectModules;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using XSockets.Plugin.Framework;
using XSockets.Plugin.Framework.Attributes;

namespace PollingDbForUpdates.XSocketModules
{
    /// <summary>
    /// This is a singleton in XSockets, only one instance of this class will exist
    /// </summary>
    [XSocketMetadata("DatabasePolling", PluginRange.Internal)]
    public class DatabasePolling : XSocketController
    {
        private readonly Timer timer;
        private DateTime latestUpdate { get; set; }

        private IKernel kernel;

        public DatabasePolling()
        {
            //Create the kernel once            
            kernel = new StandardKernel(new ServiceModule());

            //First time we want data so set time to future...
            latestUpdate = DateTime.Now.AddHours(1);

            timer = new Timer(6000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        //Ticks every X seconds...
        //Ugly but still better that one client is doing this than every client connected to your web (hundred?/thousands?)
        //
        //The important thing in this POC is how this can be done...
        //In this example I just check the latest update in the result of my query to know if data is changed.
        //You would probably do it in another (nicer) way in your solution
        // Using MSSQL cache dependency is one way, but not covered here.
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var service = kernel.Get<ISalesService>();
                //We will be using GetAllReadOnly to get fresh data since this uses "AsNoTracking" in EF                                       
                var salesData = service.GetAllReadOnly().ToList();

                //Check if there where any updates... since last check
                var updated =
                    Convert.ToDateTime(salesData.OrderByDescending(p => p.Updated).Select(p => p.Updated).First());
                if (latestUpdate > DateTime.Now)
                    latestUpdate = updated.AddSeconds(-1);

                var updatedSales = salesData.Select(sales => new SalesViewModel(sales)).ToList();

                if (updatedSales.Count > 0 && latestUpdate < updated)
                {
                    latestUpdate = updated;
                    //Send the data to all clients connected to the sales controller
                    this.InvokeToAll<Sales>(
                        new SalesInfoViewModel(updatedSales,
                            updatedSales.OrderByDescending(p => p.Updated).Select(p => p.Updated).First()),
                        "Sales-Updated");
                }
            }
            catch
            {

            }
        }
    }
}
