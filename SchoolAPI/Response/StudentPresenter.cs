using SchoolAPI.Entity;
using System.Collections.Generic;

namespace SchoolAPI.Response
{
    public class StudentPresenter
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public double Grade { get; set; }

        public static StudentPresenter getPresenter(Student student)
        {
            return new StudentPresenter()
            {
                Id = student.Id,
                Email = student.Email,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Grade = student.Grade,
            };
        }
        public static List<StudentPresenter> getPresenter(List<Student> students)
        {
            List<StudentPresenter> presenters = new List<StudentPresenter>();
            foreach (var student in students)
            {
                presenters.Add(getPresenter(student));
            }
            return presenters;
        }
    }
}
