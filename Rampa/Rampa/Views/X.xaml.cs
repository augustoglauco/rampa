﻿using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Rampa.Services;

namespace Rampa.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class X : ContentPage
    {
        //readonly FirebaseHelper firebaseHelper = new FirebaseHelper();

        // Set speed delay for monitoring changes.
        readonly SensorSpeed speed = SensorSpeed.UI;

        //public static ILocationUpdateService LocationUpdateService;

        public X()
        {
            InitializeComponent();
            Barometer.ReadingChanged += Barometer_ReadingChanged;
            //LocationUpdateService.LocationChanged += LocationUpdateService_LocationChanged; ; 
            DependencyService.Get<ILocationUpdateService>().LocationChanged += LocationUpdateService_LocationChanged; 
            DependencyService.Get<ILocationUpdateService>().GetUserLocation();

            ToggleBarometer();
            //GetPosition(this, null);
        }

        private async void GetPosition(object sender, EventArgs e)
        {
            try
            {
                Location location = null;
                //location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Best);
                    location = await Geolocation.GetLocationAsync(request);
                }

                if (location != null)
                {
                    LongitudeLabel.Text = string.Format("{0:0.0000000}", location.Longitude);
                    LatitudeLabel.Text = string.Format("{0:0.0000000}", location.Latitude);
                    AltitudeLabel.Text = string.Format("{0:0.0}", location.Altitude) + "m";
                }
                else
                {
                    LongitudeLabel.Text = "Erro";
                    LatitudeLabel.Text = "Erro";
                    AltitudeLabel.Text = "Erro";

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

        void Barometer_ReadingChanged(object sender, BarometerChangedEventArgs e)
        {
            var data = e.Reading;
            float pressao =  float.Parse(Preferences.Get("PNM", "1013.25"));
            float temperatura = float.Parse(Preferences.Get("Temperatura", "23"));

            // calculate altitude above mean sea level according to Standard Atmosphere 1976 model
            //double altitudeAMSLInMeters = 44330.76923076923077 * (1.0 - Math.Pow(pressao / data.PressureInHectopascals, -0.19026323650861));

            // calculate altitude above mean sea level according to Standard Atmosphere 1976 considering Temperature
            double altitudeAMSLInMeters = ((Math.Pow((pressao/data.PressureInHectopascals), (1/5.257)) - 1) * (temperatura + 273.15)) / 0.0065;

            // Process Pressure
            Altitude2Label.Text = string.Format("{0:0.0}", altitudeAMSLInMeters) + "m";
            PressaoLabel.Text = string.Format("{0:0.0}", data.PressureInHectopascals) + "mBar";
             
        }

        private void ToggleBarometer()
        {
            try
            {
                if (Barometer.IsMonitoring)
                    Barometer.Stop();
                else
                {
                    Barometer.Start(speed);
                    
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        private void LocationUpdateService_LocationChanged(object sender, ILocationEventArgs e)
        {

            LongitudeLabel.Text = string.Format("{0:0.0000000}", e.Longitude);
            LatitudeLabel.Text = string.Format("{0:0.0000000}", e.Latitude);
            AltitudeLabel.Text = string.Format("{0:0.0}", e.Altitude) + "m";

            //Location x = new Location(e.Latitude, e.Longitude);

            //Here you can get the user's location from "e" -> new Location(e.Latitude, e.Longitude);
            //new Location is from Xamarin.Essentials Location object.
        }

    }
}
