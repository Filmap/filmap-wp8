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
using Filmap.Models;
using System.Threading.Tasks;

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


            if ((App.Current as App).myMoviesList.Contains(movie))
            {
                ChangeButtonsStatuses(movie).ConfigureAwait(true);
            }


            // get movie poster and show on movie page if there is a poster
            if (movie.Poster != "N/A")
            {
                BitmapImage imgPoster = new BitmapImage();
                imgPoster.UriSource = new Uri(movie.Poster);
                imgMoviePoster.Source = imgPoster;
            }

            // set pivot title
            pivotMovie.Title = movie.Title;

            progressLoadingMovie.Visibility = Visibility.Collapsed;
            pivotMovie.Visibility = Visibility.Visible;
        }

        private async Task ChangeButtonsStatuses(Movie movie)
        {
            //MessageBox.Show("movie");

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((App.Current as App).filmapApiUrl);

            var response = await httpClient.GetAsync("/films/" + movie.imdbID + "?token=" + (App.Current as App).accessToken);
            var str = response.Content.ReadAsStringAsync().Result;

            try
            {
                FilmapFilm film = JsonConvert.DeserializeObject<FilmapFilm>(str);

                if (film.watched == 1)
                {
                    button1.Content = "Watched";
                    button1.IsEnabled = false;
                    button.IsEnabled = false;
                }
                else if (film.watched == 0)
                {
                    button.IsEnabled = false;
                    button.Content = "Watch later";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

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

            // get user access token
            String token = (App.Current as App).accessToken;

            if (watched)
            {
                if ((App.Current as App).myMoviesList.Contains(currentDisplayMovie))
                {
                    var resp = await httpClient.PostAsync("/films/" + currentDisplayMovie.imdbID + "/watch?token=" + token, null);

                    //MessageBox.Show(resp.StatusCode.ToString());

                    /*
                    if (resp.IsSuccessStatusCode)
                    {
                    */
                        button1.IsEnabled = false;
                        button1.Content = "Watched.";
                        return;
                    /*
                    }
                    */

                }

                watchedstatus = "1";
                
            }

            // prepara os parametros para mandar no post request
            var credentials = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("omdb", currentDisplayMovie.imdbID), 
                new KeyValuePair<string, string>("watched", watchedstatus), // zero or one
                new KeyValuePair<string, string>("lat", lat),
                new KeyValuePair<string, string>("lng", lng)
            });

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