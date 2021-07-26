using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.VehicleMakes
{
    public class CreateVehicleMakeDomainModel
    {
        public string Name { get; set; }
        public string Abrv { get; set; }

        public bool IsValid()
        {
            bool isValid = !string.IsNullOrEmpty(Name.Trim())
                && !string.IsNullOrEmpty(Abrv.Trim());

            return isValid;
        }
    }
}
