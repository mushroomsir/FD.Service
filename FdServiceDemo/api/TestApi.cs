using System.Web;
using FD.Service;
using System.Collections.Generic;

namespace FdServiceTest.api
{
    [FdService]
    public class TestApi
    {
        [FdMethod]
        public string GetDetail(string userId)
        {
            return userId + "<br/>loki";
        }

        [FdMethod]
        public static string GetUserName()
        {
            return "佚名";
        }

        [FdMethod]
        public static string RequestTest(HttpRequest request)
        {
            return request.QueryString["a"];
        }
        [FdMethod]
        public static decimal DecimalTest(decimal money_a)
        {
            return money_a;
        }

        [FdMethod]
        public static Person ObjectTest()
        {
            return new Person()
            {
                Age = 13,
                Name = "LOKI"
            };
        }

        [FdMethod]
        public static List<Person> GenericListTest()
        {
            return new List<Person>()
            { 
                new Person()
                {
                    Age = 13,
                    Name = "LOKI"
                },
                 new Person()
                {
                    Age = 14,
                    Name = "xiaoming"
                }
            };
        }
        [FdMethod]
        public static Statu EnumTest()
        {
            return Statu.alive;
        }
    }
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public enum Statu
    {
        alive = 1,
        dead =2
    }
}