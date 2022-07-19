using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TireManagment.DbModels
{
    public class MovementDetails
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public int CurrentTireDepth { get; set; }
        public int STDthreadDepth  { get; set; }
        public string KMWhileChange { get; set; }
        [ForeignKey("tire")]
        public int TireId { get; set; }
        [ForeignKey("TireMovement")]
        public int TireMovementId { get; set; }
        public Tire tire { get; set; }
        public TireMovement  TireMovement { get; set; }
        public string TruckOdometer { get; set; }
        public string Comment { get; set; }
    }
}
