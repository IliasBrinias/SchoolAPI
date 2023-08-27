using Microsoft.AspNetCore.Mvc;
using SchoolAPI.Requests;

namespace SchoolAPI.Interface
{
    public interface IStudent
    {
        Task<IActionResult> GetStudents(ControllerBase controller);
        Task<IActionResult> GetStudent(ControllerBase controller, int studentId);
        Task<IActionResult> PostStudent(ControllerBase controller, StudentRequest request);
        Task<IActionResult> EditStudent(ControllerBase controller, StudentRequest request);
        Task<IActionResult> DeleteStudent(ControllerBase controller, int studentsId);

    }
}
