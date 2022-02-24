using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.Models
{
    public class TruckViewModel
    {
        
        public int TruckId { get; set; }
        public string TruckName { get; set; }
        public string TruckNumber { get; set; }
        public string TruckCompany { get; set; }
        public int TruckYear { get; set; }
        public string Status { get; set; }
        public string Manufacturer { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Unit { get; set; }
    

        public bool Active { get; set; } = true;
        public string Chassis { get; set; }
        public string Engine { get; set; }
        public string Registeration { get; set; }
        public int AxleCount { get; set; }
        public string VehichleModelNo { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
    }

}
