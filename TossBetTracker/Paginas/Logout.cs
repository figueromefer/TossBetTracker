using System;

using Xamarin.Forms;

namespace TossBetTracker
{
    public class Logout : ContentPage
    {
        public Logout()
        {
            try
            {
                Settings.Idusuario = "";
                Application.Current.MainPage = new NavigationPage(new Prelogin());
            }
            catch (System.Exception ex)
            {

            }
           
        }
    }
}

