using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.DbModels
{
    public class TireMovement
    {
        public int Id { get; set; }
        [ForeignKey("Tireman")]
        public string TireManId { get; set; }
        public string TruckNumber { get; set; }
        public DateTime SubmitDate { get; set; }
        public string MovementType { get; set; }
        [DefaultValue(true)]
        public bool IsRead { get; set; }
        public AppUser Tireman { get; set; }
       
        public List<MovementDetails> MovementDetails { get; set; }

    }
}
