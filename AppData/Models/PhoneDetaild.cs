namespace AppData.Models
{
    public class PhoneDetaild
    {
        public Guid Id { get; set; }

        public Guid IdPhone { get; set; }

        public string? Code { get; set; }

        public Guid? IdDiscount { get; set; }

        public Guid IdMaterial { get; set; }

        public Guid IdRom { get; set; }

        public Guid IdRam { get; set; }

        public Guid IdOperatingSystem { get; set; }

        public Guid IdBattery { get; set; }

        public Guid IdSim { get; set; }

        public Guid IdChipCPU { get; set; }

        public Guid IdChipGPU { get; set; }

        public Guid IdColor { get; set; }

        public Guid IdChargingport { get; set; }

        public string? Weight { get; set; }

        public string? FrontCamera { get; set; }

        public string? MainCamera { get; set; }

        public int? Resolution { get; set; }

        public string? Size { get; set; }

        public decimal Price { get; set; }

        
        public decimal? Sale { get; set; }

        public int? Solid { get; set; }
        public int Status { get; set; }
       

        public virtual Ram? Rams { get; set; }

        public virtual Material? Materials { get; set; }

        public virtual Rom? Roms { get; set; }

        public virtual OperatingSystems? OperatingSystems { get; set; }

        public virtual Battery? Batteries { get; set; }

        public virtual Sim? Sims { get; set; }

        public virtual ChargingportType? ChargingportTypes { get; set; }

        public virtual Phone? Phones { get; set; }

        public virtual ChipCPUs? ChipCPUs { get; set; }

        public virtual ChipGPUs? ChipGPUs { get; set; }

        public virtual Color? Colors { get; set; }

        public virtual Discount? Discounts { get; set; }


    }
}
