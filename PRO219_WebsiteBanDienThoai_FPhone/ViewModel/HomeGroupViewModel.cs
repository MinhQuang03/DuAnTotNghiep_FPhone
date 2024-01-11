﻿using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class HomeGroupViewModel
    {
        public List<VTop5_PhoneSell> vTop5 { get; set; } = new List<VTop5_PhoneSell>();
        public List<VW_Phone_Group> vPhoneGroup { get; set; } = new List<VW_Phone_Group>();

        public VW_Phone_Group _VW_Phone_Group { get; set; }
    }
}
