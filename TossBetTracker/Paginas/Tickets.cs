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
    public class Tickets : ContentPage
    {
        StackLayout stack1, apuestasstack = null;
        StackLayout pestanas = null;
        Image flotante_boleto = null;
        int boletos = 0; 
        StackLayout modal = null;
        StackLayout stackboleto = null;
        AbsoluteLayout absoluteLayout = null;
        List<string> boletos_array2 = new List<string>();
        List<string> boletos_array3 = new List<string>();
        string titulo_deporte = "";
        int parlay = 0;
        string equipos = "";
        string momios_decimales = "";
        StackLayout fondo = null;
        string parlay_equipos = "";
        string parlay_tipos = "";
        string parlay_partidos = "";


        public Tickets(List<string> boletos_array, string deporte)
        {
            titulo_deporte = deporte;
            boletos_array2 = boletos_array;

            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);

            BackgroundColor = Color.White;
            absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 40, 20, 20), Spacing = 0 };
            apuestasstack = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(0, 20, 0, 20), Spacing = 0 };
            
            Label tituloparlay = new Label() { Text = "Parlay:", FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Switch switchparaly = new Switch() { HorizontalOptions = LayoutOptions.End, OnColor = Color.FromHex("#01528a") };
            switchparaly.PropertyChanged += Switchparaly_PropertyChanged;
            StackLayout stackparley = new StackLayout() { HorizontalOptions = LayoutOptions.EndAndExpand, Orientation = StackOrientation.Horizontal, Children = { tituloparlay, switchparaly } };
            stack1.Children.Add(stackparley);
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = apuestasstack };
            stack1.Children.Add(scv1);
            //GENERALES
            
            


            AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(stack1);
            fondo = new StackLayout()
            {
                BackgroundColor = Color.FromRgba(255, 255, 255, 0.7),

            };
            //MENU INFERIOR

            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            
            //ASIGNAR LAYOUT

            this.Content = absoluteLayout;
            actualizar_boletos();
            
        }

        public async void actualizar_boletos()
        {
            try
            {
                int contador = 0;
                boletos_array3 = boletos_array2;
                foreach (string boleto in boletos_array2.ToList())
                {
                    string idpartido = boleto.Split('|')[0];
                    string uriString2 = string.Format("http://toss.boveda-creativa.com/validar_partido.php?id={0}", idpartido);
                    var response2 = await httpRequest(uriString2);
                    if (response2.ToString() == "0") 
                    {
                        boletos_array3.RemoveAt(contador);
                    }
                    contador++;
                }
                boletos_array2 = boletos_array3;
            }
            catch (Exception ex)
            {

            }
            UserDialogs.Instance.ShowLoading("Validando ticket …");
            
            UserDialogs.Instance.HideLoading();
            cargar_momios();
        }

        public async void cargar_momios()
        {
            try
            {
                apuestasstack.Children.Clear();
                int contador = 0;
                double momio_parlay = 0;
               
                foreach (string boleto in boletos_array2)
                {
                    string idpartido = boleto.Split('|')[0];
                    string equipo = boleto.Split('|')[1];
                    string momio = boleto.Split('|')[2];
                    string tipo = boleto.Split('|')[3];
                    string extra = boleto.Split('|')[4];


                    if (parlay == 0)
                    {
                        String tipo1 = "";
                        if (tipo == "money_line_1") { tipo1 = "Money line"; }
                        if (tipo == "money_line_2") { tipo1 = "Money line"; }
                        if (tipo == "run_line_1") { tipo1 = "Spread"; }
                        if (tipo == "run_line_2") { tipo1 = "Spread"; }
                        if (tipo == "totalo") { tipo1 = "Over"; }
                        if (tipo == "totalu") { tipo1 = "Under"; }
                        if(tipo == "run_line_2")
                        {
                            extra = extra.Replace("+", "-");
                        }
                        Label eliminar = new Label() {ClassId = boleto, Text = "Eliminar", FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.Red, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        eliminar.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try {
                                boletos_array2.Remove(eliminar.ClassId);
                                Settings.tickets = Settings.tickets.Replace(eliminar.ClassId, "");
                                cargar_momios();
                            } catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });

                        Label Titulo = new Label() { Text = equipo, FontSize = 18, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        Label Tipo = new Label() { Margin = new Thickness(10, 5, 0, 0), Text = tipo1 + " "+ extra, FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#2DC9EB"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                        StackLayout titulo_tipo = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4, Children = { Titulo, Tipo } };

                        Entry Cantidad = new Entry() { Keyboard = Keyboard.Numeric, ClassId = contador.ToString(), FontSize = 22, FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null), TextColor = Color.FromHex("#01528a"), PlaceholderColor = Color.FromHex("#01528a"), Placeholder = "$ 0.00", HorizontalOptions = LayoutOptions.StartAndExpand };
                        Cantidad.TextChanged += Entry1_TextChanged;
                        
                        
                        Label Lblriesgo = new Label() { Text = "Toss coins", FontSize = 10, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#2DC9EB"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                        Picker riesgo = new Picker() { HorizontalOptions = LayoutOptions.CenterAndExpand, WidthRequest = 70, FontSize = 22, ClassId = contador.ToString(), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        riesgo.Items.Add("1");
                        riesgo.Items.Add("2");
                        riesgo.Items.Add("3");
                        riesgo.Items.Add("4");
                        riesgo.Items.Add("5");
                        riesgo.SelectedIndex = 0;
                        riesgo.SelectedIndexChanged += Riesgo_SelectedIndexChanged;
                        StackLayout stackriesgo = new StackLayout() { HorizontalOptions = LayoutOptions.CenterAndExpand, Spacing = 1, Children = { Lblriesgo, riesgo } };
                        Label Momio = new Label() { ClassId = momio, Text = momio, FontSize = 22, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        if (Settings.Momio != "") { Momio.Text = americano_decimal(momio); }
                        StackLayout cantidad_momio = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4, Children = { Cantidad, stackriesgo, Momio } };

                        Label Titulo_ganancia = new Label() { Text = "Ganancia:", FontSize = 12, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                        Label Ganancia = new Label() { Text = "$ 0.00", Margin = new Thickness(0), FontSize = 20, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        StackLayout stack_ganancia = new StackLayout() { Spacing = 4, Children = { Titulo_ganancia, Ganancia }, BackgroundColor = Color.FromHex("#ffffff"), HorizontalOptions = LayoutOptions.FillAndExpand };
                        Frame frameganancia = new Frame() { Padding = new Thickness(10, 5, 10, 5), Content = stack_ganancia, BorderColor = Color.FromHex("#D8D8D8"), HasShadow = false, };

                        Button aplicar = new Button() { ClassId = idpartido + "|" + equipo + "|" + momio + "|" + tipo1 + "|||"+tipo+"|1|", Text = "Aplicar", MinimumHeightRequest = 30, WidthRequest = 200, HorizontalOptions = LayoutOptions.Center, BorderRadius = 16, BackgroundColor = Color.FromHex("#2DC9EB"), TextColor = Color.FromHex("#ffffff"), FontSize = 20, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        aplicar.Clicked += Aplicar_resumen;

                        StackLayout stack_boleto = new StackLayout()
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,
                            Padding = new Thickness(20, 20),
                            ClassId = contador.ToString(),
                            Children = {
                                eliminar,
                                titulo_tipo,
                                cantidad_momio,
                                frameganancia,
                                aplicar
                            }
                        };
                        apuestasstack.Children.Add(stack_boleto);
                        
                        contador++;
                    }
                    else
                    {
                        string momio_decimal = americano_decimal(momio);
                        if (momio_parlay == 0)
                        {
                            momio_parlay = double.Parse(momio_decimal);
                            parlay_partidos = idpartido;
                            parlay_equipos = equipo;
                            parlay_tipos = tipo;
                        }
                        else
                        {
                            momio_parlay = momio_parlay * double.Parse(momio_decimal);
                            parlay_partidos = parlay_partidos + "|" + idpartido;
                            parlay_equipos = parlay_equipos + "|" + equipo;
                            parlay_tipos = parlay_tipos + "|" + tipo;
                        }
                        if(equipos == "")
                        {
                            equipos = equipo;
                        }
                        else
                        {
                            equipos = equipos + ", " + equipo;
                        }
                        Label eliminar = new Label() { ClassId = boleto, Text = "Eliminar", FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.Red, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        eliminar.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(() => {
                                try
                                {
                                    boletos_array2.Remove(eliminar.ClassId);
                                    Settings.tickets = Settings.tickets.Replace(eliminar.ClassId, "");
                                    cargar_momios();
                                }
                                catch (Exception ex) { Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK"); }
                            }),
                            NumberOfTapsRequired = 1
                        });
                        String tipo1 = "";
                        if (tipo == "money_line_1") { tipo1 = "Money line"; }
                        if (tipo == "money_line_2") { tipo1 = "Money line"; }
                        if (tipo == "run_line_1") { tipo1 = "Spread"; }
                        if (tipo == "run_line_2") { tipo1 = "Spread"; }
                        if (tipo == "totalo") { tipo1 = "Over"; }
                        if (tipo == "totalu") { tipo1 = "Under"; }
                        Label Titulo = new Label() { Text = equipo, FontSize = 18, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        Label Tipo = new Label() { Margin = new Thickness(10, 5, 0, 0), Text = tipo1, FontSize = 14, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#2DC9EB"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                        StackLayout titulo_tipo = new StackLayout() { Spacing = 4, Children = { Titulo, Tipo } };

                        
                        Label Momio = new Label() { ClassId = momio, Text = momio, WidthRequest = 80, FontSize = 22, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        if (Settings.Momio != "") { Momio.Text = americano_decimal(momio); }
                        StackLayout cantidad_momio = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4, Children = { titulo_tipo, Momio } };

                        StackLayout stack_boleto = new StackLayout()
                        {
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,
                            Padding = new Thickness(20, 20),
                            ClassId = contador.ToString(),
                            Children = {
                                eliminar,
                                cantidad_momio,
                            }
                        };
                        apuestasstack.Children.Add(stack_boleto);
                    }
                }
                if(parlay != 0)
                {
                    string momio = "0";
                    if (momio_parlay != 0)
                    {
                        momio = decimal_americano(momio_parlay);
                    }
                    
                    Label Momio = new Label() { ClassId= momio, Text = momio, FontSize = 22, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    if (Settings.Momio != "") { Momio.Text = americano_decimal(momio); }
                    Entry Cantidad = new Entry() { Keyboard = Keyboard.Numeric, ClassId = contador.ToString(), FontSize = 22, FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null), TextColor = Color.FromHex("#01528a"), PlaceholderColor = Color.FromHex("#01528a"), Placeholder = "$ 0.00", HorizontalOptions = LayoutOptions.StartAndExpand };
                    Cantidad.TextChanged += Entry1_TextChanged;
                    Label Lblriesgo = new Label() { Text = "Toss coins", FontSize = 10, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#2DC9EB"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Picker riesgo = new Picker() { HorizontalOptions = LayoutOptions.CenterAndExpand, WidthRequest = 70, FontSize = 22, ClassId = contador.ToString(), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    riesgo.Items.Add("1");
                    riesgo.Items.Add("2");
                    riesgo.Items.Add("3");
                    riesgo.Items.Add("4");
                    riesgo.Items.Add("5");
                    riesgo.SelectedIndex = 0;
                    riesgo.SelectedIndexChanged += Riesgo_SelectedIndexChanged;
                    StackLayout stackriesgo = new StackLayout() { HorizontalOptions = LayoutOptions.CenterAndExpand, Spacing = 1, Children = { Lblriesgo, riesgo } };
                    StackLayout cantidad_momio = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4, Children = { Cantidad, stackriesgo, Momio } };
                    Label Titulo_ganancia = new Label() { Text = "Ganancia:", FontSize = 12, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Label Ganancia = new Label() { Text = "$ 0.00", Margin = new Thickness(0), FontSize = 20, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    StackLayout stack_ganancia = new StackLayout() { Spacing = 4, Children = { Titulo_ganancia, Ganancia }, BackgroundColor = Color.FromHex("#ffffff"), HorizontalOptions = LayoutOptions.FillAndExpand };
                    Frame frameganancia = new Frame() { Padding = new Thickness(10, 5, 10, 5), Content = stack_ganancia, BorderColor = Color.FromHex("#D8D8D8"), HasShadow = false, };
                    

                    Button aplicar = new Button() { ClassId = "0|"+ equipos + "|" + momio + "|Parlay||||1|", Text = "Aplicar", MinimumHeightRequest = 30, WidthRequest = 200, HorizontalOptions = LayoutOptions.Center, BorderRadius = 16, BackgroundColor = Color.FromHex("#2DC9EB"), TextColor = Color.FromHex("#ffffff"), FontSize = 20, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    aplicar.Clicked += Aplicar_resumen;

                    StackLayout stack_boleto = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Start,
                        Padding = new Thickness(20, 20),
                        ClassId = "Parlay",
                        Children = {
                            cantidad_momio,
                            frameganancia,
                            aplicar
                        }
                    };
                    apuestasstack.Children.Add(stack_boleto);
                    
                }
            }
            catch (Exception ex)
            {
                Settings.tickets = "";
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

        private string decimal_americano (double momio1)
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
                    momio_americano = -100f/(momio1 -1f);
                }
                momio_americano = Math.Round(momio_americano, 0);
                return momio_americano.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private void Switchparaly_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Switch validador = (Switch)sender;
            if(validador.IsToggled == true)
            {
                parlay = 1;
            }
            else
            {
                parlay = 0;
            }
            parlay_equipos = "";
            parlay_partidos = "";
            parlay_tipos = "";
            cargar_momios();
        }

        private async void Aplicar_resumen(object sender, EventArgs e)
        {
            try
            {
                Button boton = (Button)sender;
                StackLayout padre = (StackLayout)boton.Parent;
                Label eliminar = (Label)padre.Children[0];
                boletos_array2.Remove(eliminar.ClassId);
                Settings.tickets = Settings.tickets.Replace(eliminar.ClassId, "");
            }
            catch (Exception ex)
            {

            }
            try
            {
                string boleto = ((Button)sender).ClassId;
                string idpartido = boleto.Split('|')[0];
                string equipo = boleto.Split('|')[1];
                string momio = boleto.Split('|')[2];
                string tipo = boleto.Split('|')[3];
                string tipo1 = boleto.Split('|')[6];
                string ganancia = boleto.Split('|')[4];
                string monto = boleto.Split('|')[5];
                string riesgo = boleto.Split('|')[7];
                string riesgoganado = boleto.Split('|')[8];
                if (double.Parse(monto) > 0)
                {
                    resumen(idpartido, equipo, momio, tipo, ganancia, monto, tipo1, riesgo, riesgoganado);
                }
                else
                {
                    UserDialogs.Instance.Alert("Monto debe ser mayor a 0", "Monto incompleto", "OK");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Monto debe ser mayor a 0", "Monto incompleto", "OK");
            }
            
        }

        public void resumen(string idpartido, string equipo, string momio, string tipo, string ganancia, string monto, string tipo1, string riesgo, string riesgoganado)
        {
            try
            {
                AbsoluteLayout.SetLayoutFlags(fondo, AbsoluteLayoutFlags.All);
                AbsoluteLayout.SetLayoutBounds(fondo, new Rectangle(0, 0, 1, 1));
                absoluteLayout.Children.Add(fondo);


                //Resumen
                Label Tituloresumen = new Label() { Text = "Resumen", Margin = new Thickness(0, 0, 0, 10), FontSize = 20, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                Label Labelequipo = new Label() { Text = "Equipo: " + equipo, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                Label Labelmomio = new Label() { Text = "Momio: " + momio, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                if (Settings.Momio != "") { Labelmomio.Text = "Momio: " + americano_decimal(momio); }
                Label Labeltipo = new Label() { Text = "Tipo: " + tipo, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                Label Labelganancia = new Label() { Text = "Ganancia: " + ganancia, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                Label Labelmonto = new Label() { Text = "Monto: " + monto, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                Label Labelriesgo = new Label() { Text = "Toss coins: " + riesgo, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                Label Labelriesgoganado = new Label() { Text = "Toss coins: " + riesgoganado, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                string classid = "";
                if (parlay == 1)
                {
                    classid = "0|"+ equipos + "|" + momio + "|Parlay|" + ganancia + "|" + monto+"||"+riesgo+"|"+riesgoganado;
                }
                else
                {
                    classid = idpartido + "|" + equipo + "|" + momio + "|" + tipo + "|" + ganancia + "|" + monto+"|"+tipo1+"|"+riesgo+" | "+riesgoganado;
                }
                Button continuar = new Button() { ClassId = classid, Text = "Continuar", MinimumHeightRequest = 30, WidthRequest = 200, HorizontalOptions = LayoutOptions.Center, BorderRadius = 16, BackgroundColor = Color.FromHex("#2DC9EB"), TextColor = Color.FromHex("#ffffff"), FontSize = 20, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                continuar.Clicked += Aplicar_clicked;
                StackLayout stack_resumen = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(10, 20),
                    Children = {
                        Tituloresumen,
                        Labelequipo,
                        Labelmomio,
                        Labeltipo,
                        Labelganancia,
                        Labelmonto,
                        Labelriesgo,
                        Labelriesgoganado,
                        continuar
                    }
                };
                Frame frame_resumen = new Frame()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = Color.White,
                    CornerRadius = 20,
                    Margin = new Thickness(0, 90, 0, 30),
                    Content = stack_resumen
                };
                fondo.Children.Add(frame_resumen);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

        }

        private async void Aplicar_clicked(object sender, EventArgs e)
        {
            try
            {

                UserDialogs.Instance.ShowLoading("Aplicando apuesta …");
                absoluteLayout.Children.Remove(fondo);
                string boleto = ((Button)sender).ClassId;
                string idpartido = boleto.Split('|')[0];
                string equipo = boleto.Split('|')[1];
                string momio = boleto.Split('|')[2];
                string tipo = boleto.Split('|')[3];
                string ganancia = boleto.Split('|')[4];
                string monto = boleto.Split('|')[5];
                string tipo1 = boleto.Split('|')[6];
                string riesgo = boleto.Split('|')[7];
                string riesgoganado = boleto.Split('|')[8];
                if (parlay == 1) { titulo_deporte = "parlay"; }

                

                string uriString2 = string.Format("http://toss.boveda-creativa.com/insertar_apuesta.php?usuario={0}&partido={1}&equipo={2}&momio={3}&tipo={4}&ganancia={5}&monto={6}&deporte={7}&tipo1={8}&riesgo={9}&riesgoganado={10}", Settings.Idusuario,idpartido,equipo, momio, tipo, ganancia, monto, titulo_deporte, tipo1, riesgo, riesgoganado);
                var response2 = await httpRequest(uriString2);
                if (response2.ToString() != "0")
                {
                    List<string> listapartidos = parlay_partidos.Split('|').ToList();
                    for (int contadorpartidos = 0; contadorpartidos < listapartidos.Count; contadorpartidos++)
                    {
                        try
                        {
                            string partido0 = parlay_partidos.Split('|')[contadorpartidos];
                            string equipo0 = parlay_equipos.Split('|')[contadorpartidos];
                            string tipos0 = parlay_tipos.Split('|')[contadorpartidos];
                            if (equipo0.Length > 1)
                            {
                                string uriString3 = string.Format("http://toss.boveda-creativa.com/insertar_parlay.php?apuesta={0}&equipo={1}&partido={2}&tipo={3}", response2.ToString(), equipo0, partido0, tipos0);
                                var response3 = await httpRequest(uriString3);
                            }
                        }
                        catch (Exception ex)
                        {
                            string error = ex.Message;
                        }
                        Settings.tickets = "";
                    }
                    
                    UserDialogs.Instance.HideLoading();
                    PromptResult pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                    {
                        OkText = "OK",
                        IsCancellable = false,
                        Title = "Apuesta aplicada",
                        Placeholder = "Comentarios..."
                    });
                    if(pResult.Text != "")
                    {
                        string uriString3 = string.Format("http://toss.boveda-creativa.com/insertar_comentario.php?apuesta={0}&comentario={1}", response2.ToString(), pResult.Text);
                        var response3 = await httpRequest(uriString3);
                    }
                   
                    
                    await Navigation.PushAsync(new Compartir( response2.ToString()));
                    
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    PromptResult pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                    {
                        OkText = "OK",
                        Message = "Revisa tu conexión a internet o intentalo de nuevo más tarde",
                        IsCancellable = false,
                        Title = "ERROR",
                    });
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void Riesgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Picker riesgo = (Picker)sender;
                string riesgoval = riesgo.SelectedItem.ToString();
                StackLayout stackcantidadmomio = (StackLayout)riesgo.Parent.Parent;
                StackLayout stackboleto = (StackLayout)stackcantidadmomio.Parent;
                Label momio = (Label)stackcantidadmomio.Children[2];
                if (parlay == 0)
                {
                    Button boton = (Button)stackboleto.Children[4];
                    int momio_val = int.Parse(momio.Text);
                    double ganado = Math.Round(calcular_ganado(momio_val, double.Parse(riesgoval)),2);
                    ganado = double.Parse(riesgoval) + ganado;
                    boton.ClassId = boton.ClassId.Split('|')[0] + "|"
                        + boton.ClassId.Split('|')[1] + "|"
                        + boton.ClassId.Split('|')[2] + "|"
                        + boton.ClassId.Split('|')[3] + "|"
                        + boton.ClassId.Split('|')[4] + "|"
                        + boton.ClassId.Split('|')[5] + "|"
                        + boton.ClassId.Split('|')[6] + "|"
                        + riesgoval + "|"
                        + ganado + "|";
                }
                else
                {
                    Button boton = (Button)stackboleto.Children[2];
                    int momio_val = int.Parse(momio.Text);
                    double ganado = Math.Round(calcular_ganado(momio_val, double.Parse(riesgoval)),2);
                    ganado = double.Parse(riesgoval) + ganado;
                    boton.ClassId = boton.ClassId.Split('|')[0] + "|"
                        + boton.ClassId.Split('|')[1] + "|"
                        + boton.ClassId.Split('|')[2] + "|"
                        + boton.ClassId.Split('|')[3] + "|"
                        + boton.ClassId.Split('|')[4] + "|"
                        + boton.ClassId.Split('|')[5] + "|"
                        + boton.ClassId.Split('|')[6] + "|"
                        + riesgoval + "|"
                        + ganado + "|";
                }
            }
            catch (Exception ex)
            {

            }
        }

        public double calcular_ganado(int momio, double apostado)
        {
            double ganado = 0;
            if (momio < 0)
            {
                ganado = apostado * 100 / (momio * -1);
            }
            else
            {
                ganado = apostado * momio / 100;
            }
            return ganado;
        }

        private void Entry1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string boleto = ((Entry)sender).ClassId;
            Entry entry = (Entry)sender;
            try
            {
               
                if (parlay == 0)
                {
                            
                    StackLayout stackmomiocantidad = (StackLayout)entry.Parent;
                    StackLayout stackboleto = (StackLayout)entry.Parent.Parent;
                    Frame frameganancia = (Frame)stackboleto.Children[3];
                    StackLayout stackganancia = (StackLayout) frameganancia.Content;
                    Button boton = (Button)stackboleto.Children[4];
                    Label momio = (Label)stackmomiocantidad.Children[2];
                    int momio_val = int.Parse(momio.ClassId);
                    double ganado = calcular_ganado(momio_val, double.Parse(e.NewTextValue));
                    Label ganancia = (Label)stackganancia.Children[1];
                    ganancia.Text = "$ " + Math.Round((double.Parse(e.NewTextValue) + ganado), 2);
                    string ganancia_actual = "";
                    try
                    {
                        ganancia_actual = boton.ClassId.Split('|')[4];
                        boton.ClassId = boton.ClassId.Split('|')[0] + "|" + boton.ClassId.Split('|')[1] + "|" + boton.ClassId.Split('|')[2] + "|" + boton.ClassId.Split('|')[3] + "|" + Math.Round((double.Parse(e.NewTextValue) + ganado), 2) + "|" + Math.Round((double.Parse(e.NewTextValue)), 2) + "|" + boton.ClassId.Split('|')[6] + "|" + boton.ClassId.Split('|')[7] + "|" + boton.ClassId.Split('|')[8];
                    }
                    catch (Exception ex)
                    {
                        boton.ClassId = boton.ClassId + Math.Round((double.Parse(e.NewTextValue) + ganado), 2) + "|" + Math.Round((double.Parse(e.NewTextValue)), 2) + "|" + boton.ClassId.Split('|')[6] + "|" + boton.ClassId.Split('|')[7] + "|" + boton.ClassId.Split('|')[8];
                    }
                }
                else
                {
                    StackLayout stackmomiocantidad = (StackLayout)entry.Parent;
                    StackLayout stackboleto = (StackLayout)entry.Parent.Parent;
                    Frame frameganancia = (Frame)stackboleto.Children[1];
                    StackLayout stackganancia = (StackLayout)frameganancia.Content;
                    Button boton = (Button)stackboleto.Children[2];
                    Label momio = (Label)stackmomiocantidad.Children[2];
                    int momio_val = int.Parse(momio.ClassId);
                    double ganado = calcular_ganado(momio_val, double.Parse(e.NewTextValue));
                    Label ganancia = (Label)stackganancia.Children[1];
                    ganancia.Text = "$ " + Math.Round((double.Parse(e.NewTextValue) + ganado), 2);
                    string ganancia_actual = "";
                    try
                    {
                        ganancia_actual = boton.ClassId.Split('|')[4];
                        boton.ClassId = boton.ClassId.Split('|')[0] + "|" + boton.ClassId.Split('|')[1] + "|" + boton.ClassId.Split('|')[2] + "|" + boton.ClassId.Split('|')[3] + "|" + Math.Round((double.Parse(e.NewTextValue) + ganado), 2) + "|" + Math.Round((double.Parse(e.NewTextValue)), 2)+ "|" + "|" + boton.ClassId.Split('|')[7] + "|" + boton.ClassId.Split('|')[8];
                    }
                    catch (Exception ex)
                    {
                        boton.ClassId = boton.ClassId + Math.Round((double.Parse(e.NewTextValue) + ganado), 2) + "|" + Math.Round((double.Parse(e.NewTextValue)), 2) + "|" + "|" + boton.ClassId.Split('|')[7] + "|" + boton.ClassId.Split('|')[8];
                    }
                }
                    
            }
            catch (Exception ex)
            {

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

