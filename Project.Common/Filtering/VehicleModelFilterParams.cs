using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class VehicleModelFilterParams : IVehicleModelFilterParams
    {
        public string SearchQuery { get; set; }
        public string MakeName { get; set; }

        public VehicleModelFilterParams(string searchQuery, string makeName)
        {
            SearchQuery = searchQuery;
            MakeName = makeName;
        }
    }
}
