using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class VehicleMakeFilterParams : IVehicleMakeFilterParams
    {
        public ISortParams SortParams { get; set; }
        public IPagingParams PagingParams { get; set; }
        public string SearchQuery { get; set; }
    }
}
