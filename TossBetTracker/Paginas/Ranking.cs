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
    public class Ranking : ContentPage
    {
        StackLayout stack1 = null;
        string idgrupo = "";
        string titulogrupo = "";
        string fotogrupo = "";
        string idchat = "";
        string grupo1 = "";

        AbsoluteLayout absoluteLayout = new AbsoluteLayout();
        StackLayout stackranking = null;
        StackLayout menusup_top = null;

        public Ranking(string grupo)
        {
            grupo1 = grupo;
            idgrupo = grupo.Split('|')[0];
            titulogrupo = grupo.Split('|')[1];
            fotogrupo = grupo.Split('|')[2];
            idchat = grupo.Split('|')[3];
            BackgroundColor = Color.White;
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            imagenchat.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Navigation.PushAsync(new Chat(idchat, idgrupo, grupo)); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);

            absoluteLayout.BackgroundColor = Color.White;
            stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(0, 0, 0, 20), Spacing = 0 };
            //MENU SUPERIOR
            menusup_top = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Padding = new Thickness(0), BackgroundColor = Color.White };
            stack1.Children.Add(menusup_top);



            stackranking = new StackLayout() { VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20,40) };
            Addtop();
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stackranking };
            stack1.Children.Add(scv1);
            //GENERALES

            AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(stack1);

            

            

            //MENU INFERIOR
            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            //ASIGNAR LAYOUT
            this.Content = absoluteLayout;
            ConsultarP();
        }

        public void Addtop()
        {
            try
            {
                Label Titulogrupo = new Label() { Margin = new Thickness(0, 10, 0, 0), Text = titulogrupo, FontSize = 26, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                CircleImage foto = new CircleImage
                {
                    HeightRequest = 70,
                    WidthRequest = 70,
                    Margin = new Thickness(0, 0, 20, 0),
                    Aspect = Aspect.AspectFill,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Source = new UriImageSource
                    {
                        Uri = new Uri(fotogrupo),
                        CachingEnabled = true,
                    },
                };
                StackLayout stacktop = new StackLayout() { Padding = new Thickness(20, 20, 20, 0), Orientation = StackOrientation.Horizontal, Children = { foto, Titulogrupo } };
                stacktop.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Navigation.PushAsync(new Info_grupo(idgrupo)); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                menusup_top.Children.Add(stacktop);

                //PESTAÑAS
                Label activa = new Label() { HeightRequest = 3, BackgroundColor = Color.FromHex("#2DC9EB"), VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand };
                Label tu = new Label() { Text = "RANKING", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                Label amigos = new Label() { Text = "FEED", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#D7D7D7"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                Label inactiva = new Label() { HeightRequest = 3, BackgroundColor = Color.FromHex("#F7F4F4"), VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand };
                StackLayout izq = new StackLayout() { Spacing = 0, WidthRequest = double.Parse(Settings.Ancho) / 2, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.EndAndExpand };
                izq.Children.Add(tu);
                izq.Children.Add(activa);
                StackLayout der = new StackLayout() { Spacing = 0, WidthRequest = double.Parse(Settings.Ancho) / 2, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.EndAndExpand };
                der.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try {
                        Application.Current.MainPage.Navigation.PushAsync(new Detalle_grupo(grupo1));
                    } catch (Exception ex) {
                        Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK");
                    } }), NumberOfTapsRequired = 1 });
                der.Children.Add(amigos);
                der.Children.Add(inactiva);
                StackLayout pestanas = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.FromHex("#FFFFFF"), Padding = new Thickness(0, 15, 0, 0) };
                pestanas.Children.Add(izq);
                pestanas.Children.Add(der);

                menusup_top.Children.Add(pestanas);
            }
            catch (Exception ex)
            {

            }
        }

        public async void ConsultarP()
        {
            try
            {
                
                UserDialogs.Instance.ShowLoading("Cargando ranking…");
                await actualizar();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Ayuda", ex.Message, "OK");

            }
        }

        public async Task actualizar()
        {
            try
            {
                stackranking.Children.Clear();
                string uriString2 = string.Format("http://toss.boveda-creativa.com/miembros2.php?grupo={0}", idgrupo);
                var response2 = await httpRequest(uriString2);
                if (response2.Length > 10)
                {
                    List<class_usuarios> valor = new List<class_usuarios>();

                    valor = procesar(response2);
                    IEnumerable<class_usuarios> sortDescendingQuery =
                    from w in valor
                    orderby w.ranking
                    select w;
                    valor = sortDescendingQuery.ToList();
                    for (int j = 0; j < valor.Count(); j++)
                    {
                        CircleImage foto = new CircleImage
                        {
                            HeightRequest = 45,
                            WidthRequest = 45,
                            Aspect = Aspect.AspectFill,
                            ClassId = valor.ElementAt(j).idusuario,
                            HorizontalOptions = LayoutOptions.Center,
                            Source = ImageSource.FromUri(new Uri(valor.ElementAt(j).foto))
                        };
                        foto.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async() => { try {
                                await foto.ScaleTo(1.1, 100);
                                await foto.ScaleTo(1, 100);
                                await Navigation.PushAsync(new PerfilAmigo(foto.ClassId));
                            } catch (Exception ex) { await DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                        Label nombre = new Label()
                        {
                            Text = valor.ElementAt(j).nombre,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalTextAlignment = TextAlignment.Center,
                            HeightRequest = 40
                        };
                        Label posicion = new Label() { LineBreakMode = LineBreakMode.NoWrap,Text = "#" + valor.ElementAt(j).ranking, TextColor = Color.White, FontSize = 12, WidthRequest = 15 };
                        Frame frameposicion = new Frame() { CornerRadius = 5, HasShadow = false, Padding = new Thickness(1), BackgroundColor = Color.FromHex("#DFA423"), Content = posicion, VerticalOptions = LayoutOptions.Center };
                        Image moneda = new Image() { WidthRequest = 20, Source = ImageSource.FromFile("moneda.png"), HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.CenterAndExpand };
                        Label coins = new Label()
                        {
                            Text = valor.ElementAt(j).coins,
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            VerticalTextAlignment = TextAlignment.Center,
                            TextColor = Color.FromHex("#676464"),
                            HeightRequest = 40,
                            LineBreakMode = LineBreakMode.NoWrap,
                        };
                        StackLayout ranking = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { frameposicion, nombre, moneda, coins }, HorizontalOptions = LayoutOptions.FillAndExpand };
                        
                        Label linea = new Label()
                        {
                            BackgroundColor = Color.FromHex("#D1D3D4"),
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };

                        StackLayout usuario1 = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,

                            Spacing = 15,
                            Children =
                            {
                                foto,
                                ranking,
                            }
                        };
                        stackranking.Children.Add(usuario1);
                        stackranking.Children.Add(linea);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: {0}", ex);
            }
        }

        public List<class_usuarios> procesar(string respuesta)
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
                                 idusuario = (string)r.Element("usuario"),
                                 ranking = (string)r.Element("ranking"),
                                 coins = (string)r.Element("coins"),
                                 nombre = WebUtility.UrlDecode((string)r.Element("nombre")),
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
                { using (var sr = new StreamReader(responseStream)) { received = await sr.ReadToEndAsync(); } }
            }
            return received;
        }
    }
}

