using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _02_ChatClient
{
    public partial class MainWindow : Window
    {
        // адреса віддаленого хоста
        private static string remoteIPAddress = "127.0.0.1";
        // порт віддаленого хоста
        private static int remotePort = 8080;
        // створення об'єкту UdpClient для відправки даних
        UdpClient client = new UdpClient(0);

        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();

        public MainWindow()
        {
            InitializeComponent();

            list.ItemsSource = messages;

            Task.Run(() => Listen());
        }

        private void Listen()
        {
            IPEndPoint iPEndPoint = null;
            while (true)
            {
                try
                {
                    byte[] data = client.Receive(ref iPEndPoint);

                    string msg = Encoding.UTF8.GetString(data);

                    char[] wordsSplit = new char[] { ' ' };
                    string[] words = msg.Split(wordsSplit, StringSplitOptions.RemoveEmptyEntries);
                    string[] newWords = new string[words.Length - 1];
                    for (int i = 1, j = 0; i < words.Length; i++, j++)
                    {
                        newWords[j] = words[i];
                    }

                    string name = words[0];

                    msg = String.Join(" ", newWords);

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        messages.Insert(0, new MessageInfo()
                        {
                            Name = name,
                            Time = DateTime.Now.ToShortTimeString(),
                            Text = msg
                        });
                    }));
                    if (msg == "Wheit please a fiew minutes to connect")
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            Leave_button.IsEnabled = false;
                            Join_button.IsEnabled = true;
                        }));
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SendMessage(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return;

            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(remoteIPAddress), remotePort);

            byte[] data = Encoding.UTF8.GetBytes(msg);
            client.Send(data, data.Length, iPEndPoint);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = Name_text_box.Text;
            Leave_button.IsEnabled = false;
            Join_button.IsEnabled = true;
            SendMessage(name + " <remove>");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Name_text_box.Text != "")
            {
                string name = Name_text_box.Text;
                Leave_button.IsEnabled = true;
                Join_button.IsEnabled = false;
                SendMessage(name + " <connect>");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string name = Name_text_box.Text;
            SendMessage(name + " " + txtBox.Text);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SendMessage("<remove>");
            client.Close();
        }
    }
}
