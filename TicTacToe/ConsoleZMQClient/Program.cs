using System;
using System.Threading;
using System.Windows.Forms;
using ZeroMQ;

namespace ConsoleZMQClient
{
    static class Program
    {
        static void Main(string[] args)
        {
            string port1 = "5556";
            string port2 = "5555";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var ctx = new ZContext();
            Application.Run(new ClientWindows(port1, port2));
        }
    }
}