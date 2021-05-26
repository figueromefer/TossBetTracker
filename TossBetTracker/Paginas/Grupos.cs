using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Media.Abstractions;
using Plugin.Media;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TossBetTracker
{
    public class Grupos : ContentPage
    {

        StackLayout stack1 = null;
        StackLayout stack_grupos = null;
        StackLayout pestanas = null;
        Image back = null;
        AbsoluteLayout absoluteLayout = null;


        public Grupos()
        {
            
            BackgroundColor = Color.FromHex("#01528a");
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(15, 0, 0, 0) };
            Label Titulo0 = new Label() { Text = "Grupos", WidthRequest = 100, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Image imagenchat = new Image() { IsVisible = false,Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { Titulo0, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);

            absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(10,0,10,10), Spacing = 0 };
            Label Titulo = new Label() { Text = "Grupos", FontSize = 26, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Label nuevo = new Label() { Text = "Crear grupo + ", Margin = new Thickness(0,5,0,0), FontSize = 16, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
            nuevo.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Navigation.PushAsync(new NGrupo()); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
            StackLayout stacksuperior = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { Titulo, nuevo }, Padding = new Thickness(0,0,0,15) };
            stack1.Children.Add(stacksuperior);

            stack_grupos = new StackLayout() { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Padding = new Thickness(20, 20, 20, 20), Spacing = 0 };
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack_grupos };
            Frame framegrupos = new Frame() { IsClippedToBounds = true, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, CornerRadius = 25, BackgroundColor = Color.White, Content = scv1, Padding = new Thickness(0)};
            stack1.Children.Add(framegrupos);

            //GENERALES
            AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0.90, 1, 0.93));             absoluteLayout.Children.Add(stack1);

            //MENU INFERIOR
            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            //ASIGNAR LAYOUT
            this.Content = absoluteLayout;
            cargar_chats();
            cargar_intro();
        }

        public async void cargar_intro()
        {
            try
            {
                if (!Settings.Primera.Contains("|GRUPOS|"))
                {
                    Image logointro = new Image() { Source = ImageSource.FromFile("icono_grupos.png"), HorizontalOptions = LayoutOptions.CenterAndExpand, WidthRequest = 50, Margin = new Thickness(0, 0, 0, 20), };
                    Label Titulointro = new Label() { Text = "Grupos", Margin = new Thickness(0, 0, 0, 30), FontSize = 30, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Label Contenidointro = new Label() { Text = "Crea grupos con tus amigos y ve quien es el mejor.", Margin = new Thickness(0, 0, 0, 0), FontSize = 22, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    StackLayout stackcontenido = new StackLayout() { Children = { logointro, Titulointro, Contenidointro }, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, };
                    StackLayout intro1 = new StackLayout() { Opacity = 0, Children = { stackcontenido }, BackgroundColor = Color.FromRgba(0, 0, 0, 0.8), VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(30, 50) };
                    intro1.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() => {
                            try
                            {
                                absoluteLayout.Children.Remove(intro1);
                                Settings.Primera = Settings.Primera + "|GRUPOS|";
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

        public async void cargar_chats()
        {
            try
            {
                string uriString2 = string.Format("http://toss.boveda-creativa.com/grupos.php?usuario={0}", Settings.Idusuario);
                var response2 = await httpRequest(uriString2);
                if (response2.Length > 10)
                {
                    List<class_grupos> valor = new List<class_grupos>();
                    valor = procesar(response2);
                    for (int j = 0; j < valor.Count(); j++)
                    {
                        CircleImage foto = new CircleImage
                        {
                            HeightRequest = 45,
                            WidthRequest = 45,
                            Aspect = Aspect.AspectFill,
                            HorizontalOptions = LayoutOptions.Center,
                            Source = new UriImageSource
                            {
                                Uri = new Uri(valor.ElementAt(j).foto),
                                CachingEnabled = true,
                            },
                        };
                        Label nombre = new Label()
                        {
                            Text = valor.ElementAt(j).titulo,
                            VerticalTextAlignment = TextAlignment.Center,
                            FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null),
                            TextColor = Color.FromHex("#01528a"),
                            FontSize = 16,
                        };
                        Label posicion = new Label() { Text = "#" + valor.ElementAt(j).ranking, TextColor = Color.White, FontSize = 12 };
                        Frame frameposicion = new Frame() { CornerRadius = 5, HasShadow = false, Padding = new Thickness(1), BackgroundColor = Color.FromHex("#DFA423"), Content = posicion };
                        Label ultimo = new Label()
                        {
                            Text = "Lorem impsum",
                            FontSize = 12,
                            VerticalTextAlignment = TextAlignment.Center,
                        };
                        StackLayout ranking = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { frameposicion, ultimo } };

                        Label linea = new Label()
                        {
                            BackgroundColor = Color.FromHex("#D1D3D4"),
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Margin = new Thickness(0, 0, 0, 10)
                        };
                        StackLayout nombre_ultimo = new StackLayout()
                        {
                            Orientation = StackOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,
                            Children =
                            {
                                nombre,
                                ranking
                            }
                        };
                        Label feed = new Label()
                        {
                            Text = "Feed >",
                            VerticalTextAlignment = TextAlignment.Start,
                            FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null),
                            TextColor = Color.FromHex("#DBD4C8"),
                            FontSize = 16,
                        };
                        Label chat = new Label()
                        {
                            Text = "Chat >",
                            VerticalTextAlignment = TextAlignment.End,
                            FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null),
                            TextColor = Color.FromHex("#01528a"),
                            FontSize = 14,
                        };
                        StackLayout feed_chat = new StackLayout()
                        {
                            Orientation = StackOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Start,
                            Spacing = 10,
                            Children =
                            {
                                feed,
                                chat
                            }
                        };
                        StackLayout usuario1 = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,
                            ClassId = valor.ElementAt(j).id + "|" + valor.ElementAt(j).titulo + "|" + valor.ElementAt(j).foto + "|" + valor.ElementAt(j).chat,
                            Margin = new Thickness(0, 0, 0, 10),
                            Spacing = 15,
                            Children =
                            {
                                foto,
                                nombre_ultimo,
                                feed_chat
                            }
                        };
                        nombre_ultimo.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(() => {
                                try
                                {
                                    Navigation.PushAsync(new Ranking(usuario1.ClassId));
                                }
                                catch (Exception ex)
                                {
                                    DisplayAlert("Help", ex.Message, "OK");
                                }
                            }),
                            NumberOfTapsRequired = 1
                        });
                        feed.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(() => {
                                try
                                {
                                    Navigation.PushAsync(new Ranking(usuario1.ClassId));
                                }
                                catch (Exception ex)
                                {
                                    DisplayAlert("Help", ex.Message, "OK");
                                }
                            }),
                            NumberOfTapsRequired = 1
                        });
                        chat.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(() => {
                                try
                                {
                                    Navigation.PushAsync(new Chat(usuario1.ClassId.Split('|')[3], usuario1.ClassId.Split('|')[0]));
                                }
                                catch (Exception ex)
                                {
                                    DisplayAlert("Help", ex.Message, "OK");
                                }
                            }),
                            NumberOfTapsRequired = 1
                        });
                        stack_grupos.Children.Add(usuario1);
                        stack_grupos.Children.Add(linea);
                    }
                }
                else
                {
                    Label nombre = new Label()
                    {
                        Text = "No hay grupos por mostrar, ¡crea tu primer grupo con tus amigos!",
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                        Margin = new Thickness(30,70,30,70),
                        FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null),
                        TextColor = Color.FromHex("#01528a"),
                        FontSize = 24,
                    };
                    stack_grupos.Children.Add(nombre);
                }
            }
            catch (Exception ex)
            {

            }
            
        }


        public List<class_grupos> procesar(string respuesta)
        {
            List<class_grupos> items = new List<class_grupos>();
            if (respuesta == "0")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_grupos
                             {
                                 id = (string)r.Element("id"),
                                 chat = (string)r.Element("chat"),
                                 ranking = (string)r.Element("ranking"),
                                 titulo = WebUtility.UrlDecode((string)r.Element("titulo")),
                                 descripcion = WebUtility.UrlDecode((string)r.Element("descripcion")),
                                 autor = WebUtility.UrlDecode((string)r.Element("autor")),
                                 fecha = WebUtility.UrlDecode((string)r.Element("fecha")),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
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
                {
                    using (var sr = new StreamReader(responseStream))
                    {

                        received = await sr.ReadToEndAsync();
                    }
                }
            }

            return received;
        }
    }

}