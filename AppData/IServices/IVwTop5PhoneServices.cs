﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace AppData.IServices
{
    public interface IVwTop5PhoneServices
    {
      List<VTop5_PhoneSell> listVwTop5PhoneGroup();
    }
}
