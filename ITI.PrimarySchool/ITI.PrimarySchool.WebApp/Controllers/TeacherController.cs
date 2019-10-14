using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ITI.PrimarySchool.WebApp.Controllers
{
    [Route("/api/teacher")]
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
            // System.InvalidOperationException: Unable to resolve service for type
            // 'System.String' while attempting to activate
            // 'ITI.PrimarySchool.DAL.TeacherGateway'.
            IEnumerable<TeacherData> teachers = await _teacherGateway.GetAll();
            return Ok(teachers);
        }
    }
}
