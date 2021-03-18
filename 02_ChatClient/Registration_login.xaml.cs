using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _02_ChatClient
{
    public partial class Registration_login : Window
    {
        //public UserServices UserServices;
        //public int User_id = -1;
        //public MainWindow()
        //{
        //    InitializeComponent();
        //    UserServices = new UserServices();
        //}

        //private void Text_block_change_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (text_block_change.Text == "I already have an account")
        //    {
        //        Regist_login_button.Content = "Login";
        //        text_block_change.Text = "I dont have an account";
        //    }
        //    else if (text_block_change.Text == "I dont have an account")
        //    {
        //        Regist_login_button.Content = "Registr";
        //        text_block_change.Text = "I already have an account";
        //    }
        //}

        //private void Regist_login_Click(object sender, RoutedEventArgs e)
        //{
        //    if (Name_text_box.Text != "" && Password_password_box.Password != "")
        //    {
        //        if (text_block_change.Text == "I already have an account")
        //        {
        //            foreach (var item in UserServices.Get_all_Users())
        //            {
        //                if (item.Name == Name_text_box.Text)
        //                {
        //                    MessageBox.Show("This name is already exist! Choise another name.");
        //                    return;
        //                }
        //            }
        //            Users newUser = new Users() { Name = Name_text_box.Text, Password = ComputeSha256Hash(Password_password_box.Password), IsAdmin = false };
        //            UserServices.AddUsers(newUser);
        //            UserServices.Save();
        //            MessageBox.Show("You create new account. Now login please!");
        //            if (text_block_change.Text == "I already have an account")
        //            {
        //                Regist_login_button.Content = "Login";
        //                text_block_change.Text = "I dont have an account";
        //            }
        //        }
        //        else if (text_block_change.Text == "I dont have an account")
        //        {
        //            foreach (var item in UserServices.Get_all_Users())
        //            {
        //                if (item.Name == Name_text_box.Text && item.Password == ComputeSha256Hash(Password_password_box.Password))
        //                {
        //                    User_id = item.Id;
        //                    if (item.IsAdmin == true)
        //                    {
        //                        AdminUI adminUI = new AdminUI();
        //                        adminUI.Show();
        //                    }
        //                    else
        //                    {
        //                        UserUI userUI = new UserUI(User_id);
        //                        userUI.Show();
        //                    }
        //                }
        //            }
        //            if (User_id == -1)
        //            {
        //                MessageBox.Show("Error! Incorrect name or password!");
        //            }
        //        }
        //    }
        //}

        //static string ComputeSha256Hash(string rawData)
        //{
        //    using (SHA256 sha256Hash = SHA256.Create())
        //    {
        //        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        //        StringBuilder builder = new StringBuilder();
        //        for (int i = 0; i < bytes.Length; i++)
        //        {
        //            builder.Append(bytes[i].ToString("x2"));
        //        }
        //        return builder.ToString();
        //    }
        //}

        //private void Name_text_box_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (Name_text_box.Text != "" && Password_password_box.Password != "")
        //    {
        //        Regist_login_button.IsEnabled = true;
        //    }
        //    else if (Name_text_box.Text == "" || Password_password_box.Password == "")
        //    {
        //        Regist_login_button.IsEnabled = false;
        //    }
        //}

        //private void Password_password_box_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    if (Name_text_box.Text != "" && Password_password_box.Password != "")
        //    {
        //        Regist_login_button.IsEnabled = true;
        //    }
        //    else if (Name_text_box.Text == "" || Password_password_box.Password == "")
        //    {
        //        Regist_login_button.IsEnabled = false;
        //    }
        //}

        //private void Text_block_change_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    text_block_change.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBBF1EB"));
        //}

        //private void Text_block_change_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    text_block_change.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF31C2B0"));
        //}
    }
}
