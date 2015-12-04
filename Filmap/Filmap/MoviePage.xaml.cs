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

            BitmapImage imgPoster = new BitmapImage();
            imgPoster.UriSource = new Uri(searchResultMovieLocal.Poster);
            imgMoviePoster.Source = imgPoster;
            pivotTitle.Title = searchResultMovieLocal.Title;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}