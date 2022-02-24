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
    public class TruckController : ControllerBase
    {
        TruckService truckService;
        CategoryService categoryService;
        public TruckController(TruckService truck, CategoryService _categoryService)
        {
            truckService = truck;
            categoryService = _categoryService;

        }
        [HttpGet]
        public IActionResult Trucks()
        {
            return Ok(truckService.GetAll());
        }
    }
}
