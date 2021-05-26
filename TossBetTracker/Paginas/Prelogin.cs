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
    public class Prelogin : ContentPage
    {
        StackLayout stack1 = null;
        ScrollView Scrollview1 = null;
        Entry entry1 = new Entry() { FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null), TextColor = Color.FromHex("#686868"), PlaceholderColor = Color.FromHex("#006ea9"), Placeholder = "Usuario" };
        Entry entry2 = new Entry() { IsPassword = true, FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null), TextColor = Color.FromHex("#686868"), PlaceholderColor = Color.FromHex("#006ea9"), Placeholder = "Contraseña" };
        Button boton2 = null;
        Button boton3 = null;
        StackLayout form1 = null;

        public Prelogin()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            BackgroundColor = Color.FromHex("#00518c");
            var foto_fondo = new Image() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Aspect = Aspect.AspectFill, Source = ImageSource.FromFile("fondo.png") };
            var foto_logo = new Image() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, Source = ImageSource.FromFile("logo.png"),  Margin = new Thickness(30,160,30,0) };
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(30, 30, 30, 30) };
            stack1.SizeChanged += Cambiotamano;
            stack1.Children.Add(foto_logo);
            Label texto1 = new Label() { Text = "Track your bets", FontSize = 16, Margin = new Thickness(0,20,0,0),HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#ffffff"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Label texto2 = new Label() { Text = "Comparte apuestas", Margin = new Thickness(50, 20, 50, 0), FontSize = 13, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#ffffff"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
            Label texto3 = new Label() { Text = "Pon en juego tu suerte", Margin = new Thickness(50, 0, 50, 50), FontSize = 13, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#ffffff"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
            stack1.Children.Add(texto1);
            stack1.Children.Add(texto2);
            stack1.Children.Add(texto3);
            form1 = new StackLayout() { };
            stack1.Children.Add(form1);
            boton2 = new Button() { Text = "Registrate", Margin = new Thickness(0, 10, 0, 0), MinimumHeightRequest = 40,  HorizontalOptions = LayoutOptions.FillAndExpand, BorderRadius = 10, BackgroundColor = Color.FromHex("#2DC9EB"), TextColor = Color.FromHex("#ffffff"), FontSize = 14, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            boton2.Clicked += OnRegistroClicked;
            boton3 = new Button() { Text = "Inicia sesión", Margin = new Thickness(0, 10, 0, 40), MinimumHeightRequest = 40, HorizontalOptions = LayoutOptions.FillAndExpand, BorderRadius = 10, BackgroundColor = Color.FromHex("#348da8"), TextColor = Color.FromHex("#ffffff"), FontSize = 14, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            boton3.Clicked += OnLoginClicked;
            stack1.Children.Add(boton2);
            stack1.Children.Add(boton3);
            ScrollView scv1 = new ScrollView() { Content = stack1 };             /*AbsoluteLayout.SetLayoutFlags(foto_fondo, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(foto_fondo, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(foto_fondo);*/             AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(scv1);
            this.Content = absoluteLayout;
           
        }

        public void Cambiotamano(object sender, EventArgs e)
        {
            Settings.Ancho = stack1.Width.ToString();
        }

        public async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                Button boton = (Button)sender;
                await boton.ScaleTo(1.2, 100);
                await boton.ScaleTo(1, 100);
                await Navigation.PushAsync(new Login());
                //App.Current.MainPage = new RootPage();
                    
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async void OnRegistroClicked(object sender, EventArgs e)
        {
            try
            {
                Button boton = (Button)sender;
                await boton.ScaleTo(1.2, 100);
                await boton.ScaleTo(1, 100);
                await Navigation.PushAsync(new Registro());
                //App.Current.MainPage = new RootPage();

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}

