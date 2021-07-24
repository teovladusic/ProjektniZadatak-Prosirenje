using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class VehicleMakeFilterParams : IVehicleMakeFilterParams
    {
        public string SearchQuery { get; set; }

        public VehicleMakeFilterParams(string searchQuery)
        {
            SearchQuery = searchQuery;
        }
    }
}
