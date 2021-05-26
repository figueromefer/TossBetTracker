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
    public class Verapuesta : ContentPage
    {
        StackLayout stack1 = null;
        StackLayout stackboleto = null;
        AbsoluteLayout absoluteLayout = null;
        StackLayout stacketiqueta1, stacketiqueta2, stacketiqueta3 = null;
        string idapuesta = "";

        public Verapuesta(string apuesta)
        {
            idapuesta = apuesta;
            BackgroundColor = Color.White;
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { BackgroundColor = Color.FromHex("#01528a"), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(0, 0, 0, 0), Spacing = 0 };
            Label Tituloresumen = new Label() { Margin = new Thickness(20,10),Text = "Resumen", FontSize = 26, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            stack1.Children.Add(Tituloresumen);
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack1, Padding = new Thickness(0, 0, 0, 0) };
            //GENERALES

            AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(scv1);

            //MENU INFERIOR
            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            //ASIGNAR LAYOUT
            this.Content = absoluteLayout;
            cargar_apuesta();
        }

        public async void cargar_apuesta()
        {
            try
            {
                string tipogen = "";
                string momiogen = "";
                string tipo2gen = "";
                string estatusgen = "";
                string uriString2 = "http://toss.boveda-creativa.com/apuesta.php?id=" + idapuesta;
                var response2 = await httpRequest(uriString2);
                List<class_apuestas> valor = new List<class_apuestas>();
                valor = procesar2(response2);
                for (int j = 0; j < valor.Count(); j++)
                {
                    tipogen = valor.ElementAt(j).tipo;
                    momiogen = valor.ElementAt(j).momio;
                    tipo2gen = valor.ElementAt(j).tipo2;
                    string eq = valor.ElementAt(j).equipo;
                    if (valor.ElementAt(j).tipo == "Parlay") { eq = "Múltiples equipos"; }
                    estatusgen = valor.ElementAt(j).estatus;

                    //Resumen
                    string extra = "";
                    if (int.Parse(valor.ElementAt(j).momio) > 0)
                    {
                        extra = "+";
                    }

                    string extra2 = "";
                    string valor1 = valor.ElementAt(j).extra;
                    try
                    {
                        if (int.Parse(valor.ElementAt(j).extra.Split('.')[0]) > 0)
                        {
                            extra2 = "";
                        }
                        if (tipogen == "Money line" || tipogen == "Parlay")
                        {
                            extra2 = "";
                            valor1 = "";
                        }
                        else
                        {
                            valor1 = extra2 + valor1;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    string agregar = valor.ElementAt(j).tipo + " " + valor1;

                        Label Labelequipo = new Label() { Text = "Equipo: " + eq, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Label Labelmomio = new Label() { Text = "Momio: " + extra +valor.ElementAt(j).momio, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    if (Settings.Momio != "") { Labelmomio.Text = "Momio: " + americano_decimal(valor.ElementAt(j).momio); }
                    Label Labeltipo = new Label() { Text = "Tipo: " + valor.ElementAt(j).tipo + " " + valor1 , FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Label Labelganancia = new Label() { Text = "Ganancia: " + valor.ElementAt(j).ganancia, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Label Labelmonto = new Label() { Text = "Monto: " + valor.ElementAt(j).monto, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Label Labelcomentario = new Label() { Text = "Comentarios: " + valor.ElementAt(j).comentario, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    string classid = "";
                    CircleImage imagen_usuario = new CircleImage() { ClassId = valor.ElementAt(j).usuario, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.CenterAndExpand, Source = ImageSource.FromUri(new Uri(valor.ElementAt(j).foto)), WidthRequest = 85, HeightRequest = 85, Aspect = Aspect.AspectFill };
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
                    Label lblnombre = new Label() { Text = valor.ElementAt(j).nombre, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    StackLayout stackcol1 = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        Padding = new Thickness(10, 0),
                        Children = {
                            imagen_usuario,
                            lblnombre
                        }
                    };

                    StackLayout stackcol2 = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        WidthRequest = 200,
                        VerticalOptions = LayoutOptions.Start,
                        Padding = new Thickness(10, 0),
                        Children = {
                            Labelequipo,
                            Labelmomio,
                            Labeltipo,
                            Labelcomentario,
                        }
                    };
                    StackLayout stackrow = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        Orientation = StackOrientation.Horizontal,
                        Padding = new Thickness(10, 30),
                        Children = {
                            stackcol1,
                            stackcol2
                        }
                    };

                    if (valor.ElementAt(j).usuario == Settings.Idusuario)
                    {
                        stackcol2.Children.Add(Labelganancia);
                        stackcol2.Children.Add(Labelmonto);
                    }
                    stack1.Children.Add(stackrow);
                    StackLayout stack_partido = new StackLayout()
                    {
                        BackgroundColor = Color.White,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(20,30,20,60),
                    };
                    stack1.Children.Add(stack_partido);
                    if(valor.ElementAt(j).partido == "0")
                    {
                        string uriString3 = "http://toss.boveda-creativa.com/partidos_parlay.php?id=" + valor.ElementAt(j).id;
                        var response3 = await httpRequest(uriString3);
                        List<class_partido_parlay> valor3 = new List<class_partido_parlay>();
                        valor3 = procesar3(response3);
                        for (int i = 0; i < valor3.Count(); i++)
                        {
                            string tipo = valor3.ElementAt(i).llave;
                            Label Labelseleccionado = new Label() { Text = valor3.ElementAt(i).seleccionado, FontSize = 20, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                            Label Labelpartido = new Label() { Text = valor3.ElementAt(i).equipo1 + " vs " + valor3.ElementAt(i).equipo2, FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                            Label Labelfecha = new Label() { Text = valor3.ElementAt(i).fecha.Split(' ')[0], FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                            Label Labelhora = new Label() { Text = valor3.ElementAt(i).fecha.Split(' ')[1], FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                            string imagenestatus = "ganado.png";
                            BoxView barra = new BoxView() { WidthRequest = 5,  HorizontalOptions = LayoutOptions.Start, };
                            if (valor3.ElementAt(i).estatus == "GANADA")
                            {
                                imagenestatus = "ganado.png";
                                barra.BackgroundColor = Color.FromHex("#4EDE65");
                            }
                            else if (valor3.ElementAt(i).estatus == "PERDIDA")
                            {
                                imagenestatus = "perdido.png";
                                barra.BackgroundColor = Color.Red;
                            }
                            else
                            {
                                barra.BackgroundColor = Color.Gray;
                                imagenestatus = "abierta.png";
                            }
                            Image imgestatus = new Image() { Source = imagenestatus, WidthRequest = 20, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start };
                            Label Labeltipo1 = new Label() { Text = tipo+" "+ valor3.ElementAt(i).extra, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                            Label Labelmomio1 = new Label() { Text = valor3.ElementAt(i).momio, FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                            
                            //if (Settings.Momio != "") { Labelmomio1.Text = "Momio: " + americano_decimal(valor3.ElementAt(i).momio); }
                            StackLayout stacktipomomio = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Horizontal, Children = { Labeltipo1, Labelmomio1 } };
                            StackLayout stackfecha = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Horizontal, Children = { Labelfecha, Labelhora } };
                            
                            StackLayout stackpartido = new StackLayout() { Padding = new Thickness(10, 5, 10, 5), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Children = { Labelseleccionado, Labelpartido, stacktipomomio, stackfecha, imgestatus } };
                            
                            StackLayout stackpartido0 = new StackLayout() { Spacing = 0, Orientation = StackOrientation.Horizontal, Children = { barra, stackpartido } };
                            Frame framepartido = new Frame() { Padding = new Thickness(0), Margin = new Thickness(0,0,0,20),HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start,  HasShadow = true, Visual = VisualMarker.Material, Content = stackpartido0 };
                            stack_partido.Children.Add(framepartido);
                            barra.HeightRequest = framepartido.Height;
                        }
                    }
                    else
                    {
                        string uriString3 = "http://toss.boveda-creativa.com/partido.php?id=" + valor.ElementAt(j).partido;
                        var response3 = await httpRequest(uriString3);
                        List<class_partido_parlay> valor3 = new List<class_partido_parlay>();
                        valor3 = procesar3(response3);
                        for (int i = 0; i < valor3.Count(); i++)
                        {
                            String tipo1 = "";

                            if (tipo2gen == "money_line_1") { tipo1 = "Money line"; }
                            if (tipo2gen == "money_line_2") { tipo1 = "Money line"; }
                            if (tipo2gen == "run_line_1") { tipo1 = "Spread"; }
                            if (tipo2gen == "run_line_2") { tipo1 = "Spread"; }
                            if (tipo2gen == "totalo") { tipo1 = "Over"; }
                            if (tipo2gen == "totalu") { tipo1 = "Under"; }
                            Label Labelseleccionado = new Label() { Text = eq, FontSize = 20, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                            Label Labelpartido = new Label() { Text = valor3.ElementAt(i).equipo1 + " vs " + valor3.ElementAt(i).equipo2, FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                            Label Labelfecha = new Label() { Text = valor3.ElementAt(i).fecha.Split(' ')[0], FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                            Label Labelhora = new Label() { Text = valor3.ElementAt(i).fecha.Split(' ')[1], FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };

                            string imagenestatus = "ganado.png";
                            BoxView barra = new BoxView() { WidthRequest = 5, BackgroundColor = Color.Green, HorizontalOptions = LayoutOptions.Start, };
                            if (estatusgen == "GANADA")
                            {
                                barra.BackgroundColor = Color.FromHex("#4EDE65");
                                imagenestatus = "ganado.png";
                            }
                            else if (estatusgen == "PERDIDA")
                            {
                                barra.BackgroundColor = Color.Red;
                                imagenestatus = "perdido.png";
                            }
                            else
                            {
                                barra.BackgroundColor = Color.Gray;
                                imagenestatus = "abierta.png";
                            }
                            Image imgestatus = new Image() { Source = imagenestatus, WidthRequest = 20, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start };
                            Label Labeltipo1 = new Label() { Text = agregar, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                            Label Labelmomio1 = new Label() { Text = momiogen, FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                            if (int.Parse(momiogen) > 0)
                            {
                                string txtmomio = "+" + momiogen;
                                Labelmomio1.Text = txtmomio;
                            }
                            //if (Settings.Momio != "") { Labelmomio1.Text = americano_decimal(momiogen); }
                            StackLayout stackfecha = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Horizontal, Children = { Labelfecha, Labelhora } };
                            StackLayout stacktipomomio = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Horizontal, Children = { Labeltipo1, Labelmomio1 } };
                            StackLayout stackpartido = new StackLayout() { Padding = new Thickness(10, 5, 10, 5), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Children = { Labelseleccionado, Labelpartido, stackfecha, stacktipomomio, imgestatus } };
                            
                            StackLayout stackpartido0 = new StackLayout() { Spacing = 0, Orientation = StackOrientation.Horizontal, Children = { barra, stackpartido } };
                            Frame framepartido = new Frame() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Padding = new Thickness(0), HasShadow = true, Visual = VisualMarker.Material, Content = stackpartido0 };
                            stack_partido.Children.Add(framepartido);
                        }
                    }
                    /*Image imagenbot = new Image() { Source = ImageSource.FromFile("logoazul.png"), WidthRequest = 150, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.End, Margin = new Thickness(30, 40) };
                    stack_partido.Children.Add(imagenbot);*/
                    

                }
            }
            catch (Exception ex)
            {

            }
        }

        private string americano_decimal(string momio1)
        {
            try
            {
                double momio_decimal = 0;
                int momio = int.Parse(momio1);
                if (momio > 0)
                {
                    momio_decimal = (momio / 100f) + 1f;
                }
                else
                {
                    momio_decimal = (-100f / momio) + 1f;
                }
                momio_decimal = Math.Round(momio_decimal, 2);
                return momio_decimal.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string decimal_americano(double momio1)
        {
            try
            {
                double momio_americano = 0;
                if (momio1 >= 2)
                {
                    momio_americano = (momio1 - 1f) * 100f;
                }
                else
                {
                    momio_americano = -100f / (momio1 - 1f);
                }
                momio_americano = Math.Round(momio_americano, 0);
                return momio_americano.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
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
                                 partido = WebUtility.UrlDecode((string)r.Element("partido")),
                                 usuario = WebUtility.UrlDecode((string)r.Element("usuario")),
                                 nombre = WebUtility.UrlDecode((string)r.Element("nombre")),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
                                 trn_date = WebUtility.UrlDecode((string)r.Element("trn_date")),
                                 equipo = WebUtility.UrlDecode((string)r.Element("equipo")),
                                 momio = WebUtility.UrlDecode((string)r.Element("momio")),
                                 tipo = WebUtility.UrlDecode((string)r.Element("tipo")),
                                 tipo2 = WebUtility.UrlDecode((string)r.Element("tipo2")),
                                 ganancia = WebUtility.UrlDecode((string)r.Element("ganancia")),
                                 monto = WebUtility.UrlDecode((string)r.Element("monto")),
                                 estatus = WebUtility.UrlDecode((string)r.Element("estatus")),
                                 comentario = WebUtility.UrlDecode((string)r.Element("comentario")),
                                 extra = WebUtility.UrlDecode((string)r.Element("extra"))
                             }).ToList();
                }
            }
            return items;
        }

        public List<class_partido_parlay> procesar3(string respuesta)
        {
            List<class_partido_parlay> items = new List<class_partido_parlay>();
            if (respuesta == "")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_partido_parlay
                             {
                                 id = (string)r.Element("id"),
                                 seleccionado = WebUtility.UrlDecode((string)r.Element("seleccionado")),
                                 equipo1 = WebUtility.UrlDecode((string)r.Element("HomeTeam")),
                                 equipo2 = WebUtility.UrlDecode((string)r.Element("AwayTeam")),
                                 fecha = WebUtility.UrlDecode((string)r.Element("MatchTime")),
                                 llave = WebUtility.UrlDecode((string)r.Element("tipo")),
                                 estatus = WebUtility.UrlDecode((string)r.Element("estatus")),
                                 momio = WebUtility.UrlDecode((string)r.Element("momio")),
                                 extra = WebUtility.UrlDecode((string)r.Element("extra")),
                                 total_ou = WebUtility.UrlDecode((string)r.Element("TotalNumber")),
                                 run_line_p = WebUtility.UrlDecode((string)r.Element("PointSpreadHome")),
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

