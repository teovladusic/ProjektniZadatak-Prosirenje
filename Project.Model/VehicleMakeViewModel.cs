﻿using Project.Model.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Project.Model
{
    public class VehicleMakeViewModel : IVehicleMakeViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
    }
}
