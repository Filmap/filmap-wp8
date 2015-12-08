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
using Newtonsoft.Json;

namespace Filmap
{
    public partial class MoviePage : PhoneApplicationPage
    {
        private Movie currentDisplayMovie;

        public MoviePage()
        {
            InitializeComponent();
        }

        private void PopulateMovieFields(Movie movie)
        {
            txtMovieTitle.Text = movie.Title;
            txtMovieDuration.Text = movie.Runtime;
            txtMovieGenre.Text = movie.Genre;
            txtMovieYear.Text = movie.Year;
            txtMovieImdbScore.Text = movie.imdbRating + "/10 on IMDB";
            txtMoviePlot.Text = movie.Plot;
            txtMovieType.Text = movie.Type;
            txtMovieRated.Text = "Rated " + movie.Rated;
            labelActors.Text = movie.Actors;
            labelWriter.Text = movie.Director;
            labelDirector.Text = movie.Director;
            txtMovieReleased.Text = movie.Released;
            txtMovieLanguage.Text = movie.Language;
            txtMovieCountry.Text = movie.Country;
            txtMovieAwards.Text = movie.Awards;
            txtMovieMetascore.Text = movie.Metascore + " out of 100";
            txtMovieImdb.Text = movie.imdbRating + " out of 10";
            txtImdbVotes.Text = movie.imdbVotes;

            // get movie poster and show on movie page if there is a poster
            if (movie.Poster != "N/A")
            {
                BitmapImage imgPoster = new BitmapImage();
                imgPoster.UriSource = new Uri(movie.Poster);
                imgMoviePoster.Source = imgPoster;
            }

            // set pivot title
            pivotTitle.Title = movie.Title;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dic = NavigationContext.QueryString;
            if (dic.ContainsKey("omdbid"))
            {
                GetMovie(dic["omdbid"]);
            } else
            {
                currentDisplayMovie = (App.Current as App).searchResultMovie;
                PopulateMovieFields(currentDisplayMovie);
            }
        }

        private async void GetMovie(String imdbid)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((App.Current as App).omdbApiUrl);

            var response = await httpClient.GetAsync("/?i=" + imdbid + "&y=&plot=short&r=json");
            var str = response.Content.ReadAsStringAsync().Result;

            try
            {
                Movie obj = JsonConvert.DeserializeObject<Movie>(str);
                currentDisplayMovie = obj;
                PopulateMovieFields(obj);
            } catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("There was an error in your request. Try again with a different movie.");
                // erro ao popular fields go back
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();

            }
            


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
                await (App.Current as App).GetUserLocation();
            }

            //MessageBox.Show(Regex.Match(searchResultMovieLocal.imdbID, @"\d+").Value);

            String omdbId = Regex.Match(currentDisplayMovie.imdbID, @"\d+").Value;

            // zero for watch later, one for watching now (or watched)
            String watchedstatus = "0";

            if (watched)
            {
                watchedstatus = "1";
            }

            // prepara os parametros para mandar no post request
            var credentials = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("omdb", currentDisplayMovie.imdbID), 
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
                
                (App.Current as App).myMoviesList.Insert(0, currentDisplayMovie);
            } else
            {
                // some error ocourred 
                MessageBox.Show("There was an error with your request. Please try again and if the problem persists, contact support.");
            }
        } 
    }
}