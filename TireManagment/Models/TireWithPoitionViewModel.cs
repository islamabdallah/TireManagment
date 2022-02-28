using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.Models
{
    public class TireWithPoitionViewModel
    {
        public int TireId  { get; set; }
        public string Position { get; set; }
        public int CurrentTireDepth { get; set; }
        public int STDThreadDepth { get; set; }
        public string KMWhileChange { get; set; }

    }
}
