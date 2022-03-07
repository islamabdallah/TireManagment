using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.DbModels
{
    public class TruckTire
    {
        public int Id { get; set; }
        public string TruckNumber { get; set; }

        public DateTime? LastUdateDate { get; set; }
        [ForeignKey("tire")]
        public int? TireId { get; set; }
        public Tire tire  { get; set; }
        
        public string TirePosition { get; set; }

    }
}
