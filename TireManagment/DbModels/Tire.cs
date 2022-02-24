using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.DbModels
{
    [Table("Tire")]
    public class Tire
    {
        public int ID { get; set; }
        public int? TireId { get; set; }
        [Required]
        [Remote(action: "ValidateSerial", controller: "Tire")]//Remote Validation
        public string Serial { get; set; }
      
        public TireBrand Brand { get; set; }
        public int BrandId { get; set; }
        [Required]
        public string TireStatus { get; set; }
        [Required]
        public string Size { get; set; }
        public  DateTime  SubmitDate { get; set; }

    }
}
