using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.VehicleModels
{
    public class CreateVehicleModelDomainModel
    {
        public string Name { get; set; }
        public string Abrv { get; set; }
        public int VehicleMakeId { get; set; }

        public bool IsValid()
        {
            bool isValid =
                !string.IsNullOrEmpty(Name.Trim()) &&
                !string.IsNullOrEmpty(Abrv.Trim()) &&
                VehicleMakeId > 0;

            return isValid;
        }
    }
}
