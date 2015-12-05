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
using System.Text;
using Newtonsoft.Json;

namespace Filmap
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private string address = "http://heavy-goat-5389.vagrantshare.com";

        public LoginPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            login();
        }

        private async void login()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(address);


            var credentials = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("email", "meloivanilson@gmail.com"),
                new KeyValuePair<string, string>("password", "password")
            });

            var response = await httpClient.PostAsync("/authenticate", credentials);

            var str = response.Content.ReadAsStringAsync().Result;

            Token t = JsonConvert.DeserializeObject<Token>(str);

            MessageBox.Show(t.token);
        }
    }
}