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
        public IActionResult Trucks()
        {
            var _trucks = TruckService.GetAll();
            if (_trucks != null)
                return Ok(new { Flag = true, Message = "Done", Data = _trucks });
            else
                return BadRequest(new { Flag = false, Message = "Error", Data = 0 });
        }
    }
}
