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
using ProtocolLibrary.Messaging;
using ZeroMQ;

namespace ConsoleZMQClient
{
    public partial class ClientWindows : Form
    {
        private char who;
        private static volatile bool _stopLoops;
        private readonly Random _random = new Random();
        public Guid Id { get; set; }
        public int Who { get; set; }
        public int Row = 0;
        public int Col = 0;
        public int number;
        public bool gameStarted = false;
        private string _port1, _port2;
        Dictionary<int, char> convert = new Dictionary<int, char>();
        private string configIP = "";
        public void Dispose()
        {
            _stopLoops = true;
        }
        public ClientWindows(string port1, string port2)
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + @"\..\..\config.txt");
            configIP = sr.ReadLine();
            if ((configIP == null) || (configIP.Length < 5))
                configIP = "127.0.0.1";
            convert.Add(0, '0');
            convert.Add(1, 'X');
            Id = Guid.NewGuid();
            InitializeComponent();
            _port1 = port1;
            _port2 = port2;
        }
        public void PubSub()//подписываемся
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                var server = new ZSocket(ZSocketType.SUB);
                server.Connect("tcp://" + configIP + ":" + _port1);
                //tcp://127.0.0.1:5555
                //192.168.0.56
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
                //Thread.Sleep(1000);
            });
        }

        private long counter;

        private string readed = "";

        public void PutOnField(int row, int col)//ответ
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                var server = new ZSocket(ZSocketType.REQ);
                // server.Connect("tcp://127.0.0.1:5555");
                server.Connect("tcp://192.168.0.56:" + _port2);
                // int _text = Int32.Parse(Console.ReadLine());

                while (!_stopLoops)
                {


                    try
                    {
                        var request = new MoveRequest
                        {
                            UserId = Id,
                            who = Who,
                            Rows = row,
                            Cols = col,
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
                                switch (j)
                                {
                                    case 0:
                                        button1.Text = mr.pole[j].ToString();
                                        //if (mr.pole[j].ToString() != comboBox1.SelectedText)
                                        //    button1.BackColor = Color.Red;
                                        //else
                                        //    button1.BackColor = Color.Chartreuse;
                                        break;
                                    case 1:
                                        button2.Text = mr.pole[j].ToString();
                                        //if (mr.pole[j].ToString() != comboBox1.SelectedText)
                                        //    button2.BackColor = Color.Red;
                                        //else
                                        //    button2.BackColor = Color.Chartreuse;
                                        break;
                                    case 2:
                                        button3.Text = mr.pole[j].ToString();
                                        //if (mr.pole[j].ToString() != comboBox1.SelectedText)
                                        //    button3.BackColor = Color.Red;
                                        //else
                                        //    button3.BackColor = Color.Chartreuse;
                                        break;
                                    case 3:
                                        button4.Text = mr.pole[j].ToString();
                                        //if (mr.pole[j].ToString() != comboBox1.SelectedText)
                                        //    button4.BackColor = Color.Red;
                                        //else
                                        //    button4.BackColor = Color.Chartreuse;
                                        break;
                                    case 4:
                                        button5.Text = mr.pole[j].ToString();
                                        //if (mr.pole[j].ToString() != comboBox1.SelectedText)
                                        //    button5.BackColor = Color.Red;
                                        //else
                                        //    button5.BackColor = Color.Chartreuse;
                                        break;
                                    case 5:
                                        button6.Text = mr.pole[j].ToString();
                                        //if (mr.pole[j].ToString() != comboBox1.SelectedText)
                                        //    button6.BackColor = Color.Red;
                                        //else
                                        //    button6.BackColor = Color.Chartreuse;
                                        break;
                                    case 6:
                                        button7.Text = mr.pole[j].ToString();
                                        //if (mr.pole[j].ToString() != comboBox1.SelectedText)
                                        //    button7.BackColor = Color.Red;
                                        //else
                                        //    button7.BackColor = Color.Chartreuse;
                                        break;
                                    case 7:
                                        button8.Text = mr.pole[j].ToString();
                                        //if (mr.pole[j].ToString() != comboBox1.SelectedText)
                                        //    button8.BackColor = Color.Red;
                                        //else
                                        //    button8.BackColor = Color.Chartreuse;
                                        break;
                                    case 8:
                                        button9.Text = mr.pole[j].ToString();
                                        //if (mr.pole[j].ToString() != comboBox1.SelectedText)
                                        //    button9.BackColor = Color.Red;
                                        //else
                                        //    button9.BackColor = Color.Chartreuse;
                                        break;
                                }
                            }
                            Console.WriteLine();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    Thread.Sleep(1000);
                }
            });
        }

        public List<char> comPare;
        public void RequestReply()//ответ
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                var server = new ZSocket(ZSocketType.REQ);
                server.Connect("tcp://192.168.0.56:" + _port2);
                while (!_stopLoops)
                {
                    try
                    {
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
                            counter++;
                            int counterX = 0;
                            int counterO = 0;
                            Console.WriteLine("Номер запроса {1} закончился с: {0}", mr != null && mr.Ok, counter);
                            for (int j = 0; j < 9; j++)
                            {
                                if (mr.pole[j].ToString() == "X")
                                    counterX++;
                                else if (mr.pole[j].ToString() == "0")
                                    counterO++;
                                switch (j)
                                {
                                    case 0:
                                        button1.Text = mr.pole[j].ToString();
                                        if ((button1.Text == "X") || (button1.Text == "0"))
                                        {
                                            if (button1.Text == convert[Who].ToString())
                                                button1.BackColor = Color.Green;
                                            else if (button1.Text != convert[Who].ToString())
                                                button1.BackColor = Color.Red;
                                            button1.Enabled = false;
                                        }
                                        break;
                                    case 1:
                                        button2.Text = mr.pole[j].ToString();
                                        if ((button2.Text == "X") || (button2.Text == "0"))
                                        {
                                            if (button2.Text == convert[Who].ToString())
                                                button2.BackColor = Color.Green;
                                            else if (button2.Text != convert[Who].ToString())
                                                button2.BackColor = Color.Red;
                                            button2.Enabled = false;
                                        }
                                        break;
                                    case 2:
                                        button3.Text = mr.pole[j].ToString();
                                        if ((button3.Text == "X") || (button3.Text == "0"))
                                        {
                                            if (button3.Text == convert[Who].ToString())
                                                button3.BackColor = Color.Green;
                                            else if (button3.Text != convert[Who].ToString())
                                                button3.BackColor = Color.Red;
                                            button3.Enabled = false;
                                        }
                                        break;
                                    case 3:
                                        button4.Text = mr.pole[j].ToString();
                                        if ((button4.Text == "X") || (button4.Text == "0"))
                                        {
                                            if (button4.Text == convert[Who].ToString())
                                                button4.BackColor = Color.Green;
                                            else if (button4.Text != convert[Who].ToString())
                                                button4.BackColor = Color.Red;
                                            button4.Enabled = false;
                                        }
                                        break;
                                    case 4:
                                        button5.Text = mr.pole[j].ToString();
                                        if ((button5.Text == "X") || (button5.Text == "0"))
                                        {
                                            if (button5.Text == convert[Who].ToString())
                                                button5.BackColor = Color.Green;
                                            else if (button5.Text != convert[Who].ToString())
                                                button5.BackColor = Color.Red;
                                            button5.Enabled = false;
                                        }
                                        break;
                                    case 5:
                                        button6.Text = mr.pole[j].ToString();
                                        if ((button6.Text == "X") || (button6.Text == "0"))
                                        {
                                            if (button6.Text == convert[Who].ToString())
                                                button6.BackColor = Color.Green;
                                            else if (button6.Text != convert[Who].ToString())
                                                button6.BackColor = Color.Red;
                                            button6.Enabled = false;
                                        }
                                        break;
                                    case 6:
                                        button7.Text = mr.pole[j].ToString();
                                        if ((button7.Text == "X") || (button7.Text == "0"))
                                        {
                                            if (button7.Text == convert[Who].ToString())
                                                button7.BackColor = Color.Green;
                                            else if (button7.Text != convert[Who].ToString())
                                                button7.BackColor = Color.Red;
                                            button7.Enabled = false;
                                        }
                                        break;
                                    case 7:
                                        button8.Text = mr.pole[j].ToString();
                                        if ((button8.Text == "X") || (button8.Text == "0"))
                                        {
                                            if (button8.Text == convert[Who].ToString())
                                                button8.BackColor = Color.Green;
                                            else if (button8.Text != convert[Who].ToString())
                                                button8.BackColor = Color.Red;
                                            button8.Enabled = false;
                                        }
                                        break;
                                    case 8:
                                        button9.Text = mr.pole[j].ToString();
                                        if ((button9.Text == "X") || (button9.Text == "0"))
                                        {
                                            if (button9.Text == convert[Who].ToString())
                                                button9.BackColor = Color.Green;
                                            else if (button9.Text != convert[Who].ToString())
                                                button9.BackColor = Color.Red;
                                            button9.Enabled = false;
                                        }
                                        break;
                                }
                            }
                            if (((counterX > counterO) && (Who == 1)) || ((counterX == counterO) && (Who == 0)))
                            {
                                labelStat.Text = "Ожидайте ход";
                                //disableButtons();
                            }
                            else
                                labelStat.Text = "Ваш ход";

                            if (mr.stata == GameStatus.WinZero)
                            {
                                MessageBox.Show("Нолики выиграли!");
                                _stopLoops = true;
                                break;
                            }
                            if ((mr.stata == GameStatus.NobodyWins) && (counterX + counterO == 9))
                            {
                                MessageBox.Show("Ничья!");
                                _stopLoops = true;
                                break;
                            }
                            if (mr.stata == GameStatus.WinX)
                            {
                                MessageBox.Show("Крестики выиграли!");
                                _stopLoops = true;
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    //Thread.Sleep(1000);
                }
            });
        }

        public void OnEvent(Event e)
        {
            var dfe = e as DataFacadeEvent;
            if (dfe != null) dfe.State.ForEach(user => Console.WriteLine("onEvent" + user.Id));
        }
        private void ClientWindows_Load(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            label2.Visible = false;
            buttonStart.Visible = false;
            labelStat.Visible = false;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            labelStat.Visible = true;
            PubSub();
            RequestReply();
        }


        
        private void disableButtons()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Row = 1;
            Col = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Row = 1;
            Col = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Row = 1;
            Col = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Row = 2;
            Col = 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Row = 2;
            Col = 2;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Row = 2;
            Col = 3;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Row = 3;
            Col = 1;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Row = 3;
            Col = 2;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Row = 3;
            Col = 3;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            label2.Visible = true;
            buttonStart.Visible = true;
            Who = comboBox1.SelectedIndex;
            comboBox1.DropDownHeight = 1;
            ConnectToGW("tcp://127.0.0.1:5555");
        }



        public void ConnectToGW(string args)//
        {
           // args =  "tcp://127.0.0.1:5555" ;
            string endpoint = args;
            // Create
            using (var context = new ZContext())
            using (var requester = new ZSocket(context, ZSocketType.REQ))
            {
                // Connect
                requester.Connect(endpoint);
                //for (int n = 0; n < 1; n++)
                //{
                string requestText = Who.ToString() + "|" + Id;
                Console.WriteLine("Отправил, что я: {0} и мой Id: {1}", Who, Id);

                // Send
                requester.Send(new ZFrame(requestText));


                // Receive
                using (ZFrame reply = requester.ReceiveFrame())
                {
                    roomBox.Text = reply.ReadString();
                    _port2 = roomBox.Text;
                    _port1 = (Int32.Parse(_port1) + 1).ToString();
                    Console.WriteLine(" Получил: {0} {1}", requestText, roomBox.Text);
                }
                //roomBox.Text = reply.ReadString();
                //_port1 = requestText;
                //}
                requester.Close();
                context.Dispose();
            }
        }

    }
}
