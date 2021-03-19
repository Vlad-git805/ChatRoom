using ComandClasses;
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
        int User_id;
        User user = new User();
        bool connect = false;
        public MainWindow(int id)
        {
            InitializeComponent();
            User_id = id;
            list.ItemsSource = messages;
            foreach (var item in ctx.Users)
            {
                if (item.Id == User_id)
                {
                    user.Name = item.Name;
                    user.Tag = item.Tag;
                }
            }
            Name_lable.Content = user.Name;
            Tag_lable.Content = user.Tag;
            Task.Run(() => Show_all_Contacts());
            Task.Run(() => Listen());
        }

        // адреса віддаленого хоста
        private static string remoteIPAddress = "127.0.0.1";
        // порт віддаленого хоста
        private static int remotePort = 8080;
        // створення об'єкту UdpClient для відправки даних
        UdpClient client = new UdpClient(0);

        Model1 ctx = new Model1();

        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();

        public Client_Command client_Command = new Client_Command();

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
                    if(connect == false)
                        First_connect();

                    Server_Command server_Command = (Server_Command)ByteArrayToObject(data);
                    client_Command.Name = server_Command.Name;
                    client_Command.Message = server_Command.Message;


                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (client_Command.Name == Name_lable.Content.ToString())
                        {
                            foreach (var item in ctx.Users)
                            {
                                if (item.Id == User_id)
                                {
                                    //item.Port = server_Command.iPEndPoint.Port.ToString();
                                    item.Port = server_Command.iPEndPoint.Port.ToString();
                                    client_Command.From_who_message = server_Command.iPEndPoint.Port.ToString();
                                }
                            }
                            ctx.SaveChanges();
                            user.Port = server_Command.iPEndPoint.Port.ToString();
                            
                        }
                    }));

                    string msg = client_Command.Message;

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        messages.Insert(0, new MessageInfo()
                        {
                            Name = client_Command.Name,
                            Time = DateTime.Now.ToShortTimeString(),
                            Text = msg
                        });
                    }));

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void First_connect()
        {
            client_Command.Name = Name_lable.Content.ToString();
            //Leave_button.IsEnabled = true;
            //Join_button.IsEnabled = false;
            SendMessage("<connect>");
            connect = true;
        }

        private void Show_all_Contacts()
        {
            //MessageBox.Show(ctx.Users.Count().ToString());

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                foreach (var item in ctx.Users)
                {
                    if(item.Tag != user.Tag)
                        Contact_list_box.Items.Add(item.Name + " " + item.Tag);
                }
            }));
        }

        private void Show_my_Contacts()
        {
            ////MessageBox.Show(ctx.Users.Count().ToString());

            //Application.Current.Dispatcher.Invoke(new Action(() =>
            //{
            //    foreach (var item in ctx.Users)
            //    {
            //        if (item.Tag != user.Tag)
            //            Contact_list_box.Items.Add(item.Name + " " + item.Tag);
            //    }
            //}));
        }

        private void SendMessage(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return;

            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(remoteIPAddress), remotePort);

            client_Command.Message = msg;
            byte[] data = ObjectToByteArray(client_Command);
            client.Send(data, data.Length, iPEndPoint);
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    client_Command.Name = Name_lable.Content.ToString();
        //    Leave_button.IsEnabled = false;
        //    Join_button.IsEnabled = true;
        //    SendMessage("<remove>");
        //}

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    client_Command.Name = Name_lable.Content.ToString();
        //    Leave_button.IsEnabled = true;
        //    Join_button.IsEnabled = false;
        //    SendMessage("<connect>");
        //}

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            client_Command.Name = Name_lable.Content.ToString();
            SendMessage(txtBox.Text);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            client_Command.Name = Name_lable.Content.ToString();
            SendMessage("<remove>");
            client.Close();
        }

        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        private void Refresh_contact_button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => Show_all_Contacts());
        }

        private void Add_friend_button_Click(object sender, RoutedEventArgs e)
        {
            if (Contact_list_box.SelectedItem != null)
            {
                client_Command.Name = Name_lable.Content.ToString();
                char[] wordsSplit = new char[] { ' ' };
                string[] words = Contact_list_box.SelectedItem.ToString().Split(wordsSplit, StringSplitOptions.RemoveEmptyEntries);
                string friend_tag = words[1];
                foreach (var item in ctx.Users)
                {
                    if (item.Tag == friend_tag)
                        client_Command.To_who_message = item.Port;
                }
                SendMessage($"<AddFriend>");
            }
        }

        private void All_contacts_lable_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Add_friend_button.Visibility = Visibility.Visible;
            Task.Run(() => Show_all_Contacts());
        }

        private void My_contacts_lable_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Add_friend_button.Visibility = Visibility.Hidden;
        }
    }
}
