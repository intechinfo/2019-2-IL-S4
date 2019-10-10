using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    public class ClassController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ClassGateway gateway = new ClassGateway();
            IEnumerable<ClassData> classes = await gateway.GetAll();
            ViewBag.Classes = classes;
            return View();
        }
    }
}
