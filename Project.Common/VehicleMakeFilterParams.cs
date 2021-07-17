using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class VehicleMakeFilterParams : IFilterParams
    {
        public VehicleMakeFilterParams(SortParams sortParams, PagingParams pagingParams, string searchQuery)
        {
            SortParams = sortParams;
            PagingParams = pagingParams;
            SearchQuery = searchQuery;
        }
        public SortParams SortParams { get; set; }
        public PagingParams PagingParams { get; set; }
        public string SearchQuery { get; set; }
    }
}
