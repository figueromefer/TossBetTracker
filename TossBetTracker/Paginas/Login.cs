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
    public class Login : ContentPage
    {
        StackLayout stack1 = null;
        ScrollView Scrollview1 = null;
        Entry entry1 = new Entry() { FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) ,TextColor = Color.FromHex("#ADB5BD"), PlaceholderColor = Color.FromHex("#ADB5BD"), Placeholder = "Correo", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
        Entry entry2 = new Entry() { IsPassword = true, FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null), TextColor = Color.FromHex("#ADB5BD"), PlaceholderColor = Color.FromHex("#ADB5BD"), Placeholder = "Contraseña", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
        Button boton2 = null;
        StackLayout form1 = null;

        public Login()
        {
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            BackgroundColor = Color.FromHex("#00518c");
            var foto_fondo = new Image() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Aspect = Aspect.AspectFill, Source = ImageSource.FromFile("fondo.png") };
            var foto_logo = new Image() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Source = ImageSource.FromFile("logo.png"),  Margin = new Thickness(30, 70, 30, 20) };
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(30, 30, 30, 30) };
            stack1.Children.Add(foto_logo);
            stack1.Children.Add(new Label() { Text = " ", FontSize = 5, HeightRequest = 20 });
            form1 = new StackLayout() { };
            stack1.Children.Add(form1);
            ScrollView scv1 = new ScrollView() {  Content = stack1  };             /*AbsoluteLayout.SetLayoutFlags(foto_fondo, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(foto_fondo, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(foto_fondo);*/             AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(scv1);
            this.Content = absoluteLayout;
            ConsultarP();
        }

       
        public async void ConsultarP()
        {
            try
            {
                Label titulo = new Label() { Text = "Iniciar sesión", FontSize = 18, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null), Margin = new Thickness(0, 0, 0, 20) };
                boton2 = new Button() { Text = "Inicia sesión", MinimumHeightRequest = 40, HorizontalOptions = LayoutOptions.FillAndExpand, BorderRadius = 10, BackgroundColor = Color.FromHex("#2DC9EB"), TextColor = Color.FromHex("#ffffff"), FontSize = 14, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null), Margin = new Thickness(0, 0, 0, 20) };
                boton2.Clicked += OnLoginClicked;
                Label olvidaste = new Label() { Text = "¿Olvidaste tu contraseña?", HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#ffffff"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null), Margin = new Thickness(0,0,0,20) };
                olvidaste.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Navigation.PushAsync(new Recuperar()); } catch (Exception ex) { DisplayAlert("Ayuda", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                var foto_face = new Image() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, Source = ImageSource.FromFile("face.png"), HeightRequest = 60 };
                Image i_correo = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("i_correo.png"), WidthRequest = 15 };
                Image i_pass = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("i_pass.png"), WidthRequest = 15 };
                StackLayout stackcorreo = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, Children ={ i_correo, entry1 }, Padding = new Thickness(0) };
                StackLayout stackcontra = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { i_pass, entry2 }, Padding = new Thickness(0) };
                Frame framecorreo = new Frame() { CornerRadius = 6, BackgroundColor = Color.White, Content = stackcorreo, IsClippedToBounds = true, Padding = new Thickness(15,2), Margin = new Thickness(0, 0, 0, 20) };
                Frame framecontra = new Frame() { CornerRadius = 6, BackgroundColor = Color.White, Content = stackcontra, IsClippedToBounds = true, Padding = new Thickness(15,2), Margin = new Thickness(0, 0, 0, 10) };
                var stacklista = new StackLayout()
                {                
                    Padding = new Thickness(20, 20),
                    Children = {
                        titulo,
                        framecorreo,
                        framecontra,
                        olvidaste,
                        boton2,
                        //foto_face
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
            try{
                Button boton = (Button)sender;
                await boton.ScaleTo(1.2, 100);
                await boton.ScaleTo(1, 100);
                string uriString2 = string.Format("http://toss.boveda-creativa.com/login.php?usuario={0}&password={1}", entry1.Text, entry2.Text);
                var response2 = await httpRequest(uriString2);
                List<class_usuarios> valor = new List<class_usuarios>();
                valor = procesar2(response2);
                if (valor.Count == 0)
                {
                    await DisplayAlert("Ayuda", "Usuario y/o contraseña incorrectos", "OK");
                }
                for (int i = 0; i < valor.Count(); i++)
                {
                    if (int.Parse(valor.ElementAt(i).idusuario) > 0)
                    {
                        Settings.Idusuario = valor.ElementAt(i).idusuario;
                        Settings.Nombre = valor.ElementAt(i).nombre;
                        Settings.Correo = valor.ElementAt(i).correo;
                        Settings.Foto = valor.ElementAt(i).foto;
                            Settings.Notif = valor.ElementAt(i).notif;
                            Settings.Telefono = valor.ElementAt(i).telefono;
                        App.Current.MainPage = new NavigationPage(new RootPage());
                        }
                    else
                    {
                        await DisplayAlert("Ayuda", "Usuario y/o contraseña incorrectos", "OK");
                    }
                }
            }catch(Exception ex){
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public List<class_usuarios> procesar2(string respuesta)
        {
            List<class_usuarios> items = new List<class_usuarios>();
            if (respuesta == "0")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_usuarios
                             {
                                 idusuario = (string)r.Element("idusuario"),
                                 nombre = (string)r.Element("nombre"),
                                 correo = (string)r.Element("correo"),
                                 notif = (string)r.Element("notif"),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
                                 telefono = (string)r.Element("telefono"),
                             }).ToList();
                }
            }
            return items;
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

