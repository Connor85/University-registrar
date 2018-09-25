using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Admissions.Models;

namespace Admissions.TestTools
{
  [TestClass]
  public class StudentTests : IDisposable
  {
    public StudentTests()
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
      List<Student> allStudents = Student.GetAll();
      int count = allStudents.Count;

      //Act
      //Assert
      Assert.AreEqual(count, 0);
    }

    [TestMethod]
    public void Save_SaveToDatabase_True()
    {
      //Arrange
      DateTime date = new DateTime(2018,11,11);
      Student newStudent = new Student("connor", date);

      //Act
      newStudent.Save();
      List<Student> allStudents = Student.GetAll();

      //Assert
      Assert.AreEqual(newStudent, allStudents[0]);
    }

    [TestMethod]
    public void Find_FindsStudentFromDatabase_True()
    {
      //Arrange
      DateTime date = new DateTime(2018,11,11);
      Student newStudent = new Student("Ryan", date);

      //Act
      newStudent.Save();
      int findId = newStudent.GetId();
      Student foundStudent = Student.Find(findId);

      //Assert
      Assert.AreEqual(newStudent, foundStudent);
    }
  }
}