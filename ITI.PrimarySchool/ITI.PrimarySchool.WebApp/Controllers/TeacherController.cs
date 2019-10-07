using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    [Route("/api/[controller]")]
    public class TeacherController : Controller
    {
        readonly TeacherGateway _teacherGateway;

        public TeacherController(TeacherGateway teacherGateway)
        {
            if (teacherGateway == null) throw new ArgumentNullException(nameof(teacherGateway));
            _teacherGateway = teacherGateway;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<TeacherData> teachers = await _teacherGateway.GetAll();
            return Ok(teachers);
        }
    }
}
