using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Net.Http;

namespace Filmap
{
    public partial class RegisterPage : PhoneApplicationPage
    {
        private string address = "http://heavy-goat-5389.vagrantshare.com";

        public RegisterPage()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            /*button.IsEnabled = false;
            txtConfirmPassword.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            registerFormPanel.Visibility = Visibility.Collapsed;*/

            RegisterUser();
        }

        private async void RegisterUser()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(address);


            var credentials = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("name", txtName.Text),
                new KeyValuePair<string, string>("email", txtEmail.Text),
                new KeyValuePair<string, string>("password", txtPassword.Text),
                new KeyValuePair<string, string>("confirm_password", txtConfirmPassword.Text)
            });

            var response = await httpClient.PostAsync("/user", credentials);

            var str = response.Content.ReadAsStringAsync().Result;

            //Token t = JsonConvert.DeserializeObject<Token>(str);

            MessageBox.Show(str);
        }
    }
}