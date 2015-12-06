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

        public LoginPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // esconde form e mostra loading bar
            ContentPanel.Visibility = Visibility.Collapsed;
            loadingGrid.Visibility = Visibility.Visible;

            // metodo asincrono para login
            login();            
        }

        private async void login()
        {
            // instancia um cliente http para acessar a api
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((App.Current as App).filmapApiUrl);

            // prepara os parametros para mandar no post request
            var credentials = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("email", txtEmail.Text),
                new KeyValuePair<string, string>("password", txtPassword.Password)
            });

            // faz um post request e espera o resultado asincronamente
            var response = await httpClient.PostAsync("/authenticate", credentials);
            
            // transforma o resultado do request anterior uma string (geralmente json)
            var str = response.Content.ReadAsStringAsync().Result;

            // se o status http for 200 OK (sucesso)
            if (response.IsSuccessStatusCode)
            {   
                // deserializa o json num objeto Token 
                Token t = JsonConvert.DeserializeObject<Token>(str);

                // salva o token ma memoria do aplicativo para usar depois
                (App.Current as App).accessToken = t.token;

                // vai para a pagina inicial do app
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                // ocorreu algum erro na autenticacao
                // o servidor pode estar offline ou as credenciais invalidas

                // mostra form, esconde loading bar e apaga campo de senha
                ContentPanel.Visibility = Visibility.Visible;
                loadingGrid.Visibility = Visibility.Collapsed;
                txtPassword.Password = "";


                // informa ao usuario
                MessageBox.Show("There was an error during authentication, please try again.");
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // remove all of the navigation stack, so if the user doesn't want
            // to log in, go back
            while (NavigationService.CanGoBack)
                NavigationService.RemoveBackEntry();

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // usuario nao tem cadastro, mostrar tela de registro
            NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.Relative));
        }
    }
}