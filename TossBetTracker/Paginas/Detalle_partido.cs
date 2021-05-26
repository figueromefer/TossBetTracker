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
    public class Detalle_partido : ContentPage
    {
        StackLayout stack1 = null;
        StackLayout pestanas = null;
        Image flotante_boleto = null;
        int boletos = 0;
        StackLayout modal = null;
        StackLayout stackboleto = null;
        AbsoluteLayout absoluteLayout = null;
        List<string> boletos_array = new List<string>();
        string boletos_string = "";
        StackLayout stackframes1, stackframes2, stackframes3 = null;
        StackLayout stacketiqueta1, stacketiqueta2, stacketiqueta3 = null;
        string Name_deporte = "";
        string foto_deporte = "";
        string totalou1 = "";
        string partido = "";

        public Detalle_partido(string partido)
        {
            Name_deporte = partido;
            foto_deporte = partido + ".png";
            
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            BackgroundColor = Color.White;
            absoluteLayout = new AbsoluteLayout();
            stackframes1 = new StackLayout() { Padding = new Thickness(20, 5, 20, 5), ClassId = "STACKFRAMES", Orientation = StackOrientation.Horizontal, Spacing = 10 };
            stackframes2 = new StackLayout() { Padding = new Thickness(20, 5, 20, 5), ClassId = "STACKFRAMES", Orientation = StackOrientation.Horizontal, Spacing = 10 };
            stackframes3 = new StackLayout() { Padding = new Thickness(20, 5, 20, 5), ClassId = "STACKFRAMES", Orientation = StackOrientation.Horizontal, Spacing = 10 };
            stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 40, 20, 60), Spacing = 0 };
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack1 };
            //GENERALES

            AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(scv1);

            //MENU INFERIOR
            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            //BOLETITO FLOTANTE
            
            flotante_boleto = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("boleto_i.png"), WidthRequest = 55 };
            if (boletos_array.Count == 0)
            {
                flotante_boleto.Source = ImageSource.FromFile("boleto_i.png");
            }
            else
            {
                flotante_boleto.Source = ImageSource.FromFile("boleto_a.png");
            }
            AbsoluteLayout.SetLayoutFlags(flotante_boleto, AbsoluteLayoutFlags.PositionProportional);             AbsoluteLayout.SetLayoutBounds(flotante_boleto, new Rectangle(0.95, 0.88, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));             absoluteLayout.Children.Add(flotante_boleto);
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
                            Navigation.PushAsync(new Ticket2(boletos_array, Name_deporte));
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Ayuda", ex.Message, "OK");
                    }
                }),
                NumberOfTapsRequired = 1
            });


            stackboleto = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            ScrollView scrollboletos = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stackboleto };
            Frame contenedor = new Frame() { Visual = VisualMarker.Material, HasShadow = true, CornerRadius = 30, BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, Content = scrollboletos };
            modal = new StackLayout() { BackgroundColor = Color.FromRgba(0, 0, 0, 0.7), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(10, 40), Spacing = 0, Children = { contenedor } };
            modal.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    try
                    {
                        stackboleto.Children.Clear();
                        absoluteLayout.Children.RemoveAt(absoluteLayout.Children.Count - 1);
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Ayuda", ex.Message, "OK");
                    }
                }),
                NumberOfTapsRequired = 1
            });
            //ASIGNAR LAYOUT

            this.Content = absoluteLayout;
            cargar_intro();
        }

        public async void cargar_intro()
        {
            try
            {
                if (!Settings.Primera.Contains("|DETALLEPARTIDO|"))
                {

                    Label Nameintro = new Label() { Text = "Detalles del partido", Margin = new Thickness(0, 0, 0, 30), FontSize = 30, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Label Contenidointro = new Label() { Text = "Aquí podrás ver las diferentes líneas disponibles para el partido seleccionado, estas líneas solo estarán disponibles hasta unos minutos antes del comienzo del partido. \n\nUna vez seleccionadas las líneas ingresa al ticket dorado para completar la apuesta.", Margin = new Thickness(0, 0, 0, 0), FontSize = 22, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    StackLayout stackcontenido = new StackLayout() { Children = { Nameintro, Contenidointro }, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, };
                    StackLayout intro1 = new StackLayout() { Opacity = 0, Children = { stackcontenido }, BackgroundColor = Color.FromRgba(0, 0, 0, 0.8), VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(30, 50) };
                    intro1.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() => {
                            try
                            {
                                absoluteLayout.Children.Remove(intro1);
                                Settings.Primera = Settings.Primera + "|DETALLEPARTIDO|";
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

        protected override void OnAppearing()
        {
            boletos_string = "";
            boletos_array.Clear();
            foreach (string boleto in Settings.tickets.Split('#'))
            {
                if (boleto.Length > 1)
                {
                    boletos_array.Add(boleto);
                    boletos_string = boletos_string + "#" + boleto;
                }
                
            }
            if (Settings.Remover != "" && boletos_array.Count > 0)
            {
                Settings.Remover = "";
                boletos_string = "";
                foreach (string apuesta in Settings.Remover.Split('*'))
                {
                    if(apuesta != "")
                    {
                        boletos_array.Remove(apuesta);
                        
                        foreach(string boleto in boletos_array)
                        {
                            boletos_string = boletos_string + "#" + boleto;
                        }
                        Settings.tickets = boletos_string;
                        cargar_momios();
                    }
                }
            }
            else
            {
                
                cargar_momios();
            }
            if (boletos_array.Count == 0)
            {
                flotante_boleto.Source = ImageSource.FromFile("boleto_i.png");
            }
            else
            {
                flotante_boleto.Source = ImageSource.FromFile("boleto_a.png");
            }
            base.OnAppearing();
        }

        public void MyMethod()
        {
            //your code
            
        }

        public async void cargar_momios()
        {
            try
            {
                stack1.Children.Clear();
                stackframes1.Children.Clear();
                stackframes2.Children.Clear();
                stackframes3.Children.Clear();
                string uriString2 = "http://toss.boveda-creativa.com/partido.php?id=" + Settings.Partido;

                var response2 = await httpRequest(uriString2);
                List<class_partido> valor = new List<class_partido>();
                valor = procesar(response2);
                for (int j = 0; j < valor.Count(); j++)
                {
                    
                    Image icono_deporte = new Image() { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, WidthRequest = 55, Source = ImageSource.FromFile(foto_deporte) };
                    string partido1 = valor.ElementAt(j).HomeTeam + " vs " + valor.ElementAt(j).AwayTeam;
                    partido = partido1;
                    Label vs = new Label() { Text = partido1, FontSize = 20, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    string[] meses = { "", "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                    string fecha0 = valor.ElementAt(j).MatchTime.Split(' ')[0];
                    fecha0 = fecha0.Split('-')[2] + "/" + meses[int.Parse(fecha0.Split('-')[1])] + "/" + fecha0.Split('-')[0];
                    string hora = valor.ElementAt(j).MatchTime.Split(' ')[1];
                    hora = hora.Split(':')[0] + ":" + hora.Split(':')[1] + " hrs";
                    string fechastring = fecha0 + "\n" + hora;
                    Label fecha = new Label() { Text = fechastring, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("##979797"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Label etiqueta1 = new Label() { Text = "MONEY LINE", Margin = new Thickness(0, 10, 0, 0), FontSize = 20, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Label etiqueta2 = new Label() { Text = "SPREAD", Margin = new Thickness(0, 10, 0, 0), FontSize = 20, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Label etiqueta3 = new Label() { Text = "OVER UNDER "+valor.ElementAt(j).TotalNumber, Margin = new Thickness(0, 10, 0, 0), FontSize = 20, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    totalou1 = valor.ElementAt(j).TotalNumber;
                    StackLayout stackinfo = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Children = { vs, fecha } };
                    StackLayout cols_partido = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { icono_deporte, stackinfo } };
                    
                    
                    StackLayout stack_partido = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Start,
                        Padding = new Thickness(0),
                        ClassId = "STACKPARTIDO",
                        Children =
                        {
                            cols_partido,
                            etiqueta1,
                            stackframes1,
                            etiqueta2,
                            stackframes2,
                            etiqueta3,
                            stackframes3,
                        }
                    };
                    //MOMIOS
                    momios_moneyline(valor.ElementAt(j).MoneyLineHome, valor.ElementAt(j).MoneyLineAway, valor.ElementAt(j).EventId, valor.ElementAt(j).HomeTeam, valor.ElementAt(j).AwayTeam);
                    //MOMIOS 2
                    momios_runline(valor.ElementAt(j).PointSpreadHome, valor.ElementAt(j).PointSpreadAway, valor.ElementAt(j).EventId, valor.ElementAt(j).HomeTeam, valor.ElementAt(j).AwayTeam, valor.ElementAt(j).PointSpreadHomeLine, valor.ElementAt(j).PointSpreadAwayLine);
                    //MOMIOS 3
                    momios_ou(valor.ElementAt(j).OverLine, valor.ElementAt(j).UnderLine, valor.ElementAt(j).EventId, valor.ElementAt(j).HomeTeam, valor.ElementAt(j).AwayTeam);
                    stack1.Children.Add(stack_partido);
                    
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async void momios_moneyline( string moneyline, string moneyline2, string id, string equipo, string equipo2)
        {
            try
            {
                Color backframe1 = Color.White;
                Color textframe1 = Color.FromHex("#A7A7A7");
                Color backframe2 = Color.White;
                Color textframe2 = Color.FromHex("#A7A7A7");
                for (int i = 0; i < boletos_array.Count; i++)
                {
                    string idboleto = boletos_array[i].Split('|')[0];
                    string momio = boletos_array[i].Split('|')[2];
                    string equipo1 = boletos_array[i].Split('|')[1];
                    string tipo = boletos_array[i].Split('|')[3];
                    if (idboleto == id && momio == moneyline && equipo1 == equipo && tipo == "money_line_1")
                    {
                        backframe1 = Color.FromHex("#69D8F0");
                        textframe1 = Color.White;
                    }
                    if (idboleto == id && momio == moneyline2 && equipo1 == equipo2 && tipo == "money_line_2")
                    {
                        backframe2 = Color.FromHex("#69D8F0");
                        textframe2 = Color.White;
                    }
                }
                string textmoneyline = moneyline.Contains("-") ? moneyline : "+" + moneyline;
                Label momio1 = new Label() { Text = textmoneyline, ClassId = moneyline, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = textframe1, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                if(Settings.Momio != "") { momio1.Text = americano_decimal(moneyline); }
                StackLayout stackframe1 = new StackLayout() { Padding = new Thickness(40, 0), Spacing = 4, Children = { momio1 } };
                
                Frame fequipo1 = new Frame() { Padding = new Thickness(0, 10, 0, 10), BackgroundColor = backframe1, Visual = VisualMarker.Material, HasShadow = true, ClassId = id + "|" + equipo + "|" + moneyline + "|money_line_1||"+Name_deporte+"|"+ partido, Content = stackframe1, HorizontalOptions = LayoutOptions.CenterAndExpand};
                fequipo1.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        string identificador = fequipo1.ClassId;
                        momioclick(identificador);
                    }),
                    NumberOfTapsRequired = 1
                });
                if (textmoneyline == "0")
                {
                    fequipo1.IsEnabled = false;
                }
                string textmoneyline2 = moneyline2.Contains("-") ? moneyline2 : "+" + moneyline2;
                Label momio2 = new Label() { Text = textmoneyline2, ClassId = moneyline2, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = textframe2, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                if (Settings.Momio != "") { momio2.Text = americano_decimal(moneyline2); }
                StackLayout stackframe2 = new StackLayout() { Padding = new Thickness(40, 0), Spacing = 4, Children = { momio2 } };
                Frame fequipo2 = new Frame() { Padding = new Thickness(0, 10, 0, 10), BackgroundColor = backframe2, Visual = VisualMarker.Material, HasShadow = true, ClassId = id + "|" + equipo2 + "|" + moneyline2 + "|money_line_2||" + Name_deporte + "|"+ partido, Content = stackframe2, HorizontalOptions = LayoutOptions.CenterAndExpand};
                fequipo2.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        string identificador = fequipo2.ClassId;
                        momioclick(identificador);

                    }),
                    NumberOfTapsRequired = 1
                });
                if (textmoneyline2 == "0")
                {
                    fequipo2.IsEnabled = false;
                }
                stackframes1.Children.Add(fequipo1);
                stackframes1.Children.Add(fequipo2);
                
            }
            catch (Exception ex)
            {

            }
        }

        public async void momios_runline(string runline, string runline2, string id, string equipo, string equipo2, string runlinep, string runlinen)
        {
            try
            {
                Color backframe1 = Color.White;
                Color textframe1 = Color.FromHex("#A7A7A7");
                Color backframe2 = Color.White;
                Color textframe2 = Color.FromHex("#A7A7A7");
                for (int i = 0; i < boletos_array.Count; i++)
                {
                    string idboleto = boletos_array[i].Split('|')[0];
                    string momio = boletos_array[i].Split('|')[2];
                    string equipo1 = boletos_array[i].Split('|')[1];
                    string tipo = boletos_array[i].Split('|')[3];
                    if (idboleto == id && momio == runline && equipo1 == equipo && tipo == "run_line_1")
                    {
                        backframe1 = Color.FromHex("#69D8F0");
                        textframe1 = Color.White;
                    }
                    if (idboleto == id && momio == runline2 && equipo1 == equipo2 && tipo == "run_line_2")
                    {
                        backframe2 = Color.FromHex("#69D8F0");
                        textframe2 = Color.White;
                    }
                }
                string runlinelabel = "";
                if (!runline.Contains("-"))
                {
                    runlinelabel = "+" + runline;
                }
                else
                {
                    runlinelabel = runline;
                }
                string runlinelabel2 = "";
                if (!runline2.Contains("-"))
                {
                    runlinelabel2 = "+" + runline2;
                }
                else
                {
                    runlinelabel2 = runline2;
                }
                string textmomio1 = int.Parse(runlinep) > 0 ? "+" + runlinep : runlinep;
                string textmomio2 = int.Parse(runlinen) > 0 ? "+" + runlinen : runlinen;
                Label label1 = new Label() { Text = runlinelabel, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = textframe1, FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                Label momio1 = new Label() { Text = textmomio1, ClassId = runline, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = textframe1, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                if (Settings.Momio != "") { momio1.Text = americano_decimal(runline); }
                StackLayout stackframe1 = new StackLayout() { Padding = new Thickness(40, 0), Spacing = 4, Children = { label1, momio1 } };
                Frame fequipo1 = new Frame() { Padding = new Thickness(0, 10, 0, 10), BackgroundColor = backframe1, Visual = VisualMarker.Material, HasShadow = true, ClassId = id + "|" + equipo + "|" + runlinep + "|run_line_1|" + runline +"|"+ Name_deporte + "|" + partido, Content = stackframe1, HorizontalOptions = LayoutOptions.CenterAndExpand };
                fequipo1.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        string identificador = fequipo1.ClassId;
                        momioclick(identificador);
                    }),
                    NumberOfTapsRequired = 1
                });
                if (textmomio1 == "0")
                {
                    fequipo1.IsEnabled = false;
                }
                Label label2 = new Label() { Text = runlinelabel2, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = textframe2, FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                Label momio2 = new Label() { Text = textmomio2, ClassId = runline2 , FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = textframe2, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                if (Settings.Momio != "") { momio2.Text = americano_decimal(runline2); }
                StackLayout stackframe2 = new StackLayout() { Padding = new Thickness(40, 0), Spacing = 4, Children = { label2, momio2 } };
                Frame fequipo2 = new Frame() { Padding = new Thickness(0, 10, 0, 10), BackgroundColor = backframe2, Visual = VisualMarker.Material, HasShadow = true, ClassId = id + "|" + equipo2 + "|" + runlinen + "|run_line_2|" + runline2+"|" + Name_deporte + "|" + partido, Content = stackframe2, HorizontalOptions = LayoutOptions.CenterAndExpand };
                fequipo2.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        string identificador = fequipo2.ClassId;
                        momioclick(identificador);

                    }),
                    NumberOfTapsRequired = 1
                });
                if (textmomio2 == "0")
                {
                    fequipo2.IsEnabled = false;
                }
                stackframes2.Children.Add(fequipo1);
                stackframes2.Children.Add(fequipo2);
            }
            catch (Exception ex)
            {

            }
        }

        public async void momios_ou(string totalo, string totalu, string id, string equipo, string equipo2)
        {
            try
            {
                Color backframe1 = Color.White;
                Color textframe1 = Color.FromHex("#A7A7A7");
                Color backframe2 = Color.White;
                Color textframe2 = Color.FromHex("#A7A7A7");
                for (int i = 0; i < boletos_array.Count; i++)
                {
                    string idboleto = boletos_array[i].Split('|')[0];
                    string momio = boletos_array[i].Split('|')[2];
                    string equipo1 = boletos_array[i].Split('|')[1];
                    string tipo = boletos_array[i].Split('|')[3];
                    if (idboleto == id && momio == totalo && equipo1 == equipo && tipo == "totalo")
                    {
                        backframe1 = Color.FromHex("#69D8F0");
                        textframe1 = Color.White;
                    }
                    if (idboleto == id && momio == totalu && equipo1 == equipo2 && tipo == "totalu")
                    {
                        backframe2 = Color.FromHex("#69D8F0");
                        textframe2 = Color.White;
                    }
                }
                string textmomio1 = int.Parse(totalo) > 0 ? "+" + totalo : totalo;
                string textmomio2 = int.Parse(totalu) > 0 ? "+" + totalu : totalu;
                Label labelo = new Label() { Text = "OVER", FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = textframe1, FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                Label momio1 = new Label() { ClassId = totalo, Text = textmomio1, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = textframe1, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                if (Settings.Momio != "") { momio1.Text = americano_decimal(totalo); }
                StackLayout stackframe1 = new StackLayout() { Padding = new Thickness(40, 0), Spacing = 4, Children = { labelo,momio1 } };
                Frame fequipo1 = new Frame() { Padding = new Thickness(0, 10, 0, 10), BackgroundColor = backframe1, Visual = VisualMarker.Material, HasShadow = true, ClassId = id + "|" + equipo + "|" + totalo + "|totalo|" + totalou1 + "|" + Name_deporte + "|" + partido, Content = stackframe1, HorizontalOptions = LayoutOptions.CenterAndExpand };
                fequipo1.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        string identificador = fequipo1.ClassId;
                        momioclick(identificador);
                    }),
                    NumberOfTapsRequired = 1
                });
                if (textmomio1 == "0")
                {
                    fequipo1.IsEnabled = false;
                }
                Label labelu = new Label() { Text = "UNDER", FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = textframe2, FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                Label momio2 = new Label() { ClassId = totalu ,Text = textmomio2, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = textframe2, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                if (Settings.Momio != "") { momio2.Text = americano_decimal(totalu); }
                StackLayout stackframe2 = new StackLayout() { Padding = new Thickness(40, 0), Spacing = 4, Children = { labelu, momio2 } };
                Frame fequipo2 = new Frame() { Padding = new Thickness(0, 10, 0, 10), BackgroundColor = backframe2, Visual = VisualMarker.Material, HasShadow = true, ClassId = id + "|" + equipo2 + "|" + totalu + "|totalu|" + totalou1 + "|" + Name_deporte + "|" + partido, Content = stackframe2, HorizontalOptions = LayoutOptions.CenterAndExpand };
                fequipo2.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() => {
                        string identificador = fequipo2.ClassId;
                        momioclick(identificador);

                    }),
                    NumberOfTapsRequired = 1
                });
                if (textmomio2 == "0")
                {
                    fequipo2.IsEnabled = false;
                }
                stackframes3.Children.Add(fequipo1);
                stackframes3.Children.Add(fequipo2);
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

        public async void momioclick(string identificador)
        {
            //valor.ElementAt(j).id + "|" + valor.ElementAt(j).equipo2 + "|" + valor.ElementAt(j).money_line_2 + "|money_line_2"
            string idpartido = identificador.Split('|')[0];
            string equipo = identificador.Split('|')[1];
            string momio = identificador.Split('|')[2];
            string tipo = identificador.Split('|')[3];
            string extra1 = identificador.Split('|')[4];
            string contraparte = "";
            string elementoremover = "";
            if (tipo == "money_line_1") { contraparte = "money_line_2"; }
            if (tipo == "money_line_2") { contraparte = "money_line_1"; }
            if (tipo == "run_line_1") { contraparte = "run_line_2"; }
            if (tipo == "run_line_2") { contraparte = "run_line_1"; }
            if (tipo == "totalo") { contraparte = "totalu"; }
            if (tipo == "totalu") { contraparte = "totalo"; }
            try
            {
                foreach (var stack0 in stack1.Children)
                {
                    string nombre = ((StackLayout)stack0).ClassId;
                    if (nombre == "STACKPARTIDO")
                    {
                        foreach (var stack2 in ((StackLayout)stack0).Children)
                        {
                            string nombre2 = stack2.ClassId;
                            if (nombre2 == "STACKFRAMES")
                            {
                                foreach (var frame0 in ((StackLayout)stack2).Children)
                                {
                                    Frame frame_actual = (Frame)frame0;
                                    if (frame_actual.ClassId == identificador)
                                    {
                                        if(frame_actual.BackgroundColor == Color.FromHex("#69D8F0")) //YA ACTIVO
                                        {
                                            frame_actual.BackgroundColor = Color.White;
                                            ((Label)((StackLayout)frame_actual.Content).Children[0]).TextColor = Color.FromHex("#A7A7A7");
                                            try
                                            {
                                                ((Label)((StackLayout)frame_actual.Content).Children[1]).TextColor = Color.FromHex("#A7A7A7");
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            boletos_array.Remove(identificador);
                                            boletos_string = "";
                                            foreach (string boleto in boletos_array)
                                            {
                                                boletos_string = boletos_string + "#" + boleto;
                                            }
                                            Settings.tickets = boletos_string;
                                        }
                                        else                                                         //INACTIVO
                                        {
                                            if (boletos_string.Contains(identificador))
                                            {
                                                UserDialogs.Instance.Alert("Ticket duplicado", "Alerta", "OK");
                                            }
                                            else
                                            {
                                                frame_actual.BackgroundColor = Color.FromHex("#69D8F0");
                                                ((Label)((StackLayout)frame_actual.Content).Children[0]).TextColor = Color.White;
                                                try
                                                {
                                                    ((Label)((StackLayout)frame_actual.Content).Children[1]).TextColor = Color.White;
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                                boletos_array.Add(identificador);
                                                boletos_string = "";
                                                foreach (string boleto in boletos_array)
                                                {
                                                    boletos_string = boletos_string + "#" + boleto;
                                                }
                                                Settings.tickets = boletos_string;
                                            }
                                            
                                        }
                                    }
                                    if (frame_actual.ClassId.Split('|')[3] == contraparte)
                                    {
                                        try
                                        {
                                            foreach (string elemento in boletos_array)
                                            {
                                                if (elemento.Split('|')[3] == contraparte && elemento.Split('|')[0] == idpartido)
                                                {
                                                    elementoremover = elemento;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if(elementoremover != "")
                {
                    boletos_array.Remove(elementoremover);
                    boletos_string = "";
                    foreach (string boleto in boletos_array)
                    {
                        boletos_string = boletos_string + "#" + boleto;
                    }
                    Settings.tickets = boletos_string;
                    cargar_momios();
                }

                if(boletos_array.Count == 0)
                {
                    flotante_boleto.Source = ImageSource.FromFile("boleto_i.png");
                }
                else
                {
                    flotante_boleto.Source = ImageSource.FromFile("boleto_a.png");
                }
            }
            catch (Exception ex)
            {

            }
        } 

        public List<class_partido> procesar(string respuesta)
        {
            List<class_partido> items = new List<class_partido>();
            if (respuesta == "")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_partido
                             {
                                 id = (string)r.Element("id"),
                                 EventId = WebUtility.UrlDecode((string)r.Element("EventId")),
                                 HomeTeam = WebUtility.UrlDecode((string)r.Element("HomeTeam")),
                                 AwayTeam = WebUtility.UrlDecode((string)r.Element("AwayTeam")),
                                 MatchTime = WebUtility.UrlDecode((string)r.Element("MatchTime")),
                                 MoneyLineAway = WebUtility.UrlDecode((string)r.Element("MoneyLineAway")),
                                 MoneyLineHome = WebUtility.UrlDecode((string)r.Element("MoneyLineHome")),
                                 OverLine = WebUtility.UrlDecode((string)r.Element("OverLine")),
                                 TotalNumber = WebUtility.UrlDecode((string)r.Element("TotalNumber")),
                                 UnderLine = WebUtility.UrlDecode((string)r.Element("UnderLine")),
                                 PointSpreadAway = WebUtility.UrlDecode((string)r.Element("PointSpreadAway")),
                                 PointSpreadHome = WebUtility.UrlDecode((string)r.Element("PointSpreadHome")),
                                 PointSpreadAwayLine = WebUtility.UrlDecode((string)r.Element("PointSpreadAwayLine")),
                                 PointSpreadHomeLine = WebUtility.UrlDecode((string)r.Element("PointSpreadHomeLine")),
                                 LastUpdated = WebUtility.UrlDecode((string)r.Element("LastUpdated"))
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

