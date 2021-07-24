using System;

namespace Project.Model.Common
{
    public interface IVehicleMakeViewModel
    {
        int Id { get; set; }
        string Name { get; set; }
        string Abrv { get; set; }
    }
}
