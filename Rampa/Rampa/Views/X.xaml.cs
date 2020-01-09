﻿using System;
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
        //readonly FirebaseHelper firebaseHelper = new FirebaseHelper();
        //public ICommand NavigateCommand { get; private set; }

        // Set speed delay for monitoring changes.
        readonly SensorSpeed speed = SensorSpeed.UI;
        
        public X()
        {
            InitializeComponent();
            Barometer.ReadingChanged += Barometer_ReadingChanged;
            ToggleBarometer();
            GetPosition(this, null);
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

        void Barometer_ReadingChanged(object sender, BarometerChangedEventArgs e)
        {
            var data = e.Reading;

            // calculate altitude above mean sea level according to Standard Atmosphere 1976 model
            double altitudeAMSLInMeters = 44330.76923076923077 * (1.0 - Math.Pow(1010 / data.PressureInHectopascals, -0.19026323650861));

            // Process Pressure
            Altitude2Label.Text = string.Format("{0:0.0000000}", altitudeAMSLInMeters/0.305);
            PressaoLabel.Text = string.Format("{0:0.0000000}", data.PressureInHectopascals);

            // Console.WriteLine($"Reading: Pressure: {data.PressureInHectopascals} hectopascals");
        }

        private void ToggleBarometer()
        {
            try
            {
                if (Barometer.IsMonitoring)
                    Barometer.Stop();
                else
                    Barometer.Start(speed);
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
    }
}
