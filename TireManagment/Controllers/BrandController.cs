using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;
using TireManagment.Services;

namespace TireManagment.Controllers
{
    public class BrandController : Controller
    {
        BrandService brandService;
        public BrandController(BrandService _brandservice)
        {
            brandService = _brandservice;
        }
        public IActionResult Index()
        {
            ViewBag.trucks = brandService.GetAll();
        
            if (TempData["success"] != null && (string)TempData["success"] == "true")
            {
                ViewBag.success = "true";
            }
            ViewBag.brands = brandService.GetAll();
            return View("brands",new TireBrand());
        }
        public IActionResult AddBrand(TireBrand brand)
        {
           brandService.Insert(brand);
            //     ViewBag.success = "true";
            //ViewBag.trucks = truckService.GetAll();
            //ViewBag.categories = categoryService.GetAll();
            TempData["success"] = "true";
            return RedirectToAction("Index");
        }
        
        
    }
}
