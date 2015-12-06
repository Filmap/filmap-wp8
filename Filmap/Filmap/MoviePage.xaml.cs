using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.Net.Http;
using Windows.Devices.Geolocation;
using System.Text.RegularExpressions;

namespace Filmap
{
    public partial class MoviePage : PhoneApplicationPage
    {
        public Movie searchResultMovieLocal;

        public MoviePage()
        {
            InitializeComponent();
            searchResultMovieLocal = (App.Current as App).searchResultMovie;
            txtMovieTitle.Text = searchResultMovieLocal.Title;
            txtMovieDuration.Text = searchResultMovieLocal.Runtime;
            txtMovieGenre.Text = searchResultMovieLocal.Genre;
            txtMovieYear.Text = searchResultMovieLocal.Year;
            txtMovieImdbScore.Text = searchResultMovieLocal.imdbRating;
            txtMoviePlot.Text = searchResultMovieLocal.Plot;


            if (searchResultMovieLocal.Poster != "N/A")
            {
                BitmapImage imgPoster = new BitmapImage();
                imgPoster.UriSource = new Uri(searchResultMovieLocal.Poster);
                imgMoviePoster.Source = imgPoster;
            }

            pivotTitle.Title = searchResultMovieLocal.Title;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SaveMovie();
        }

        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private async void SaveMovie()
        {
            // instancia um cliente http para acessar a api
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((App.Current as App).filmapApiUrl);

            // instancia geolocalizador para pegar posicao do usuario
            Geolocator geo = new Geolocator();
            geo.DesiredAccuracy = PositionAccuracy.High;
            var position = await geo.GetGeopositionAsync();

            // get latitude and longitude
            String lat = Convert.ToString(position.Coordinate.Point.Position.Latitude);
            String lgn = Convert.ToString(position.Coordinate.Point.Position.Longitude);

            //MessageBox.Show(Regex.Match(searchResultMovieLocal.imdbID, @"\d+").Value);

            String omdbId = Regex.Match(searchResultMovieLocal.imdbID, @"\d+").Value;

            

            // prepara os parametros para mandar no post request
            var credentials = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("omdb", searchResultMovieLocal.imdbID), 
                new KeyValuePair<string, string>("watched", "1"), // zero or one
                new KeyValuePair<string, string>("lat", lat),
                new KeyValuePair<string, string>("lng", lgn)
            });

            String token = (App.Current as App).accessToken;

            // faz um post request e espera o resultado asincronamente
            var response = await httpClient.PostAsync("/films?token=" + token, credentials);

            // transforma o resultado do request anterior uma string (geralmente json)
            var str = response.Content.ReadAsStringAsync().Result;

            // se o filme for adicionado com sucesso
            if (response.IsSuccessStatusCode)
            {
                // desativar o botao para nao poder adicionar novamente
                button1.IsEnabled = false;
                button1.Content = "Watched";
            }

            // mostrar mensagem
            MessageBox.Show(str + " | " + response.StatusCode);

            /*
            // se o status http for 200 OK
            if (response.IsSuccessStatusCode)
            {
                // deserializa o json num objeto Token 
                Token t = JsonConvert.DeserializeObject<Token>(str);
                // mostra o token na tela
                

                // aqui tem que salvar o token no app e no banco de dados
                // para poder acessar a api depois com o token

            }
            else
            {
                // ocorreu algum erro na autenticacao
                // o servidor pode estar offline ou as credenciais invalidas
                // informa ao usuario
                MessageBox.Show("ocorreu um erro na autenticacao");
            }
            */
        } 
    }
}