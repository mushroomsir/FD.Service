using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using FD.Service;
using System.Collections.Generic;
using FD.Service.Http;

namespace FdServiceTest.api
{

    [FdService(SessionMode = SessionMode.Support, IsPublicAllMethod = true)]
    [Auth(Message = "登录验证")]
    public class SchoolApi
    {

        [FdMethod]
        public static string GetTeacherName()
        {
            return "Odin";
        }

        [FdMethod]
        public string GetNameById(string studentId)
        {
            return studentId + ":loki";
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

        [FdMethod(ResponseFormat = ResponseFormat.Json)]
        public static Student GetTeacherInfo()
        {
            return new Student()
            {
                Age = 33,
                Name = "Odin"
            };
        }

        [FdMethod(ResponseFormat = ResponseFormat.Json)]
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

        [FdMethod(ResponseFormat = ResponseFormat.Json)]
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

        [FdMethod]
        [Auth(Order = 1, Message = "权限验证")]
        [log(Order = 2, Message = "日志记录")]
        public static int GetPointsById(int id, int sid)
        {
            return 10;
        }

        [FdMethod]
        public static string SessionTest(HttpContext hc)
        {
            if (hc.Session != null)
                return "Support Session";

            return "Not Support Session";
        }

        public static object PublicMethod(HttpContext hc)
        {
            return "Public all the Method";
        }


        public static void OriginalErrorTest(HttpContext hc)
        {
            string str = null;
            str.ToList();
            throw new HttpException((int) HttpStatusCode.BadRequest, "Test");
        }

        public static void CustomErrorTest(HttpContext hc)
        {
            var hrMessage = new HttpResponseMessage(HttpStatusCode.Found);
            hrMessage.StringContent = new StringContent("Value cannot be null", Encoding.UTF8);
            throw new HttpResponseException(hrMessage);
        }
        [Error(Message = "Log Record")]
        public static void OverrideErrorTest(HttpContext hc)
        {
            string str = null;
            str.ToList();
            throw new HttpException((int)HttpStatusCode.BadRequest, "Test");
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