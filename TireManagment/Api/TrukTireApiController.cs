using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.Hubs;
using TireManagment.Services;

namespace TireManagment.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrukTireApiController : ControllerBase
    {
        TruckService truckService;
        CategoryService categoryService;
        TruckTireService TruckTireService;
        IHubContext<notifyHub> hubContext;
        public TrukTireApiController(IHubContext<notifyHub> _hubContext,TruckTireService _truckTireService, TruckService truck, CategoryService _categoryService)
        {
            TruckTireService = _truckTireService;
            truckService = truck;
            categoryService = _categoryService;
            hubContext = _hubContext;

        }
        [HttpGet]
        public IActionResult GetTruckTires(string TruckNumber)
        {
           
            var res = truckService.GetTruckTires(TruckNumber);
            return Ok(res);
        }
        [HttpPost]
        public IActionResult AddNewMovement(Models.TruckMovementViewModel truckMovement)
        {

           bool success= TruckTireService.AddNewMovement(truckMovement);
            if (success)
            {
             
                return Ok("Transaction Commited Succesfully");
            }
            return BadRequest("Transaction didn't complete successfully");
                
        }
    }
}
