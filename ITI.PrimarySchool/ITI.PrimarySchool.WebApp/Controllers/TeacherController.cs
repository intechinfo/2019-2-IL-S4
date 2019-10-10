using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    [Route("/api/teacher")]
    public class TeacherController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //IEnumerable<TeacherData> teachers = gateway.GetAll();
            //return Ok(teachers);

            throw new System.NotImplementedException();
        }
    }
}
