using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class AccountServiceProxy : IAccountService
    {
        private readonly IAccountService accountService;
        public AccountServiceProxy()
        {
            this.accountService = new AccountService();
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
}
