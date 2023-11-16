﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppData.ViewModels.Phones
{
    [Serializable]
    [Table(name: "VW_Phone_Group")]
    public class VW_Phone_Group
    {
  
        public Guid IdPhone { get; set; }
        public string PhoneName { get; set; }
        public string Image { get; set; }
        public string ProductionComanyName { get; set; }
        public string Price { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;

    }
}
