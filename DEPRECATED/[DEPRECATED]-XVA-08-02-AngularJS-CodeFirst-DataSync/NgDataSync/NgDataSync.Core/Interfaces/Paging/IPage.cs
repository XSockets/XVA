//IMPORTANT - Modifications to this file may be overwritten:
//If you need to implement your own logic/code do it in a partial class/interface.
using System.Collections.Generic;

namespace NgDataSync.Core.Interfaces.Paging
{
    public interface IPage<T>
    {
        int CurrentPage { get; set; }
        int PagesCount { get; set; }
        int PageSize { get; set; }
        int Count { get; set; }
        IEnumerable<T> Entities { get; set; }
    }
}