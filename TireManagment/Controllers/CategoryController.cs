using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    }
}
