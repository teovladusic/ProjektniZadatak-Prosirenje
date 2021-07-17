using AutoMapper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new VehicleMakesProfile());
                mc.AddProfile(new VehicleModelsProfile());
            });
            return mapperConfig.CreateMapper();
        }
    }
}
