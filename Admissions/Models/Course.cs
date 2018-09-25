using System;
using System.Collections.Generic;
using Admissions;
using MySql.Data.MySqlClient;

namespace Admissions.Models
{
  public class Course
  {
    private int _id;
    private string _courseNumber;
    private string _name;

    public Course(string name, string courseNumber, int id = 0)
    {
      _name = name;
      _courseNumber = courseNumber;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetCourseNumber()
    {
      return _courseNumber;
    }

    public int GetId()
    {
      return _id;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"TRUNCATE TABLE courses;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

  }
}