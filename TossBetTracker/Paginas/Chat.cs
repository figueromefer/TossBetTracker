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
using System.Diagnostics;
using Plugin.DeviceInfo;

namespace TossBetTracker
{
    public class Chat : KeyboardResizingAwareContentPage
    {

        StackLayout stack1, stackheader, stackchat, stackmensaje = null;
        Image  back, logo, foto, enviar, looks, opc = null;
        Entry mensaje = null;
        string chat_id = "";
        MediaFile file = null;
        string receptor = "";
        Label notif = null;
        Xamarin.Forms.ScrollView scv1 = null;
        AbsoluteLayout absoluteLayout = null;
        string y = "";
        string isgrupo = "0";
        string lastusuario = "";
        Image imagenchat = null;
        string grupo2_1 = "";

        public Chat(string id_chat = "0", string grupo = "0", string grupo2 = "")
        {
            grupo2_1 = grupo2;
            chat_id = id_chat;
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            imagenchat = new Image() { Source = ImageSource.FromFile("icono_grupos.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            if (grupo != "0") {
                isgrupo = grupo;
                imagenchat.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () => {
                        try
                        {
                            await Navigation.PopAsync();
                        }
                        catch (Exception ex) { await Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK"); }
                    }),
                    NumberOfTapsRequired = 1
                });
            }
            else
            {
                imagenchat.Opacity = 0;
            }
            NavigationPage.SetTitleView(this, stacknav);
            BackgroundColor = Color.FromHex("#F2ECEC");
            
            

            stackchat = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 30, 20, 10), BackgroundColor = Color.Transparent };
            stackmensaje = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, Padding = new Thickness(10, 10, 10, 10), BackgroundColor = Color.White, Orientation = StackOrientation.Horizontal, Spacing = 10 };
            if (CrossDeviceInfo.Current.Model == "iPhone")
            {
                stackmensaje.Padding = new Thickness(10, 10, 10, 25);
            }
            scv1 = new Xamarin.Forms.ScrollView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = stackchat,

            };
            stackchat.SizeChanged += scv1cambia;
            stack1 = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(0),
                Spacing = 0,
                Children = {
                    
                    scv1,
                    stackmensaje
                }
            };

          
            absoluteLayout = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0, 1, 1));
            absoluteLayout.Children.Add(stack1);

            this.Content = absoluteLayout;

            Contenido();
        }

        public List<class_grupos> procesar_grupo(string respuesta)
        {
            List<class_grupos> items = new List<class_grupos>();
            if (respuesta == "0")
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
                                 chat = (string)r.Element("chat"),
                                 ranking = (string)r.Element("ranking"),
                                 titulo = WebUtility.UrlDecode((string)r.Element("titulo")),
                                 descripcion = WebUtility.UrlDecode((string)r.Element("descripcion")),
                                 autor = WebUtility.UrlDecode((string)r.Element("autor")),
                                 fecha = WebUtility.UrlDecode((string)r.Element("fecha")),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
                             }).ToList();
                }
            }
            return items;
        }

        void scv1cambia(object sender, EventArgs e)
        {
            try
            {
                int ultimo = stackchat.Children.Count - 1;
                double parte1 = stackchat.Children.ElementAt(ultimo).Y;
                double parte2 = stackchat.Children.ElementAt(ultimo).Height;
                scv1.ScrollToAsync(0, parte1 + parte2, false);
            }
            catch (Exception ex)
            {

            }
        }

        public async void Contenido()
        {
            enviar = new Image()
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Source = ImageSource.FromFile("chat_enviar"),
                Opacity = 1,
                WidthRequest = 30
            };
            enviar.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { enviar_mensaje(); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
            mensaje = new Entry() { Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence), BackgroundColor = Color.Transparent, TextColor = Color.FromHex("#6D6E71"), HorizontalTextAlignment = TextAlignment.Start, FontSize = 12, Placeholder = "|", PlaceholderColor = Color.FromHex("#BCBEC0"), HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(-3, 0, -3, 0) };
            
            StackLayout stackbuscar = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Horizontal, Spacing = 6, Padding = new Thickness(10, 0), Children = { mensaje } };
            Frame redondo = new Frame() { CornerRadius = 15, HasShadow = false, Padding = new Thickness(0), IsClippedToBounds = true, Content = stackbuscar, BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand, BorderColor = Color.Transparent, Margin = 0 };
            StackLayout stackbuscador = new StackLayout { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.FromHex("#ffffff"), Orientation = StackOrientation.Vertical, Children = { redondo } };

            
            stackmensaje.Children.Add(stackbuscador);
            stackmensaje.Children.Add(enviar);
            
            string id_otro = "";
            string uriString2 = string.Format("http://toss.boveda-creativa.com/chat_perfil.php?usuario={0}&chat={1}&grupo={2}", Settings.Idusuario, chat_id, isgrupo);
            var response2 = await httpRequest(uriString2);
            if (response2.Length > 10)
            {
                List<class_chat_perfil> valor = new List<class_chat_perfil>();
                valor = procesar2(response2);
                for (int j = 0; j < valor.Count(); j++)
                {
                    Title = valor.ElementAt(j).nombre;
                }
            }
            try
            {
                await cargar_mensajes();

                double iScrollPosition = double.Parse(y);
                if (iScrollPosition > scv1.Height)
                {
                    await scv1.ScrollToAsync(0, iScrollPosition, false);
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task cargar_mensajes()
        {
            try
            {
                string uriString2 = string.Format("http://toss.boveda-creativa.com/cargar_mensajes.php?chat={0}", chat_id);
                var response2 = await httpRequest(uriString2);
                List<class_mensajes> valor = new List<class_mensajes>();
                valor = procesar(response2);
                for (int j = 0; j < valor.Count(); j++)
                {
                    if (valor.ElementAt(j).usuario == Settings.Idusuario)
                    {
                       
                        if (valor.ElementAt(j).mensaje != "")
                        {
                            mensaje_nuevo(valor.ElementAt(j).mensaje, valor.ElementAt(j).fecha, valor.ElementAt(j).nombre);
                        }
                        
                    }
                    else
                    {
                        if (valor.ElementAt(j).mensaje != "")
                        {
                            mensaje_nuevo2(valor.ElementAt(j).mensaje, valor.ElementAt(j).fecha, valor.ElementAt(j).nombre);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

        public List<class_chat_perfil> procesar2(string respuesta)
        {
            List<class_chat_perfil> items = new List<class_chat_perfil>();
            if (respuesta == "")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_chat_perfil
                             {
                                 id = (string)r.Element("id"),
                                 estado = WebUtility.UrlDecode((string)r.Element("estado")),
                                 nombre = WebUtility.UrlDecode((string)r.Element("nombre")),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto"))
                             }).ToList();
                }
            }
            return items;
        }

        public List<class_mensajes> procesar(string respuesta)
        {
            List<class_mensajes> items = new List<class_mensajes>();
            if (respuesta == "0")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_mensajes
                             {
                                 id = (string)r.Element("id"),
                                 usuario = WebUtility.UrlDecode((string)r.Element("usuario")),
                                 nombre = WebUtility.UrlDecode((string)r.Element("nombre")),
                                 mensaje = WebUtility.UrlDecode((string)r.Element("mensaje")),
                                 fecha = WebUtility.UrlDecode((string)r.Element("fecha"))
                             }).ToList();
                }
            }
            return items;
        }

        public async void enviar_mensaje()
        {
            try
            {
                string message = mensaje.Text;
                if (message.Length > 0 && message != "")
                {
                    message = WebUtility.UrlEncode(message);
                    string uriString2 = string.Format("http://toss.boveda-creativa.com/enviar_mensaje.php?usuario={0}&chat={1}&mensaje={2}", Settings.Idusuario, chat_id, message);
                    var response2 = await httpRequest(uriString2);
                    if (response2 != "")
                    {
                        mensaje_nuevo(WebUtility.UrlDecode(message), response2, Settings.Nombre);
                    }
                }
                mensaje.Text = "";
            }
            catch (Exception ex)
            {

            }

        }

        public void mensaje_nuevo(string mens, string hora, string nombre)
        {
            StackLayout stackrenglon = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = Color.Transparent,
                Padding = new Thickness(40, 10, 10, 10),
                Orientation = StackOrientation.Horizontal,
            };
            Label nombre1 = new Label() { Text = nombre + ":", TextColor = Color.FromHex("#01528a"), HorizontalOptions = LayoutOptions.End, FontSize = 14, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Label texto = new Label() { Text = mens, TextColor = Color.White, HorizontalOptions = LayoutOptions.Start, FontSize = 12 };
            string fecha = hora.Split(' ')[0].ToString();
            fecha = fecha.Split('-')[1] + "-" + fecha.Split('-')[2];
            string hour = hora.Split(' ')[1].ToString();
            hour = hour.Split(':')[0].ToString() + ":" + hour.Split(':')[1].ToString();
            Label hora1 = new Label() { Text = hour, TextColor = Color.White, HorizontalOptions = LayoutOptions.Start, FontSize = 12 };
            
            StackLayout m1 = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                Padding = new Thickness(5),
                Spacing = 10,
                Orientation = StackOrientation.Horizontal,
                Children = {
                    texto,
                    hora1

                }
            };
            
            Frame f1 = new Frame()
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = Color.FromHex("#56CCF2"),
                Padding = new Thickness(0),
                IsClippedToBounds = true,
                CornerRadius = 8,
                HasShadow = false,
                Content = m1
            };
            //stackchat.Children.Insert(0,m1);
            if(isgrupo != "0" && nombre != lastusuario)
            {
                stackchat.Children.Add(nombre1);
            }
            lastusuario = nombre;
            stackchat.Children.Add(f1);
            y = ((f1.Y) + (f1.Height)).ToString();
        }

        public void mensaje_nuevo2(string mens, string hora, string nombre)
        {
            StackLayout stackrenglon = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = Color.Transparent,
                Padding = new Thickness(40, 10, 10, 10),
                Orientation = StackOrientation.Horizontal,
            };
            Label nombre1 = new Label() { Text = nombre+":", TextColor = Color.FromHex("#01528a"), HorizontalOptions = LayoutOptions.Start, FontSize = 14, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            Label texto = new Label() { Text = mens, TextColor = Color.FromHex("#231F20"), HorizontalOptions = LayoutOptions.Start, FontSize = 12 };
            string fecha = hora.Split(' ')[0].ToString();
            fecha = fecha.Split('-')[1] + "-" + fecha.Split('-')[2];
            string hour = hora.Split(' ')[1].ToString();
            hour = hour.Split(':')[0].ToString() + ":" + hour.Split(':')[1].ToString();

            Label hora1 = new Label() { Text = hour, TextColor = Color.FromHex("#01528a"), HorizontalOptions = LayoutOptions.Start, FontSize = 12 };

            StackLayout m1 = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
                Padding = new Thickness(5),
                Spacing = 10,
                Orientation = StackOrientation.Horizontal,
                Children = {
                    hora1,
                    texto

                }
            };
            
            Frame f1 = new Frame()
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
                BackgroundColor = Color.FromHex("#ffffff"),
                Padding = new Thickness(0),
                IsClippedToBounds = true,
                CornerRadius = 8,
                HasShadow = false,
                Content = m1
            };
            //stackchat.Children.Insert(0,m1);
            if (isgrupo != "0" && nombre != lastusuario)
            {
                stackchat.Children.Add(nombre1);
            }
            lastusuario = nombre;
            stackchat.Children.Add(f1);
            y = ((f1.Y) + (f1.Height)).ToString();
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