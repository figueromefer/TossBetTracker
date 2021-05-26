
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
    public partial class Info_grupo : ContentPage
    {
        string grupo = "";
        MediaFile file = null;

        public Info_grupo(string grupo1)
        {
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            InitializeComponent();
            grupo = grupo1;
            fotogrupo.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Tomar_foto_perfil(); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
        }

        protected override void OnAppearing()
        {
           
            base.OnAppearing();
            cargargrupo();
            actualizar();
        }

        public async void cargargrupo()
        {
            try
            {
                string uriString2 = string.Format("http://toss.boveda-creativa.com/grupo.php?grupo={0}", grupo);
                var response2 = await httpRequest(uriString2);
                List<class_grupos> valor = new List<class_grupos>();
                valor = procesar2(response2);
                for (int j = 0; j < valor.Count(); j++)
                {
                    fotogrupo.Source = new UriImageSource
                    {
                        Uri = new Uri(valor.ElementAt(j).foto),
                        CachingEnabled = true,
                    };
                    nombregrupo.TextColor = Color.FromHex("#01528a");
                    nombregrupo.FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null);
                    lblmiembros.TextColor = Color.FromHex("#01528a");
                    lblmiembros.FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null);
                    nombregrupo.Text = valor.ElementAt(j).titulo;
                    descripciongrupo.Text = valor.ElementAt(j).descripcion;
                    descripciongrupo.FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null);
                }

                lblnuevo.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Navigation.PushAsync(new NMiembro(grupo)); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
            }
            catch (Exception ex)
            {

            }
        }

        public List<class_grupos> procesar2(string respuesta)
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

        public async void actualizar()
        {
            try
            {
                stackmiembros.Children.Clear();
                string uriString2 = string.Format("http://toss.boveda-creativa.com/miembros.php?grupo={0}", grupo);
                var response2 = await httpRequest(uriString2);
                if (response2.Length > 10)
                {
                    List<class_usuarios> valor = new List<class_usuarios>();
                    
                    valor = procesar(response2);
                    IEnumerable<class_usuarios> sortDescendingQuery =
                    from w in valor
                    orderby w.ranking
                    select w;
                    valor = sortDescendingQuery.ToList();
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
                        Label posicion = new Label() { Text = "#" + valor.ElementAt(j).ranking, TextColor = Color.White, FontSize = 12, WidthRequest = 15 };
                        Frame frameposicion = new Frame() { CornerRadius = 5, HasShadow = false, Padding = new Thickness(1), BackgroundColor = Color.FromHex("#DFA423"), Content = posicion, VerticalOptions = LayoutOptions.Center };
                        StackLayout ranking = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { frameposicion, nombre }, HorizontalOptions = LayoutOptions.FillAndExpand };
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
                                    var res = await DisplayAlert("Opciones", "Eliminar del grupo", "Eliminar", "Cancelar");
                                    if (res)
                                    {
                                        string uriString1 = string.Format("http://toss.boveda-creativa.com/eliminar_del_grupo.php?usuario={0}&grupo={1}", eliminar.ClassId, grupo);
                                        string response1 = await httpRequest(uriString1);
                                        StackLayout actual = new StackLayout();
                                        actual = (StackLayout)eliminar.Parent;
                                        int indice_linea = stackmiembros.Children.IndexOf(actual);
                                        stackmiembros.Children.Remove(actual);
                                        stackmiembros.Children.RemoveAt(indice_linea);
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
                                ranking,
                                eliminar
                            }
                        };
                        stackmiembros.Children.Add(usuario1);
                        stackmiembros.Children.Add(linea);
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
                                 idusuario = (string)r.Element("usuario"),
                                 ranking = (string)r.Element("ranking"),
                                 nombre = WebUtility.UrlDecode((string)r.Element("nombre")),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
                             }).ToList();
                }
            }
            return items;
        }

        #region "tomar foto perfil"
        public async void Tomar_foto_perfil()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Sin camara", "No hay una camara disponible.", "OK");
                return;
            }

            file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,

            });
            if (file == null)
                return;
            subir_perfil();
            fotogrupo.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStreamWithImageRotatedForExternalStorage();

                return stream;
            });
        }

        public async void subir_perfil()
        {
            try
            {
                string now1 = DateTime.Now.ToString().Replace(' ', '_').Replace('/', '_').Replace(':', '_');
                if (file == null) { await DisplayAlert("Alerta", "Falta definir una foto.", "OK"); }
                await Upload(file, grupo + "_fotogrupo_" + now1 + ".jpg");

                file.Dispose();
                var foto1 = "http://toss.boveda-creativa.com/upload/" + grupo + "_fotogrupo_" + now1 + ".jpg";
                foto1 = WebUtility.UrlEncode(foto1);
                try { enviar_perfil(foto1); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); }

            }
            catch (Exception ex)
            {

            }

        }

        public async void enviar_perfil(string foto)
        {
            string uriString1 = string.Format("http://toss.boveda-creativa.com/actualizar_perfil_grupo.php?grupo={0}&foto={1}", grupo, foto);
            string response1 = await httpRequest(uriString1);
        }

        public async Task<bool> Upload(MediaFile mediaFile, string filename)
        {
            try
            {
                byte[] bitmapData;
                var stream = new MemoryStream();
                mediaFile.GetStreamWithImageRotatedForExternalStorage().CopyTo(stream);
                bitmapData = stream.ToArray();
                var fileContent = new ByteArrayContent(bitmapData);

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "fileUpload",
                    FileName = filename
                };

                string boundary = "---8393774hhy37373773";
                MultipartFormDataContent multipartContent = new MultipartFormDataContent(boundary);
                multipartContent.Add(fileContent);


                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync("http://toss.boveda-creativa.com/subir.php", multipartContent);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    Settings.Foto = "http://toss.boveda-creativa.com/upload/" + filename;
                    string content = await response.Content.ReadAsStringAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        #endregion

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
