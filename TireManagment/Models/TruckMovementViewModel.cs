using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.Models
{
    public class TruckMovementViewModel
    {
        public DateTime SubmitDate { get; set; }
        public string TruckNumber { get; set; }
        public string UserId { get; set; }
        public string MovementType  { get; set; }
        public string TruckOdometer { get; set; }
        public List <TirePositionViewModel> TirePositionViewModel { get; set; }
    }
}
