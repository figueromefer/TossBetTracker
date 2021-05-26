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

namespace TossBetTracker
{
    public class Recuperar : ContentPage
    {
        StackLayout stack1 = null;
        ScrollView Scrollview1 = null;
        Entry entry1 = new Entry() { FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null), TextColor = Color.FromHex("#ADB5BD"), PlaceholderColor = Color.FromHex("#ADB5BD"), Placeholder = "Correo", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
        Button boton2 = null;
        StackLayout form1 = null;

        public Recuperar()
        {
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            BackgroundColor = Color.FromHex("#01528a");
            
            var foto_logo = new Image() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, Source = ImageSource.FromFile("logo.png"), WidthRequest = 130, Margin = new Thickness(0, 70, 0, 0) };
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(30, 30, 30, 30) };
            stack1.Children.Add(foto_logo);
            stack1.Children.Add(new Label() { Text = " ", FontSize = 5, HeightRequest = 20 });
            form1 = new StackLayout() { };
            stack1.Children.Add(form1);
            ScrollView scv1 = new ScrollView() { Content = stack1 };             AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(scv1);
            this.Content = absoluteLayout;
            ConsultarP();
        }

        public async void ConsultarP()
        {
            try
            {
                Label titulo = new Label() { Text = "Recuperar contraseña", FontSize = 18, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null), Margin = new Thickness(0, 0, 0, 20) };
                boton2 = new Button() { Text = "Recuperar", MinimumHeightRequest = 40, HorizontalOptions = LayoutOptions.FillAndExpand, BorderRadius = 10, BackgroundColor = Color.FromHex("#2DC9EB"), TextColor = Color.FromHex("#ffffff"), FontSize = 14, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null), Margin = new Thickness(0, 0, 0, 20) };
                boton2.Clicked += OnLoginClicked;
                Image i_correo = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("i_correo.png"), WidthRequest = 15 };
                StackLayout stackcorreo = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { i_correo, entry1 }, Padding = new Thickness(0) };
                Frame framecorreo = new Frame() { CornerRadius = 6, BackgroundColor = Color.White, Content = stackcorreo, IsClippedToBounds = true, Padding = new Thickness(15, 2), Margin = new Thickness(0, 0, 0, 20) };
                var stacklista = new StackLayout()
                {
                    Padding = new Thickness(20, 20),
                    Children = {
                        titulo,
                        framecorreo,
                        boton2,
                    }
                };
                form1.Children.Add(stacklista);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ayuda", "Problema de conectividad, favor de validar la red .", "OK");
            }
        }

        public async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                string uriString2 = string.Format("http://toss.boveda-creativa.com/recuperar.php?correo={0}", entry1.Text);
                var response2 = await httpRequest(uriString2);
                await DisplayAlert("Ayuda", "Recuperación de contraseña solicitada, revisa tu correo.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task<string> httpRequest(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            string received;
            using (var response = (HttpWebResponse)(await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null)))
            {
                using (var responseStream = response.GetResponseStream())
                { using (var sr = new StreamReader(responseStream)) { received = await sr.ReadToEndAsync(); } }
            }
            return received;
        }
    }
}

