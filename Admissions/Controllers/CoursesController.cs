using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Admissions.Models;

namespace Admissions.Controllers
{
  public class CoursesController : Controller
  {
    [HttpPost("/courses/new")]
    public ActionResult Create()
    {
      Course newCourse = new Course(Request.Form["newName"], Request.Form["newCourseNumber"]);
      newCourse.Save();
      return RedirectToAction("Index", "Home");
    }

    [HttpGet("/courses/{id}")]
    public ActionResult Details(int id)
    {
      Course foundCourse = Course.Find(id);
      return View(foundCourse);
    }

    [HttpGet("/courses/{id}/delete")]
    public ActionResult Delete(int id)
    {
     Course foundCourse =Course.Find(id);
     foundCourse.Delete();
     return RedirectToAction("Index","Home");
    }
  }
}