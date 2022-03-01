using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;
using TireManagment.Enums;
using TireManagment.Services;

namespace TireManagment.Controllers
{
    [Authorize(Roles =Role.Admin)]
    public class TruckController : Controller
    {
        TruckService truckService;
        TruckTireService truckTireService;
        CategoryService categoryService;
        public TruckController(TruckService truck,CategoryService _categoryService,TruckTireService _truckTireService)
        {
            truckService = truck;
          
            truckTireService = _truckTireService;
            categoryService = _categoryService;

        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.trucks = truckService.GetAll();
            ViewBag.categories = categoryService.GetAll();
            if(TempData["success"]!=null&&(string)TempData["success"]=="true")
            {
                ViewBag.success = "true";
            }
            return View(new DbModels.Truck());
        }
        public IActionResult AddTruck()
        {
            var categories = categoryService.GetAll();
            
            ViewBag.categories = categories;
            return View();
        }
        [HttpPost]
        public IActionResult AddTruck(Truck truck)
        {
            truckService.Insert(truck);
       //     ViewBag.success = "true";
            //ViewBag.trucks = truckService.GetAll();
            //ViewBag.categories = categoryService.GetAll();
            TempData["success"] = "true";
            return RedirectToAction("Index"); 
        }
        public IActionResult TruckDetails()
        {
            
            ViewBag.trucknumbers = truckService.GetAllTruckNumbers();
            return View();
        }
        public IActionResult GetTruckData(int truckid)
        {
         
            ViewBag.trucknumbers = truckService.GetAllTruckNumbers();
            var truck = truckService.GetTruckDetails(truckid);
            ViewBag.trucktires = truckService.GetTruckTires(truck.TruckNumber);//gets the current positions of tire
            ViewBag.truckmovements = truckTireService.GetTruckMovements(truck.TruckNumber);
            return View("TruckDetails", truck);
        }
        public IActionResult table()
        {
            return View();
        }
      public JsonResult  ValidateTruckNumber(string TruckNumber)
       {
            bool truck = truckService.checknumberExists(TruckNumber);
            if (!truck)
                return Json(true);
            return Json($"Truck Number Already Exists");
        }
    }
}
