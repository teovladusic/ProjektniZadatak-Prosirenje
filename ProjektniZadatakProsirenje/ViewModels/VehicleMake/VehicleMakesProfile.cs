using AutoMapper;
using DAL.Models;
using Model.VehicleMakes;
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
            CreateMap<VehicleMakeDomainModel, VehicleMake>();
            CreateMap<VehicleMake, VehicleMakeDomainModel>();

            CreateMap<VehicleMakeDomainModel, VehicleMakeViewModel>();
            CreateMap<VehicleMakeViewModel, VehicleMakeDomainModel>();

            CreateMap<CreateVehicleMakeViewModel, CreateVehicleMakeDomainModel>();
            CreateMap<CreateVehicleMakeDomainModel, CreateVehicleMakeViewModel>();

            CreateMap<CreateVehicleMakeDomainModel, VehicleMake>();
            CreateMap<VehicleMake, CreateVehicleMakeDomainModel>();
        }
    }
}
