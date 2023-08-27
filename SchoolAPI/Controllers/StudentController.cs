using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Interface;
using SchoolAPI.Requests;

namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase
    {
        IStudent _student;
        public StudentController(IStudent student)
        {
            _student = student;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents() {
            return await _student.GetStudents(this);
        }
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudent(int studentId)
        {
            return await _student.GetStudent(this, studentId);
        }
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] StudentRequest request)
        {
            return await _student.PostStudent(this, request);
        }
        [HttpPatch]
        public async Task<IActionResult> PatchStudent([FromBody] StudentRequest request)
        {
            return await _student.EditStudent(this, request);
        }
        [HttpDelete("studentId")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            return await _student.DeleteStudent(this, studentId);
        }
    }
}
