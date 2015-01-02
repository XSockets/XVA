using System.Collections.Generic;
using System.Web.Mvc;
using PollingDbForUpdates.Core.Model;
using PollingDbForUpdates.Core.Interfaces.Service;
using PollingDbForUpdates.Core.ViewModel;
namespace PollingDbForUpdates.Controllers
{
    public partial class SalesController : BaseController<Sales, SalesViewModel>
    {
        protected ISalesService SalesService;

        public SalesController(ISalesService salesService)
            : base()
        {
            base.Service = this.SalesService = salesService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Update(IList<SalesViewModel> sales)
        {
            foreach (var salesViewModel in sales)
            {
                var entity = this.SalesService.GetById(salesViewModel.Id);
                if (entity != null)
                {
                    entity.Hardware = salesViewModel.Hardware;
                    entity.Software = salesViewModel.Software;
                    entity.Services = salesViewModel.Services;
                    this.SalesService.SaveOrUpdate(entity);
                }
            }
            return new JsonResult { Data = new { Status = "Ok" } };
        }
    }
}