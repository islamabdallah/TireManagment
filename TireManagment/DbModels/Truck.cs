using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.DbModels
{
    [Table("Truck")]
    public class Truck
    {
        public int ID { get; set; }
        public int? TruckId { get; set; }
        [Required]
        public string TruckName { get; set; }
        [Required]
        [Remote(action: "ValidateTruckNumber", controller: "Truck")]//Remote Validation
     
        public string TruckNumber { get; set; }
        [Required]
        public string Company { get; set; }
        [Required]
        public int Year { get; set; }
    

      
        public string Status { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public string Unit { get; set; }
      
        public DateTime SubmitDate { get; set; }

        public bool Active { get; set; } = true;
        public string Chassis { get; set; }
        [Required]
        public string Engine { get; set; }
        [Required]
        public string Registeration { get; set; }
        [Required]
        public int AxleCount { get; set; }
        [Required]
        public string VehichleModelNo { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public TruckCategory  Category { get; set; }
    }
}
