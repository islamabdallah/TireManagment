using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.DbModels;
using TireManagment.Enums;
using TireManagment.Models;
using TireManagment.Services;

namespace TireManagment.Controllers
{
    [Authorize(Roles =Role.Admin)]
    public class TruckController : Controller
    {
        TruckService truckService;
        TruckTireService truckTireService;
        CategoryService categoryService;
        TireService TireService;
        public TruckController(TireService tireService, TruckService truck,CategoryService _categoryService,TruckTireService _truckTireService)
        {
            truckService = truck;
            TireService = tireService;
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
                ViewBag.id = TempData["id"];
                ViewBag.trucknumber = TempData["trucknumber"];
               
            }
            //if (truck.TruckNumber != null)
            //    return View(truck);
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
            TempData["id"] = truck.ID;
            TempData["trucknumber"] = truck.TruckNumber;
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
        //public IActionResult table()
        //{
        //    return View();
        //}
        public IActionResult assigntires(int id,string trucknumber)
        {
            ViewBag.tires = TireService.GetNewandRetreadtires();//this new and retead tires
            ViewBag.positios = truckService.GetTruckTires(trucknumber);//get all positions for this truck from trucktire controller
            return View();
        }
      public JsonResult  ValidateTruckNumber(string TruckNumber)
       {
            bool truck = truckService.checknumberExists(TruckNumber);
            if (!truck)
                return Json(true);
            return Json($"Truck Number Already Exists");
        }
        public string AssignTiress(List<string>position,List<string>serial)
        {
            return "fff";
        }
    }
}
