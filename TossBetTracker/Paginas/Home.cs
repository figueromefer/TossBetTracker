using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Acr.UserDialogs;
using Com.OneSignal;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.DeviceInfo;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace TossBetTracker
{
    public class Home : ContentPage
    {
        StackLayout stack1 = null;
        ScrollView Scrollview1 = null;
        StackLayout form1 = null;
        Label semanales = null;
        Label mensuales = null;
        string onesingnal1 = "";
        AbsoluteLayout absoluteLayout = new AbsoluteLayout();

        public Home()
        {
            BackgroundColor = Color.White;
            
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25,0,0,0) };
            Label Titulo0 = new Label() { Text = "Feed", WidthRequest = 100, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Image imagenchat = new Image() { Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0,0,10,0) };
            imagenchat.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Navigation.PushAsync(new Chats()); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
            StackLayout stacknav = new StackLayout() { Children = { Titulo0, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            absoluteLayout.BackgroundColor = Color.White;
            stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(0, 0, 0, 20), Spacing = 0 };
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack1 };

            //GENERALES
            AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(scv1);

            //MENU SUPERIOR
            StackLayout menusup_top = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Padding = new Thickness(0), BackgroundColor = Color.White};
            AbsoluteLayout.SetLayoutFlags(menusup_top, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(menusup_top, new Rectangle(0, 0, 1, 0.1));             absoluteLayout.Children.Add(menusup_top);

            //PESTAÑAS
            Label activa = new Label() { HeightRequest = 3, BackgroundColor = Color.FromHex("#2DC9EB"), VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand };
            Label tu = new Label() { Text = "TÚ", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Label amigos = new Label() { Text = "AMIGOS", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#D7D7D7"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Label inactiva = new Label() { HeightRequest = 3, BackgroundColor = Color.FromHex("#F7F4F4"), VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand };
            StackLayout izq = new StackLayout() { Spacing = 0, WidthRequest = double.Parse(Settings.Ancho) / 2, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.EndAndExpand };
            izq.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Application.Current.MainPage = new NavigationPage(new RootPage()); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda4", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
            izq.Children.Add(tu);
            izq.Children.Add(activa);             StackLayout der = new StackLayout() { Spacing = 0, WidthRequest = double.Parse(Settings.Ancho) / 2, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.EndAndExpand };
            der.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Application.Current.MainPage = new NavigationPage(new RootPage2()); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
            der.Children.Add(amigos);
            der.Children.Add(inactiva);
            StackLayout pestanas = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.FromHex("#FFFFFF"), Padding = new Thickness(0,15,0,0 ) };             pestanas.Children.Add(izq);             pestanas.Children.Add(der);              menusup_top.Children.Add(pestanas);

            try
            {
                if (!Settings.Foto.Contains("http"))
                {
                    Settings.Foto = "http://toss.boveda-creativa.com/upload/" + Settings.Foto;
                }
            }
            catch (Exception ex)
            {

            }


            //MENU INFERIOR
            absoluteLayout.Children.Add(cargar_menu_inferior());

            //ASIGNAR LAYOUT
            this.Content = absoluteLayout;

            ConsultarP();
            act_onesingal1();
            OneSignal.Current.IdsAvailable(IdsAvailable);
            
        }

        private async void IdsAvailable(string userID, string pushToken)
        {

            try
            {
                if (Settings.OneSignal == "" || onesingnal1 != userID)
                {
                    string uriString4 = string.Format("http://toss.boveda-creativa.com/update_notif.php?id={0}&onesignal={1}", Settings.Idusuario, userID);
                    var response4 = await httpRequest(uriString4);
                    Settings.OneSignal = userID;
                }
            }
            catch (Exception ex)
            {

            }

        }

        async void act_onesingal1()
        {
            try
            {
                string uriString2 = string.Format("http://toss.boveda-creativa.com/onesignal.php?usuario={0}", Settings.Idusuario);
                var response2 = await httpRequest(uriString2);
                onesingnal1 = response2.ToString();
            }
            catch (Exception ex)
            {

            }
        }

        public static AbsoluteLayout cargar_menu_inferior()
        {
            try
            {
                
                AbsoluteLayout absoluteLayout1 = new AbsoluteLayout();
                AbsoluteLayout.SetLayoutFlags(absoluteLayout1, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(absoluteLayout1, new Rectangle(0, 1, 1, 0.1));
                StackLayout menu = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, Padding = new Thickness(10, 5, 10, 5), BackgroundColor = Color.FromHex("#ffffff"), Orientation = StackOrientation.Horizontal };
                if (CrossDeviceInfo.Current.Model == "iPhone")
                {
                    menu.Padding = new Thickness(10, 5, 10, 25);
                }
                AbsoluteLayout.SetLayoutFlags(menu, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(menu, new Rectangle(1, 1, 1, 1));
                absoluteLayout1.Children.Add(menu);


               
                var icono_home = new Image() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("icono_home2.png"), HeightRequest = 26 };
                Label lblhome = new Label() { Text = "Feed", FontSize = 8, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#C9C9C9"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                StackLayout stackhome = new StackLayout() { Children = { icono_home,lblhome },Spacing = 2, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
                menu.Children.Add(stackhome);
                stackhome.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command( async () => { try {
                        await stackhome.ScaleTo(1.1, 100);
                        await stackhome.ScaleTo(1, 100);
                        Application.Current.MainPage = new NavigationPage(new RootPage()); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });

                var icono_amigos = new Image() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("icono_grupos.png"), HeightRequest = 24 };
                Label lblgrupos = new Label() { Text = "Grupos", FontSize = 8, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#C9C9C9"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                StackLayout stackgrupos = new StackLayout() { Children = { icono_amigos, lblgrupos }, Spacing = 2, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
                menu.Children.Add(stackgrupos);
                stackgrupos.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => {
                    try
                    {
                        await stackgrupos.ScaleTo(1.1, 100);
                        await stackgrupos.ScaleTo(1, 100);
                        await Application.Current.MainPage.Navigation.PushAsync(new Grupos()); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });

                var icono_deportes = new Image() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("icono_deportes.png"), HeightRequest = 55, Opacity = 0 };
                menu.Children.Add(icono_deportes);
                var icono_deportes2 = new Image() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("icono_deportes.png"), HeightRequest = 55, WidthRequest = 55};
                icono_deportes2.GestureRecognizers.Add(new TapGestureRecognizer {
                    Command = new Command(async () => {
                        try
                        {
                            await icono_deportes2.ScaleTo(1.1, 100);
                            await icono_deportes2.ScaleTo(1, 100);
                            await Application.Current.MainPage.Navigation.PushAsync(new Deportes()); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                AbsoluteLayout.SetLayoutFlags(icono_deportes2, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(icono_deportes2, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
                absoluteLayout1.Children.Add(icono_deportes2);


                var icono_estadisticas = new Image() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("icono_estadisticas.png"), HeightRequest = 24 };
                Label lblesta = new Label() { Text = "Estadísticas", FontSize = 8, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#C9C9C9"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                StackLayout stackesta = new StackLayout() { Children = { icono_estadisticas, lblesta }, Spacing = 2, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
                menu.Children.Add(stackesta);
                stackesta.GestureRecognizers.Add(new TapGestureRecognizer {
                    Command = new Command(async () => {
                        try
                        {
                            await stackesta.ScaleTo(1.1, 100);
                            await stackesta.ScaleTo(1, 100);
                            await Application.Current.MainPage.Navigation.PushAsync(new Estadisticas()); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });

                var icono_spb = new Image() { HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("icono_spb.png"), HeightRequest = 24 };
                Label lblspb = new Label() { Text = "Perfil", FontSize = 8, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#C9C9C9"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                StackLayout stackspb = new StackLayout() { Children = { icono_spb, lblspb }, Spacing = 2, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center };
                menu.Children.Add(stackspb);
                stackspb.GestureRecognizers.Add(new TapGestureRecognizer {
                    Command = new Command(async () => {
                        try
                        {
                            await stackspb.ScaleTo(1.1, 100);
                            await stackspb.ScaleTo(1, 100);
                            await Application.Current.MainPage.Navigation.PushAsync(new Perfil2()); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });

                return absoluteLayout1;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async void ConsultarP()
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    UserDialogs.Instance.ShowLoading("Cargando tus apuestas …");
                }
                else
                {
                    // all others
                }
               
                string uriString2 = "http://toss.boveda-creativa.com/misapuestas.php?usuario=" + Settings.Idusuario;
                var response2 = await httpRequest(uriString2);
                List<class_apuestas> valor = new List<class_apuestas>();
                valor = procesar(response2);
                StackLayout stack_partido = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Start,
                    Padding = new Thickness(20,80,20,30),
                    ClassId = "STACKPARTIDO"
                };
                int contador = 0;
                for (int j = 0; j < valor.Count(); j++)
                {
                    string tipo = "";
                    if(valor.ElementAt(j).tipo == "run_line_1") { tipo = "Spread"; }
                    else if (valor.ElementAt(j).tipo == "run_line_2") { tipo = "Spread"; }
                    else if (valor.ElementAt(j).tipo == "money_line_1") { tipo = "Money\nLine"; }
                    else if (valor.ElementAt(j).tipo == "money_line_2") { tipo = "Money\nLine"; }
                    else if (valor.ElementAt(j).tipo == "total_o") { tipo = "Over"; }
                    else if (valor.ElementAt(j).tipo == "total_u") { tipo = "Under"; }
                    else if (valor.ElementAt(j).tipo == "Parlay") { tipo = "Parlay"; }
                    else if (tipo == "") { tipo = valor.ElementAt(j).tipo; }
                    string stringtitulo = "";
                    if(valor.ElementAt(j).equipo1 == "Apuesta Parlay")
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
                    Image icono_deporte = new Image() { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile(foto+".png"), WidthRequest = 25 };
                    StackLayout titulo_logo = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4, Children = { icono_deporte, Titulo } };
                    stack_partido.Children.Add(titulo_logo);
                    Label Tipo = new Label() { Text = tipo + " " +  valor.ElementAt(j).valor1, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#E0B928"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    //StackLayout stackcol1 = new StackLayout() { Spacing = 2, Padding = new Thickness(5, 0, 5, 0), Children = {  Tipo } };
                    string eq = valor.ElementAt(j).equipo;
                    if (valor.ElementAt(j).equipo1 == "Apuesta Parlay") { eq = "Múltiples equipos"; }
                    Label equipo = new Label() { Text = eq, FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Label Monto = new Label() { Text = "$" + valor.ElementAt(j).monto, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Label nriesgo = new Label() { Text = valor.ElementAt(j).riesgo, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Image imagetoss = new Image() { Source = ImageSource.FromFile("moneda.png"), WidthRequest = 20, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start };
                    StackLayout stacktoss = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { imagetoss, nriesgo }, HorizontalOptions = LayoutOptions.StartAndExpand };
                    StackLayout stackcol2 = new StackLayout() { Padding = new Thickness(0, 15, 0, 0), HorizontalOptions = LayoutOptions.FillAndExpand ,Children = { equipo, Tipo, Monto, stacktoss } };
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
                    Image imgestatus = new Image(){ Source = imagenestatus, WidthRequest = 20, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start};
                    Label ganancia = new Label() { Text = "Ganancia: $" + valor.ElementAt(j).ganancia, FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    string momio0 = valor.ElementAt(j).momio;
                    if(int.Parse(valor.ElementAt(j).momio) > 0) { momio0 = "+" + momio0; }

                    Label vacio = new Label() { Text = momio0, FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Label nriesgo_ganados = new Label() { Text = valor.ElementAt(j).riesgo2, FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Image imagetoss_ganados = new Image() { Source = ImageSource.FromFile("moneda.png"), WidthRequest = 20, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start };
                    StackLayout stacktoss_ganados = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { imagetoss_ganados, nriesgo_ganados }, HorizontalOptions = LayoutOptions.EndAndExpand };
                    StackLayout stackcol3 = new StackLayout() { Padding = new Thickness(0, 15, 0, 0), Children = { imgestatus, vacio,ganancia, stacktoss_ganados }, WidthRequest = 150, HorizontalOptions = LayoutOptions.EndAndExpand };

                    StackLayout stackapuesta = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4, Children = {stackcol2, stackcol3 } };
                    Frame frameapuesta = new Frame() { Visual = VisualMarker.Material, Margin = new Thickness(0,0,0,20), HasShadow = true, IsClippedToBounds = true, Padding = new Thickness(15, 5), Content = stackapuesta, ClassId = valor.ElementAt(j).id };
                    frameapuesta.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => {
                        try
                        {
                            await frameapuesta.ScaleTo(1.1, 100);
                            await frameapuesta.ScaleTo(1, 100);
                            await Navigation.PushAsync(new Verapuesta(frameapuesta.ClassId));
                        }
                        catch (Exception ex) {
                            await Application.Current.MainPage.DisplayAlert("Ayuda1", ex.Message, "OK");
                        }
                    }), NumberOfTapsRequired = 1 });
                    stack_partido.Children.Add(frameapuesta);
                    contador++;
                }
                stack1.Children.Add(stack_partido);
                if(!Settings.Primera.Contains("|HOME|"))
                {
                    Settings.Primera = Settings.Primera + "|HOME|";
                    Image intro1 = new Image() { BackgroundColor= Color.FromRgba(0,0,0,0.8), Source = ImageSource.FromFile("intro1.png"), Aspect = Aspect.AspectFill, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
                    intro1.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { absoluteLayout.Children.Remove(intro1); } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                    AbsoluteLayout.SetLayoutFlags(intro1, AbsoluteLayoutFlags.All);
                    AbsoluteLayout.SetLayoutBounds(intro1, new Rectangle(0, 0, 1, 1));
                    absoluteLayout.Children.Add(intro1);
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public List<class_apuestas> procesar(string respuesta)
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
                                 riesgo2 = WebUtility.UrlDecode((string)r.Element("riesgo2")),
                                 riesgoganado = WebUtility.UrlDecode((string)r.Element("riesgoganado"))
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

