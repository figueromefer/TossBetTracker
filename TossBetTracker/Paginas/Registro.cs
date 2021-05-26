using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace TossBetTracker
{
    public class Registro : ContentPage
    {
        StackLayout stack1 = null;
        ScrollView Scrollview1 = null;
        Entry entry0 = new Entry() { FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null), TextColor = Color.FromHex("#ADB5BD"), PlaceholderColor = Color.FromHex("#ADB5BD"), Placeholder = "Nombre", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
        Entry entrycapital = new Entry() { FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null), TextColor = Color.FromHex("#ADB5BD"), PlaceholderColor = Color.FromHex("#ADB5BD"), Keyboard = Keyboard.Numeric, Placeholder = "Capital inicial", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
        Entry entry1 = new Entry() { FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null), TextColor = Color.FromHex("#ADB5BD"), PlaceholderColor = Color.FromHex("#ADB5BD"), Placeholder = "Correo", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
        CheckBox check = new CheckBox() { VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Start, Color = Color.FromHex("ffffff")};
        Label acepto = new Label() { VerticalOptions = LayoutOptions.CenterAndExpand, Text = "Acepto los términos y condiciones", FontSize = 14, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null)};
        
        Entry entry2 = new Entry() { IsPassword = true, FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null), TextColor = Color.FromHex("#ADB5BD"), PlaceholderColor = Color.FromHex("#ADB5BD"), Placeholder = "Contraseña", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start };
        Button boton2 = null;
        string foto = "http://boveda-creativa.net/tossapp/profile.png";
        StackLayout form1 = null;
        CircleImage perfil = null;
        MediaFile file = null;
        Stream stream = null;

        public Registro()
        {
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand };
            acepto.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Device.OpenUri(new Uri("http://toss.boveda-creativa.com/TERMINOS%20Y%20CONDICIONES.pdf")); } catch (Exception ex) {  } }), NumberOfTapsRequired = 1 });
            StackLayout stacknav = new StackLayout() { Children = { imagennav }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);
            BackgroundColor = Color.FromHex("#00518c");
            var foto_fondo = new Image() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Aspect = Aspect.AspectFill, Source = ImageSource.FromFile("fondo.png") };
            
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(30, 30, 30, 30) };
            
            stack1.Children.Add(new Label() { Text = " ", FontSize = 5, HeightRequest = 20 });
            form1 = new StackLayout() { };
            stack1.Children.Add(form1);
            ScrollView scv1 = new ScrollView() { Content = stack1 };             /*AbsoluteLayout.SetLayoutFlags(foto_fondo, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(foto_fondo, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(foto_fondo);*/             AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(scv1);
            this.Content = absoluteLayout;
            ConsultarP();
        }


        public async void ConsultarP()
        {
            try
            {
                Label titulo = new Label() { Text = "Registro", FontSize = 18, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null), Margin = new Thickness(0, 0, 0, 20) };
                perfil = new CircleImage
                {
                    BorderColor = Color.White,
                    BorderThickness = 3,
                    HeightRequest = 140,
                    WidthRequest = 140,
                    Margin = new Thickness(0,30,0,30),
                    Aspect = Aspect.AspectFill,
                    HorizontalOptions = LayoutOptions.Center,
                    Source = ImageSource.FromFile("profile.png")
                };
                perfil.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Tomar_foto_perfil(); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                Image lapiz = new Image() { Source = ImageSource.FromFile("editar.png"), Aspect = Aspect.AspectFit, WidthRequest = 30, Margin = new Thickness(60,-60,0,0)};
                boton2 = new Button() { IsEnabled = true,Text = "Registrarme", MinimumHeightRequest = 40, HorizontalOptions = LayoutOptions.FillAndExpand, BorderRadius = 10, BackgroundColor = Color.FromHex("#FFFFFF"), TextColor = Color.FromHex("#00518c"), FontSize = 14, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null), Margin = new Thickness(0, 0, 0, 20) };
                boton2.Clicked += OnLoginClicked;
                Label info = new Label() { FontSize = 16, TextColor = Color.White, Text = "i", HorizontalOptions = LayoutOptions.CenterAndExpand };
                Frame frame_info = new Frame() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Padding = new Thickness(2), BackgroundColor = Color.FromHex("#00518c"), CornerRadius = 10, Content = info, WidthRequest = 20, HeightRequest = 20, Visual = VisualMarker.Material };
                frame_info.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async() => { try { await DisplayAlert("Información", "Ingresa el cápital inicial con el que se calculará tu estadística personal, este capital solo será visible para ti.", "OK"); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                Image i_money = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("i_money.png"), WidthRequest = 15 };
                Image i_correo = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("i_correo.png"), WidthRequest = 15 };
                Image i_pass = new Image() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Source = ImageSource.FromFile("i_pass.png"), WidthRequest = 15 };
                StackLayout stacknombre = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { i_correo, entry0 }, Padding = new Thickness(0) };
                StackLayout stackcorreo = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { i_correo, entry1 }, Padding = new Thickness(0) };
                StackLayout stackcontra = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { i_pass, entry2 }, Padding = new Thickness(0) };
                StackLayout stackcapital = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { i_money, entrycapital, frame_info }, Padding = new Thickness(0) };
                StackLayout stackterminos = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(0,20), VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { check, acepto }, Padding = new Thickness(0) };
                Frame framenombre = new Frame() { CornerRadius = 6, BackgroundColor = Color.White, Content = stacknombre, IsClippedToBounds = true, Padding = new Thickness(15, 2), Margin = new Thickness(0, 0, 0, 20) };
                Frame framecorreo = new Frame() { CornerRadius = 6, BackgroundColor = Color.White, Content = stackcorreo, IsClippedToBounds = true, Padding = new Thickness(15, 2), Margin = new Thickness(0, 0, 0, 20) };
                Frame framecontra = new Frame() { CornerRadius = 6, BackgroundColor = Color.White, Content = stackcontra, IsClippedToBounds = true, Padding = new Thickness(15, 2), Margin = new Thickness(0, 0, 0, 10) };
                Frame framecapital = new Frame() { CornerRadius = 6, BackgroundColor = Color.White, Content = stackcapital, IsClippedToBounds = true, Padding = new Thickness(15, 2), Margin = new Thickness(0, 0, 0, 20) };
                var stacklista = new StackLayout()
                {
                    Padding = new Thickness(20, 20),
                    Children = {
                        perfil,
                        lapiz,
                        titulo,
                        framenombre,
                        framecorreo,
                        framecontra,
                        framecapital,
                        stackterminos,
                        boton2
                    }
                };
                form1.Children.Add(stacklista);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ayuda", "Problema de conectividad, favor de validar la red .", "OK");
            }

        }

        public async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                if (check.IsChecked)
                {

                    if (foto == "")
                    {
                        await DisplayAlert("Ayuda", "Necesitas subir una foto de perfil", "OK");
                    }
                    else
                    {
                        string uriString2 = string.Format("http://toss.boveda-creativa.com/registro.php?usuario={0}&password={1}&nombre={2}&foto={3}&capital={4}", entry1.Text, entry2.Text, entry0.Text, foto, entrycapital.Text);
                        var response2 = await httpRequest(uriString2);
                        if (response2.ToString() != "")
                        {
                            Settings.Idusuario = response2.ToString();
                            Settings.Nombre = entry0.Text;
                            Settings.Correo = entry1.Text;
                            Settings.Foto = WebUtility.UrlDecode(foto);
                            App.Current.MainPage = new NavigationPage(new RootPage());
                        }
                        else
                        {
                            await DisplayAlert("Ayuda", "Algo salió mal.", "OK");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Ayuda", "Es necesario aceptar los términos y condiciones para completar tu registro.", "OK");
                }
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public List<class_usuarios> procesar2(string respuesta)
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
                                 idusuario = (string)r.Element("idusuario"),
                                 nombre = (string)r.Element("nombre"),
                                 correo = (string)r.Element("correo"),
                                 foto = WebUtility.UrlDecode((string)r.Element("foto")),
                                 telefono = (string)r.Element("telefono"),
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
            
        }

        public async void subir_perfil()
        {
            try
            {
                string now1 = DateTime.Now.ToString().Replace(' ', '_').Replace('/', '_').Replace(':', '_');
                if (file == null) { await DisplayAlert("Alerta", "Falta definir una foto.", "OK"); }
                await Upload(file, "_foto_" + now1 + ".jpg");
                
                var foto1 = "http://toss.boveda-creativa.com/upload/_foto_" + now1 + ".jpg";
                
                perfil.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStreamWithImageRotatedForExternalStorage();
                    return stream;
                });
                file.Dispose();

            }
            catch (Exception ex)
            {

            }
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
                    string content = await response.Content.ReadAsStringAsync();
                    filename = WebUtility.UrlEncode(filename);
                    foto = filename;
                    boton2.IsEnabled = true;
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
                { using (var sr = new StreamReader(responseStream)) { received = await sr.ReadToEndAsync(); } }
            }
            return received;
        }
    }
}

