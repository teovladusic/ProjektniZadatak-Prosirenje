using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class VehicleModel : BaseModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Abrv { get; set; }

        [Required]
        public virtual VehicleMake VehicleMake { get; set; }

        [Required]
        public int VehicleMakeId { get; set; }
    }
}
