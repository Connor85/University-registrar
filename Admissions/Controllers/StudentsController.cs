using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Admissions.Models;

namespace Admissions.Controllers
{
  public class StudentsController : Controller
  {
    [HttpGet("/students/directory")]
    public ActionResult Index()
    {
      List <Student> allStudents = Student.GetAll();
      return View(allStudents);
    }

    [HttpPost("/students/new")]
    public ActionResult Create()
    {
      DateTime newDate = Convert.ToDateTime(Request.Form["newEnrollmentDate"]);
      Student newStudent = new Student(Request.Form["newStudentName"], newDate);
      newStudent.Save();
      return RedirectToAction("Index");
    }
    [HttpGet("/students/{id}")]
    public ActionResult Details(int id)
    {
      Student foundStudent = Student.Find(id);
      List<Course> enrolledCourses = foundStudent.GetCourses();
      List<Course> allCourses = Course.GetAll();
      Dictionary<string, object> model = new Dictionary<string, object> {};
      model.Add("student", foundStudent);
      model.Add("courses", allCourses);
      model.Add("enrolledCourses", enrolledCourses);

      return View(model);
    }

    [HttpPost("/students/{id}/enroll")]
    public ActionResult Enrolled(int id)
    {
      Student foundStudent = Student.Find(id);
      Course foundCourse = Course.Find(int.Parse(Request.Form["courseId"]));
      foundCourse.AddStudent(foundStudent);
      return RedirectToAction("Details", new {id = id});
    }
  }
}
