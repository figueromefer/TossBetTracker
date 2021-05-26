using TossBetTracker.Controls;
using TossBetTracker.iOS;
using CoreAnimation;
using CoreGraphics;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BetterPicker), typeof(BetterPickerRenderer))]

namespace TossBetTracker.iOS
{
    public class BetterPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.TextAlignment = UITextAlignment.Center;
            }
        }
    }
}