using AutoMapper;
using DAL.Models;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class VehicleMakesProfile : Profile
    {

        public VehicleMakesProfile()
        {
            CreateMap<CreateVehicleMakeViewModel, VehicleMake>();

            CreateMap<VehicleMakeViewModel, VehicleMake>();
            CreateMap<VehicleMake, VehicleMakeViewModel>();
        }
    }
}
