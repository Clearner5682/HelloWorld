using System.Diagnostics;
using Common;
using Activity = Common.Activity;

namespace CustomExternalLib
{
    public class MyApprove : Activity
    {
        public override void Execute()
        {
            Console.WriteLine($"{nameof(MyApprove)}.Executed");
        }
    }
}
