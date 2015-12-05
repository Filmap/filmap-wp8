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
        public ObservableCollection<Search> searchResultList = new ObservableCollection<Search>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            
            // Binda as ObservableCollections com as listas na tela pra mostrar.
            myMoviesDisplayList.ItemsSource = myMoviesList;
            searchMovieDisplayList.ItemsSource = searchResultList;


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
            //NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.Relative));

        }

        private async void SearchMovie(String query)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(address);

            var response = await httpClient.GetAsync("?s=" + query + "&r=json");
            var str = response.Content.ReadAsStringAsync().Result;

            SearchList obj = JsonConvert.DeserializeObject<SearchList>(str);


            searchResultList = obj.Search;

            searchMovieDisplayList.ItemsSource = searchResultList;

            //MessageBox.Show(Convert.ToString(searchResultList.Count));


            //searchMovieDisplayList.ItemsSource = obj.Search;

            /*
            SearchResultList srl = new SearchResultList();

            SearchResult sr3 = new SearchResult();
            sr3.Title = "qqqqqqq";
            sr3.Year = "3214";

            srl.Results.Add(sr3);

            MessageBox.Show(srl.Results.First().Title);

            searchMovieDisplayList.ItemsSource = obj.Results;

            //sear = srl.Results;




            // MessageBox.Show(obj.Results.ToString());

            /*
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(address);

            var response = await httpClient.GetAsync("/?t=" + query + "&y=&plot=short&r=json");
            var str = response.Content.ReadAsStringAsync().Result;

            Movie obj = JsonConvert.DeserializeObject<Movie>(str);


            (App.Current as App).searchResultMovie = obj;

            NavigationService.Navigate(new Uri("/MoviePage.xaml", UriKind.Relative));
            */

            //this.test = obj.Title;

            //MessageBox.Show(this.test);

            /*
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
            */


            //MessageBox.Show(obj.Title + " - " + obj.Year);
        }

        private void searchMovieDisplayList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Search s = (Search) searchMovieDisplayList.SelectedItem;
            //MessageBox.Show(s.Title);

            OpenMovie(s.imdbID);
            
        }

        private async void OpenMovie(String imdbid)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(address);

            var response = await httpClient.GetAsync("/?i=" + imdbid + "&y=&plot=short&r=json");
            var str = response.Content.ReadAsStringAsync().Result;

            Movie obj = JsonConvert.DeserializeObject<Movie>(str);


            (App.Current as App).searchResultMovie = obj;

            //MessageBox.Show(str);

            NavigationService.Navigate(new Uri("/MoviePage.xaml", UriKind.Relative));
        }


        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchMovie(txtSearch.Text);
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