using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace TossBetTracker
{
    public class Cerrar : ContentPage
    {

        public Cerrar()
        {
            try
            {
                Settings.Idusuario = "";
                App.Current.MainPage = new Xamarin.Forms.NavigationPage(new Prelogin());
                Navigation.PopAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }
}




