using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ProtoBuf;
using ProtocolLibrary;
using ProtocolLibrary.Messaging;
using ZeroMQ;

namespace ConsoleZMQClient
{
    internal class Client : IDisposable
    {
        private static volatile bool _stopLoops;
        private readonly Random _random = new Random();

        public Client()
        {
            PubSub();
            RequestReply();
        }

        public Guid Id { get; set; }
        public int Who { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int number;
        public bool gameStarted = false;
        public string room = "";
        public void Dispose()
        {
            _stopLoops = true;
        }

        public void PubSub()//подписываемся
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                var server = new ZSocket(ZSocketType.SUB);
                //считывать с файла ip-шник
                server.Connect("tcp://192.168.0.56:5556");
                //tcp://127.0.0.1:5555
                //server.Connect("tcp://192..0.1:5556");
                while (!_stopLoops)
                {
                    try
                    {
                        using (var receiveFrame = server.ReceiveFrame())
                        {
                            var @event = Serializer.Deserialize<Event>(receiveFrame);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
        }

        private long counter;

        private string readed = "";
        public void RequestReply()//ответ
        {         
            ThreadPool.QueueUserWorkItem(state =>
            {
                var server = new ZSocket(ZSocketType.REQ);
                // server.Connect("tcp://127.0.0.1:5555");
                server.Connect("tcp://192.168.0.56:5555");
                // int _text = Int32.Parse(Console.ReadLine());
                while (!_stopLoops)
                {
                    try
                    {                        
                        Console.WriteLine("Номер строки: ");
                      //  Thread.Sleep(2000);
                        readed = Console.ReadLine();
                        Row = Int32.Parse(readed);
                        Console.WriteLine("Номер колонки:");
                        //Thread.Sleep(2000);

                        readed = Console.ReadLine();
                        Col = Int32.Parse(readed);
                        var request = new MoveRequest
                        {
                            UserId = Id,
                            who = Who,
                            Rows = Row,
                            Cols = Col,
                            text = number,
                            Direction = new List<int> {
                                1,
                                _random.Next(),
                                _random.Next()
                            }
                        };
                        using (var responseStream = new MemoryStream())
                        {
                            Serializer.Serialize(responseStream, request);
                            server.Send(new ZFrame(responseStream.ToArray()));
                        }
                        using (var receiveFrame = server.ReceiveFrame())
                        {
                            var reply = Serializer.Deserialize<Response>(receiveFrame);
                            var mr = reply as MoveResponse;
                            number = mr.text;
                            
                            //if (++counter % 30000 == 0)
                                    //{
                                    //Console.WriteLine("Введите эхо");
                                    //string a = Console.ReadLine();
                                    counter++;
                                Console.WriteLine("Запрос {1}; Клиент: {0}; Значение:{2}", mr != null && mr.Ok, counter, mr.text);
                            //}

                            for (int j = 0; j < 9; j++)
                            {
                                if (j % 3 == 0)
                                    Console.WriteLine();
                                Console.Write(mr.pole[j].ToString() + ' ');
                            }
                            Console.WriteLine();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
        }

        public void OnEvent(Event e)
        {
            var dfe = e as DataFacadeEvent;
            if (dfe != null) dfe.State.ForEach(user => Console.WriteLine("onEvent"+user.Id));
        }
    }
}