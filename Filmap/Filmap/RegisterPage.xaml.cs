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
        //private string address = "http://apifilmap.jovemexemplar.com";
        
        public RegisterPage()
        {
            InitializeComponent();
            //address = (App.Current as App).filmapApiUrl;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            /*button.IsEnabled = false;
            txtConfirmPassword.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            registerFormPanel.Visibility = Visibility.Collapsed;*/

            registerFormPanel.Visibility = Visibility.Collapsed;
            loadingGrid.Visibility = Visibility.Visible;
            RegisterUser();
        }

        private async void RegisterUser()
        {
            // instancia cliente http para acesso a api
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((App.Current as App).filmapApiUrl);

            // seta os parametros para envio a api e registro de usuario
            var credentials = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("name", txtName.Text),
                new KeyValuePair<string, string>("email", txtEmail.Text),
                new KeyValuePair<string, string>("password", txtPassword.Text),
                new KeyValuePair<string, string>("password_confirmation", txtConfirmPassword.Text)
            });

            // faz request ao servidor e espera resposta
            var response = await httpClient.PostAsync("/user", credentials);
            
            // pega o resultado do request e converte para string para verificacao
            // de status e para mostrar erros se necessario
            var str = response.Content.ReadAsStringAsync().Result;

            // sucesso no registro, 200 OK
            if(response.IsSuccessStatusCode)
            {   
                // ir para a pagina de login e passar o email como parametro para facilitar
                NavigationService.Navigate(new Uri("/LoginPage.xaml?email=" + txtEmail.Text, UriKind.Relative));

                NavigationService.GoBack();
            } else
            {
                // ocorreu um erro ao registrar usuario
                // mostrar o formulario novamente
                registerFormPanel.Visibility = Visibility.Visible;
                loadingGrid.Visibility = Visibility.Collapsed;
                txtConfirmPassword.Text = "";
                txtPassword.Text = "";

                // mostrar mensagem de errro. seria bom detalhar um pouco mais usando a resposta do servidor
                MessageBox.Show("There was an error during the registration process. Please make sure you have filled all the fields and try again." + str);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // usuario ja tem registro, ir para a pagina de login
            NavigationService.GoBack();
        }
    }
}