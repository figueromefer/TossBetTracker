
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TossBetTracker
{
    public class Buscar_amigos : ContentPage
    {

        StackLayout stack1, stackheader, stackcontactos, stacksup = null;
        Image back, logo, chats, nlook = null;
        string nuevos = "";
        MediaFile file = null;
        Label notif = null;
        Entry buscador = null;
       
        string contactos = "";
        int sugeridos = 0;

        public Buscar_amigos()
        {
            
            BackgroundColor = Color.FromHex("#FFFFFF");
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            stacksup = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, };
            stackcontactos = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 10, 20, 10), BackgroundColor = Color.FromHex("#FFFFFF") };

            Xamarin.Forms.ScrollView scv1 = new Xamarin.Forms.ScrollView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = stackcontactos,
            };
            stack1 = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(20,20,20,40),
                Spacing = 0,
                Children = {
                    stacksup,
                    scv1
                }
            };

            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0, 1, 1));
            absoluteLayout.Children.Add(stack1);


            
            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            this.Content = absoluteLayout;

            Contenido();
        }

        public async void Contenido()
        {

            
            logo = new Image()
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Source = ImageSource.FromFile("general_logo"),
                Opacity = 1,
                WidthRequest = 45
            };
            
            
            buscador = new Entry() { Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence), Text = "", BackgroundColor = Color.Transparent, TextColor = Color.FromHex("#6D6E71"), HorizontalTextAlignment = TextAlignment.Center, FontSize = 12, Placeholder = "Encuentra a tus amigos por su nombre", PlaceholderColor = Color.FromHex("#BCBEC0")};
            buscador.Completed += (sender, e) =>
            {
                try
                {
                    if (buscador.Text.Length > 0)
                    {
                        buscar();
                    }

                }
                catch (Exception ex)
                {
                    DisplayAlert("Help", ex.Message, "OK");
                }
            };
            Image lupa = new Image()
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Source = ImageSource.FromFile("usuarios_lupa"),
                Opacity = 1,
                WidthRequest = 20
            };
            lupa.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => {
                    try
                    {
                        if (buscador.Text.Length > 0)
                        {
                            buscar();
                        }
                    }
                    catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); }
                }),
                NumberOfTapsRequired = 1
            });
            StackLayout stackbuscar = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Horizontal, Spacing = 6, Padding = new Thickness(10, 0), Children = { lupa, buscador } };
            Frame redondo = new Frame() { CornerRadius = 15, HasShadow = false, Padding = new Thickness(0), IsClippedToBounds = true, Content = stackbuscar, BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand, BorderColor = Color.FromHex("#58595B"), Margin = 0 };
            StackLayout stackbuscador = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.FromHex("#ffffff"), Orientation = StackOrientation.Vertical, Children = { redondo } };
            stacksup.Children.Add(stackbuscador);

            enviadas();
        }

        private async void enviadas()
        {
            try
            {

                stackcontactos.Children.Clear();
                string uriString2 = string.Format("https://toss.boveda-creativa.com/solicitudes_enviadas.php?usuario={0}", Settings.Idusuario);

                var response2 = await httpRequest(uriString2);
                if (response2.Length > 10)
                {
                    List<class_usuarios> valor = new List<class_usuarios>();
                    valor = procesar(response2);
                    for (int j = 0; j < valor.Count(); j++)
                    {
                        CircleImage foto = new CircleImage
                        {
                            HeightRequest = 45,
                            WidthRequest = 45,
                            Aspect = Aspect.AspectFill,
                            ClassId = valor.ElementAt(j).idusuario,
                            HorizontalOptions = LayoutOptions.Center,
                            Source = ImageSource.FromUri(new Uri(valor.ElementAt(j).foto))
                        };
                        foto.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(async () => {
                                try
                                {
                                    await foto.ScaleTo(1.1, 100);
                                    await foto.ScaleTo(1, 100);
                                    await Navigation.PushAsync(new PerfilAmigo(foto.ClassId));
                                }
                                catch (Exception ex) { await DisplayAlert("Help", ex.Message, "OK"); }
                            }),
                            NumberOfTapsRequired = 1
                        });
                        Label nombre = new Label()
                        {
                            Text = valor.ElementAt(j).nombre,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalTextAlignment = TextAlignment.Center,
                            HeightRequest = 40
                        };
                        Label linea = new Label()
                        {
                            BackgroundColor = Color.FromHex("#D1D3D4"),
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        
                        Label solicitado = new Label()
                        {
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center,
                            Text = "Enviado",
                            FontSize = 10,
                        };
                       
                        StackLayout usuario1 = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,

                            Spacing = 15,
                            Children =
                            {
                                foto,
                                nombre,
                                solicitado
                            }
                        };
                        stackcontactos.Children.Add(usuario1);
                        stackcontactos.Children.Add(linea);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: {0}", ex);
            }

        }

        private async void buscar()
        {
            try
            {

                stackcontactos.Children.Clear();
                string texto = buscador.Text;
                string uriString2 = string.Format("https://toss.boveda-creativa.com/buscar.php?texto={0}&id={1}", texto, Settings.Idusuario);
                
                var response2 = await httpRequest(uriString2);
                if (response2.Length > 10)
                {
                    List<class_usuarios> valor = new List<class_usuarios>();
                    valor = procesar(response2);
                    for (int j = 0; j < valor.Count(); j++)
                    {
                        CircleImage foto = new CircleImage
                        {
                            HeightRequest = 45,
                            WidthRequest = 45,
                            Aspect = Aspect.AspectFill,
                            ClassId = valor.ElementAt(j).idusuario,
                            HorizontalOptions = LayoutOptions.Center,
                            Source = ImageSource.FromUri(new Uri(valor.ElementAt(j).foto))
                        };
                        foto.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(async () => {
                                try
                                {
                                    await foto.ScaleTo(1.1, 100);
                                    await foto.ScaleTo(1, 100);
                                    await Navigation.PushAsync(new PerfilAmigo(foto.ClassId));
                                }
                                catch (Exception ex) { await DisplayAlert("Help", ex.Message, "OK"); }
                            }),
                            NumberOfTapsRequired = 1
                        });
                        Label nombre = new Label()
                        {
                            Text = valor.ElementAt(j).nombre,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalTextAlignment = TextAlignment.Center,
                            HeightRequest = 40
                        };
                        Label linea = new Label()
                        {
                            BackgroundColor = Color.FromHex("#D1D3D4"),
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        Image solicitar = new Image()
                        {
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center,
                            Source = ImageSource.FromFile("friends_solicitar"),
                            WidthRequest = 28,
                            ClassId = valor.ElementAt(j).idusuario
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
                        StackLayout usuario1 = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,

                            Spacing = 15,
                            Children =
                            {
                                foto,
                                nombre,
                                solicitar
                            }
                        };
                        stackcontactos.Children.Add(usuario1);
                        stackcontactos.Children.Add(linea);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: {0}", ex);
            }

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