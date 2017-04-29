using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ProtoBuf;
using ProtocolLibrary;
using ProtocolLibrary.Datastructures;
using ProtocolLibrary.Messaging;
using ZeroMQ;

namespace ConsoleZMQServer
{
    public class Server : IDisposable
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
        public Server(string port1, string port2)
        {
            Console.WriteLine("Узел({0}, {1}) создан", port1, port2);
            _port1 = port1;
            _port2 = port2;
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
                        Console.Write(e.ToString()); ;
                        Console.ReadLine();
                    }
                }
            });
        }



        public void RequestReply()
        {
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
                        Console.WriteLine(e.ToString());
                        Console.ReadLine();
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
                    StreamWriter _sw = new StreamWriter(Directory.GetCurrentDirectory() + @"\..\..\logs"+_port2+".txt");
                    switch (convert[mr.who])
                    {
                        case '0':
                            if (oPlayer == Guid.Empty)
                            {
                                oPlayer = mr.UserId;
                                //_sw.WriteLineAsync("0: " + mr.UserId);
                                Console.WriteLine(_port1 + "; Нолики: " + mr.UserId);                           
                            }
                            break;

                        case 'X':
                            if (xPlayer == Guid.Empty)
                            {
                                xPlayer = mr.UserId;
                                Console.WriteLine(_port1 + "; Крестики: " + mr.UserId);
                               // _sw.WriteLineAsync("X: " + mr.UserId);
                            }
                            break;
                    }
                    int countX = 0;
                    int countO = 0;
                    for (int i = 0; i < _field._size; i++)
                        for (int j = 0; j < _field._size; j++)
                        {
                            if (_field._fields[i][j] == 'X')
                                countX++;
                            else
                            if (_field._fields[i][j] == '0')
                                countO++;
                        }

                    if (((countX == 0) && (countO == 0) && (mr.UserId == xPlayer)) || (countX == countO) && (mr.UserId == xPlayer) || (((countX - countO) == 1) && (mr.UserId == oPlayer)))
                    {
                        GameStatus a;
                        a = _field.Play(mr.Rows, mr.Cols, convert[mr.who]);
                       if ((a!=GameStatus.FieldIsUsed)&&(mr.Rows!=0)&&(mr.Cols!=0))
                            Console.WriteLine(_port1 + "; " +DateTime.Now + ": игрок '" + convert[mr.who] + "' сходил в (" + mr.Rows + ";" + mr.Cols + ")");
                            //_sw.WriteLineAsync(DateTime.Now + ": игрок '"+ convert[mr.who] +"' сходил в ("+mr.Rows + ";" + mr.Cols + ")");
                        curGame = _field.Status();
                        switch (curGame)
                        {
                            case GameStatus.WinX:
                                gameStata = GameStatus.WinX;
                               // _sw.WriteLineAsync(DateTime.Now + ": крестики выиграли");
                               Console.WriteLine(_port1 + "; " + DateTime.Now + ": крестики выиграли");
                                curStatus += "Крестики выиграли" + Environment.NewLine;
                                break;
                            case GameStatus.WinZero:
                                gameStata = GameStatus.WinZero;
                                Console.WriteLine(_port1 + "; "+ DateTime.Now + ": нолики выиграли");
                               // _sw.WriteLineAsync(DateTime.Now + ": нолики выиграли");
                                curStatus += "Нолики выиграли" + Environment.NewLine;
                                break;
                            case GameStatus.NobodyWins:
                                gameStata = GameStatus.NobodyWins;
                                curStatus += "Продолжаем" + Environment.NewLine;
                                break;
                        }
                    }
                    else
                        gameStata = _field.Status();
                    Thread.Sleep(1000);
                    counter++;
                    r.text++;
                    mr.text++;
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
                    _sw.Close();
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
    }
}