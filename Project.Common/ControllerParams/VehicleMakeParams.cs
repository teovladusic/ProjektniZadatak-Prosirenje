using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    //model for getting parameters in controller
    public class VehicleMakeParams : Params
    {
        public string SearchQuery { get; set; }
    }
}
