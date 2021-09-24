using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Utils;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        MyLoger logger = new MyLoger("Form1");
        
        public Form1()
        {
            InitializeComponent();

            Run();
        }

        public void Run()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, e) => {
                int count = 0;
                while (true)
                {
                    if (count > 20)
                    {
                        logger.Info("程序退出");
                        this.Invoke(new Action(()=> {
                            Exit();
                        }));
                    }
                    logger.Info("Form1 Running");

                    Thread.Sleep(2000);
                    count++;
                }
            };
            worker.RunWorkerAsync();
        }

        public void Exit()
        {
            Application.Exit();
        }
    }
}
