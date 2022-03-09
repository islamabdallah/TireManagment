using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.Services;

namespace TireManagment.Controllers
{
    public class TireMovementController : Controller
    {
        TruckTireService trucktireservice;
        public TireMovementController(TruckTireService _truckTireService)
        {
            trucktireservice = _truckTireService;
        }
        public IActionResult GetMovementDetail(int id)
        {
          var movement=trucktireservice.GetMovementDetail(id);
            movement.IsRead = true;
            trucktireservice.UpdateMovement(movement);
          string categoryImage=trucktireservice.GetCategoryImage(movement.TruckNumber);
            ViewBag.categoryImage = categoryImage;
            return View("MovementDetail",movement);
        }
        public IActionResult GetAll()
        {
            var movements = trucktireservice.GetAllMovements();
            
            return View("AllMovements", movements);
        }
    }
}
