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

  }
}