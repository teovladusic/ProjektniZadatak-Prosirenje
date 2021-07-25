using AutoMapper;
using DAL.Models;
using Model.VehicleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class VehicleModelsProfile : Profile
    {

        public VehicleModelsProfile()
        {
            CreateMap<VehicleModel, VehicleModelDomainModel>();
            CreateMap<VehicleModelDomainModel, VehicleModel>();

            CreateMap<VehicleModelViewModel, VehicleModelDomainModel>();
            CreateMap<VehicleModelDomainModel, VehicleModelViewModel>()
                .ForMember(dest => dest.MakeName,
                options => options.MapFrom(source =>
                string.Join(
                    " ",
                    source.VehicleMake.Name)));

            CreateMap<EditVehicleModelViewModel, VehicleModelDomainModel>();
            CreateMap<VehicleModelDomainModel, EditVehicleModelViewModel>();

            CreateMap<CreateVehicleModelDomainModel, CreateVehicleModelViewModel>();
            CreateMap<CreateVehicleModelViewModel, CreateVehicleModelDomainModel>();

            CreateMap<CreateVehicleModelDomainModel, VehicleModel>();
            CreateMap<VehicleModel, VehicleModel>();
        }
    }
}
