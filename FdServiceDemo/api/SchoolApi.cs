using System.Web;
using FD.Service;
using System.Collections.Generic;
using System;

namespace FdServiceTest.api
{
    [FdService]
    public class SchoolApi
    {

        [FdMethod]
        public static string GetTeacherName()
        {
            return "Odin";
        }

        [FdMethod]
        public string GetNameById(string StudentId)
        {
            return  StudentId + ":loki";
        }

        [FdMethod]
        public static string GetSelfName(HttpRequest request)
        {
            return request.QueryString["selfname"];
        }
        [FdMethod]
        public static decimal GetScore(decimal score)
        {
            return score;
        }

        [FdMethod]
        public static Student GetTeacherInfo()
        {
            return new Student()
            {
                Age = 33,
                Name = "Odin"
            };
        }

        [FdMethod]
        public static List<Student> GetStudentList()
        {
            return new List<Student>()
            { 
                new Student()
                {
                    Age = 13,
                    Name = "LOKI"
                },
                 new Student()
                {
                    Age = 14,
                    Name = "Frigga"
                }
            };
        }
        [FdMethod]
        public static List<Student> GetStudentListByClassId(int classId)
        {

            if (classId == 1)
            {
                return new List<Student>()
               { 
                 new Student()
                {
                    Age = 14,
                    Name = "Frigga"
                }
                };
            }
            else
            {
                return new List<Student>()
                { 
                   new Student()
                    {
                      Age = 13,
                      Name = "LOKI"
                     }
              };
            }
        }
        [FdMethod]
        public static Statu GetSchoolStatus()
        {
            return Statu.alive;
        }
        [FdMethod]
        public static string ContentType()
        {
            return "Thor";
        }
    }
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public enum Statu
    {
        alive = 1,
        dead = 2
    }
}