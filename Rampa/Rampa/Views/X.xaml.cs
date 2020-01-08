using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Rampa.Helper;
using System.Windows.Input;

namespace Rampa.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class X : ContentPage
    {
        readonly FirebaseHelper firebaseHelper = new FirebaseHelper();
        public ICommand NavigateCommand { get; private set; }

        public X()
        {
            InitializeComponent();
        }

        private async void GetPosition(object sender, EventArgs e)
        {
            //var locator = CrossGeolocator.Current;
            //locator.DesiredAccuracy = 10;

            //var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            //LongitudeLabel.Text = string.Format("{0:0.0000000}", position.Longitude);
            //LatitudeLabel.Text = string.Format("{0:0.0000000}", position.Latitude);
            //AltitudeLabel.Text = string.Format("{0:0.0000000}", position.Altitude);

            try
            {
                Location location = null;
                location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Best);
                    location = await Geolocation.GetLocationAsync(request);
                }

                if (location != null)
                {
                    LongitudeLabel.Text = string.Format("{0:0.0000000}", location.Longitude);
                    LatitudeLabel.Text = string.Format("{0:0.0000000}", location.Latitude);
                    AltitudeLabel.Text = string.Format("{0:0.0000000}", location.Altitude);
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

                    
    }
}
