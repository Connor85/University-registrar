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

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course>() {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string courseNumber = rdr.GetString(2);
        Course newCourse = new Course(name, courseNumber, id);
        allCourses.Add(newCourse);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCourses;
    }

  }
}