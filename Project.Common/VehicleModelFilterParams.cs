using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class VehicleModelFilterParams : IFilterParams
    {
        public PagingParams PagingParams { get; set; }
        public SortParams SortParams { get; set; }
        public string SearchQuery { get; set; }
        public string MakeName { get; set; }

        public VehicleModelFilterParams(PagingParams pagingParams, SortParams sortParams, string searchQuery,
            string makeName)
        {
            PagingParams = pagingParams;
            SortParams = sortParams;
            SearchQuery = searchQuery;
            MakeName = makeName;
        }
    }
}
