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

        public IActionResult Excel()
        {
            var brands = brandService.GetAll();
            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("brands");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Brand Name";


                foreach (var category in brands)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = category.Id;
                    worksheet.Cell(currentRow, 2).Value = category.Name;



                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "brands.xlsx");
                }
            }
        }

    }
}
