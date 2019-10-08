using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.PrimarySchool.DAL;
using ITI.PrimarySchool.WebApp.Models;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeacherModel model)
        {
            Result<int> result = await _teacherGateway.Create(model.FirstName, model.LastName);
            if (result.IsSuccess)
            {
                return Ok(new { IsSuccess = true, TeacherId = result.Value });
            }
            else
            {
                return BadRequest(new { IsSuccess = false, ErrorMessage = result.ErrorMessage });
            }
        }

        [HttpGet("{teacherId}")]
        public async Task<IActionResult> Get(int teacherId)
        {
            TeacherData teacher = await _teacherGateway.GetById(teacherId);
            if( teacher == null ) return NotFound();

            return Ok(teacher);
        }
    }
}
