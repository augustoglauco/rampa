using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;
using Rampa.Helper;
using Rampa.Model;


namespace Rampa
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        readonly FirebaseHelper firebaseHelper = new FirebaseHelper();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await FetchAllPersons();
        }


        private async void GetPosition(object sender, EventArgs e)
        {
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

        private async Task FetchAllPersons()
        {
            var allPersons = await firebaseHelper.GetAllPersons();

            LstPersons.ItemsSource = allPersons;
        }

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                await DisplayAlert("Error", "Name and person are required fields", "OK");
                return;
            }

            var person = await firebaseHelper.GetPerson(TxtName.Text);

            if (person != null)
            {
                await DisplayAlert("Error", "A person with that name already exist", "OK");
                return;
            }

            await firebaseHelper.AddPerson(TxtName.Text, TxtPhone.Text);

            TxtName.Text = string.Empty;
            TxtPhone.Text = string.Empty;

            await DisplayAlert("Success", "Person Added Successfully", "OK");

            await FetchAllPersons();
        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            if (SelectedPerson == null)
            {
                await DisplayAlert("Error", "A person must be selected to proceed", "OK");
                return;
            }

            if (!IsFormValid())
            {
                await DisplayAlert("Error", "Name and person are required fields", "OK");
                return;
            }

            var person = await firebaseHelper.GetPerson(TxtName.Text);

            if (person != null && person.PersonId != SelectedPerson.PersonId)
            {
                await DisplayAlert("Error", "A person with that name already exist", "OK");
                return;
            }

            await firebaseHelper.UpdatePerson(SelectedPerson.PersonId, TxtName.Text, TxtPhone.Text);

            TxtName.Text = string.Empty;
            TxtPhone.Text = string.Empty;

            await DisplayAlert("Success", "Person Updated Successfully", "OK");

            await FetchAllPersons();
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            if (SelectedPerson == null)
            {
                await DisplayAlert("Error", "A person must be selected to proceed", "OK");
                return;
            }

            await firebaseHelper.DeletePerson(SelectedPerson.PersonId);

            await DisplayAlert("Success", "Person Deleted Successfully", "OK");

            await FetchAllPersons();
        }

        private async void LstPersons_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var person = await firebaseHelper.GetPerson(SelectedPerson.PersonId);

            TxtName.Text = person.Name;
            TxtPhone.Text = person.Phone;
        }

        private Person SelectedPerson => (Person)LstPersons.SelectedItem;

        private bool IsFormValid() => IsNameValid() && IsPhoneValid();

        private bool IsNameValid() => !string.IsNullOrWhiteSpace(TxtName.Text);

        private bool IsPhoneValid() => !string.IsNullOrWhiteSpace(TxtPhone.Text);
    }
}
