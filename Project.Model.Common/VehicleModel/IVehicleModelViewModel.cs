using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public interface IVehicleModelViewModel
    {
        int Id { get; set; }
        string Name { get; set; }
        string Abrv { get; set; }
        string MakeName { get; set; }
    }
}
