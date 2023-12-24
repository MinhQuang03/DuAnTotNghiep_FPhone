﻿using System.Collections;
using AppData.Models;
using AppData.ViewModels.Options;
using AppData.ViewModels.Phones;

namespace PRO219_WebsiteBanDienThoai_FPhone.ViewModel
{
    public class AdPhoneDetailViewModel
    {
        public List<Material> listMaterial { get; set; } = new List<Material>();
        public List<ChipCPUs> listChipCPU { get; set; } = new List<ChipCPUs>();
        public List<Ram> listRam { get; set; } = new List<Ram>();
        public List<Rom> listRom { get; set; } = new List<Rom>();

        public List<VW_PhoneDetail> ListVwPhoneDetail { get; set; } = new List<VW_PhoneDetail>();
        public VW_PhoneDetail SearchData { get; set; } = new VW_PhoneDetail();
        public ListOptions ListOptions { get; set; } = new ListOptions();
      //Edit
      public Phone PhoneDetail { get; set; } = new Phone();
      public IFormFile file { get; set; }
    }
}
