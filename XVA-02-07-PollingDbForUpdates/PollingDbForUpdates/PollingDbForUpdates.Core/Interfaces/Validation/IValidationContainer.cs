//IMPORTANT - Modifications to this file may be overwritten:
//If you need to implement your own logic/code do it in a partial class/interface.
using System.Collections.Generic;

namespace PollingDbForUpdates.Core.Interfaces.Validation
{
    public interface IValidationContainer<out T, out TA>
    {
        IDictionary<string, IList<string>> ValidationErrors { get; }
        bool IsValid { get;  }
        T Entity { get;}
        TA EntityViewModel { get; }
        void AddError(string key, string value);
        void SetViewModel();
    }
}