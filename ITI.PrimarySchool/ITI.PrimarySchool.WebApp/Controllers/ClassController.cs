using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    public class ClassController : Controller
    {
        public async Task<IActionResult> All()
        {
            ClassGateway gateway = new ClassGateway();
            IEnumerable<ClassData> viewModel = await gateway.GetAll();
            return View(viewModel);
        }

        public async Task<IActionResult> Index(int id)
        {
            throw new NotImplementedException();
        }
    }
}
