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
using Windows.Devices.Geolocation;

namespace Filmap
{
    public partial class MainPage : PhoneApplicationPage
    {
        //public ObservableCollection<Movie> myMoviesList = new ObservableCollection<Movie>();
        public ObservableCollection<Search> searchResultList = new ObservableCollection<Search>();
        //public ObservableCollection<NearbyMovie> nearbyMovies = new ObservableCollection<NearbyMovie>();



        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            // Binda as ObservableCollections com as listas na tela pra mostrar.




            // call method as standard method
            //

            StartAndPopulateLists();
            
        }

        public async void StartAndPopulateLists()
        {
            myMoviesDisplayList.ItemsSource = (App.Current as App).myMoviesList;
            searchMovieDisplayList.ItemsSource = searchResultList;
            aroundMeList.ItemsSource = (App.Current as App).nearbyMovies;

            PopulateMyMoviesList();

            await (App.Current as App).GetUserLocation();

            PopulateNearbyMoviesList();
        }

        // popula a lista de filmes do usuario
        public async void PopulateMyMoviesList()
        {
            // cliente http para acessar a filmap api
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((App.Current as App).filmapApiUrl);

            // pega todos os filmes marcados pelo usuario atual
            var response = await httpClient.GetAsync("/films?token=" + (App.Current as App).accessToken);
            var str = response.Content.ReadAsStringAsync().Result;

            try
            {
                // tenta deserializar os filmes do user numa colecao
                ObservableCollection<UserMovie> omdbUserMovies = JsonConvert.DeserializeObject<ObservableCollection<UserMovie>>(str);
                
                // instancia um segundo httpclient para acessar a api omdb
                HttpClient httpClient2 = new HttpClient();
                httpClient2.BaseAddress = new Uri((App.Current as App).omdbApiUrl);

                // inverte a ordem dos resultados para mostrar os mais recentes no topo da lista
                omdbUserMovies = new ObservableCollection<UserMovie>(omdbUserMovies.Reverse());

                // de um em um na lista de filmes do usuario
                foreach (UserMovie movie in omdbUserMovies) {
                    // pega detalhes sobre o filme na api omdb no formato json
                    var omdbresponse = await httpClient2.GetAsync("/?i=" + movie.omdb + "&plot=short&r=json");
                    var omdbstr = omdbresponse.Content.ReadAsStringAsync().Result;

                    try
                    {   
                        // tenta deserializar o json retornado em um objeto do tipo movie
                        Movie m = JsonConvert.DeserializeObject<Movie>(omdbstr);
                        // se deserializar com sucesso, insere o objeto na lista para mostrar na tela

                        // checa se o objeto resposta tem pelo menos um titulo para mostrar na lista
                        if (m.Title != null)
                        {
                            (App.Current as App).myMoviesList.Add(m);
                        }
                        
                    } catch(Exception e)
                    {
                        // nao deu para deserializar o filme, printa erro no console
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Newtonsoft.Json.JsonSerializationException e)
            {
                // nao deu para deserializar a lista de filmes, printa erro no console
                Console.WriteLine(e.Message);
            }
        }


        // popula a lista de filmes assistidos proximo ao usuario
        private async void PopulateNearbyMoviesList()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((App.Current as App).filmapApiUrl);

            //if ((App.Current as App).GetUserLocation().)

            String lat = (App.Current as App).lat;
            String lng = (App.Current as App).lng;

            if (lat != null && lng != null)
            {
                // location is set, do nothing
            } else
            {
                await (App.Current as App).GetUserLocation();
            }

            // 15 eh o raio em kms
            var response = await httpClient.GetAsync("/near/15," + lat + "," + lng);
            var str = response.Content.ReadAsStringAsync().Result;

            //MessageBox.Show(lat + "|" + lng);

            try
            {
                ObservableCollection<NearbyMovie> nearbyMv = JsonConvert.DeserializeObject<ObservableCollection<NearbyMovie>>(str);
                //aroundMeList.ItemsSource = nearbyMovies;

                foreach(NearbyMovie movie in nearbyMv)
                {
                    (App.Current as App).nearbyMovies.Add(movie);   
                    //MessageBox.Show(movie.omdb);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            //searchResultList = obj.Search;

            //MessageBox.Show(nearbyMovies.First().omdb);


            // searchMovieDisplayList.ItemsSource = searchResultList;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            // verifica se existe um token salvo na aplicacao
            // caso contrario, mostrar pagina de login/cadastro
            if ((App.Current as App).accessToken == null || (App.Current as App).accessToken == "")
            {
                // show login page
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            } else
            {
                // user logado                
            }

        }

        private async void SearchMovie(String query)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri((App.Current as App).omdbApiUrl);

            var response = await httpClient.GetAsync("?s=" + query + "&r=json");
            var str = response.Content.ReadAsStringAsync().Result;

            SearchList obj = JsonConvert.DeserializeObject<SearchList>(str);

            searchResultList = obj.Search;
            searchMovieDisplayList.ItemsSource = searchResultList;
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
            httpClient.BaseAddress = new Uri((App.Current as App).omdbApiUrl);

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


        private void myMoviesDisplayList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //PopulateNearbyMoviesList();
            Movie mv = (Movie)myMoviesDisplayList.SelectedItem;

            String omdbid = mv.imdbID;
            NavigationService.Navigate(new Uri("/MoviePage.xaml?omdbid=" + omdbid, UriKind.Relative));
        }

        private void aroundMeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //PopulateNearbyMoviesList();
            NearbyMovie nm = (NearbyMovie)aroundMeList.SelectedItem;

            String omdbid = nm.omdb;
            NavigationService.Navigate(new Uri("/MoviePage.xaml?omdbid=" + omdbid, UriKind.Relative));
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