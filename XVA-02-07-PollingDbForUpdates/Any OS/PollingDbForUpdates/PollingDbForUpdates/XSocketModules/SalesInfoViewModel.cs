using System.Collections.Generic;
using PollingDbForUpdates.Core.ViewModel;

namespace PollingDbForUpdates.XSocketModules
{
    public class SalesInfoViewModel
    {
        public IList<SalesViewModel> Sales { get; set; }
        public string Updated { get; set; }

        public SalesInfoViewModel()
        {
            this.Sales = new List<SalesViewModel>();
        }
        public SalesInfoViewModel(IList<SalesViewModel> salesViewModels, string updated)
        {
            this.Sales = salesViewModels;
            this.Updated = updated;
        }
    }
}