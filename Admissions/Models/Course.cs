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

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE * FROM courses WHERE id = @searchId; DELETE * FROM students_courses WHERE course_id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", _id);

      cmd.ExecuteNonQuery();

      conn.Close();
      if(conn != null)
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

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO courses (name, course_number) VALUE (@name, @courseNumber);";

      cmd.Parameters.AddWithValue("@name", _name);
      cmd.Parameters.AddWithValue("@courseNumber", _courseNumber);

      cmd.ExecuteNonQuery();

      _id = (int) cmd.LastInsertedId;

       conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Course Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int newId = 0;
      string newName = "";
      string newCourseNumber = "";

      while(rdr.Read())
      {
        newId = rdr.GetInt32(0);
        newName = rdr.GetString(1);
        newCourseNumber = rdr.GetString(2);
      }

      Course foundCourse = new Course(newName, newCourseNumber, newId);

      conn.Close();
        if (conn != null)
      {
        conn.Dispose();
      }

      return foundCourse;
    }

    public void AddStudent(Student newStudent)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students_courses (student_id, course_id) VALUES (@studentId, @courseId);";

      cmd.Parameters.AddWithValue("@studentId", newStudent.GetId());
      cmd.Parameters.AddWithValue("@courseId", _id);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Student> GetStudents()
    {
      List<Student> foundStudents = new List<Student> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT students.* FROM courses JOIN students_courses ON (courses.id = students_courses.course_id) JOIN students ON (students_courses.student_id = students.id) WHERE courses.id = @courseId;";

      cmd.Parameters.AddWithValue("@courseId", _id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
        int newId = rdr.GetInt32(0);
        string newName = rdr.GetString(1);
        DateTime newDate = rdr.GetDateTime(2);
        Student newStudent = new Student(newName, newDate, newId);
        foundStudents.Add(newStudent);
      }

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return foundStudents;
    }
  }
}