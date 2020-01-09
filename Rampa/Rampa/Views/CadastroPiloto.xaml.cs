using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Rampa.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class CadastroPiloto : ContentPage
    {
        //readonly FirebaseHelper firebaseHelper = new FirebaseHelper();
        //public ICommand NavigateCommand { get; private set; }

        public CadastroPiloto()
        {
            InitializeComponent();
            VerificaId();
        }

        private void CadNome(object sender, EventArgs e)
        {
            Preferences.Clear();
            Preferences.Set("id", LblId.Text.Trim());
            Preferences.Set("Nome", TxtName.Text.Trim());
        }

        private void VerificaId()
        {
            var id = Preferences.Get("id", "");
            if (id != "")
            {
                LblId.Text = id;
                TxtName.Text = Preferences.Get("Nome", "");
            }
            else
            {
                LblId.Text = Guid.NewGuid().ToString().Trim(); 
            }
        }
    }
}
