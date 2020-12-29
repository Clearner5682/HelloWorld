using System;
using System.Collections.Generic;
using System.Text;
using ConsoleApp1.Models;

namespace ConsoleApp1
{
    public interface IAccountService
    {
        void Reg(UserInfo user);
    }

    public class AccountService : IAccountService
    {
        public void Reg(UserInfo user)
        {
            Console.WriteLine($"用户[{user.UserName}]注册成功");
        }
    }

    public class AccountServiceDecorator : IAccountService
    {
        private readonly IAccountService accountService;

        public AccountServiceDecorator(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public void Reg(UserInfo user)
        {
            this.Before();
            this.accountService.Reg(user);
            this.After();
        }

        public void Before()
        {
            Console.WriteLine("注册之前执行");
        }

        public void After()
        {
            Console.WriteLine("注册之后执行");
        }
    }

    public class Decorator
    {
        public static void Test()
        {
            IAccountService accountService = new AccountService();
            IAccountService accountServiceDecorator = new AccountServiceDecorator(accountService);
            accountServiceDecorator.Reg(new UserInfo { UserName="hongyan" });
        }
    }
}
