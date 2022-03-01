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
    [Authorize(Roles = Role.Admin)]
    public class TireController : Controller
    {
        TireService tireService;
        BrandService brandService;
     public   TireController(TireService _tireservice,BrandService _brandService)
        {
            brandService = _brandService;
            tireService = _tireservice;
        }
        public IActionResult Index()
        {
            ViewBag.brands = brandService.GetAll();
            ViewBag.tires = tireService.GetAll();
            if (TempData["success"] != null && (string)TempData["success"] == "true")
            {
                ViewBag.success = "true";
            }
         
            return View(new DbModels.Tire());
        }
     
        [HttpPost]
        public IActionResult AddTire(Tire tire)
        {
            var CurrentDate = DateTime.Now;
            tire.SubmitDate = CurrentDate;
            tire.TireStatus = TireStatus.New;
        tireService.Insert(tire);
           
            
                TempData["success"] = "true";
                return RedirectToAction("Index");
           
        }
        public IActionResult ValidateSerial(string serial)
        {
            bool tire = tireService.checkserialExists(serial);
            if (!tire)
                return Json(true);
            return Json($"Serial Number Already Exists");
        }
        public IActionResult Tiresserials()
        {

            ViewBag.tireserials = tireService.gettireserials();
            return View("TireDetails");
        }
        public IActionResult TireDetials(int tireid)
        {
            ViewBag.tireserials = tireService.gettireserials();
            ViewBag.TruckNumber = tireService.GetTruckNumber(tireid);
            ViewBag.TireHistory = tireService.GetTireHistory(tireid);
            var tire = tireService.GetTireDetails(tireid);
            return View("TireDetails", tire);
        }
    }
}
