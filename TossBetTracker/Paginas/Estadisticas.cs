using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Acr.UserDialogs;
using ImageCircle.Forms.Plugin.Abstractions;
using Microcharts;
using Microcharts.Forms;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SkiaSharp;
using Xamarin.Forms;
using Entry = Microcharts.Entry;

namespace TossBetTracker
{
    public class Estadisticas : ContentPage
    {
        StackLayout stack1 = null;
        StackLayout stackboleto = null;
        AbsoluteLayout absoluteLayout = null;
        StackLayout stacketiqueta1, stacketiqueta2, stacketiqueta3 = null;
        string idapuesta = "";
        Grid grid = null;
        Grid griddeportes = null;
        int numdeportes = 0;
        double sumacol1, sumacol2, sumacol3 = 0;
        double contcol1, contcol2, contcol3 = 0;
        string periodo = "1 año";
        Frame periodo1, periodo2, periodo3, periodo4, periodo5, periodo6 = null;
        ChartView chartView1, chartView2, chartView3, chartView4, chartView5, chartView6, chartView7 = null;
        List<Entry> entries_aciertos1 = new List<Entry> { };
        List<Entry> entries_aciertos2 = new List<Entry> { };
        List<Entry> entries_aciertos3 = new List<Entry> { };
        List<Entry> entries_aciertos4 = new List<Entry> { };
        List<Entry> entries_aciertos5 = new List<Entry> { };
        List<Entry> entries_aciertos6 = new List<Entry> { };
        List<Entry> entries_aciertos7 = new List<Entry> { };

        public Estadisticas()
        {
            BackgroundColor = Color.White;
            //Title = "Estadísticas";
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(15, 0, 0, 0) };
            Label Name = new Label() { Text = "Estadísticas", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand,  TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            if (Device.RuntimePlatform == Device.Android)
            {
                Name.Margin = new Thickness(0, 0, 25, 0);
            }
            Image imagenchat = new Image() { IsVisible = false, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { Name, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { BackgroundColor = Color.FromHex("#ffffff"), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 40, 20, 20), Spacing = 0 };
            ScrollView scv0 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack1 };
            
            ScrollView scv1 = new ScrollView() { BackgroundColor = Color.FromHex("#01528a"), Orientation = ScrollOrientation.Both, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Padding = new Thickness(2,0,0,0) };
            
            //GENERALES
            //
            //stack1.Children.Add(Name);
            Picker picker_periodo = new Picker() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            

            
            periodo1 = new Frame() { Padding = new Thickness(7, 10), Visual = VisualMarker.Material, HasShadow = true, BackgroundColor = Color.FromHex("#01528a"), Content = new Label() { Text = "1 año", FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = Color.White } };
            periodo1.GestureRecognizers.Add(new TapGestureRecognizer
             {
                 Command = new Command(async() => {
                     try
                     {
                         await periodo1.ScaleTo(1.1, 250);
                         await periodo1.ScaleTo(1, 250);
                         periodo1.BackgroundColor = Color.FromHex("#01528a");
                         ((Label)periodo1.Content).TextColor = Color.White;

                         periodo2.BackgroundColor = Color.White;
                         ((Label)periodo2.Content).TextColor = Color.FromHex("#01528a");
                         periodo3.BackgroundColor = Color.White;
                         ((Label)periodo3.Content).TextColor = Color.FromHex("#01528a");
                         periodo4.BackgroundColor = Color.White;
                         ((Label)periodo4.Content).TextColor = Color.FromHex("#01528a");
                         periodo5.BackgroundColor = Color.White;
                         ((Label)periodo5.Content).TextColor = Color.FromHex("#01528a");
                         periodo6.BackgroundColor = Color.White;
                         ((Label)periodo6.Content).TextColor = Color.FromHex("#01528a");
                         Picker_periodo_SelectedIndexChanged(((Label)periodo1.Content).Text);
                     }
                     catch (Exception ex)
                     {
                         Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK");
                     }
                 }),
                 NumberOfTapsRequired = 1
             });
            periodo2 = new Frame() { Padding = new Thickness(7, 10), Visual = VisualMarker.Material, HasShadow = true, BackgroundColor = Color.FromHex("#FFFFFF"), Content = new Label() { Text = "6 meses", FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = Color.FromHex("#01528a") } };
            periodo2.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => {
                    try
                    {
                        await periodo2.ScaleTo(1.1, 250);
                        await periodo2.ScaleTo(1, 250);
                        periodo2.BackgroundColor = Color.FromHex("#01528a");
                        ((Label)periodo2.Content).TextColor = Color.White;

                        periodo1.BackgroundColor = Color.White;
                        ((Label)periodo1.Content).TextColor = Color.FromHex("#01528a");
                        periodo3.BackgroundColor = Color.White;
                        ((Label)periodo3.Content).TextColor = Color.FromHex("#01528a");
                        periodo4.BackgroundColor = Color.White;
                        ((Label)periodo4.Content).TextColor = Color.FromHex("#01528a");
                        periodo5.BackgroundColor = Color.White;
                        ((Label)periodo5.Content).TextColor = Color.FromHex("#01528a");
                        periodo6.BackgroundColor = Color.White;
                        ((Label)periodo6.Content).TextColor = Color.FromHex("#01528a");
                        Picker_periodo_SelectedIndexChanged(((Label)periodo2.Content).Text);
                    }
                    catch (Exception ex)
                    {
                        Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK");
                    }
                }),
                NumberOfTapsRequired = 1
            });
            periodo3 = new Frame() { Padding = new Thickness(7, 10), Visual = VisualMarker.Material, HasShadow = true, BackgroundColor = Color.FromHex("#FFFFFF"), Content = new Label() { Text = "3 meses", FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = Color.FromHex("#01528a") } };
            periodo3.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => {
                    try
                    {
                        await periodo3.ScaleTo(1.1, 250);
                        await periodo3.ScaleTo(1, 250);
                        periodo3.BackgroundColor = Color.FromHex("#01528a");
                        ((Label)periodo3.Content).TextColor = Color.White;

                        periodo2.BackgroundColor = Color.White;
                        ((Label)periodo2.Content).TextColor = Color.FromHex("#01528a");
                        periodo1.BackgroundColor = Color.White;
                        ((Label)periodo1.Content).TextColor = Color.FromHex("#01528a");
                        periodo4.BackgroundColor = Color.White;
                        ((Label)periodo4.Content).TextColor = Color.FromHex("#01528a");
                        periodo5.BackgroundColor = Color.White;
                        ((Label)periodo5.Content).TextColor = Color.FromHex("#01528a");
                        periodo6.BackgroundColor = Color.White;
                        ((Label)periodo6.Content).TextColor = Color.FromHex("#01528a");
                        Picker_periodo_SelectedIndexChanged(((Label)periodo3.Content).Text);
                    }
                    catch (Exception ex)
                    {
                        Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK");
                    }
                }),
                NumberOfTapsRequired = 1
            });
            periodo4 = new Frame() { Padding = new Thickness(7, 10), Visual = VisualMarker.Material, HasShadow = true, BackgroundColor = Color.FromHex("#FFFFFF"), Content = new Label() { Text = "1 mes", FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = Color.FromHex("#01528a") } };
            periodo4.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => {
                    try
                    {
                        await periodo4.ScaleTo(1.1, 250);
                        await periodo4.ScaleTo(1, 250);
                        periodo4.BackgroundColor = Color.FromHex("#01528a");
                        ((Label)periodo4.Content).TextColor = Color.White;

                        periodo2.BackgroundColor = Color.White;
                        ((Label)periodo2.Content).TextColor = Color.FromHex("#01528a");
                        periodo3.BackgroundColor = Color.White;
                        ((Label)periodo3.Content).TextColor = Color.FromHex("#01528a");
                        periodo1.BackgroundColor = Color.White;
                        ((Label)periodo1.Content).TextColor = Color.FromHex("#01528a");
                        periodo5.BackgroundColor = Color.White;
                        ((Label)periodo5.Content).TextColor = Color.FromHex("#01528a");
                        periodo6.BackgroundColor = Color.White;
                        ((Label)periodo6.Content).TextColor = Color.FromHex("#01528a");
                        Picker_periodo_SelectedIndexChanged(((Label)periodo4.Content).Text);
                    }
                    catch (Exception ex)
                    {
                        Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK");
                    }
                }),
                NumberOfTapsRequired = 1
            });
            periodo5 = new Frame() { Padding = new Thickness(7, 10), Visual = VisualMarker.Material, HasShadow = true, BackgroundColor = Color.FromHex("#FFFFFF"), Content = new Label() { Text = "Esta semana", FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = Color.FromHex("#01528a") } };
            periodo5.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => {
                    try
                    {
                        await periodo5.ScaleTo(1.1, 250);
                        await periodo5.ScaleTo(1, 250);
                        periodo5.BackgroundColor = Color.FromHex("#01528a");
                        ((Label)periodo5.Content).TextColor = Color.White;

                        periodo2.BackgroundColor = Color.White;
                        ((Label)periodo2.Content).TextColor = Color.FromHex("#01528a");
                        periodo3.BackgroundColor = Color.White;
                        ((Label)periodo3.Content).TextColor = Color.FromHex("#01528a");
                        periodo4.BackgroundColor = Color.White;
                        ((Label)periodo4.Content).TextColor = Color.FromHex("#01528a");
                        periodo1.BackgroundColor = Color.White;
                        ((Label)periodo1.Content).TextColor = Color.FromHex("#01528a");
                        periodo6.BackgroundColor = Color.White;
                        ((Label)periodo6.Content).TextColor = Color.FromHex("#01528a");
                        Picker_periodo_SelectedIndexChanged(((Label)periodo5.Content).Text);
                    }
                    catch (Exception ex)
                    {
                        Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK");
                    }
                }),
                NumberOfTapsRequired = 1
            });
            periodo6 = new Frame() { Padding = new Thickness(7, 10), Visual = VisualMarker.Material, HasShadow = true, BackgroundColor = Color.FromHex("#FFFFFF"), Content = new Label() { Text = "Ayer", FontSize = 14, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, TextColor = Color.FromHex("#01528a") } };
            periodo6.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () => {
                    try
                    {
                        await periodo6.ScaleTo(1.1, 250);
                        await periodo6.ScaleTo(1, 250);
                        periodo6.BackgroundColor = Color.FromHex("#01528a");
                        ((Label)periodo6.Content).TextColor = Color.White;

                        periodo2.BackgroundColor = Color.White;
                        ((Label)periodo2.Content).TextColor = Color.FromHex("#01528a");
                        periodo3.BackgroundColor = Color.White;
                        ((Label)periodo3.Content).TextColor = Color.FromHex("#01528a");
                        periodo4.BackgroundColor = Color.White;
                        ((Label)periodo4.Content).TextColor = Color.FromHex("#01528a");
                        periodo5.BackgroundColor = Color.White;
                        ((Label)periodo5.Content).TextColor = Color.FromHex("#01528a");
                        periodo1.BackgroundColor = Color.White;
                        ((Label)periodo1.Content).TextColor = Color.FromHex("#01528a");
                        Picker_periodo_SelectedIndexChanged(((Label)periodo6.Content).Text);
                    }
                    catch (Exception ex)
                    {
                        Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK");
                    }
                }),
                NumberOfTapsRequired = 1
            });
            StackLayout stack_periodos = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(3, 5, 3, 15),
                Spacing = 12,
                Children =
                {
                    periodo1,
                    periodo2,
                    periodo3,
                    periodo4,
                    periodo5,
                    periodo6
                }
            };
            ScrollView scroll_periodos = new ScrollView()
            {
                Orientation = ScrollOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = stack_periodos
            };
            stack1.Children.Add(scroll_periodos);


            /*Label LblPeriodo = new Label() { Text = "Periodo:", FontSize = 18, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            StackLayout stkperiodo = new StackLayout() { Children = {  picker_periodo}, Padding = new Thickness(0,20,0,20),HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Horizontal };
            */

            grid = new Grid { BackgroundColor = Color.FromHex("#01528a"), RowSpacing = 1, ColumnSpacing = 1 };
            scv1.Content = grid;
            griddeportes = new Grid { WidthRequest = 60,BackgroundColor = Color.FromHex("#FFFFFF"), RowSpacing = 1, ColumnSpacing = 1 };
            StackLayout stackgrid2 = new StackLayout() { Children = { griddeportes, scv1 },Orientation = StackOrientation.Horizontal,HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
            Frame frametabla = new Frame() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Content = stackgrid2, Padding = new Thickness(0,0), HasShadow = true, Visual = VisualMarker.Material };
            
            stack1.Children.Add(frametabla);

            chartView7 = new ChartView() { HeightRequest = 120, HorizontalOptions = LayoutOptions.Fill };
            Image imagen_deporte7 = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("parlay.png"), WidthRequest = 50, BackgroundColor = Color.FromHex("#FFFFFF") };
            StackLayout graf7 = new StackLayout() { Spacing = 15, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { imagen_deporte7, chartView7 } };
            Frame frame_graf7 = new Frame() { Visual = VisualMarker.Material, Padding = new Thickness(10), HasShadow = true, HorizontalOptions = LayoutOptions.FillAndExpand, Content = graf7 };
            StackLayout stackgraficas7 = new StackLayout() { Padding = new Thickness(0, 20, 0, 40), Children = { frame_graf7 } };
            stack1.Children.Add(stackgraficas7);

            chartView1 = new ChartView() { HeightRequest = 120, HorizontalOptions = LayoutOptions.Fill };
            Image imagen_deporte1 = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("NFL.png"), WidthRequest = 50, BackgroundColor = Color.FromHex("#FFFFFF") };
            StackLayout graf1 = new StackLayout() { Spacing = 15, HorizontalOptions = LayoutOptions.FillAndExpand,Children = { imagen_deporte1, chartView1 } };
            Frame frame_graf1 = new Frame() { Visual = VisualMarker.Material, Padding = new Thickness(10), HasShadow = true, HorizontalOptions = LayoutOptions.FillAndExpand ,Content = graf1 };
            StackLayout stackgraficas1 = new StackLayout() { Padding = new Thickness(0, 20,0,40), Children = { frame_graf1 } };
            stack1.Children.Add(stackgraficas1);

            chartView2 = new ChartView() { HeightRequest = 120, HorizontalOptions = LayoutOptions.Fill };
            Image imagen_deporte2 = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("MLB.png"), WidthRequest = 50, BackgroundColor = Color.FromHex("#FFFFFF") };
            StackLayout graf2 = new StackLayout() { Spacing = 15, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { imagen_deporte2, chartView2 } };
            Frame frame_graf2 = new Frame() { Visual = VisualMarker.Material, Padding = new Thickness(10), HasShadow = true, HorizontalOptions = LayoutOptions.FillAndExpand, Content = graf2 };
            StackLayout stackgraficas2 = new StackLayout() { Padding = new Thickness(0, 20, 0, 40), Children = { frame_graf2 } };
            stack1.Children.Add(stackgraficas2);

            chartView3 = new ChartView() { HeightRequest = 120, HorizontalOptions = LayoutOptions.Fill };
            Image imagen_deporte3 = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("NBA.png"), WidthRequest = 50, BackgroundColor = Color.FromHex("#FFFFFF") };
            StackLayout graf3 = new StackLayout() { Spacing = 15, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { imagen_deporte3, chartView3 } };
            Frame frame_graf3 = new Frame() { Visual = VisualMarker.Material, Padding = new Thickness(10), HasShadow = true, HorizontalOptions = LayoutOptions.FillAndExpand, Content = graf3 };
            StackLayout stackgraficas3 = new StackLayout() { Padding = new Thickness(0, 20, 0, 40), Children = { frame_graf3 } };
            stack1.Children.Add(stackgraficas3);

            chartView4 = new ChartView() { HeightRequest = 120, HorizontalOptions = LayoutOptions.Fill };
            Image imagen_deporte4 = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("NHL.png"), WidthRequest = 50, BackgroundColor = Color.FromHex("#FFFFFF") };
            StackLayout graf4 = new StackLayout() { Spacing = 15, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { imagen_deporte4, chartView4 } };
            Frame frame_graf4 = new Frame() { Visual = VisualMarker.Material, Padding = new Thickness(10), HasShadow = true, HorizontalOptions = LayoutOptions.FillAndExpand, Content = graf4 };
            StackLayout stackgraficas4 = new StackLayout() { Padding = new Thickness(0, 20, 0, 40), Children = { frame_graf4 } };
            stack1.Children.Add(stackgraficas4);

            chartView5 = new ChartView() { HeightRequest = 120, HorizontalOptions = LayoutOptions.Fill };
            Image imagen_deporte5 = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("NCAAF.png"), WidthRequest = 50, BackgroundColor = Color.FromHex("#FFFFFF") };
            StackLayout graf5 = new StackLayout() { Spacing = 15, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { imagen_deporte5, chartView5 } };
            Frame frame_graf5 = new Frame() { Visual = VisualMarker.Material, Padding = new Thickness(10), HasShadow = true, HorizontalOptions = LayoutOptions.FillAndExpand, Content = graf5 };
            StackLayout stackgraficas5 = new StackLayout() { Padding = new Thickness(0, 20, 0, 40), Children = { frame_graf5 } };
            stack1.Children.Add(stackgraficas5);

            chartView6 = new ChartView() { HeightRequest = 120, HorizontalOptions = LayoutOptions.Fill };
            Image imagen_deporte6 = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("NCAAB.png"), WidthRequest = 50, BackgroundColor = Color.FromHex("#FFFFFF") };
            StackLayout graf6 = new StackLayout() { Spacing = 15, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { imagen_deporte6, chartView6 } };
            Frame frame_graf6 = new Frame() { Visual = VisualMarker.Material, Padding = new Thickness(10), HasShadow = true, HorizontalOptions = LayoutOptions.FillAndExpand, Content = graf6 };
            StackLayout stackgraficas6 = new StackLayout() { Padding = new Thickness(0, 20, 0, 40), Children = { frame_graf6 } };
            stack1.Children.Add(stackgraficas6);

            


            AbsoluteLayout.SetLayoutFlags(scv0, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(scv0, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(scv0);

            //MENU INFERIOR
            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            //ASIGNAR LAYOUT
            this.Content = absoluteLayout;
            cargar_estadisticas();
            cargar_intro();
            cargar_aciertos(chartView1, "NFL", entries_aciertos1);
            cargar_aciertos(chartView2, "MLB", entries_aciertos2);
            cargar_aciertos(chartView3, "NBA", entries_aciertos3);
            cargar_aciertos(chartView4, "NHL", entries_aciertos4);
            cargar_aciertos(chartView5, "NCAAF", entries_aciertos5);
            cargar_aciertos(chartView6, "NCAAB", entries_aciertos6);
            cargar_aciertos(chartView7, "parlay", entries_aciertos7);
        }

        public async void cargar_aciertos(ChartView chart, string deporte, List<Entry> entries_aciertos)
        {
            try
            {
                entries_aciertos.Clear();
                string uriString2 = string.Format("http://toss.boveda-creativa.com/graf_aciertos.php?usuario={0}&periodo={1}&deporte={2}", Settings.Idusuario, periodo, deporte);
                var response2 = await httpRequest(uriString2);
                List<class_grafica> valor = new List<class_grafica>();
                valor = procesar(response2);
                for (int i = 0; i < valor.Count; i++)
                {
                    string datoy = valor.ElementAt(i).datoy;
                    if (datoy == "NAN") { datoy = "0"; }
                    Entry entry1 = new Entry(int.Parse(datoy))
                    {
                        Label = valor.ElementAt(i).datox,
                        ValueLabel = datoy+"%"
                    };
                    var random = new Random();
                    //var color = String.Format("#{0:X6}", random.Next(0x1000000));
                    //entry1.Color = SKColor.Parse(color);
                    entry1.Color = SKColor.Parse("#01528a");
                    entries_aciertos.Add(entry1);
                }
                chart.Chart = new LineChart() { Entries = entries_aciertos, LabelTextSize = 30,
                    LineMode = LineMode.Spline,
                    LineSize = 14,
                    PointMode = PointMode.Circle,
                    PointSize = 14,
                };
            }
            catch (Exception ex)
            {

            }
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

        public async void cargar_intro()
        {
            try
            {
                if (!Settings.Primera.Contains("|ESTADISTICAS|"))
                {
                    Image logointro = new Image() { Source = ImageSource.FromFile("icono_estadisticas.png"), HorizontalOptions = LayoutOptions.CenterAndExpand, WidthRequest = 50, Margin = new Thickness(0, 0, 0, 20), };
                    Label Nameintro = new Label() { Text = "Estadísticas", Margin = new Thickness(0, 0, 0, 30), FontSize = 30, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Label Contenidointro = new Label() { Text = "Aquí podrás encontrar tus estadísticas por deporte, así como filtrar por periodos de tiempo", Margin = new Thickness(0, 0, 0, 0), FontSize = 22, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    StackLayout stackcontenido = new StackLayout() { Children = { logointro, Nameintro, Contenidointro }, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, };
                    StackLayout intro1 = new StackLayout() { Opacity = 0, Children = { stackcontenido }, BackgroundColor = Color.FromRgba(0, 0, 0, 0.8), VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(30, 50) };
                    intro1.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() => {
                            try
                            {
                                absoluteLayout.Children.Remove(intro1);
                                Settings.Primera = Settings.Primera + "|ESTADISTICAS|";
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

        public async void Picker_periodo_SelectedIndexChanged(string periodo0)
        {
            try
            {
                sumacol1 = 0;
                sumacol2 = 0;
                sumacol3 = 0;
                contcol1 = 0;
                contcol2 = 0;
                contcol3 = 0;
                periodo = periodo0;
                cargar_estadisticas();
                cargar_aciertos(chartView1, "NFL", entries_aciertos1);
                cargar_aciertos(chartView2, "MLB", entries_aciertos2);
                cargar_aciertos(chartView3, "NBA", entries_aciertos3);
                cargar_aciertos(chartView4, "NHL", entries_aciertos4);
                cargar_aciertos(chartView5, "NCAAF", entries_aciertos5);
                cargar_aciertos(chartView6, "NCAAB", entries_aciertos6);
                cargar_aciertos(chartView7, "parlay", entries_aciertos7);
            }
            catch (Exception ex)
            {

            }
        }

        public async void cargar_estadisticas()
        {

            try
            {
                griddeportes.Children.Clear();
                grid.Children.Clear();
                griddeportes.RowDefinitions.Clear();
                grid.RowDefinitions.Clear();
                griddeportes.ColumnDefinitions.Clear();
                grid.ColumnDefinitions.Clear();
                
                UserDialogs.Instance.ShowLoading("Cargando tus estadisticas …");
                string uriString2 = "http://toss.boveda-creativa.com/deportes.php";
                var response2 = await httpRequest(uriString2);
                List<class_deportes> valor = new List<class_deportes>();
                valor = procesar2(response2);
                numdeportes = valor.Count - 1;
                for (int i = 0; i <= (numdeportes + 4); i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = 30 });
                    griddeportes.RowDefinitions.Add(new RowDefinition { Height = 30 });
                }
                for (int i = 1; i < 4; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) });
                }
                griddeportes.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(48, GridUnitType.Star) });

                Label col0 = new Label() { Text = "Dep.", FontSize = 10, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null), BackgroundColor = Color.FromHex("#ffffff"), Margin = new Thickness(5, 0) };
                griddeportes.Children.Add(col0, 0, 0);
                Label col1 = new Label() {  Text = "% Aciertos", FontSize = 10, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#ffffff"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null), BackgroundColor = Color.FromHex("#01528a"), Margin = new Thickness(5,0) };
                grid.Children.Add(col1, 0, 0);
                Label col2 = new Label() { Text = "Util. S/Cap.", FontSize = 10, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#ffffff"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) , BackgroundColor = Color.FromHex("#01528a"), Margin = new Thickness(5, 0) };
                grid.Children.Add(col2, 1, 0);
                Label col3 = new Label() { Text = "Util. S/Ap.", FontSize = 10, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#ffffff"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) , BackgroundColor = Color.FromHex("#01528a"), Margin = new Thickness(5, 0) };
                grid.Children.Add(col3, 2, 0);
                /*Label col4 = new Label() { Text = "Graficas", FontSize = 10, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#ffffff"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null), BackgroundColor = Color.FromHex("#01528a"), Margin = new Thickness(5, 0) };
                grid.Children.Add(col4, 3, 0);*/

                for (int j = 0; j < valor.Count(); j++)
                {
                    try
                    {
                        agregardeporte(j, valor);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task agregardeporte(int j, List<class_deportes> valor)
        {
            try
            {
                string iddeporte = valor.ElementAt(j).Name;
                string uriString_aciertos = "http://toss.boveda-creativa.com/aciertos.php?usuario=" + Settings.Idusuario + "&deporte=" + iddeporte+"&periodo=" + periodo;
                var aciertos = await httpRequest(uriString_aciertos);
                string uriString_utcap = "http://toss.boveda-creativa.com/utilidad_capital.php?usuario=" + Settings.Idusuario + "&deporte=" + iddeporte + "&periodo=" + periodo;
                var utilidad_capital = await httpRequest(uriString_utcap);
                string uriString_utap = "http://toss.boveda-creativa.com/utilidad_apuesta.php?usuario=" + Settings.Idusuario + "&deporte=" + iddeporte + "&periodo=" + periodo;
                var utilidad_apuesta = await httpRequest(uriString_utap);
                if(utilidad_apuesta == "nan") { utilidad_apuesta = "0"; };
                if (aciertos == "nan") { aciertos = "0"; };
                if (utilidad_capital == "nan") { utilidad_capital = "0"; };
                string foto = valor.ElementAt(j).Name;
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
                if (foto == "LIGA MX") { foto = "FIFA"; }
                Image imagen_deporte = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile(foto + ".png"), WidthRequest = 28, Margin = new Thickness(0,5), BackgroundColor = Color.FromHex("#FFFFFF") };
                Label nombre_deporte = new Label() { Margin = new Thickness(0, 2), Text = valor.ElementAt(j).Name, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                StackLayout stackcol1 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { imagen_deporte }, BackgroundColor = Color.FromHex("#FFFFFF"), Padding = new Thickness(2, 5) };
                griddeportes.Children.Add(imagen_deporte, 0, j+1);

                Label aciertos_deporte = new Label() { Margin = new Thickness(0, 2), Text = aciertos + " %", FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                StackLayout stackcol2 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Children = { aciertos_deporte }, BackgroundColor = Color.FromHex("#FFFFFF"), Padding = new Thickness(5, 5) };
                grid.Children.Add(stackcol2, 0, j+1);
                if(double.Parse(aciertos) > 0)
                {
                    contcol1++;
                    sumacol1 = sumacol1 + double.Parse(aciertos);
                }
                

                Label capital_deporte = new Label() { Margin = new Thickness(0, 2), Text = utilidad_capital + " %", FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                StackLayout stackcol3 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Children = { capital_deporte }, BackgroundColor = Color.FromHex("#FFFFFF"), Padding = new Thickness(5, 5) };
                grid.Children.Add(stackcol3, 1, j+1);
                if (double.Parse(utilidad_capital) > 0)
                {
                    contcol2++;
                    sumacol2 = sumacol2 + double.Parse(utilidad_capital);
                }

                Label apuesta_deporte = new Label() { Margin = new Thickness(0, 2), Text = utilidad_apuesta + " %", FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                StackLayout stackcol4 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Children = { apuesta_deporte }, BackgroundColor = Color.FromHex("#FFFFFF"), Padding = new Thickness(5, 5) };
                grid.Children.Add(stackcol4, 2, j+1);
                if (double.Parse(utilidad_apuesta) > 0)
                {
                    contcol3++;
                    sumacol3 = sumacol3 + double.Parse(utilidad_apuesta);
                }

                /*Button botongraf = new Button() { ClassId = iddeporte, Padding = new Thickness(10, 5), TextColor = Color.White, Text = "Graficas", BackgroundColor = Color.FromHex("#DFA423"), CornerRadius = 10 };
                botongraf.Clicked += Botongraf_Clicked;
                grid.Children.Add(botongraf, 3, j+1);*/

                if(j == (valor.Count - 1))
                {
                    agregarparlays();
                    
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async void agregarparlays()
        {
            try
            {
                string iddeporte = "Parlay";
                string uriString_aciertos = "http://toss.boveda-creativa.com/aciertos.php?usuario=" + Settings.Idusuario + "&deporte=" + iddeporte + "&periodo=" + periodo;
                var aciertos = await httpRequest(uriString_aciertos);
                string uriString_utcap = "http://toss.boveda-creativa.com/utilidad_capital.php?usuario=" + Settings.Idusuario + "&deporte=" + iddeporte + "&periodo=" + periodo;
                var utilidad_capital = await httpRequest(uriString_utcap);
                string uriString_utap = "http://toss.boveda-creativa.com/utilidad_apuesta.php?usuario=" + Settings.Idusuario + "&deporte=" + iddeporte + "&periodo=" + periodo;
                var utilidad_apuesta = await httpRequest(uriString_utap);
                if (utilidad_apuesta == "nan") { utilidad_apuesta = "0"; };
                if (aciertos == "nan") { aciertos = "0"; };
                if (utilidad_capital == "nan") { utilidad_capital = "0"; };
                Image imagen_deporte = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("parlay.png"), WidthRequest = 30 };
                Label nombre_deporte = new Label() { Margin = new Thickness(0, 2), Text = iddeporte, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                StackLayout stackcol1 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { imagen_deporte, nombre_deporte }, BackgroundColor = Color.FromHex("#FFFFFF"), Padding = new Thickness(5, 5) };
                griddeportes.Children.Add(imagen_deporte, 0, numdeportes + 2);

                Label aciertos_deporte = new Label() { Margin = new Thickness(0, 2), Text = aciertos + " %", FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                StackLayout stackcol2 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Children = { aciertos_deporte }, BackgroundColor = Color.FromHex("#FFFFFF"), Padding = new Thickness(5, 5) };
                grid.Children.Add(stackcol2, 0, numdeportes + 2);
                if (double.Parse(aciertos) > 0)
                {
                    contcol1++;
                    sumacol1 = sumacol1 + double.Parse(aciertos);
                }

                Label capital_deporte = new Label() { Margin = new Thickness(0, 2), Text = utilidad_capital + " %", FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                StackLayout stackcol3 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Children = { capital_deporte }, BackgroundColor = Color.FromHex("#FFFFFF"), Padding = new Thickness(5, 5) };
                grid.Children.Add(stackcol3, 1, numdeportes + 2);
                double d_utilidad_capital = 0;
                try
                {
                    d_utilidad_capital = double.Parse(utilidad_capital);
                }
                catch (Exception ex)
                {

                }
                
                if (d_utilidad_capital > 0)
                {
                    contcol2++;
                    sumacol2 = sumacol2 + double.Parse(utilidad_capital);
                }

                Label apuesta_deporte = new Label() { Margin = new Thickness(0, 2), Text = utilidad_apuesta + " %", FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                StackLayout stackcol4 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Children = { apuesta_deporte }, BackgroundColor = Color.FromHex("#FFFFFF"), Padding = new Thickness(5, 5) };
                grid.Children.Add(stackcol4, 2, numdeportes + 2);
                if (double.Parse(utilidad_apuesta) > 0)
                {
                    contcol3++;
                    sumacol3 = sumacol3 + double.Parse(utilidad_apuesta);
                }

                /*Button botongraf = new Button() { ClassId = iddeporte, Padding = new Thickness(10, 5), TextColor = Color.White, Text = "Graficas", BackgroundColor = Color.FromHex("#DFA423"), CornerRadius = 10 };
                botongraf.Clicked += Botongraf_Clicked;
                grid.Children.Add(botongraf, 3, numdeportes + 2);*/
                agregartotales();
            }
            catch (Exception ex)
            {

            }
        }

        public async void agregartotales()
        {
            try
            {
                sumacol1 = sumacol1 / contcol1;
                sumacol2 = sumacol2 / contcol2;
                sumacol3 = sumacol3 / contcol3;
                if (sumacol3.ToString() == "NaN") { sumacol3 = 0; };
                if (sumacol2.ToString() == "NaN") { sumacol2 = 0; };
                if (sumacol1.ToString() == "NaN") { sumacol1 = 0; };
                Label totallabel = new Label() { Margin = new Thickness(0, 2), Text = "Tot:", FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                StackLayout footcol1 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { totallabel }, BackgroundColor = Color.FromHex("#01528a"), Padding = new Thickness(5, 5) };
                griddeportes.Children.Add(totallabel, 0, numdeportes + 3);

                Label totalaciertos = new Label() { Text = Math.Round(sumacol1, 2) +"%", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                StackLayout footcol2 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { totalaciertos }, BackgroundColor = Color.FromHex("#01528a"), Padding = new Thickness(5, 5) };
                grid.Children.Add(totalaciertos, 0, numdeportes + 3);

                Label totalucapital = new Label() {  Text = Math.Round(sumacol2, 2) + "%", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                StackLayout footcol3 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { totalucapital }, BackgroundColor = Color.FromHex("#01528a"), Padding = new Thickness(5, 5) };
                grid.Children.Add(totalucapital, 1, numdeportes + 3);

                Label totaluapuesta = new Label() {Text = Math.Round(sumacol3, 2) + "%", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                StackLayout footcol4 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { totaluapuesta }, BackgroundColor = Color.FromHex("#01528a"), Padding = new Thickness(5, 5) };
                grid.Children.Add(totaluapuesta, 2, numdeportes + 3);

                Label totalvacio = new Label() { Text = "", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                StackLayout footcol5 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { }, BackgroundColor = Color.FromHex("#01528a"), Padding = new Thickness(5, 5) };
                grid.Children.Add(totalvacio, 3, numdeportes + 3);
            }
            catch (Exception ex)
            {

            }
        } 

        private void Botongraf_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Graficas(periodo, ((Button)sender).ClassId));
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

