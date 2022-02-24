 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.DbModels
{
    [Table("TruckCategory")]
    public class TruckCategory
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int TireCount { get; set; }
        public int FrontTires { get; set; }
        public int RareTires { get; set; }
        public int SpareTires { get; set; }
    
     
        public bool Active { get; set; }
      
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? SubmitDate { get; set; } 
        public List<Truck> trucks { get; set; }
        public List<TireDistribution>  Distributions { get; set; }
    }
}
