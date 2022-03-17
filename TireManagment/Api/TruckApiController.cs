using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.Services;

namespace TireManagment.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckApiController : ControllerBase
    {
        TruckService TruckService;
        CategoryService CategoryService;

        public TruckApiController(TruckService truck, CategoryService _categoryService)
        {
            TruckService = truck;
            CategoryService = _categoryService;

        }

        [HttpGet("TruckList")]
        public async Task<IActionResult> Trucks()
        {
            var _trucks =await TruckService.GetAll();
            if (_trucks != null)
                //return Ok(new { Flag = true, Message = "Done", Data = _trucks });
                return Ok(_trucks);
            else
                return BadRequest(new { Flag = false, Message = "Error", Data = 0 });
        }
    }
}
