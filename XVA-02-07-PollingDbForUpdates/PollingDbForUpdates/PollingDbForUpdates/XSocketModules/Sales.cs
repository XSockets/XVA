using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using PollingDbForUpdates.Core.Interfaces.Service;
using PollingDbForUpdates.Core.ViewModel;
using PollingDbForUpdates.NinjectModules;
using XSockets.Core.XSocket;
using XSockets.Core.XSocket.Helpers;
using System.Threading.Tasks;

namespace PollingDbForUpdates.XSocketModules
{
    /// <summary>
    /// Implement/Override your custom actionmethods, events etc in this real-time MVC controller
    /// </summary>
    public class Sales : XSocketController
    {
        //Ninject
        private static IKernel kernel;

        static Sales()
        {
            //Create the kernel once
            kernel = new StandardKernel(new ServiceModule());
        }
        
        public override async Task OnOpened()
        {
            await this.GetSales();
        }

        /// <summary>
        /// Send the sales to the client asking for it
        /// </summary>
        public async Task GetSales()
        {
            try
            {
                var service = kernel.Get<ISalesService>();
                var salesData = service.GetAllReadOnly().ToList();

                var salesInfo = new SalesInfoViewModel(salesData.Select(sales => new SalesViewModel(sales)).ToList(), salesData.OrderByDescending(p => p.Updated).Select(p => p.Updated).First());
                await this.Invoke(salesInfo, "sales-updated");
            }
            catch (Exception ex)
            {
                await this.InvokeError(ex, "Error in SalesController.GetSales");
            }
        }

        /// <summary>
        /// Send the sales to everyone!
        /// </summary>
        /// <param name="salesInfo"></param>
        public async Task SalesUpdated(SalesInfoViewModel salesInfo)
        {
            try
            {
                await this.InvokeToAll(salesInfo, "sales-updated");
            }
            catch (Exception ex)
            {
                await this.InvokeError(ex, "Error in SalesController.SalesUpdated");
            }
        }

        /// <summary>
        /// Update the database with new values...
        /// </summary>
        /// <param name="sales"></param>
        public async Task UpdateSales(IList<SalesViewModel> sales)
        {
            try
            {
                var service = kernel.Get<ISalesService>();

                foreach (var salesViewModel in sales)
                {
                    var entity = service.GetById(salesViewModel.Id);
                    if (entity != null)
                    {
                        entity.Hardware = salesViewModel.Hardware;
                        entity.Software = salesViewModel.Software;
                        entity.Services = salesViewModel.Services;
                        service.SaveOrUpdate(entity);
                    }
                }
                await this.SalesUpdated(new SalesInfoViewModel(sales, DateTime.Now.ToString()));

            }
            catch (Exception ex)
            {
                await this.InvokeError(ex, "Error in SalesController.UpdateSales");
            }
        }
    }
}
