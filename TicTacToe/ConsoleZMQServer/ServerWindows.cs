using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProtoBuf;
using ProtocolLibrary;
using ProtocolLibrary.Datastructures;
using ProtocolLibrary.Messaging;
using ZeroMQ;

namespace ConsoleZMQServer
{
    public partial class ServerWindows : Form
    {
        public bool clickOff = false;
        private static volatile bool _stopLoops;
        Dictionary<int, char> convert = new Dictionary<int, char>();
        TicTacToe _field = new TicTacToe();
        //сделать список игр
        private GameStatus curGame;
        public Guid nullPlayer = new Guid();
        public Guid xPlayer = new Guid();
        public Guid oPlayer = new Guid();
        public int turn = 0; //%2 - крестик
        private string _port1, _port2;
        public ServerWindows(string port1, string port2)
        {
            _port1 = port1;
            _port2 = port2;
            InitializeComponent();
            convert.Add(0, '0');
            convert.Add(1, 'X');
            RequestReply();
            PubSub();
        }
        public void Dispose()
        {
            _stopLoops = true;
        }

        public void PubSub()
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                var server = new ZSocket(ZSocketType.PUB);
                server.Bind("tcp://*:" + _port1);
                while (!_stopLoops)
                {
                    try
                    {
                        var dataFacadeEvent = new DataFacadeEvent { State = new List<User> { new User { Id = 1 } } };
                        using (var responseStream = new MemoryStream())
                        {
                            Serializer.Serialize(responseStream, dataFacadeEvent);
                            server.Send(new ZFrame(responseStream.ToArray()));
                        }
                        Thread.Sleep(1000);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
            });
        }

        public void RequestReply()
        {
            //TODO: move into server app!
            ThreadPool.QueueUserWorkItem(state =>
            {
                var server = new ZSocket(ZSocketType.REP);
                server.Bind("tcp://*:" + _port2);
                while (!_stopLoops)
                {
                    try
                    {
                        using (var receiveFrame = server.ReceiveFrame())
                        {
                            //request.Direction
                            var request = Serializer.Deserialize<Request>(receiveFrame);
                            var response = OnRequest(request);

                            // response.text++;

                            using (var responseStream = new MemoryStream())
                            {
                                Serializer.Serialize(responseStream, response);
                                server.Send(new ZFrame(responseStream.ToArray()));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
            });
        }
        long counter;
        private string curStatus;
        private GameStatus gameStata;
        public Response OnRequest(Request r)
        {
            switch (r.RequesType)
            {
                case RequesType.MoveRequest:
                    var mr = (MoveRequest)r;
                    List<char> curField = new List<char>();
                    switch (convert[mr.who])
                    {
                        case '0':
                            if (oPlayer == Guid.Empty)
                            {
                                oPlayer = mr.UserId;

                                textBoxO.Invoke(
                (ThreadStart)delegate ()
                {
                    textBoxO.Text = mr.UserId.ToString();
                });
                            }
                            break;

                        case 'X':
                            if (xPlayer == Guid.Empty)
                            {
                                xPlayer = mr.UserId;
                                textBoxX.Invoke(
                (ThreadStart)delegate ()
                {
                    textBoxX.Text = mr.UserId.ToString();
                });

                            }
                            break;
                    }


                    //if ((mr.UserId == oPlayer) || (mr.UserId == xPlayer))
                    //{
                        if (((turn % 2 == 0) && (mr.UserId == xPlayer)) || ((turn % 2 == 1) && (mr.UserId == oPlayer)))
                        {
                            GameStatus a;
                            a = _field.Play(mr.Rows, mr.Cols, convert[mr.who]);
                            curGame = _field.Status();
                            if (a!=GameStatus.FieldIsUsed)
                            turn++;
                            switch (curGame)
                            {
                                case GameStatus.WinX:
                                    gameStata = GameStatus.WinX;
                                    curStatus += "Крестики выиграли" + Environment.NewLine;
                                    break;
                                case GameStatus.WinZero: 
                                    gameStata = GameStatus.WinZero;                                   
                                    curStatus+= "Нолики выиграли" + Environment.NewLine;
                                    break;
                                case GameStatus.NobodyWins:
                                    gameStata = GameStatus.NobodyWins;
                                    curStatus+= "Продолжаем" + Environment.NewLine;
                                    break;
                            }
                        }
                        //Thread.Sleep(2000);
                        counter++;
                        r.text++;
                        mr.text++;
                        //Console.WriteLine("Запрос: {1}; От клиента: {0}; Ответ {2}", mr.UserId, counter, mr.text);
                        for (int i = 1; i <= _field._size; i++)
                        {
                            for (int j = 1; j <= _field._size; j++)
                                if (_field._fields[i - 1][j - 1].IsNotEmpty())
                                {
                                    curField.Add(_field._fields[i - 1][j - 1]);
                                    curStatus += _field._fields[i - 1][j - 1].ToString() + " ";                                  
                                }
                                else
                                {
                                    curField.Add('-');
                                    curStatus += "- ";              
                                }
                            curStatus += Environment.NewLine;
                        }
                   // }
                    //else
                    //{
                    //    curStatus+= "Игра уже идёт. Пытался войти: " + mr.UserId.ToString() + Environment.NewLine;                       
                    //}
                    consoleBox.Invoke(
                (ThreadStart)delegate ()
                {

                    //consoleBox.Text = curStatus;

//                    consoleBox.Text = curStatus + consoleBox.Text;
                });
                    return new MoveResponse
                    {
                        pole = curField,
                        text = mr.text++,
                        Ok = true,
                        stata = gameStata
                    };
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //buttonRunServer.Visible = false;
            //var ctx = new ZContext();
            //var s = new Server();
            //while (Console.ReadLine() != "exit")
            //{
            //    if (clickOff)
            //    {
            //        clickOff = false;
            //        break;
            //    }
            //    Thread.Sleep(10);
            //}
            //buttonRunServer.Visible = true;
            //s.Dispose();
        }

        private void buttonOffServer_Click(object sender, EventArgs e)
        {
            clickOff = true;
        }

        private void ServerWindows_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
