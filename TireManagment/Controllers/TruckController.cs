using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
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
        UserManager<AppUser> Manager;
        public TruckController(UserManager<AppUser> _Manager,TireService tireService, TruckService truck,CategoryService _categoryService,TruckTireService _truckTireService)
        {
            truckService = truck;
            Manager = _Manager;
            TireService = tireService;
            truckTireService = _truckTireService;
            categoryService = _categoryService;

        }
        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            ViewBag.trucks =await truckService.GetAll();
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
        public IActionResult assigntires(string trucknumber)
        {
            var tires= TireService.GetNewandRetreadtires().ToList();
          //  var fi= new tirewithserial(){ serial = "Select Serial", tierid = -1 };
            //tires.Insert(0, fi);
            ViewBag.tires = tires; //this new and retead tires
            ViewBag.positios = truckService.GetTruckTires(trucknumber);//get all positions for this truck from trucktire controller
            ViewBag.trucknumber = trucknumber;
            return View();
        }
      public JsonResult  ValidateTruckNumber(string TruckNumber)
       {
            bool truck = truckService.checknumberExists(TruckNumber);
            if (!truck)
                return Json(true);
            return Json($"Truck Number Already Exists");
        }
        public IActionResult AssignTiress(List<string> position, List<int> tireids, string trucknumber)
        {

            var userid = Manager.GetUserAsync(User).Result.Id;

            truckTireService.updatetrucktire(position, tireids, trucknumber,userid);
            
       
            return RedirectToAction("Index","Home");
        }
        public IActionResult freepositionstrucks()
        {
            
            var freepositionstrucks=truckTireService.GetTrucksWithFreePositions();
            ViewBag.freepositionstrucks = freepositionstrucks;
            return View();
        }
        public async Task<IActionResult> Excel()
        {
            var trucks =await truckService.GetAll();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Trucks");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "TruckNumber";
                worksheet.Cell(currentRow, 3).Value = "TruckName";
                worksheet.Cell(currentRow, 4).Value = "Type";
                worksheet.Cell(currentRow, 5).Value = "Unit";
                worksheet.Cell(currentRow, 6).Value = "VehichleModelNo";
                worksheet.Cell(currentRow, 7).Value = "Active";
                worksheet.Cell(currentRow, 8).Value = "AxleCount";
                worksheet.Cell(currentRow, 9).Value = "Category";
                worksheet.Cell(currentRow, 10).Value = "Company";
                worksheet.Cell(currentRow, 11).Value = "Registeration";

                worksheet.Cell(currentRow, 12).Value = "Size";
                worksheet.Cell(currentRow, 13).Value = "Status";
                worksheet.Cell(currentRow, 14).Value = "Engine";
                worksheet.Cell(currentRow, 15).Value = "Chassis";
                foreach (var truck in trucks)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = truck.TruckId;
                    worksheet.Cell(currentRow, 2).Value = truck.TruckNumber;
                    worksheet.Cell(currentRow, 3).Value = truck.TruckName;
                    worksheet.Cell(currentRow, 4).Value = truck.Type;
                    worksheet.Cell(currentRow, 5).Value = truck.Unit;
                    worksheet.Cell(currentRow, 6).Value = truck.VehichleModelNo;
                    worksheet.Cell(currentRow, 7).Value = truck.Active;
                    worksheet.Cell(currentRow, 8).Value = truck.AxleCount;
                    worksheet.Cell(currentRow, 9).Value = truck.Category;
                    worksheet.Cell(currentRow, 10).Value = truck.TruckCompany;
                    worksheet.Cell(currentRow, 11).Value = truck.Registeration;

                    worksheet.Cell(currentRow, 12).Value = truck.Size;
                    worksheet.Cell(currentRow, 13).Value = truck.Status;
                    worksheet.Cell(currentRow, 14).Value = truck.Engine;
                    worksheet.Cell(currentRow, 15).Value = truck.Chassis;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "trucks.xlsx");
                }
            }
        }
    }
}
