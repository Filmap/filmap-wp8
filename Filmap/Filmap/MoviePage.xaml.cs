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

            // populate fields on movie page
            searchResultMovieLocal = (App.Current as App).searchResultMovie;
            txtMovieTitle.Text = searchResultMovieLocal.Title;
            txtMovieDuration.Text = searchResultMovieLocal.Runtime;
            txtMovieGenre.Text = searchResultMovieLocal.Genre;
            txtMovieYear.Text = searchResultMovieLocal.Year;
            txtMovieImdbScore.Text = searchResultMovieLocal.imdbRating + "/10 on IMDB";
            txtMoviePlot.Text = searchResultMovieLocal.Plot;
            txtMovieType.Text = searchResultMovieLocal.Type;
            txtMovieRated.Text = "Rated " + searchResultMovieLocal.Rated;
            labelActors.Text = searchResultMovieLocal.Actors;
            labelWriter.Text = searchResultMovieLocal.Director;
            labelDirector.Text = searchResultMovieLocal.Director;
            txtMovieReleased.Text = searchResultMovieLocal.Released;
            txtMovieLanguage.Text = searchResultMovieLocal.Language;
            txtMovieCountry.Text = searchResultMovieLocal.Country;
            txtMovieAwards.Text = searchResultMovieLocal.Awards;
            txtMovieMetascore.Text = searchResultMovieLocal.Metascore + " out of 100";
            txtMovieImdb.Text = searchResultMovieLocal.imdbRating + " out of 10";
            txtImdbVotes.Text = searchResultMovieLocal.imdbVotes;

            // get movie poster and show on movie page if there is a poster
            if (searchResultMovieLocal.Poster != "N/A")
            {
                BitmapImage imgPoster = new BitmapImage();
                imgPoster.UriSource = new Uri(searchResultMovieLocal.Poster);
                imgMoviePoster.Source = imgPoster;
            }

            // set pivot title
            pivotTitle.Title = searchResultMovieLocal.Title;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // save movie in your list to watch later
            SaveMovie();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // mark movie as watched
            SaveMovie(true);
            
        }

        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private async void SaveMovie(bool watched = false)
        {
            // instancia um cliente http para acessar a api
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((App.Current as App).filmapApiUrl);

            String lat = (App.Current as App).lat;
            String lng = (App.Current as App).lng;

            if (lat != null && lng != null)
            {
                // location is set, do nothing
            }
            else
            {
                Geolocator geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;

                var position = await geolocator.GetGeopositionAsync();

                lat = Convert.ToString(position.Coordinate.Point.Position.Latitude);
                lng = Convert.ToString(position.Coordinate.Point.Position.Longitude);
            }

            //MessageBox.Show(Regex.Match(searchResultMovieLocal.imdbID, @"\d+").Value);

            String omdbId = Regex.Match(searchResultMovieLocal.imdbID, @"\d+").Value;

            // zero for watch later, one for watching now (or watched)
            String watchedstatus = "0";

            if (watched)
            {
                watchedstatus = "1";
            }

            // prepara os parametros para mandar no post request
            var credentials = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("omdb", searchResultMovieLocal.imdbID), 
                new KeyValuePair<string, string>("watched", watchedstatus), // zero or one
                new KeyValuePair<string, string>("lat", lat),
                new KeyValuePair<string, string>("lng", lng)
            });

            // get user access token
            String token = (App.Current as App).accessToken;

            // faz um post request e espera o resultado asincronamente
            var response = await httpClient.PostAsync("/films?token=" + token, credentials);

            // transforma o resultado do request anterior uma string (geralmente json)
            var str = response.Content.ReadAsStringAsync().Result;

            // se o filme for adicionado com sucesso
            if (response.IsSuccessStatusCode)
            {
                // desativar o botao para nao poder adicionar novamente
                if (watched)
                {
                    // filme assistido, desativar botao assistir
                    button1.IsEnabled = false;
                    button1.Content = "Watched.";
                    button.IsEnabled = false;
                } else
                {
                    button.IsEnabled = false;
                    button.Content = "Ok, later. :)";
                }
                
                (App.Current as App).myMoviesList.Insert(0, searchResultMovieLocal);
            } else
            {
                // some error ocourred 
                MessageBox.Show("There was an error with your request. Please try again and if the problem persists, contact support.");
            }
        } 
    }
}