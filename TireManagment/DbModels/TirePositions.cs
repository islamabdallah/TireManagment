using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.DbModels
{
    [Table("TireDistribution")]
    public class TireDistribution
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public TruckCategory Category { get; set; }
    }
}
