using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class LinqTest
    {
        public static void Test()
        {
            var listA = new List<UserInfo>() {
                new UserInfo{ UserId="111",UserName="name111",Age=18},
                new UserInfo{ UserId="222",UserName="name333",Age=18},
                new UserInfo{ UserId="333",UserName="name444",Age=18},
            };
            var listB = new List<UserInfo>() {
                //new UserInfo{ UserId="111",UserName="name111",Age=18},
                new UserInfo{ UserId="222",UserName="name555",Age=18},
                new UserInfo{ UserId="333",UserName="name444",Age=18},
            };

            var listC = from a in listA
                        join b in listB on a.UserId equals b.UserId
                        //where a.UserName != b.UserName
                        select new UserInfo { UserId = a.UserId, UserName = b.UserName, Age = b.Age };
        }
    }
}
