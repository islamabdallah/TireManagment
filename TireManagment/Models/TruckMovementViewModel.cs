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
        public int CurrentTireDepth { get; set; }
        public int STDthreadDepth { get; set; }
        public List <TireWithPoitionViewModel> tireWithPoitionViewModels { get; set; }



    }
}
