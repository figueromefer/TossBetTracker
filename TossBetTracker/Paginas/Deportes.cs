using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Acr.UserDialogs;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace TossBetTracker
{
    public class Deportes : ContentPage
    {
        StackLayout stack1 = null;
        StackLayout pestanas = null;
        StackLayout modal = null;
        StackLayout stackboleto = null;
        AbsoluteLayout absoluteLayout = null;
        List<string> boletos_array = new List<string>();

        public Deportes()
        { 
            
            BackgroundColor = Color.White;
            Label Name = new Label() { Text = "Deportes", WidthRequest = 100, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { Name, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            foreach (string boleto in Settings.tickets.Split('#'))
            {
                if (boleto.Length > 1)
                {
                    boletos_array.Add(boleto);
                }
            }
            absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20,40,20,60), Spacing = 0 };
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack1, HorizontalOptions = LayoutOptions.FillAndExpand};
            //GENERALES


            AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(scv1);

            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            //ASIGNAR LAYOUT

            this.Content = absoluteLayout;
            ConsultarP();
            cargar_intro();
        }

        public async void cargar_intro()
        {
            try
            {
                if (!Settings.Primera.Contains("|DEPORTES|"))
                {
                    
                    //Label Nameintro = new Label() { Text = "Deportes", Margin = new Thickness(0, 0, 0, 30), FontSize = 30, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Label Contenidointro = new Label() { Text = "Selecciona un deporte para ver los partidos disponibles.", Margin = new Thickness(0, 0, 0, 0), FontSize = 22, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    StackLayout stackcontenido = new StackLayout() { Children = {   Contenidointro }, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, };
                    StackLayout intro1 = new StackLayout() { Opacity = 0, Children = { stackcontenido }, BackgroundColor = Color.FromRgba(0, 0, 0, 0.8), VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(30, 50) };
                    intro1.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() => {
                            try
                            {
                                absoluteLayout.Children.Remove(intro1);
                                Settings.Primera = Settings.Primera + "|DEPORTES|";
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
                    absoluteLayout.Children.Add(intro1);
                    await intro1.FadeTo(1, 700, Easing.Linear);
                    await intro1.ScaleTo(1.1, 300);
                    await intro1.ScaleTo(1, 300);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async void ConsultarP()
        {
            try
            {
                Label Name = new Label() { Text = "Deportes", FontSize = 26, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };


                Image flotante_boleto = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("boleto_i.png"), WidthRequest = 55 };
                if (boletos_array.Count == 0)
                {
                    flotante_boleto.Source = ImageSource.FromFile("boleto_i.png");
                }
                else
                {
                    flotante_boleto.Source = ImageSource.FromFile("boleto_a.png");
                }
                flotante_boleto.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        try
                        {
                            if (boletos_array.Count == 0)
                            {
                                
                                var aConfi = new AlertConfig();
                                aConfi.SetMessage("No hay tickets activos");
                                aConfi.SetTitle("Alerta");
                                aConfi.SetOkText("Ok");
                                UserDialogs.Instance.Alert(aConfi);

                            }
                            else
                            {
                                Navigation.PushAsync(new Ticket2(boletos_array, ""));
                            }
                        }
                        catch (Exception ex)
                        {
                            DisplayAlert("Ayuda", ex.Message, "OK");
                        }
                    }),
                    NumberOfTapsRequired = 1
                });
                StackLayout stackName = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { Name, flotante_boleto} };
                stack1.Children.Add(stackName);
                string uriString2 = "http://toss.boveda-creativa.com/deportes.php";
                var response2 = await httpRequest(uriString2);
                List<class_deportes> valor = new List<class_deportes>();
                valor = procesar2(response2);
                for (int j = 0; j < valor.Count(); j++)
                {
                    StackLayout row = new StackLayout() { HorizontalOptions = LayoutOptions.CenterAndExpand, Orientation = StackOrientation.Horizontal };
                    stack1.Children.Add(row);
                    try
                    {
                        string foto = valor.ElementAt(j).Name;
                        if(foto == "LIGA MX") { foto = "FIFA"; }
                        if (foto == "lmb") {
                            foto = "MLB";
                        }
                        if (foto == "lmp") {
                            foto = "MLB";
                        }
                        if (foto == "soccer")
                        {
                            foto = "FIFA";
                        }
                        foto = foto.ToUpper();  
                        Image imagen_deporte = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile(foto + ".png"), WidthRequest = 80 };
                        if(int.Parse(valor.ElementAt(j).activo) < 1)
                        {
                            imagen_deporte.Opacity = 0.5;
                        }
                        Label nombre_deporte = new Label() { Text = valor.ElementAt(j).Name.ToUpper(), FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        StackLayout stackcol1 = new StackLayout() { HorizontalOptions = LayoutOptions.Center, ClassId = valor.ElementAt(j).SportId+"|"+ valor.ElementAt(j).Name, Spacing = 2, Padding = new Thickness(20), Children = { imagen_deporte, nombre_deporte } };
                        
                        Frame framecol1 = new Frame() { Margin = new Thickness(5,10),Content = stackcol1, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, Padding = new Thickness(5), HasShadow = true, Visual = VisualMarker.Material };
                        framecol1.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(async () => {
                                try
                                {
                                    await framecol1.ScaleTo(1.1, 100);
                                    await framecol1.ScaleTo(1, 100);
                                    await Navigation.PushAsync(new Partidos(stackcol1.ClassId));
                                }
                                catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK"); }
                            }),
                            NumberOfTapsRequired = 1
                        }); row.Children.Add(framecol1);
                        j++;
                        foto = valor.ElementAt(j).Name;
                        if (foto == "LIGA MX") { foto = "FIFA"; }
                        if (foto == "lmb")
                        {
                            foto = "MLB";
                        }
                        if (foto == "lmp")
                        {
                            foto = "MLB";
                        }
                        if (foto == "soccer")
                        {
                            foto = "FIFA";
                        }
                        foto = foto.ToUpper();
                        Image imagen_deporte2 = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile(foto + ".png"), WidthRequest = 80 };
                        if (int.Parse(valor.ElementAt(j).activo) < 1)
                        {
                            imagen_deporte2.Opacity = 0.5;
                        }
                        Label nombre_deporte2 = new Label() { Text = valor.ElementAt(j).Name.ToUpper(), FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        StackLayout stackcol2 = new StackLayout() { HorizontalOptions = LayoutOptions.Center, ClassId = valor.ElementAt(j).SportId + "|" + valor.ElementAt(j).Name, Spacing = 2, Padding = new Thickness(20), Children = { imagen_deporte2, nombre_deporte2 } };
                        Frame framecol2 = new Frame() { Margin = new Thickness(5, 10), Content = stackcol2, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, Padding = new Thickness(5), HasShadow = true, Visual = VisualMarker.Material };
                        framecol2.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => { try {
                                await framecol2.ScaleTo(1.1, 100);
                                await framecol2.ScaleTo(1, 100);
                                await Navigation.PushAsync(new Partidos(stackcol2.ClassId)); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                        row.Children.Add(framecol2);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ayuda", ex.Message, "OK");
            }
        }

        

        public List<class_deportes> procesar2(string respuesta)
        {
            List<class_deportes> items = new List<class_deportes>();
            if (respuesta == "")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_deportes
                             {
                                 id = (string)r.Element("id"),
                                 activo = WebUtility.UrlDecode((string)r.Element("activo")),
                                 Name = WebUtility.UrlDecode((string)r.Element("Name")),
                                 SportId = WebUtility.UrlDecode((string)r.Element("SportId"))
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

