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
    public class NMensaje : ContentPage
    {
        StackLayout stack1, stackheader, stackcontactos, stackmenu2, stackmenusup = null;
        Image back, logo, looks = null;
        Label notif = null;
        string nuevos = "";
        string contactos = "";
        Entry buscador = null;

        public NMensaje()
        {
            BackgroundColor = Color.FromHex("#FFFFFF");
            NavigationPage.SetHasNavigationBar(this, false);
            stackheader = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Padding = new Thickness(10, 20, 10, 10), BackgroundColor = Color.FromHex("#ffffff"), Orientation = StackOrientation.Horizontal };
            stackcontactos = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 10, 20, 10), BackgroundColor = Color.FromHex("#FFFFFF") };
            stackmenusup = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.FromHex("#FFFFFF") };
            stackmenu2 = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Padding = new Thickness(20, 10, 20, 10), BackgroundColor = Color.FromHex("#FFFFFF") };
            stackmenusup.Children.Add(stackmenu2);
            stack1 = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0),
                Spacing = 0,
                Children = {
                    stackheader,
                    stackmenusup,
                    stackcontactos,
                }
            };

            Xamarin.Forms.ScrollView scv1 = new Xamarin.Forms.ScrollView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = stack1,
            };

            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0, 1, 1));
            absoluteLayout.Children.Add(scv1);


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
            stackheader.Children.Add(back);

            
            buscador = new Entry() { Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence), BackgroundColor = Color.Transparent, TextColor = Color.FromHex("#6D6E71"), HorizontalTextAlignment = TextAlignment.Center, FontSize = 12, Placeholder = "Busqueda rápida", PlaceholderColor = Color.FromHex("#BCBEC0"),  HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(-3, 0, -3, 0) };
            buscador.Completed += (sender, e) =>
            {
                try
                {
                    if (buscador.Text.Length > 0)
                    {
                        Buscar();
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
                            Buscar();
                        }
                    }
                    catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); }
                }),
                NumberOfTapsRequired = 1
            });
            StackLayout stackbuscar = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Horizontal, Spacing = 6, Padding = new Thickness(10, 0), Children = { lupa, buscador } };
            Frame redondo = new Frame() { CornerRadius = 15, HasShadow = false, Padding = new Thickness(0), IsClippedToBounds = true, Content = stackbuscar, BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand, BorderColor = Color.FromHex("#58595B"), Margin = 0 };
            StackLayout stackbuscador = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.FromHex("#ffffff"), Orientation = StackOrientation.Vertical, Children = { redondo } };
            stackmenu2.Children.Add(stackbuscador);
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
                            ClassId = valor.ElementAt(j).idusuario,
                            Spacing = 15,
                            Children =
                            {
                                foto,
                                nombre
                            }
                        };
                        usuario1.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(() => {
                                try
                                {
                                    iniciar_chat(usuario1.ClassId);
                                }
                                catch (Exception ex)
                                {
                                    DisplayAlert("Help", ex.Message, "OK");
                                }
                            }),
                            NumberOfTapsRequired = 1
                        });
                        stackcontactos.Children.Add(usuario1);
                        stackcontactos.Children.Add(linea);
                    }
                }
            }
            catch (Exception ex)
            {

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
                        nuevos = "1";
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
                            ClassId = valor.ElementAt(j).idusuario,
                            Spacing = 15,
                            Children =
                            {
                                foto,
                                nombre
                            }
                        };
                        usuario1.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(() => {
                                try
                                {
                                    iniciar_chat(usuario1.ClassId);
                                }
                                catch (Exception ex)
                                {
                                    DisplayAlert("Help", ex.Message, "OK");
                                }
                            }),
                            NumberOfTapsRequired = 1
                        });
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


        public async void iniciar_chat(string usuario)
        {
            string uriString1 = string.Format("http://toss.boveda-creativa.com/nchat.php?usuario1={0}&usuario2={1}", Settings.Idusuario, usuario);
            string response1 = await httpRequest(uriString1);
            if (response1 != "")
            {
                await Navigation.PushAsync(new Chat(response1));
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


/**/
