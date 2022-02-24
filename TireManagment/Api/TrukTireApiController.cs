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
    public class TrukTireApiController : ControllerBase
    {
        TruckService truckService;
        CategoryService categoryService;
        public TrukTireApiController(TruckService truck, CategoryService _categoryService)
        {
            truckService = truck;
            categoryService = _categoryService;

        }
        [HttpGet]
        public IActionResult GetTruckTires(string TruckNumber)
        {
           
            var res = truckService.GetTruckTires(TruckNumber);
            return Ok(res);
        }
    }
}
