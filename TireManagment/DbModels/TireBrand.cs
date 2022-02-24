﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.DbModels
{
    [Table("TireBrand")]
    public class TireBrand
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Tire> Tires { get; set; }
    }
}
