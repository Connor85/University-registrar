using System;
using System.Collections.Generic;
using Admissions.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admissions.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }
  }
}