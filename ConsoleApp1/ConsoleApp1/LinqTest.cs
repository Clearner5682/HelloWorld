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
                new UserInfo{ UserId="222",UserName="name222",Age=18},
                new UserInfo{ UserId="333",UserName="name333",Age=18},
            };
            var listB = new List<UserInfo>() {
                //new UserInfo{ UserId="111",UserName="name111",Age=18},
                new UserInfo{ UserId="222",UserName="name222",Age=18},
                new UserInfo{ UserId="333",UserName="name333",Age=18},
                new UserInfo{ UserId="444",UserName="name444",Age=18}
            };
            var listScore = new List<UserScore> { 
                new UserScore{ UserId="111",Score=75},
                new UserScore{ UserId="222",Score=60},
                new UserScore{ UserId="333",Score=88}
            };

            // 内连接
            var listC = from a in listA
                        join b in listB on a.UserId equals b.UserId
                        //where a.UserName != b.UserName
                        select new UserInfo { UserId = a.UserId, UserName = a.UserName+":"+b.UserName, Age = a.Age+b.Age};

            // 左连接
            var listD = from a in listA
                        join b in listB on a.UserId equals b.UserId into result
                        from item in result.DefaultIfEmpty()
                        select new UserInfo { UserId = a.UserId, UserName = a.UserName+":"+(item==null?"":item.UserName), Age =a.Age+(item==null?0:item.Age) };

            // 全连接
            var listFull = from a in listA
                           from b in listB
                           select new { User1 = a, User2 = b };


            var listE = from a in listB
                        join b in listScore on a.UserId equals b.UserId
                        orderby b.Score descending
                        select a;


            var list1 = new List<UserInfo>() { 
                new UserInfo{ UserId="111",UserName="name111",Age=18},
                new UserInfo{ UserId="111",UserName="name111",Age=22},
                new UserInfo{ UserId="333",UserName="name333",Age=55},
                new UserInfo{ UserId="333",UserName="name333",Age=50 }
            };

            var list2 = from a in list1
                        group a by new { UserId = a.UserId, UserName = a.UserName } into g
                        let maxAge = g.OrderBy(o => o.Age).Last().Age
                        select new UserInfo { UserId=g.Key.UserId,UserName=g.Key.UserName,Age=maxAge };
                      
        }
    }
}
