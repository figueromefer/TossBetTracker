using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Abstractions;
using Xamarin.Forms;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TossBetTracker
{
    public class Editar_capital : ContentPage
    {
        StackLayout stack1 = null;
        Picker p_movimiento = null;
        Entry e_monto = null;
        Button boton2 = null;
        Label capital = null;

        public Editar_capital()
        {
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            BackgroundColor = Color.FromHex("#01528a");
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(0, 30, 0, 30) };
            StackLayout header = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Horizontal, Padding = new Thickness(0, 0, 0, 20) };
            capital = new Label() { Text = "$", Margin = new Thickness(0,20,0,40), FontSize = 40, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#ffffff"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
            
            stack1.Children.Add(capital);
            stack1.Children.Add(header);
            stack1.Children.Add(new Label() { Text = " ", FontSize = 5, HeightRequest = 20 });

            //PERFIL
            
            Label titulo = new Label() { Text = "Editar capital", FontSize = 22, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            stack1.Children.Add(titulo);

            //FRAME SUPERIOR
                         Label movimiento = new Label() { Text = "Movimiento", FontSize = 12, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#000000"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
            p_movimiento = new Picker() { Title = "Movimiento", HorizontalOptions = LayoutOptions.FillAndExpand,};
            p_movimiento.Items.Add("Deposito");
            p_movimiento.Items.Add("Retiro");
            Label monto = new Label() { Text = "Monto", FontSize = 12, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#000000"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
            e_monto = new Entry() { HorizontalOptions = LayoutOptions.FillAndExpand };
            boton2 = new Button() { Text = "Aplicar", MinimumHeightRequest = 40, WidthRequest = 200, HorizontalOptions = LayoutOptions.Center, BorderRadius = 20, BackgroundColor = Color.FromHex("#2DC9EB"), TextColor = Color.FromHex("#ffffff"), FontSize = 18, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            boton2.Clicked += OnLoginClicked;
            StackLayout form01 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Spacing = 0, Padding = new Thickness(0,30,0,170) };
            form01.Children.Add(movimiento);
            form01.Children.Add(p_movimiento);
            form01.Children.Add(monto);
            form01.Children.Add(e_monto);
            form01.Children.Add(new Label() { Text = " ", FontSize = 5, HeightRequest = 20 });
            form01.Children.Add(boton2);
            Frame frame1 = new Frame() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.EndAndExpand, Content = form01, Margin = new Thickness(0), BackgroundColor = Color.White, HasShadow = true, IsClippedToBounds = false, CornerRadius = 35, Padding = new Thickness(15) };
            stack1.Children.Add(frame1);              
            AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(stack1);

            //MENU

            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            //ASIGNAR LAYOUT

            this.Content = absoluteLayout;
            ConsultarCapital();
        }

        public async void ConsultarCapital()
        {
            try
            {
                string uriString2 = string.Format("http://toss.boveda-creativa.com/capital.php?usuario={0}", Settings.Idusuario);
                var response2 = await httpRequest(uriString2);
                capital.Text = "$ " + response2.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ayuda", ex.Message, "OK");
            }
        }

        public async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                string uriString2 = string.Format("http://toss.boveda-creativa.com/editar_capital.php?usuario={0}&monto={1}&movimiento={2}", Settings.Idusuario, e_monto.Text,p_movimiento.SelectedItem.ToString());
                var response2 = await httpRequest(uriString2);
                if(response2.ToString() == "1")
                {
                    await DisplayAlert("Excelente", "Movimiento registrado correctamente", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Se presentó un error al capturar el movimiento", "OK");
                }
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

