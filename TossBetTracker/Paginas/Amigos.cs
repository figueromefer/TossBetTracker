
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
    public class Amigos : ContentPage
    {

        StackLayout stack1,  stackcontactos = null;
        string contactos = "";
        Entry buscador = null;

        public Amigos()
        {
            BackgroundColor = Color.FromHex("#FFFFFF");
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Label Titulo0 = new Label() { Text = "Amigos", WidthRequest = 100, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Image imagenchat = new Image() { IsVisible = false, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { Titulo0, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            stackcontactos = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 10, 20, 10), BackgroundColor = Color.FromHex("#FFFFFF") };
            
            Xamarin.Forms.ScrollView scv1 = new Xamarin.Forms.ScrollView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = stackcontactos,
            };


            buscador = new Entry() { Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence), Text = "", BackgroundColor = Color.Transparent, TextColor = Color.FromHex("#6D6E71"), HorizontalTextAlignment = TextAlignment.Center, FontSize = 12, Placeholder = "Encuentra a tus amigos por su nombre", PlaceholderColor = Color.FromHex("#BCBEC0") };
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
            


            Label friends_request = new Label() { Text = "SOLICITUDES", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Frame versolicitudes = new Frame() { Padding = new Thickness(10, 5), BackgroundColor = Color.FromHex("#2DC9EB"), Visual = VisualMarker.Material, HasShadow = true,  Content = friends_request, HorizontalOptions = LayoutOptions.CenterAndExpand, CornerRadius = 10, VerticalOptions = LayoutOptions.Start };
            versolicitudes.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async() => {
                    try
                    {
                        await versolicitudes.ScaleTo(1.1, 100);
                        await versolicitudes.ScaleTo(1, 100);
                        await Navigation.PushAsync(new Solicitudes());
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Ayuda", ex.Message, "OK");
                    }
                }),
                NumberOfTapsRequired = 1
            });
            Label Titulo = new Label() { Text = "Amigos", Margin = new Thickness(0,10,0,0), FontSize = 26, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            StackLayout cols_partido = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { Titulo } };
            stack1 = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(20,40,20,40),
                Spacing = 0,
                Children = {
                    stackbuscador,
                    cols_partido,
                    versolicitudes,
                    scv1
                }
            };


            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0, 1, 1));
            absoluteLayout.Children.Add(stack1);

            

            absoluteLayout.Children.Add(Home.cargar_menu_inferior());
            /*Image flotante = new Image()
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 55,
                WidthRequest = 55,
                Source = ImageSource.FromFile("flotante_buscar.png"),

            };
            flotante.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Navigation.PushAsync(new Buscar_amigos()); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
            AbsoluteLayout.SetLayoutFlags(flotante, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(flotante, new Rectangle(0.95, 0.90, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absoluteLayout.Children.Add(flotante);*/

            this.Content = absoluteLayout;

            Contenido();
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

        public async void Contenido()
        {
            string uriString4 = string.Format("http://toss.boveda-creativa.com/contador_requests.php?usuario={0}", Settings.Idusuario);
            var response4 = await httpRequest(uriString4);
            string contador = "0";
            if (int.Parse(response4.ToString()) >= 0)
            {
                contador = response4.ToString();
            }
            Label cantidad = new Label() { Text = contador, FontSize = 14, TextColor = Color.FromHex("#6D6E71"), HorizontalOptions = LayoutOptions.End };
           
            

            actualizar();

        }



        public async void actualizar()
        {
            try
            {
                stackcontactos.Children.Clear();
                string uriString2 = string.Format("http://toss.boveda-creativa.com/contactos.php?usuario={0}", Settings.Idusuario);
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
                        Image eliminar = new Image()
                        {
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center,
                            Source = ImageSource.FromFile("chat_eliminar"),
                            ClassId = valor.ElementAt(j).idusuario,
                            Opacity = 1,
                            WidthRequest = 28
                        };
                        eliminar.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(async () => {
                                try
                                {
                                    var res = await DisplayAlert("Opciones", "Eliminar contacto", "Eliminar", "Cancelar");
                                    if (res)
                                    {
                                        string uriString1 = string.Format("http://toss.boveda-creativa.com/eliminar_usuario.php?usuario1={0}&usuario2={1}", Settings.Idusuario, eliminar.ClassId);
                                        string response1 = await httpRequest(uriString1);
                                        StackLayout actual = new StackLayout();
                                        actual = (StackLayout)eliminar.Parent;
                                        int indice_linea = stackcontactos.Children.IndexOf(actual);
                                        stackcontactos.Children.Remove(actual);
                                        stackcontactos.Children.RemoveAt(indice_linea);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    await DisplayAlert("Help", ex.Message, "OK");
                                }
                            }),
                            NumberOfTapsRequired = 1
                        });
                        Label linea = new Label()
                        {
                            BackgroundColor = Color.FromHex("#D1D3D4"),
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
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
                                eliminar
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
            contactos = "";
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

        private async void Buscar()
        {
            try
            {

                stackcontactos.Children.Clear();
                string texto = buscador.Text;
                string uriString2 = string.Format("http://toss.boveda-creativa.com/buscar2.php?texto={0}&id={1}", texto, Settings.Idusuario);
                
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
                        foto.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try {  } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
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
                        solicitar.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(async () => {
                                try
                                {
                                    string uriString3 = string.Format("http://toss.boveda-creativa.com/solicitud_amistad.php?usuario={0}&usuario2={1}", Settings.Idusuario, solicitar.ClassId);
                                    var response3 = await httpRequest(uriString3);
                                    solicitar.WidthRequest = 1;
                                    solicitar.Opacity = 0;
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
                                nombre
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