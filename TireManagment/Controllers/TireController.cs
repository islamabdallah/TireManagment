using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
        [HttpGet]
        public IActionResult TireDetials()
        {
            var tire = tireService.GetFirstTire();
            if (tire != null)
            {
                ViewBag.tireserials = tireService.gettireserials();
                ViewBag.TruckNumber = tireService.GetTruckNumber((int)tire.TireId);
                ViewBag.TireHistory = tireService.GetTireHistory((int)tire.TireId);


                return View("TireDetails", tire);
            }
            return View("TireDetails");
        }
        public IActionResult GetDetials(int tireid)
        {
            ViewBag.tireserials = tireService.gettireserials();
            ViewBag.TruckNumber = tireService.GetTruckNumber(tireid);
            ViewBag.TireHistory = tireService.GetTireHistory(tireid);
            var tire = tireService.GetTireDetails(tireid);
            return View("TireDetails", tire);
        }
        public IActionResult Excel()
        {
            var tires = tireService.GetAll();
            using (var workbook = new XLWorkbook())
            {
                
                var worksheet = workbook.Worksheets.Add("Tires");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Serial";
             
                worksheet.Cell(currentRow, 3).Value = "SubmitDate";
                worksheet.Cell(currentRow, 4).Value = "Status";
                worksheet.Cell(currentRow, 5).Value = "Brand";
                worksheet.Cell(currentRow, 6).Value = "Size";

                foreach (var tire in tires)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = tire.ID;
                    worksheet.Cell(currentRow, 2).Value = tire.Serial;
                    worksheet.Cell(currentRow, 3).Value = tire.SubmitDate;
                    worksheet.Cell(currentRow, 4).Value = tire.TireStatus;
                    worksheet.Cell(currentRow, 5).Value = tire.Brand.Name;
                    worksheet.Cell(currentRow, 6).Value = tire.Size;
                  
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Tires.xlsx");
                }
            }
        }
        public IActionResult FindMovements(DateTime startdate, DateTime enddate, int tireid)
        {

            var movements = tireService.GetTireMovemnts(startdate, enddate,tireid );
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Trucks");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "MovementDate";
        
                worksheet.Cell(currentRow, 3).Value = "MovementType";
                worksheet.Cell(currentRow, 4).Value = "TireMan";
                worksheet.Cell(currentRow, 5).Value = "Serial";
                worksheet.Cell(currentRow, 6).Value = "Position";
                worksheet.Cell(currentRow, 7).Value = "CurrentTireDeoth";

              
                worksheet.Cell(currentRow, 8).Value = "KMWhileChange";
          
                worksheet.Cell(currentRow, 9).Value = "STDthreadDepth";
                foreach (var move in movements)
                {
                   
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = move.Id;
                    worksheet.Cell(currentRow, 2).Value = move.TireMovement.SubmitDate;

                    worksheet.Cell(currentRow, 3).Value = move.TireMovement.MovementType;
                    worksheet.Cell(currentRow, 4).Value = move.TireMovement.Tireman.Name;
                    worksheet.Cell(currentRow, 5).Value = move.tire.Serial;
                    worksheet.Cell(currentRow, 6).Value = move.Position;
                        worksheet.Cell(currentRow, 7).Value = move.KMWhileChange;
                        worksheet.Cell(currentRow, 8).Value = move.STDthreadDepth;
                        worksheet.Cell(currentRow, 6).Value = move.CurrentTireDepth;
                     
                      
                     





                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "truckmovements.xlsx");
                }
            }
        }

    }
}
