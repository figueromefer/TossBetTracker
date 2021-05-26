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
    public class Solicitudes : ContentPage
    {

        StackLayout stack1, stackheader, stackcontactos, stackmenusup, stackmenu = null;
        Image back = null;
        string nuevos = "";
        string solicitudes = "";


        public Solicitudes()
        {
            
            BackgroundColor = Color.FromHex("#FFFFFF");
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);

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
                Padding = new Thickness(0),
                Spacing = 0,
                Children = {
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
            try
            {
                string uriString2 = string.Format("http://toss.boveda-creativa.com/solicitudes.php?usuario={0}", Settings.Idusuario);
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
                            ClassId = valor.ElementAt(j).idusuario,
                            HorizontalOptions = LayoutOptions.Center,
                            
                        };
                        try
                        {
                            foto.Source = ImageSource.FromUri(new Uri(valor.ElementAt(j).foto));
                        }
                        catch (Exception ex)
                        {

                        }
                        foto.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                        Label nombre = new Label()
                        {
                            Text = valor.ElementAt(j).nombre,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            HeightRequest = 40
                        };
                        Label linea = new Label()
                        {

                            BackgroundColor = Color.FromHex("#D1D3D4"),
                            HeightRequest = 1,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        };
                        Image aceptar = new Image()
                        {
                            ClassId = valor.ElementAt(j).idusuario + "|" + stackcontactos.Children.Count(),

                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Start,
                            Source = ImageSource.FromFile("solicitudes_aceptar"),
                            Opacity = 1,
                            WidthRequest = 30
                        };
                        solicitudes = solicitudes + "|" + valor.ElementAt(j).idusuario + "|";
                        aceptar.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(() => {
                                try
                                {
                                    Enviar_aceptacion(aceptar.ClassId);

                                }
                                catch (Exception ex)
                                {
                                    DisplayAlert("Help", ex.Message, "OK");
                                }
                            }),
                            NumberOfTapsRequired = 1
                        });
                        Image negar = new Image()
                        {
                            ClassId = valor.ElementAt(j).idusuario + "|" + stackcontactos.Children.Count(),
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Start,
                            Source = ImageSource.FromFile("solicitudes_negar"),
                            Opacity = 1,
                            WidthRequest = 30
                        };
                        negar.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Enviar_negacion(negar.ClassId); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                        StackLayout usuario1 = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.Start,

                            Spacing = 15,
                            Children =
                            {
                                foto,
                                nombre, aceptar, negar
                            }
                        };
                        stackcontactos.Children.Add(usuario1);
                        stackcontactos.Children.Add(linea);
                    }
                    if (valor.Count() == 0)
                    {
                        Label Titulonopartidos = new Label() { Text = "No hay solicitudes de amistad pendientes. ", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null), Margin = new Thickness(0, 50, 0, 0) };
                        stackcontactos.Children.Add(Titulonopartidos);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

       

        private async void Enviar_aceptacion(string id)
        {
            try
            {
                string soli1 = id.Split('|')[0];
                string row = id.Split('|')[1];
                string uriString2 = string.Format("http://toss.boveda-creativa.com/aceptar.php?solicitud={0}", soli1);
                var response2 = await httpRequest(uriString2);
                if (response2 == "1")
                {
                    Desaparecer(row);
                }
                else
                {
                    await DisplayAlert("Error", "Something went wrong, Check your internet connection.", "OK");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void Enviar_negacion(string id)
        {
            try
            {
                string soli1 = id.Split('|')[0];
                string row = id.Split('|')[1];
                string uriString2 = string.Format("http://toss.boveda-creativa.com/negar.php?solicitud={0}", soli1);
                var response2 = await httpRequest(uriString2);
                if (response2 == "1")
                {
                    Desaparecer(row);
                }
                else
                {
                    await DisplayAlert("Error", "Something went wrong, Check your internet connection.", "OK");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void Desaparecer(string renglon)
        {
            try
            {
                await stackcontactos.Children.ElementAt(int.Parse(renglon)).FadeTo(0, 700);
                await stackcontactos.Children.ElementAt(int.Parse(renglon) + 1).FadeTo(0, 700);
                stackcontactos.Children.RemoveAt(int.Parse(renglon));
                stackcontactos.Children.RemoveAt(int.Parse(renglon));
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