using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Filmap.Resources;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace Filmap
{
    public partial class MainPage : PhoneApplicationPage
    {
        private string address = "http://www.omdbapi.com";
        public ObservableCollection<Movie> myMoviesList = new ObservableCollection<Movie>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            Movie m = new Movie();
            m.Title = "TEST TITLE";
            m.Plot = "test plot";

            myMoviesList.Add(m);

            myMoviesDisplayList.ItemsSource = myMoviesList;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //moviesList.GetType();
            //MessageBox.Show(moviesList.GetValue().ToString());
            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchMovie(txtSearch.Text);
        }

        private async void SearchMovie(String query)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(address);

            var response = await httpClient.GetAsync("/?t=" + query + "&y=&plot=short&r=json");
            var str = response.Content.ReadAsStringAsync().Result;

            Movie obj = JsonConvert.DeserializeObject<Movie>(str);

            labelMovieTitle.Text = obj.Title;
            labelMovieDate.Text = obj.Year;
            labelMovieGenre.Text = obj.Genre;
            labelMovieDuration.Text = obj.Runtime;
            labelMovieTags.Text = obj.Metascore;
            labelMoviePlot.Text = obj.Plot;

            labelImdbScore.Text = "IMDB Score: " + obj.imdbRating;
            
            
            BitmapImage img = new BitmapImage();
            img.UriSource = new Uri(obj.Poster);
            imgMoviePoster.Source = img;

            myMoviesList.Add(obj);
            

            //MessageBox.Show(obj.Title + " - " + obj.Year);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}