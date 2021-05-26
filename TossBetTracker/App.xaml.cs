using System;
using Com.OneSignal;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TossBetTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try             {                 if (Settings.Idusuario != "")                 {                     MainPage = new NavigationPage(new RootPage());
                    OneSignal.Current.StartInit("37e3276a-9102-483b-9305-83cd1e1930f0").EndInit();
                }                 else                 {                     MainPage = new NavigationPage(new Prelogin());
                    OneSignal.Current.StartInit("37e3276a-9102-483b-9305-83cd1e1930f0").EndInit();
                }              }             catch (Exception ex)             {              }
        }

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
