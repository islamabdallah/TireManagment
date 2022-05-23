using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.Models
{
    public class TruckTireViewModel
    {
        public int Id { get; set; }
        public int? TireId { get; set; }
        public string Tirebrand { get; set; }
        public string TireSerial { get; set; }
        public DateTime?  LastUpdateTime { get; set; }
        public string TirePosition { get; set; }
        public string TruckNumber { get; set; }
        public string TireStatus { get; set; }
        public string TireSize { get; set; }
    }
}
