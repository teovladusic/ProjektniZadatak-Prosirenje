﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public interface ICreateVehicleMakeViewModel
    {
        string Name { get; set; }
        string Abrv { get; set; }
    }
}