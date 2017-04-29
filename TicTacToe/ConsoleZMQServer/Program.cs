using System;
using System.Threading;
using System.Windows.Forms;
using ZeroMQ;

namespace ConsoleZMQServer
{
    //посмотреть
    //https://github.com/alex-pope/tic-tac-toe/blob/master/TicTacToe/TicTacToe.cs
    static class Program
    {

        static void Main(string[] args)
        {
            
            string port1 = "5556";
            string port2 = "5555";

            //Application.Run(new ServerWindows(port1, port2));
            var ctx = new ZContext();
            var s = new Server(port1, port2);
            while (Console.ReadLine() != "exit")
            {
                Thread.Sleep(10);
            }
            s.Dispose();
        }
    }
}