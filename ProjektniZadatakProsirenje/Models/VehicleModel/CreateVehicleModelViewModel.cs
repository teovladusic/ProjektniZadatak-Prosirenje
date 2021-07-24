using Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CreateVehicleModelViewModel : ICreateVehicleModelViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
        [Required]
        public int VehicleMakeId { get; set; }
    }
}
