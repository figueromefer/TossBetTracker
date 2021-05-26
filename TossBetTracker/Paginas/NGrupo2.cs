using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ImageCircle.Forms.Plugin.Abstractions;

namespace TossBetTracker
{
    public class NGrupo2 : ContentPage
    {

        StackLayout stack1, stackheader, stackcontactos, stackmenu = null;
        Image back, logo, enviar = null;
        Label notif, nombres = null;
        string nuevos = "";
        string contactos = "";
        string grupo = "";
        string usuarios = "";
        string usuarios_nombres = "";
        string usuarios_nombres2 = "";
        string seleccionado = "";
        StackLayout stack_form = null;

        AbsoluteLayout absoluteLayout = null;

        public NGrupo2(string grupo1)
        {
            
            grupo = grupo1;
            BackgroundColor = Color.FromHex("#01528a");
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);

            absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(10, 0, 10, 10), Spacing = 0 };
            Label Titulo = new Label() { Text = "Miembros:", FontSize = 26, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            
            StackLayout stacksuperior = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { Titulo }, Padding = new Thickness(0, 0, 0, 15) };
            stack1.Children.Add(stacksuperior);

            stack_form = new StackLayout() { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Padding = new Thickness(20, 20, 20, 20), Spacing = 0 };
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack_form };
            Frame framegrupos = new Frame() { IsClippedToBounds = true, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, CornerRadius = 25, BackgroundColor = Color.White, Content = scv1, Padding = new Thickness(0) };
            stack1.Children.Add(framegrupos);
            //GENERALES
            AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0.90, 1, 0.93));             absoluteLayout.Children.Add(stack1);

            //MENU INFERIOR
            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            //ASIGNAR LAYOUT
            this.Content = absoluteLayout;
            

            Contenido();
        }



        public async void Contenido()
        {

            try
            {
                string uriString2 = string.Format("http://toss.boveda-creativa.com/contactos.php?usuario={0}", Settings.Idusuario);
                var response2 = await httpRequest(uriString2);
                if (response2.Length > 10 && nuevos == "")
                {
                    List<class_usuarios> valor = new List<class_usuarios>();
                    valor = procesar(response2);
                    for (int j = 0; j < valor.Count(); j++)
                    {
                        nuevos = "1";
                        CheckBox chk = new CheckBox()
                        {
                            WidthRequest = 25,

                            ClassId = valor.ElementAt(j).idusuario + "|" + valor.ElementAt(j).nombre,
                            IsChecked = false,
                        };

                        chk.CheckedChanged += checkedcambio;


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
                            Text = valor.ElementAt(j).nombre,
                            VerticalTextAlignment = TextAlignment.Center,
                            HeightRequest = 40
                        };

                        StackLayout usuario1 = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,
                            ClassId = valor.ElementAt(j).idusuario,
                            Spacing = 15,
                            Margin = new Thickness(0, 0, 0, 15),
                            Children =
                            {
                                chk,
                                foto,
                                nombre
                            }
                        };

                        stack_form.Children.Add(usuario1);
                    }
                    Button Crear = new Button() { Margin = new Thickness(0, 20, 0, 0), Text = "Crear grupo", MinimumHeightRequest = 30, WidthRequest = 200, HorizontalOptions = LayoutOptions.Center, BorderRadius = 16, BackgroundColor = Color.FromHex("#2DC9EB"), TextColor = Color.FromHex("#ffffff"), FontSize = 20, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                    Crear.Clicked += CrearClicked;
                    stack_form.Children.Add(Crear);
                }
                else
                {
                    Label nombre = new Label()
                    {
                        Text = "No hay amigos por mostrar, ¡Agrega a tus amigos dando click aquí!",
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                        Margin = new Thickness(30, 70, 30, 70),
                        FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null),
                        TextColor = Color.FromHex("#01528a"),
                        FontSize = 24,
                    };
                    nombre.GestureRecognizers.Add(new TapGestureRecognizer
                    {
                        Command = new Command(() => {
                            try
                            {
                                Navigation.PushAsync(new Amigos());
                            }
                            catch (Exception ex2)
                            {
                                DisplayAlert("Help", ex2.Message, "OK");
                            }
                        }),
                        NumberOfTapsRequired = 1
                    });
                    stack_form.Children.Add(nombre);
                }
            }
            catch (Exception ex)
            {
                
            }

        }

        private void CrearClicked(object sender, EventArgs e)
        {
            enviar_a_usuarios();
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
                                 idusuario = (string)r.Element("id"),
                                 nombre = WebUtility.UrlDecode((string)r.Element("nombre")),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto"))
                             }).ToList();
                }
            }
            return items;
        }

        public async void checkedcambio(object sender, EventArgs e)
        {

            CheckBox check1 = (CheckBox)sender;
            string nombre1 = check1.ClassId.Split('|')[1];
            string usuario1 = check1.ClassId.Split('|')[0];
            if (check1.IsChecked)
            {
                usuarios = usuarios + "|" + usuario1 + "|";
                usuarios_nombres = usuarios_nombres + "|" + nombre1 + "|";
                if (usuarios_nombres2.Length > 1)
                {
                    usuarios_nombres2 = usuarios_nombres2 + ", " + nombre1;
                }
                else
                {
                    usuarios_nombres2 = nombre1;
                }
                
            }
            else
            {
                if (usuarios_nombres2.Contains(","))
                {
                    usuarios_nombres2 = usuarios_nombres2.Replace(", " + nombre1, "");
                }
                else
                {
                    usuarios_nombres2 = "";
                }
                usuarios_nombres2 = usuarios_nombres2.Replace(", " + nombre1, "");
                usuarios = usuarios.Replace("|" + usuario1 + "|", "");
                usuarios_nombres = usuarios_nombres.Replace("|" + nombre1 + "|", "");
                
            }
        }

        public async void enviar_a_usuarios()
        {
            try
            {
                string uriString1 = string.Format("http://toss.boveda-creativa.com/agregar_grupo.php?grupo={0}&usuarios={1}", grupo, "|"+Settings.Idusuario+"|"+usuarios);
                string response1 = await httpRequest(uriString1);
                if (response1 != "")
                {
                    Application.Current.MainPage = new NavigationPage(new Home());
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Help", ex.Message, "OK");
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

    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
    }
}
