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
    public class TireApiController : ControllerBase
    {
        TruckService TruckService;
        //CategoryService CategoryService;
        TruckTireService TruckTireService;
        // IHubContext<notifyHub> HubContext;
        TireService TireService;

        public TireApiController(IHubContext<notifyHub> hubContext,TruckTireService truckTireService, TruckService truck, CategoryService categoryService , TireService tireService)
        {
            TruckTireService = truckTireService;
            TruckService = truck;
          //  CategoryService = categoryService;
           // HubContext = hubContext;
            TireService =  tireService;
        }

        [HttpGet("TiresByTruck")]
        public IActionResult GetTruckTires(string truckNumber)
        {
            if(!string.IsNullOrEmpty(truckNumber))
            {
                var _result = TruckService.GetTruckTires(truckNumber);
                var _newTires = TireService.GetNewTires();
                if (_result != null)
                {
                    return Ok(new { Flag = true, Message = "Done", TruckTire = _result , NewTires = _newTires });
                }
                return BadRequest(new { Flag = false, Message = "Error", Data = 0 });
            }
            return BadRequest(new { Flag = false, Message = "Error", Data = 0 });
        }

        [HttpPost("TireMovement")]
        public IActionResult AddNewMovement(Models.TruckMovementViewModel tireMovement)
        {
            if(tireMovement != null)
            {
                bool _result = TruckTireService.AddNewMovement(tireMovement);
                if(_result)
                    return Ok(new { Flag = true, Message = "Movement done successfully", Data = _result });

                return BadRequest(new { Flag = false, Message = "Error in updating the truck tires", Data = 0 });
            }
            return BadRequest(new { Flag = false, Message = "Error in updating the truck tires", Data = 0 });
        }
    }
}
