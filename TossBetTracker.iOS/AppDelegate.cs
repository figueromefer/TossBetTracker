using System;
using System.Collections.Generic;
using System.Linq;
using Com.OneSignal;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using UIKit;

namespace TossBetTracker.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();



            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(1, 82, 138);
            UINavigationBar.Appearance.TintColor = UIColor.White;
            /*UINavigationItem NavigationItem = new UINavigationItem();
            NavigationItem.TitleView = new UIImageView(UIImage.FromFile("logo2.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate));*/

            OneSignal.Current.StartInit("37e3276a-9102-483b-9305-83cd1e1930f0")
                  .EndInit();
            LoadApplication(new App());
            ImageCircleRenderer.Init();
            return base.FinishedLaunching(app, options);
        }

        [Export("oneSignalApplicationDidBecomeActive:")]
        public void OneSignalApplicationDidBecomeActive(UIApplication application)
        {
            Console.WriteLine("oneSignalApplicationDidBecomeActive:");
        }

        [Export("oneSignalApplicationWillResignActive:")]
        public void oneSignalApplicationWillResignActive(UIApplication application)
        {
            Console.WriteLine("oneSignalApplicationWillResignActive:");
        }
    }
}
