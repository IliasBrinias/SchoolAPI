using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SchoolAPI.Entity;
using SchoolAPI.Interface;
using SchoolAPI.Requests;
using SchoolAPI.Response;

namespace SchoolAPI.Service
{
    public class StudentService : IStudent
    {
        private NpgsqlConnection dataSource;
        public StudentService(IConfiguration _config) {

            //dataSource = new NpgsqlConnection(_config.GetConnectionString("ConnectionStrings:PostgreSQL"));
            dataSource = new NpgsqlConnection("Server=host.docker.internal;Database=SchoolAPI;Username=postgres;Password=admin;");
        }
        public async Task<IActionResult> GetStudent(ControllerBase controller, int studentId)
        {
            Student student = null;
            dataSource.Open();
            try
            {
                await using (var cmd = new NpgsqlCommand("SELECT * FROM student WHERE ID =" + studentId, dataSource))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        student = buildStudent(reader);
                    }
                    if (student == null) { return controller.BadRequest(new ErrorResponse() { messsage = ErrorMessages.STUDENT_NOT_FOUND }); }
                }
            }
            catch (Exception)
            {
                dataSource.Close();
                return controller.BadRequest(new ErrorResponse() { messsage = ErrorMessages.SOMETHING_HAPPEND });
            }
            dataSource.Close();
            return controller.Ok(StudentPresenter.getPresenter(student));
        }

        public async Task<IActionResult> GetStudents(ControllerBase controller)
        {
            List<Student> students = new List<Student>();
            dataSource.Open();
            try
            {
                await using (var cmd = new NpgsqlCommand("SELECT * FROM student", dataSource))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        students.Add(buildStudent(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                dataSource.Close();
                return controller.BadRequest(new ErrorResponse() { messsage = ErrorMessages.SOMETHING_HAPPEND });
            }
            dataSource.Close();
            return controller.Ok(StudentPresenter.getPresenter(students));
        }
        public async Task<IActionResult> PostStudent(ControllerBase controller, StudentRequest request)
        {
            dataSource.Open();
            try
            {
                string comm = "INSERT INTO student (email, password, first_name, last_name, grade) VALUES(" +
                    "'" + request.Email + "' ," +
                    "'" + request.Password + "' ," +
                    "'" + request.FirstName + "' ," +
                    "'" + request.LastName + "' ," +
                    request.Grade + " )";
                var cmd = new NpgsqlCommand(comm, dataSource);
                var rows = await cmd.ExecuteNonQueryAsync();
                dataSource.Close();
                if (rows == 0)
                {
                    return controller.BadRequest(new ErrorResponse() { messsage = ErrorMessages.SOMETHING_HAPPEND });
                }
                else
                {
                    return controller.Ok();
                }
            }catch (PostgresException ex)
            {
                dataSource.Close();
                return controller.BadRequest(new ErrorResponse() { messsage = ex.Message });
            }
        }
        public async Task<IActionResult> EditStudent(ControllerBase controller, int studentId, StudentRequest request)
        {
            dataSource.Open();
            try
            {
                string comm = "UPDATE student SET " +
                    "email = '"+ request.Email + "', " +
                    "password = '"+ request.Password + "', " +
                    "first_name = '" + request.FirstName + "', " +
                    "last_name = '" + request.LastName + "', " +
                    "grade = " + request.Grade + " " +
                    "WHERE id = " + studentId;
                var cmd = new NpgsqlCommand(comm, dataSource);
                var rows = await cmd.ExecuteNonQueryAsync();
                dataSource.Close();
                if (rows == 0)
                {
                    return controller.BadRequest(new ErrorResponse() { messsage = ErrorMessages.SOMETHING_HAPPEND });
                }
                else
                {
                    return controller.Ok();
                }
            }
            catch (PostgresException ex)
            {
                dataSource.Close();
                return controller.BadRequest(new ErrorResponse() { messsage = ex.Message });
            }
        }

        public async Task<IActionResult> DeleteStudent(ControllerBase controller, int studentsId)
        {
            dataSource.Open();
            try
            {
                var cmd = new NpgsqlCommand("DELETE FROM student WHERE id = " + studentsId, dataSource);
                var rows = await cmd.ExecuteNonQueryAsync();
                dataSource.Close();
                if (rows == 0)
                {
                    return controller.BadRequest(new ErrorResponse() { messsage = ErrorMessages.SOMETHING_HAPPEND });
                }
                else
                {
                    return controller.Ok();
                }
            }
            catch (PostgresException ex)
            {
                dataSource.Close();
                return controller.BadRequest(new ErrorResponse() { messsage = ex.Message });
            }
        }

        private Student buildStudent(NpgsqlDataReader reader)
        {
            return new Student()
            {
                Id = reader.GetInt32(0),
                Email = reader.GetString(1),
                Password = reader.GetString(2),
                FirstName = reader.GetString(3),
                LastName = reader.GetString(4),
                Grade = reader.GetDouble(5),
            };
        }
    }
}
