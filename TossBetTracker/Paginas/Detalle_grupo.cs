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
    public class Detalle_grupo : ContentPage
    {
        StackLayout stack1 = null;
        ScrollView Scrollview1 = null;
        StackLayout form1 = null;
        Label semanales = null;
        Label mensuales = null;
        string idgrupo = "";
        string titulogrupo = "";
        string fotogrupo = "";
        string idchat = "";
        AbsoluteLayout absoluteLayout = new AbsoluteLayout();
        StackLayout menusup_top = null;
        StackLayout stackfeed = null;

        public Detalle_grupo(string grupo)
        {
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
            stackfeed = new StackLayout() { VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 40) };
            Addtop();
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stackfeed };
            stack1.Children.Add(scv1);
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
                Label Titulogrupo = new Label() { Margin = new Thickness(0, 0, 0, 0), Text = titulogrupo, FontSize = 18, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
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
                Label tu = new Label() { Text = "RANKING", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#D7D7D7"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                Label amigos = new Label() { Text = "FEED", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                Label inactiva = new Label() { HeightRequest = 3, BackgroundColor = Color.FromHex("#F7F4F4"), VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand };
                StackLayout izq = new StackLayout() { Spacing = 0, WidthRequest = double.Parse(Settings.Ancho) / 2, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.EndAndExpand };
                izq.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Application.Current.MainPage.Navigation.PopAsync(); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                izq.Children.Add(tu);
                izq.Children.Add(inactiva);
                StackLayout der = new StackLayout() { Spacing = 0, WidthRequest = double.Parse(Settings.Ancho) / 2, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.EndAndExpand };
                der.Children.Add(amigos);
                der.Children.Add(activa);
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
                UserDialogs.Instance.ShowLoading("Cargando feed…");
                string uriString2 = "http://toss.boveda-creativa.com/apuestasgrupo.php?grupo=" + idgrupo;
                var response2 = await httpRequest(uriString2);
                List<class_apuestas_publicas> valor = new List<class_apuestas_publicas>();
                valor = procesar(response2);
                StackLayout stack_partido = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Start,
                    Padding = new Thickness(0, 20, 0, 30),
                    ClassId = "STACKPARTIDO"
                };
                int contador = 0;
                for (int j = 0; j < valor.Count(); j++)
                {
                    string tipo = "";
                    if (valor.ElementAt(j).tipo == "run_line_1") { tipo = "Spread"; }
                    else if (valor.ElementAt(j).tipo == "run_line_2") { tipo = "Spread"; }
                    else if (valor.ElementAt(j).tipo == "money_line_1") { tipo = "Money\nLine"; }
                    else if (valor.ElementAt(j).tipo == "money_line_2") { tipo = "Money\nLine"; }
                    else if (valor.ElementAt(j).tipo == "total_o") { tipo = "Over"; }
                    else if (valor.ElementAt(j).tipo == "total_u") { tipo = "Under"; }
                    else if (valor.ElementAt(j).tipo == "Palay") { tipo = "Parlay"; }
                    string stringtitulo = "";
                    if (valor.ElementAt(j).equipo1 == "Apuesta Parlay")
                    {
                        stringtitulo = valor.ElementAt(j).equipo1;
                    }
                    else
                    {
                        stringtitulo = valor.ElementAt(j).equipo1 + " vs " + valor.ElementAt(j).equipo2;
                    }
                    Label Titulo = new Label() { Text = stringtitulo, FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Image icono_deporte = new Image() { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile(valor.ElementAt(j).deporte + ".png"), WidthRequest = 25 };
                    StackLayout titulo_logo = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4, Children = { icono_deporte, Titulo } };
                    stack_partido.Children.Add(titulo_logo);
                    CircleImage imagen_usuario = new CircleImage() { ClassId = valor.ElementAt(j).usuario, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.CenterAndExpand, Source = ImageSource.FromUri(new Uri(valor.ElementAt(j).foto)), WidthRequest = 35, HeightRequest = 35, Aspect = Aspect.AspectFill };
                    imagen_usuario.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(async () => {
                            try
                            {
                                await imagen_usuario.ScaleTo(1.1, 100);
                                await imagen_usuario.ScaleTo(1, 100);
                                await Navigation.PushAsync(new PerfilAmigo(imagen_usuario.ClassId));
                            }
                            catch (Exception ex)
                            {
                                await Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
                            }
                        }),
                        NumberOfTapsRequired = 1
                    });
                    Label Tipo = new Label() { Text = tipo, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#E0B928"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    StackLayout stackcol1 = new StackLayout() { Spacing = 2, Padding = new Thickness(5, 0, 5, 0), Children = { imagen_usuario, Tipo } };
                    Label nombre = new Label() { Text = valor.ElementAt(j).nombre, FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    string eq = valor.ElementAt(j).equipo;
                    if (valor.ElementAt(j).equipo1 == "Apuesta Parlay") { eq = "Múltiples equipos"; }
                    Label equipo = new Label() { Text = eq, FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    //Label Monto = new Label() { Text = "$" + valor.ElementAt(j).monto, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    StackLayout stackcol2 = new StackLayout() { Padding = new Thickness(0, 15, 0, 0), HorizontalOptions = LayoutOptions.FillAndExpand, Children = { nombre, equipo } };
                    string imagenestatus = "ganado.png";
                    if (valor.ElementAt(j).estatus == "GANADA")
                    {
                        imagenestatus = "ganado.png";
                    }
                    else if (valor.ElementAt(j).estatus == "PERDIDA")
                    {
                        imagenestatus = "perdido.png";
                    }
                    else
                    {
                        imagenestatus = "abierta.png";
                    }
                    Image imgestatus = new Image() { Source = imagenestatus, WidthRequest = 20, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start };
                    Label nriesgo = new Label() { Text = valor.ElementAt(j).riesgo, FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Image imagetoss = new Image() { Source = ImageSource.FromFile("moneda.png"), WidthRequest = 20, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start };
                    StackLayout stacktoss = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { imagetoss, nriesgo }, HorizontalOptions = LayoutOptions.EndAndExpand };
                    StackLayout stackcol3 = new StackLayout() { Padding = new Thickness(0, 15, 0, 0), Children = { imgestatus, stacktoss }, HorizontalOptions = LayoutOptions.EndAndExpand };

                    StackLayout stackapuesta = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4, Children = { stackcol1, stackcol2, stackcol3 } };
                    Frame frameapuesta = new Frame() { Margin = new Thickness(0, 0, 0, 20), HasShadow = true, IsClippedToBounds = true, Padding = new Thickness(15, 5), Content = stackapuesta, ClassId = valor.ElementAt(j).id };
                    frameapuesta.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() => {
                            try
                            {
                                Navigation.PushAsync(new Verapuesta(frameapuesta.ClassId));
                            }
                            catch (Exception ex)
                            {
                                Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
                            }
                        }),
                        NumberOfTapsRequired = 1
                    });
                    stack_partido.Children.Add(frameapuesta);
                    contador++;
                }
                stackfeed.Children.Add(stack_partido);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Ayuda", ex.Message, "OK");

            }
        }

        public List<class_apuestas_publicas> procesar(string respuesta)
        {
            List<class_apuestas_publicas> items = new List<class_apuestas_publicas>();
            if (respuesta == "")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_apuestas_publicas
                             {
                                 id = (string)r.Element("id"),
                                 usuario = (string)r.Element("usuario"),
                                 nombre = WebUtility.UrlDecode((string)r.Element("nombre")),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
                                 trn_date = WebUtility.UrlDecode((string)r.Element("trn_date")),
                                 partido = WebUtility.UrlDecode((string)r.Element("partido")),
                                 equipo = WebUtility.UrlDecode((string)r.Element("equipo")),
                                 momio = WebUtility.UrlDecode((string)r.Element("momio")),
                                 tipo = WebUtility.UrlDecode((string)r.Element("tipo")),
                                 ganancia = WebUtility.UrlDecode((string)r.Element("ganancia")),
                                 monto = WebUtility.UrlDecode((string)r.Element("monto")),
                                 fecha = WebUtility.UrlDecode((string)r.Element("fecha")),
                                 estatus = WebUtility.UrlDecode((string)r.Element("estatus")),
                                 publico = WebUtility.UrlDecode((string)r.Element("publico")),
                                 equipo1 = WebUtility.UrlDecode((string)r.Element("equipo1")),
                                 equipo2 = WebUtility.UrlDecode((string)r.Element("equipo2")),
                                 titulo = WebUtility.UrlDecode((string)r.Element("titulo")),
                                 riesgo = WebUtility.UrlDecode((string)r.Element("riesgo")),
                                 riesgoganado = WebUtility.UrlDecode((string)r.Element("riesgoganado")),
                                 deporte = WebUtility.UrlDecode((string)r.Element("deporte"))
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

