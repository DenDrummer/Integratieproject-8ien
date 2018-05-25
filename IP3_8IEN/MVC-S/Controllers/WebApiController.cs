using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC_S.Controllers
{
    [RoutePrefix("api/Students")]
    public class WebApiController : ApiController
    {
        [HttpGet]
        [Route("GetStudents")]
        public List<Student> GetStudents()
        {
            List<Student> studentList = new List<Student>();

            string[] students = { "John", "Henry", "Jane", "Martha" };

            foreach (string student in students)
            {
                Student currentStudent = new Student
                {
                    Name = student,
                    Email = student + "@academy.com"
                };
                studentList.Add(currentStudent);
            }
            return studentList;
        }
        public class Student
        {
            public String Name { get; set; }
            public String Email { get; set; }
        }
    }
}
