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
    public class Compartir : ContentPage
    {
        StackLayout stack1 = null;
        StackLayout stackboleto = null;
        AbsoluteLayout absoluteLayout = null;
        StackLayout stacketiqueta1, stacketiqueta2, stacketiqueta3 = null;
        string idapuesta = "";

        public Compartir(string apuesta)
        {
            idapuesta = apuesta;
            BackgroundColor = Color.White;
            Label Titulo = new Label() { Text = "Resumen", WidthRequest = 100, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { Titulo, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { BackgroundColor = Color.FromHex("#01528a"), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(0, 0, 0, 0), Spacing = 0 };
            
            //GENERALES

            AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(stack1);

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

                string uriString2 = "http://toss.boveda-creativa.com/apuesta.php?id=" + idapuesta;
                var response2 = await httpRequest(uriString2);
                List<class_apuestas> valor = new List<class_apuestas>();
                valor = procesar2(response2);
                for (int j = 0; j < valor.Count(); j++)
                {
                    string extra = "";
                    if(int.Parse(valor.ElementAt(j).momio) > 0)
                    {
                        extra = "+";
                    }
                    //Resumen
                    //Label Tituloresumen = new Label() { Text = "Resumen", Margin = new Thickness(0, 0, 0, 10), FontSize = 20, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Label Labelequipo = new Label() { Text = "Equipo: " + valor.ElementAt(j).equipo, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Label Labelmomio = new Label() { Text = "Momio: " + extra +valor.ElementAt(j).momio, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Label Labeltipo = new Label() { Text = "Tipo: " + valor.ElementAt(j).tipo, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Label Labelganancia = new Label() { Text = "Ganancia: " + valor.ElementAt(j).ganancia, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    Label Labelmonto = new Label() { Text = "Monto: " + valor.ElementAt(j).monto, FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                    string classid = "";
                    
                    StackLayout stack_resumen = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Start,
                        Padding = new Thickness(20, 30),
                        Children = {
                           // Tituloresumen,
                            Labelequipo,
                            Labelmomio,
                            Labeltipo,
                            Labelganancia,
                            Labelmonto,
                        }
                    };
                    
                    stack1.Children.Add(stack_resumen);

                    //SECCIÓN COMPARTIR
                    Label Titulocompartir = new Label() { Text = "Tu publicación no mostrará el monto apostado.", Margin = new Thickness(0, 0, 0, 10), FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };

                    CheckBox check_amigos = new CheckBox() { ClassId = "Amigos"};
                    Label Labelamigos = new Label() { Text = "Amigos.", Margin = new Thickness(0,5,0,0), FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    StackLayout stackamigos = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { check_amigos, Labelamigos} };

                    CheckBox check_grupos = new CheckBox() { ClassId = "Grupos" };
                    check_grupos.CheckedChanged += Check_grupos_CheckedChanged;
                    Label Labelgrupos = new Label() { Text = "Grupos:", Margin = new Thickness(0, 5, 0, 0), FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    StackLayout stackgrupos = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { check_grupos, Labelgrupos } };

                    StackLayout stackchecks= new StackLayout() { Children = {  stackamigos, stackgrupos} };
                    ScrollView scrollchecks = new ScrollView() { Content = stackchecks };

                    Button publicar = new Button() {  Text = "Publicar", MinimumHeightRequest = 30, WidthRequest = 200, HorizontalOptions = LayoutOptions.Center, BorderRadius = 16, BackgroundColor = Color.FromHex("#2DC9EB"), TextColor = Color.FromHex("#ffffff"), FontSize = 20, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    publicar.Clicked += Publicar_Clicked;

                    StackLayout stack_compartir = new StackLayout()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.White,
                        Padding = new Thickness(20, 30, 20, 65),
                        Children = {
                            Titulocompartir,
                            scrollchecks,
                            
                        }
                    };

                    string uriString_grupos = "http://toss.boveda-creativa.com/grupos.php?usuario=" + Settings.Idusuario;
                    var response_grupos = await httpRequest(uriString_grupos);
                    List<class_grupos> valor_grupos = new List<class_grupos>();
                    valor_grupos = procesar_grupo(response_grupos);
                    for (int k = 0; k < valor_grupos.Count(); k++)
                    {
                        CheckBox check_grupo = new CheckBox() { ClassId = valor_grupos.ElementAt(k).id };
                        Label Labelgrupo = new Label() { Text = valor_grupos.ElementAt(k).titulo, Margin = new Thickness(0, 5, 0, 0), FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#979797"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                        StackLayout stackgrupo = new StackLayout() { ClassId = "grupo|"+ valor_grupos.ElementAt(k).id, Margin = new Thickness(25,0,0,0), Orientation = StackOrientation.Horizontal, Children = { check_grupo, Labelgrupo } };
                        stackchecks.Children.Add(stackgrupo);
                    }
                    stack1.Children.Add(stack_compartir);
                    stack_compartir.Children.Add(publicar);

            }
            }
            catch (Exception ex)
            {

            }
        }

        private async void Publicar_Clicked(object sender, EventArgs e)
        {
            try
            {
                string grupos = "";
                string amigos = "";
                Button publicar = (Button)sender;
                StackLayout stack_compartir = (StackLayout)publicar.Parent;
                ScrollView scrollchecks = (ScrollView)stack_compartir.Children[1];
                StackLayout stackchecks = (StackLayout)scrollchecks.Content;
                StackLayout stackamigos = (StackLayout)stackchecks.Children[0];
                CheckBox checkamigos = (CheckBox)stackamigos.Children[0];
                if (checkamigos.IsChecked)
                {
                    amigos = "1";
                }
                for (int i = 2; i < stackchecks.Children.Count; i++)
                {
                    StackLayout stackgrupo = (StackLayout)stackchecks.Children[i];
                    CheckBox check_grupo = (CheckBox)stackgrupo.Children[0];
                    if (check_grupo.IsChecked)
                    {
                        grupos = grupos + "|" + check_grupo.ClassId;
                    }
                }
                if(amigos != "" || grupos != "")
                {
                    string uriString_publicar = "http://toss.boveda-creativa.com/publicar.php?apuesta=" + idapuesta+"&amigos=" + amigos + "&grupos="+grupos;
                    UserDialogs.Instance.ShowLoading("Publicando");
                    var response_publicar = await httpRequest(uriString_publicar);
                    UserDialogs.Instance.HideLoading();
                    Application.Current.MainPage = new NavigationPage(new RootPage());
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(new RootPage());
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Check_grupos_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            try
            {
                CheckBox checkgrupos = (CheckBox)sender;
                bool estado = checkgrupos.IsChecked;
                StackLayout stackchecks = (StackLayout)checkgrupos.Parent.Parent;
                for (int i = 2; i < stackchecks.Children.Count; i++)
                {
                    StackLayout stackgrupo = (StackLayout)stackchecks.Children[i];
                    CheckBox check_grupo = (CheckBox)stackgrupo.Children[0];
                    check_grupo.IsChecked = estado;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<class_grupos> procesar_grupo(string respuesta)
        {
            List<class_grupos> items = new List<class_grupos>();
            if (respuesta == "")
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
                                 titulo = WebUtility.UrlDecode((string)r.Element("titulo")),
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
                                 equipo = WebUtility.UrlDecode((string)r.Element("equipo")),
                                 momio = WebUtility.UrlDecode((string)r.Element("momio")),
                                 tipo = WebUtility.UrlDecode((string)r.Element("tipo")),
                                 ganancia = WebUtility.UrlDecode((string)r.Element("ganancia")),
                                 monto = WebUtility.UrlDecode((string)r.Element("monto"))
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

