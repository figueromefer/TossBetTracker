using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TossBetTracker
{
    public class MenuPage : ContentPage
    {
        public ListView Menu { get; set; }

        public MenuPage()
        {
            Title = "MENÚ";
            Icon = "menu.png";
            BackgroundColor = Color.FromHex("#01528a");
            Menu = new MenuListView() { Margin = new Thickness(0,70,0,10)};


            var layout = new StackLayout
            {
                BackgroundColor = Color.FromHex("#01528a"),
                Spacing = 0,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(20,40,20,50),
            };
            var imagen = new Image() { Source = ImageSource.FromFile("logo.png"), WidthRequest = 100, Aspect = Aspect.AspectFit };
            var stackimg = new StackLayout { BackgroundColor = Color.FromHex("#01528a"), HorizontalOptions = LayoutOptions.FillAndExpand, Children = { imagen } };
            layout.Children.Add(stackimg);
            Label legal = new Label() { Margin = new Thickness(10, 50, 0, 10), TextColor = Color.White, FontSize = 16, Text = "Aviso deprivacidad" };
            legal.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => {
                    try
                    {
                        string uri = "http://toss.boveda-creativa.com/AVISO%20DE%20PRIVACIDAD.pdf";
                        if (await Xamarin.Essentials.Launcher.CanOpenAsync(uri))
                            await Xamarin.Essentials.Launcher.OpenAsync(uri);
                    }
                    catch (Exception ex) { await Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK"); }
                }),
                NumberOfTapsRequired = 1
            });

            Label terminos = new Label() { Margin = new Thickness(10, 0, 0, 10), TextColor = Color.White, FontSize = 16, Text = "Términos y condiciones" };
            terminos.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => {
                    try
                    {
                        string uri = "http://toss.boveda-creativa.com/TERMINOS%20Y%20CONDICIONES.pdf";
                        if (await Xamarin.Essentials.Launcher.CanOpenAsync(uri))
                            await Xamarin.Essentials.Launcher.OpenAsync(uri);
                    }
                    catch (Exception ex) { await Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK"); }
                }),
                NumberOfTapsRequired = 1
            });

            Label acerca = new Label() { Margin = new Thickness(10, 0, 0, 30), TextColor = Color.White, FontSize = 16, Text = "Acerca de Toss" };
            var redes = new Image()
            {
                HorizontalOptions = LayoutOptions.Start,
                Source = ImageSource.FromFile("redes.png"),
                WidthRequest = 140,
                Aspect = Aspect.AspectFit
            };
            Label version = new Label() { Margin = new Thickness(10, 0, 0, 10), TextColor = Color.White, FontSize = 14, Text = "V" + VersionTracking.CurrentVersion };
           
            var stack2 = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Vertical,
                Children = {
                    Menu,
                    legal,
                    terminos,
                    acerca,
                    version
                    //redes
                }
            };
            
            
            layout.Children.Add(stack2);

            Content = layout;
        }
    }
}


