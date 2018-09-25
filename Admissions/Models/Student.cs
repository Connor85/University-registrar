using System;
using System.Collections.Generic;
using Admissions;
using MySql.Data.MySqlClient;

namespace Admissions.Models
{
  public class Student
  {

    private int _id;
    private string _name;
    private DateTime _enrollmentDate;

    public Student(string name, DateTime enrollmentDate, int id = 0)
    {
      _id = id;
      _name = name;
      _enrollmentDate = enrollmentDate;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public DateTime GetEnrollmentDate()
    {
      return _enrollmentDate;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if (!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool nameEquality = (this._name == newStudent.GetName());
        bool dateEquality = (this._enrollmentDate == newStudent.GetEnrollmentDate());
        return (nameEquality && dateEquality);
      }
    }

    public override int GetHashCode()
    {
      string allHash = this.GetName() + this.GetEnrollmentDate();
      return allHash.GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students (name, enrollment_date) VALUES (@newName, @newDate);";

      MySqlParameter newName = new MySqlParameter();
      newName.ParameterName = "@newName";
      newName.Value = this._name;
      cmd.Parameters.Add(newName);

      MySqlParameter newDate = new MySqlParameter();
      newDate.ParameterName = "@newDate";
      newDate.Value = this._enrollmentDate;
      cmd.Parameters.Add(newDate);

      cmd.ExecuteNonQuery();

      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"TRUNCATE TABLE students;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student>() {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        DateTime date = rdr.GetDateTime(2);

        Student newStudent = new Student(name, date, id);
        allStudents.Add(newStudent);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }

    public static Student Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      int newId = 0;
      string newName = "";
      DateTime newEnrollmentDate = new DateTime (1111,11,11);
      
      while(rdr.Read())
      {
        newId = rdr.GetInt32(0);
        newName = rdr.GetString(1);
        newEnrollmentDate = rdr.GetDateTime(2);
      }

      Student foundStudent = new Student (newName, newEnrollmentDate, newId);

      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return foundStudent;
    }

    public void Edit(string newName, DateTime newDate)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE students SET name = @newName, enrollment_date = @newDate WHERE id = @searchId;";

      cmd.Parameters.AddWithValue("@newName", newName);
      cmd.Parameters.AddWithValue("@newDate", newDate);
      cmd.Parameters.AddWithValue("@searchId", _id);

      cmd.ExecuteNonQuery();

      _name = newName;
      _enrollmentDate = newDate;

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
      cmd.CommandText = @"DELETE FROM students where id = @searchId; DELETE FROM students_courses WHERE student_id = @searchId;";

      cmd.Parameters.AddWithValue("@searchId", _id);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddCourse(Course newCourse)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students_courses (student_id, course_id) VALUES (@studentId, @courseId);";

      cmd.Parameters.AddWithValue("@studentId", _id);
      cmd.Parameters.AddWithValue("@courseId", newCourse.GetId());

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Course> GetCourses()
    {
      List<Course> allCourses = new List<Course>() {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT courses.* FROM students JOIN students_courses ON (students.id = students_courses.student_id) JOIN courses ON (students_courses.course_id = courses.id) WHERE student.id = @studentId;";

      cmd.Parameters.AddWithValue("@studentId", _id);

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