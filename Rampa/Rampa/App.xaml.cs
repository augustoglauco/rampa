using Rampa.Services;
using Xamarin.Forms;


namespace Rampa
{
    public partial class App : Application
    {
        public static ILocationUpdateService LocationUpdateService;
        
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            //DependencyService.Get<ILocationUpdateService>().GetUserLocation();
            //DependencyService.Get<ILocationUpdateService>().LocationChanged += LocationUpdateService_LocationChanged; ;
        }

        //private void LocationUpdateService_LocationChanged(object sender, ILocationEventArgs e)
        //{
        //    var x = string.Format("{0:0.0000000}", e.Longitude);
        //    //Here you can get the user's location from "e" -> new Location(e.Latitude, e.Longitude);
        //    //new Location is from Xamarin.Essentials Location object.
        //}


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
