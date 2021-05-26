using Microcharts;
using SkiaSharp;
using Microcharts.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.Entry;
using System.Net;
using System.IO;
using System.Xml.Linq;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Net.Http;
using System.Net.Http.Headers;
using Acr.UserDialogs;

namespace TossBetTracker
{
    public partial class PerfilAmigo : ContentPage
    {

        List<Entry> entries_aciertos = new List<Entry> { };
        MediaFile file = null;
        string idusuario = "";

        public PerfilAmigo(string id)
        {
            idusuario = id;
            InitializeComponent();
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Label Titulo0 = new Label() { Text = "Perfil Amigo", WidthRequest = 200, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Image imagenchat = new Image() { IsVisible = false, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { Titulo0, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            absolutelayout.Children.Add(Home.cargar_menu_inferior());
            UserDialogs.Instance.ShowLoading("Cargando tus apuestas …");
            cargar_aciertos();
            cargar_perfil();
            ConsultarFeed();
            UserDialogs.Instance.HideLoading();
        }

        public async void cargar_perfil()
        {
            try
            {
                string uriString2 = string.Format("http://toss.boveda-creativa.com/usuario.php?usuario={0}", idusuario);
                var response2 = await httpRequest(uriString2);
                List<class_usuarios> valor = new List<class_usuarios>();
                valor = procesar3(response2);
                for (int i = 0; i < valor.Count(); i++)
                {
                    try
                    {
                        fotoperfil.Source = ImageSource.FromUri(new Uri(valor.ElementAt(i).foto));
                    }
                    catch (Exception ex)
                    {
                        fotoperfil.Source = ImageSource.FromFile("profile.png");
                    }

                    nombreusuario.Text = valor.ElementAt(i).nombre;
                }
                

                string uriString_aciertos = "http://toss.boveda-creativa.com/bets.php?usuario=" + idusuario;
                var bets = await httpRequest(uriString_aciertos);
                lblapuestas.Text = bets;
                string uriString_utcap = "http://toss.boveda-creativa.com/utilidad_capital_personal.php?usuario=" + idusuario;
                var uti_cap = await httpRequest(uriString_utcap);
                if (uti_cap.ToString() == "nan") { uti_cap = "0"; }
                lblutscap.Text = uti_cap + "%";
                string uriString_utap = "http://toss.boveda-creativa.com/utilidad_apuesta_personal.php?usuario=" + idusuario;
                var utiap = await httpRequest(uriString_utap);
                if (utiap.ToString() == "nan") { utiap = "0"; }
                lblutsap.Text = utiap + "%";
                string uriString_tosscoins = "http://toss.boveda-creativa.com/tosscoins.php?usuario=" + idusuario;
                var tosscoins = await httpRequest(uriString_tosscoins);
                lbltosscoins.Text = tosscoins;
                Label Titulo = new Label() { Text = "Solicitar amistad ", FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                Image solicitar = new Image()
                {
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center,
                    Source = ImageSource.FromFile("friends_solicitar"),
                    WidthRequest = 28,
                    ClassId = idusuario
                };
                Label solicitado = new Label()
                {
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center,
                    Text = "Enviado",
                    FontSize = 10,
                };
                solicitar.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () => {
                        try
                        {
                            string uriString3 = string.Format("https://toss.boveda-creativa.com/solicitud_amistad.php?usuario={0}&usuario2={1}", Settings.Idusuario, solicitar.ClassId);
                            var response3 = await httpRequest(uriString3);
                            solicitar.WidthRequest = 1;
                            solicitar.Opacity = 0;
                            StackLayout padre = (StackLayout)solicitar.Parent;
                            padre.Children.Remove(solicitar);
                            padre.Children.Add(solicitado);
                        }
                        catch (Exception ex)
                        {
                            DisplayAlert("Help", ex.Message, "OK");
                        }
                    }),
                    NumberOfTapsRequired = 1
                });
                StackLayout stackhorizontal = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        Titulo,
                        solicitar
                    }
                };
                stackgen.Children.Add(stackhorizontal);
            }
            catch (Exception ex)
            {

            }
        }

        public async void cargar_aciertos()
        {
            try
            {
                string uriString2 = string.Format("http://toss.boveda-creativa.com/graf_aciertos.php?usuario={0}", idusuario);
                var response2 = await httpRequest(uriString2);
                List<class_grafica> valor = new List<class_grafica>();
                valor = procesar(response2);
                for (int i = 0; i < valor.Count; i++)
                {
                    Entry entry1 = new Entry(int.Parse(valor.ElementAt(i).datoy))
                    {
                        Label = valor.ElementAt(i).datox,
                        ValueLabel = valor.ElementAt(i).datoy
                    };
                    if (valor.ElementAt(i).datox == "Ganadas")
                    {
                        entry1.Color = SKColor.Parse("#DFA423");
                        lblganadas.Text = valor.ElementAt(i).datoy + "%";
                    }
                    else
                    {
                        entry1.Color = SKColor.Parse("#025189");
                        lblperdidas.Text = valor.ElementAt(i).datoy + "%";
                    }
                    entries_aciertos.Add(entry1);
                }
                Chart4.Chart = new DonutChart() { Entries = entries_aciertos, LabelTextSize = 0, HoleRadius = 0.7f };
            }
            catch (Exception ex)
            {

            }
        }

        public async void ConsultarFeed()
        {
            try
            {
                string uriString2 = "http://toss.boveda-creativa.com/misapuestas.php?usuario=" + idusuario + "&limite=10";
                var response2 = await httpRequest(uriString2);
                List<class_apuestas> valor = new List<class_apuestas>();
                valor = procesar2(response2);
                StackLayout stack_partido = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Start,
                    Padding = new Thickness(20, 30, 20, 30),
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
                    else if (valor.ElementAt(j).tipo == "Parlay") { tipo = "Parlay"; }
                    else if (tipo == "") { tipo = valor.ElementAt(j).tipo; }
                    string stringtitulo = "";
                    if (valor.ElementAt(j).equipo1 == "Apuesta Parlay")
                    {
                        stringtitulo = valor.ElementAt(j).equipo1;
                    }
                    else
                    {
                        stringtitulo = valor.ElementAt(j).equipo1 + " vs " + valor.ElementAt(j).equipo2;
                    }
                    string foto = valor.ElementAt(j).deporte;
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
                    if (foto == "parlay")
                    {
                        foto = "parlay";
                    }
                    else
                    {
                        foto = valor.ElementAt(j).deporte.ToUpper();
                    }
                    Label Titulo = new Label() { Text = stringtitulo, FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Image icono_deporte = new Image() { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile(foto + ".png"), WidthRequest = 25 };
                    StackLayout titulo_logo = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4, Children = { icono_deporte, Titulo } };
                    stack_partido.Children.Add(titulo_logo);
                    Image imagen_tipo = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("Bank.png"), WidthRequest = 35 };
                    Label Tipo = new Label() { Text = tipo + " " + valor.ElementAt(j).valor1, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#E0B928"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    //StackLayout stackcol1 = new StackLayout() { Spacing = 2, Padding = new Thickness(5, 0, 5, 0), Children = {  Tipo } };
                    string eq = valor.ElementAt(j).equipo;
                    if (valor.ElementAt(j).equipo1 == "Apuesta Parlay") { eq = "Múltiples equipos"; }
                    Label equipo = new Label() { Text = eq, FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    //Label Monto = new Label() { Text = "$" + valor.ElementAt(j).monto, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    StackLayout stackcol2 = new StackLayout() { Padding = new Thickness(0, 15, 0, 0), HorizontalOptions = LayoutOptions.FillAndExpand, Children = { equipo, Tipo } };
                    Color colorestatus = Color.FromHex("#979797");

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
                    Image imgestatus = new Image() { Source = imagenestatus, WidthRequest = 20, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start };
                    //Label ganancia = new Label() { Text = "Ganancia: $" + valor.ElementAt(j).ganancia, FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };

                    Label nriesgo = new Label() { Text = valor.ElementAt(j).riesgo, FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Image imagetoss = new Image() { Source = ImageSource.FromFile("moneda.png"), WidthRequest = 20, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start };
                    StackLayout stacktoss = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { imagetoss, nriesgo }, HorizontalOptions = LayoutOptions.EndAndExpand };
                    StackLayout stackcol3 = new StackLayout() { Padding = new Thickness(0, 15, 0, 0), Children = { imgestatus, stacktoss }, WidthRequest = 150, HorizontalOptions = LayoutOptions.EndAndExpand };

                    StackLayout stackapuesta = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4, Children = { stackcol2, stackcol3 } };
                    Frame frameapuesta = new Frame() { Visual = VisualMarker.Material, Margin = new Thickness(0, 0, 0, 20), HasShadow = true, IsClippedToBounds = true, Padding = new Thickness(15, 5), Content = stackapuesta, ClassId = valor.ElementAt(j).id };
                    frameapuesta.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(async () => {
                            try
                            {
                                await frameapuesta.ScaleTo(1.1, 100);
                                await frameapuesta.ScaleTo(1, 100);
                                await Navigation.PushAsync(new Verapuesta(frameapuesta.ClassId));
                            }
                            catch (Exception ex)
                            {
                                Application.Current.MainPage.DisplayAlert("Ayuda1", ex.Message, "OK");
                            }
                        }),
                        NumberOfTapsRequired = 1
                    });
                    stack_partido.Children.Add(frameapuesta);
                    contador++;
                }
                Stackfeed.Children.Add(stack_partido);
            }
            catch (Exception ex)
            {
            }
        }

        public List<class_usuarios> procesar3(string respuesta)
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
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
                             }).ToList();
                }
            }
            return items;
        }

        public List<class_apuestas> procesar2(string respuesta)
        {
            List<class_apuestas> items = new List<class_apuestas>();
            if (respuesta == "")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_apuestas
                             {
                                 id = (string)r.Element("id"),
                                 trn_date = WebUtility.UrlDecode((string)r.Element("trn_date")),
                                 partido = WebUtility.UrlDecode((string)r.Element("partido")),
                                 equipo = WebUtility.UrlDecode((string)r.Element("equipo")),
                                 momio = WebUtility.UrlDecode((string)r.Element("momio")),
                                 tipo = WebUtility.UrlDecode((string)r.Element("tipo")),
                                 valor1 = WebUtility.UrlDecode((string)r.Element("valor1")),
                                 ganancia = WebUtility.UrlDecode((string)r.Element("ganancia")),
                                 monto = WebUtility.UrlDecode((string)r.Element("monto")),
                                 fecha = WebUtility.UrlDecode((string)r.Element("fecha")),
                                 estatus = WebUtility.UrlDecode((string)r.Element("estatus")),
                                 publico = WebUtility.UrlDecode((string)r.Element("publico")),
                                 equipo1 = WebUtility.UrlDecode((string)r.Element("equipo1")),
                                 equipo2 = WebUtility.UrlDecode((string)r.Element("equipo2")),
                                 titulo = WebUtility.UrlDecode((string)r.Element("titulo")),
                                 deporte = WebUtility.UrlDecode((string)r.Element("deporte")),
                                 riesgo = WebUtility.UrlDecode((string)r.Element("riesgo")),
                                 riesgoganado = WebUtility.UrlDecode((string)r.Element("riesgoganado"))
                             }).ToList();
                }
            }
            return items;
        }

        public List<class_grafica> procesar(string respuesta)
        {
            List<class_grafica> items = new List<class_grafica>();
            if (respuesta == "")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_grafica
                             {
                                 datox = (string)r.Element("datox"),
                                 datoy = (string)r.Element("datoy")
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
