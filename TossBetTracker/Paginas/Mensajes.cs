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
    public class Mensajes : ContentPage
    {

        StackLayout stack1 = null;
        StackLayout pestanas = null;
        Image back = null;
        AbsoluteLayout absoluteLayout = null;
        

        public Mensajes()
        {
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            BackgroundColor = Color.White;
            absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 20, 20, 20), Spacing = 0 };
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack1 };
            //GENERALES

            AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0.90, 1, 0.93));             absoluteLayout.Children.Add(scv1);

            

            //MENU INFERIOR

            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            

            //ASIGNAR LAYOUT

            this.Content = absoluteLayout;
            Contenido();
        }

      

        public async void Contenido()
        {
            back = new Image()
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Source = ImageSource.FromFile("general_back2"),
                Opacity = 1,
                WidthRequest = 40
            };
            back.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Navigation.PopAsync(); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
            
            cargar_chats();


        }

        public async void cargar_chats()
        {
            string uriString2 = string.Format("http://toss.boveda-creativa.com/chats.php?usuario={0}", Settings.Idusuario);
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
                        FontSize = 16,
                        HeightRequest = 20
                    };
                    Label ultimo = new Label()
                    {
                        Text = valor.ElementAt(j).ultimo,
                        FontSize = 12,
                        VerticalTextAlignment = TextAlignment.Center,
                        HeightRequest = 20
                    };
                    Label linea = new Label()
                    {
                        BackgroundColor = Color.FromHex("#D1D3D4"),
                        HeightRequest = 1,
                        HorizontalOptions = LayoutOptions.FillAndExpand
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
                        ClassId = valor.ElementAt(j).id,
                        Spacing = 15,
                        Children =
                            {
                                foto,
                                nombre_ultimo
                            }
                    };
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
                    stack1.Children.Add(usuario1);
                    stack1.Children.Add(linea);
                }
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
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
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