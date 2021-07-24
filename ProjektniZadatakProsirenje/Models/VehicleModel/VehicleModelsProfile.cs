using AutoMapper;
using DAL.Models;
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
            CreateMap<CreateVehicleModelViewModel, VehicleModel>();
            CreateMap<VehicleModel, CreateVehicleMakeViewModel>();

            CreateMap<EditVehicleModelViewModel, VehicleModel>();

            CreateMap<VehicleModelViewModel, VehicleModel>();
            CreateMap<VehicleModel, VehicleModelViewModel>()
                .ForMember(
                dest => dest.MakeName,
                options => options.MapFrom(source =>
                string.Join(
                    " ",
                    source.VehicleMake.Name)));
        }
    }
}
