using TossBetTracker.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(LargeHeaderFixingNavigationPageRenderer))]
namespace TossBetTracker.iOS.Renderers
{
    public class LargeHeaderFixingNavigationPageRenderer : NavigationRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (Element is NavigationPage navigationPage)
            {
                var barBackgroundColor = navigationPage.BarBackgroundColor;
                try
                {
                    NavigationBar.StandardAppearance.BackgroundColor = barBackgroundColor == Color.Default
                    ? UINavigationBar.Appearance.BarTintColor
                    : barBackgroundColor.ToUIColor();

                    NavigationBar.StandardAppearance.TitleTextAttributes = NavigationBar.TitleTextAttributes;
                    NavigationBar.StandardAppearance.LargeTitleTextAttributes = NavigationBar.LargeTitleTextAttributes;
                    NavigationBar.ScrollEdgeAppearance = NavigationBar.StandardAppearance;
                    NavigationBar.CompactAppearance = NavigationBar.StandardAppearance;
                }
                catch (System.Exception ex)
                {

                }

            }
        }
    }
}