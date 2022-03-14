using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TireManagment.Enums;
using TireManagment.Services;

namespace TireManagment.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class CategoryController : Controller
    {
        CategoryService categoryService;
            public CategoryController(CategoryService _categoryService)
        {
            categoryService = _categoryService;
        }
        public IActionResult Index()
        {
            var categories=categoryService.GetAll();
            return View(categories);
        }
        public IActionResult Excel()
        {
            var categories = categoryService.GetAll();
            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("Categories");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Active";

                worksheet.Cell(currentRow, 3).Value = "Description";
                worksheet.Cell(currentRow, 4).Value = "TireCount";
                worksheet.Cell(currentRow, 5).Value = "FrontTires";
                worksheet.Cell(currentRow, 6).Value = "SpareTires";
                worksheet.Cell(currentRow, 7).Value = "RareTires";

                foreach (var category in categories)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = category.Id;
                    worksheet.Cell(currentRow, 2).Value = category.Active;
                    worksheet.Cell(currentRow, 3).Value = category.Description;
                    worksheet.Cell(currentRow, 4).Value = category.TireCount;
                    worksheet.Cell(currentRow, 5).Value = category.FrontTires;
                    worksheet.Cell(currentRow, 6).Value = category.SpareTires;
                    worksheet.Cell(currentRow, 7).Value = category.RareTires;
              

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Categories.xlsx");
                }
            }
        }
    }
}
