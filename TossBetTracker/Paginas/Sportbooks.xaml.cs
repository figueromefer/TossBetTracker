using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TossBetTracker
{
    public partial class Sportbooks : ContentPage
    {
        public Sportbooks()
        {
            InitializeComponent();
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            absolutelayout.Children.Add(Home.cargar_menu_inferior());
            cargar_intro();
        }

        public async void cargar_intro()
        {
            try
            {
                if (!Settings.Primera.Contains("|SPORTSBOOKS|"))
                {
                    Image logointro = new Image() { Source = ImageSource.FromFile("icono_spb.png"), HorizontalOptions = LayoutOptions.CenterAndExpand, WidthRequest = 50, Margin = new Thickness(0, 0, 0, 20), };
                    Label Titulointro = new Label() { Text = "Sportsbooks", Margin = new Thickness(0, 0, 0, 30), FontSize = 30, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Label Contenidointro = new Label() { Text = "Aquí podrás encontrar sugerencias de sportsbooks.", Margin = new Thickness(0, 0, 0, 0), FontSize = 22, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    StackLayout stackcontenido = new StackLayout() { Children = { logointro, Titulointro, Contenidointro }, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, };
                    StackLayout intro1 = new StackLayout() { Opacity = 0, Children = { stackcontenido }, BackgroundColor = Color.FromRgba(0, 0, 0, 0.8), VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(30, 50) };
                    intro1.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() => {
                            try
                            {
                                absolutelayout.Children.Remove(intro1);
                                Settings.Primera = Settings.Primera + "|SPORTSBOOKS|";
                            }
                            catch (Exception ex)
                            {
                                Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK");
                            }
                        }),
                        NumberOfTapsRequired = 1
                    });
                    AbsoluteLayout.SetLayoutFlags(intro1, AbsoluteLayoutFlags.All);
                    AbsoluteLayout.SetLayoutBounds(intro1, new Rectangle(0, 0, 1, 1));
                    absolutelayout.Children.Add(intro1);
                    await intro1.FadeTo(1, 700, Easing.Linear);
                    await intro1.ScaleTo(1.1, 300);
                    await intro1.ScaleTo(1, 300);
                }
            }
            catch (Exception ex)
            {

            }
        }

        void Bet365_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                Device.OpenUri(new Uri("https://www.bet365.com/"));
            }
            catch (Exception ex)
            {

            }
        }

        void Betcris_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                Device.OpenUri(new Uri("https://www.betcris.com/"));
            }
            catch (Exception ex)
            {

            }
        }

        void Betway_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                Device.OpenUri(new Uri("https://betway.mx/"));
            }
            catch (Exception ex)
            {

            }
        }

        void Ganabet_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                Device.OpenUri(new Uri("https://ganabet.mx/sports/"));
            }
            catch (Exception ex)
            {

            }
        }

        void Caliente_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                Device.OpenUri(new Uri("https://www.caliente.mx/"));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
