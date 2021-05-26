using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Media.Abstractions;
using Plugin.Media;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TossBetTracker
{
    public class Chats : ContentPage
    {

        StackLayout stack1 = null;
        StackLayout stack_grupos = null;
        StackLayout pestanas = null;
        Image back = null;
        AbsoluteLayout absoluteLayout = null;


        public Chats()
        {
            BackgroundColor = Color.FromHex("#01528a");
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);

            absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(10, 0, 10, 10), Spacing = 0 };
            Label Titulo = new Label() { Text = "Chats", FontSize = 26, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Label nuevo = new Label() { Text = "Crear chat + ", Margin = new Thickness(0, 5, 0, 0), FontSize = 16, HorizontalTextAlignment = TextAlignment.End, HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
            nuevo.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Navigation.PushAsync(new NMensaje()); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
            StackLayout stacksuperior = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { Titulo, nuevo }, Padding = new Thickness(0, 0, 0, 15) };
            stack1.Children.Add(stacksuperior);

            stack_grupos = new StackLayout() { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Padding = new Thickness(20, 20, 20, 20), Spacing = 0 };
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack_grupos };
            Frame framegrupos = new Frame() { IsClippedToBounds = true, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, CornerRadius = 25, BackgroundColor = Color.White, Content = scv1, Padding = new Thickness(0) };
            stack1.Children.Add(framegrupos);

            //GENERALES
            AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0.90, 1, 0.93));             absoluteLayout.Children.Add(stack1);

            //MENU INFERIOR
            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            //ASIGNAR LAYOUT
            this.Content = absoluteLayout;
            cargar_chats();
        }

        public async void cargar_chats()
        {
            try
            {
                string uriString2 = string.Format("http://ec2-18-212-22-223.compute-1.amazonaws.com/chats.php?usuario={0}", Settings.Idusuario);
                var response2 = await httpRequest(uriString2);
                if (response2.Length > 10)
                {
                    List<class_chats> valor = new List<class_chats>();
                    valor = procesar(response2);
                    for (int j = 0; j < valor.Count(); j++)
                    {
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
                            FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null),
                            TextColor = Color.FromHex("#01528a"),
                            FontSize = 16,
                        };
                        Label ultimo = new Label()
                        {
                            Text = valor.ElementAt(j).ultimo,
                            FontSize = 12,
                            VerticalTextAlignment = TextAlignment.Center,
                            HeightRequest = 20
                        };
                        Image eliminar = new Image()
                        {
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center,
                            Source = ImageSource.FromFile("chat_eliminar"),
                            ClassId = valor.ElementAt(j).id,
                            Opacity = 1,
                            WidthRequest = 28
                        };
                        eliminar.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(async () => {
                                try
                                {

                                    var res = await DisplayAlert("Opciones", "¿Deseas eliminar este chat?", "Eliminar", "Cancelar");
                                    if (res)
                                    {

                                        string uriString3 = string.Format("http://ec2-18-212-22-223.compute-1.amazonaws.com/eliminar_chat.php?id={0}", eliminar.ClassId);
                                        var response3 = await httpRequest(uriString3);
                                        StackLayout stackeliminar = (StackLayout)eliminar.Parent;
                                        int idindex = stack_grupos.Children.IndexOf(stackeliminar);
                                        stack_grupos.Children.RemoveAt(idindex);
                                        stack_grupos.Children.RemoveAt(idindex);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }),
                            NumberOfTapsRequired = 1
                        });
                        Label linea = new Label()
                        {
                            BackgroundColor = Color.FromHex("#D1D3D4"),
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Margin = new Thickness(0, 0, 0, 10)
                        };
                        StackLayout nombre_ultimo = new StackLayout()
                        {
                            Orientation = StackOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,
                            Children =
                            {
                                nombre,
                                ultimo
                            }
                        };
                        
                        StackLayout usuario1 = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,
                            
                            Margin = new Thickness(0, 0, 0, 10),
                            Spacing = 15,
                            Children =
                            {
                                foto,
                                nombre_ultimo,
                                
                            }
                        };
                        if(valor.ElementAt(j).grupo != null)
                        {
                            usuario1.ClassId = valor.ElementAt(j).id+"|"+ valor.ElementAt(j).grupo;
                            nombre.Text = nombre.Text + " (Grupo)";
                            usuario1.GestureRecognizers.Add(new TapGestureRecognizer
                            {
                                Command = new Command(() => {
                                    try
                                    {
                                        Navigation.PushAsync(new Chat(usuario1.ClassId.Split('|')[0], usuario1.ClassId.Split('|')[1]));
                                    }
                                    catch (Exception ex)
                                    {
                                        DisplayAlert("Help", ex.Message, "OK");
                                    }
                                }),
                                NumberOfTapsRequired = 1
                            });
                        }
                        else
                        {
                            usuario1.Children.Add(eliminar);
                            usuario1.ClassId = valor.ElementAt(j).id;
                            usuario1.GestureRecognizers.Add(new TapGestureRecognizer
                            {
                                Command = new Command(() => {
                                    try
                                    {
                                        Navigation.PushAsync(new Chat(usuario1.ClassId));
                                    }
                                    catch (Exception ex)
                                    {
                                        DisplayAlert("Help", ex.Message, "OK");
                                    }
                                }),
                                NumberOfTapsRequired = 1
                            });
                        }
                        
                        stack_grupos.Children.Add(usuario1);
                        stack_grupos.Children.Add(linea);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }


        public List<class_chats> procesar(string respuesta)
        {
            List<class_chats> items = new List<class_chats>();
            if (respuesta == "0")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_chats
                             {
                                 id = (string)r.Element("id"),
                                 nombre = WebUtility.UrlDecode((string)r.Element("nombre")),
                                 ultimo = WebUtility.UrlDecode((string)r.Element("ultimo")),
                                 grupo = WebUtility.UrlDecode((string)r.Element("grupo")),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto"))
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

}