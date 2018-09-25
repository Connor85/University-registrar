using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Admissions.Models;

namespace Admissions.TestTools
{
  [TestClass]
  public class CourseTests : IDisposable
  {
    public CourseTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=admissions_test;";
    }

    public void Dispose()
    {
      Student.DeleteAll();
      Course.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DatabaseStartsEmpty_0()
    {
      //Arrange
      List<Course> allCourses = Course.GetAll();
      int count = allCourses.Count;

      //Act
      //Assert
      Assert.AreEqual(count, 0);
    }

    [TestMethod]
    public void Save_SavedItemIsSavedToDB_True()
    {
      //Arrange
      //Act
      //Assert
    }

  }
}