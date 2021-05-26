using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ImageCircle.Forms.Plugin.Abstractions;
using Xamarin.Forms;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TossBetTracker
{
    public class Perfil : ContentPage
    {
        StackLayout stack1 = null;
        ScrollView Scrollview1 = null;
        StackLayout form1, form2, form3 = null;
        Label promedio = null;
        Label utilidad_capital = null;
        Label utilidad_apuesta = null;
        MediaFile file = null;
        Stream stream = null;
        CircleImage foto_perfil = null;
        Label capital = null;

        public Perfil()
        {
            try
            {
                Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
                Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
                StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
                NavigationPage.SetTitleView(this, stacknav);
                BackgroundColor = Color.FromHex("#01528a");
                AbsoluteLayout absoluteLayout = new AbsoluteLayout();
                stack1 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(30, 30, 30, 30) };
                StackLayout header = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Orientation = StackOrientation.Horizontal, Padding = new Thickness(0, 0, 0, 20) };
                capital = new Label() { Text = "$", FontSize = 26, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#ffffff"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                Label capital2 = new Label() { Text = "Editar capital", FontSize = 10, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#ffffff"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                capital.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Navigation.PushAsync(new Editar_capital());  } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                capital2.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Navigation.PushAsync(new Editar_capital()); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                stack1.Children.Add(capital);
                stack1.Children.Add(capital2);
                stack1.Children.Add(header);
                stack1.Children.Add(new Label() { Text = " ", FontSize = 5, HeightRequest = 20 });

                //PERFIL
                
                 foto_perfil = new CircleImage
                {
                    BorderColor = Color.Transparent,
                    BorderThickness = 2,
                    HeightRequest = 130,
                    WidthRequest = 130,
                    Aspect = Aspect.AspectFill,
                    HorizontalOptions = LayoutOptions.Center,
                };
                try
                {
                    foto_perfil.Source = ImageSource.FromUri(new Uri(Settings.Foto));
                }
                catch (Exception ex)
                {
                    foto_perfil.Source = ImageSource.FromFile("profile.png");
                }
                foto_perfil.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Tomar_foto_perfil(); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });
                stack1.Children.Add(foto_perfil);
                Label Nombre = new Label() { Text = Settings.Nombre, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#000000"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                stack1.Children.Add(Nombre);
                Label Correo = new Label() { Text = Settings.Correo, FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#686868"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                stack1.Children.Add(Correo);

                StackLayout stackcontenido = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                ScrollView scv1 = new ScrollView() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Content = stackcontenido };
                //FRAME SUPERIOR

                form1 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };
                             promedio = new Label() { Text = "", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#000000"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };                 StackLayout stackpromedio = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                    {
                        new Label() { Text = "Aciertos", FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#686868"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) },
                        promedio
                    }
                };                             ConsultarP();                 form1.Children.Add(stackpromedio);
                StackLayout form01 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Spacing = 0 };
                form01.Children.Add(form1);
            
                Frame frame1 = new Frame() { Content = form01, Margin = new Thickness(0,10), BackgroundColor = Color.White, HasShadow = true, IsClippedToBounds = true, CornerRadius = 35, Padding = new Thickness(15) };
                stackcontenido.Children.Add(frame1);                     //FRAME SUPERIOR

                form2 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };

                utilidad_capital = new Label() { Text = "", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#000000"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };                 StackLayout stackpromedio2 = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                    {
                        new Label() { Text = "Utilidad Sobre Capital", FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#686868"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) },
                        utilidad_capital
                    }
                };                             ConsultarP();                 form2.Children.Add(stackpromedio2);             
                StackLayout form02 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Spacing = 0 };
                form02.Children.Add(form2);
                Frame frame2 = new Frame() { Content = form02, Margin = new Thickness(0,10), BackgroundColor = Color.White, HasShadow = true, IsClippedToBounds = true, CornerRadius = 35, Padding = new Thickness(15) };
                stackcontenido.Children.Add(frame2);                       form3 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };

                utilidad_apuesta = new Label() { Text = "", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#000000"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };                 StackLayout stackpromedio3 = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                    {
                        new Label() { Text = "Utilidad Sobre Apuesta", FontSize = 14, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#686868"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) },
                        utilidad_apuesta
                    }
                };                             ConsultarP();                 form3.Children.Add(stackpromedio3);             
                StackLayout form03 = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Spacing = 0 };
                form03.Children.Add(form3);
                Frame frame3 = new Frame() { Content = form03, Margin = new Thickness(0,10), BackgroundColor = Color.White, HasShadow = true, IsClippedToBounds = true, CornerRadius = 35, Padding = new Thickness(15) };
                stackcontenido.Children.Add(frame3);                  stack1.Children.Add(scv1);                  //FONDOS                             Frame frame_back = new Frame() { BackgroundColor = Color.FromHex("#f2f2f2"), HasShadow = true, IsClippedToBounds = true, CornerRadius = 55, Padding = new Thickness(15), Margin = new Thickness(0, 160, 0, 0) };
                AbsoluteLayout.SetLayoutFlags(frame_back, AbsoluteLayoutFlags.All);                 AbsoluteLayout.SetLayoutBounds(frame_back, new Rectangle(0, 0, 1, 1));                 absoluteLayout.Children.Add(frame_back);
                AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);                 AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0, 1, 1));                 absoluteLayout.Children.Add(stack1);

                //MENU

                absoluteLayout.Children.Add(Home.cargar_menu_inferior());

                //ASIGNAR LAYOUT

                this.Content = absoluteLayout;
            }
            catch (Exception ex)
            {

            }
            
            
        }
        protected override void OnAppearing()
        {
            ConsultarP();
            base.OnAppearing();
        }

        

        public async void ConsultarP()
        {
            try
            {
                string uriString_aciertos = "http://toss.boveda-creativa.com/aciertos_personal.php?usuario=" + Settings.Idusuario;
                var aciertos = await httpRequest(uriString_aciertos);
                promedio.Text = aciertos + "%";
                string uriString_utcap = "http://toss.boveda-creativa.com/utilidad_capital_personal.php?usuario=" + Settings.Idusuario;
                var uti_cap = await httpRequest(uriString_utcap);
                utilidad_capital.Text = uti_cap + "%";
                string uriString_utap = "http://toss.boveda-creativa.com/utilidad_apuesta_personal.php?usuario=" + Settings.Idusuario;
                var utiap = await httpRequest(uriString_utap);
                utilidad_apuesta.Text = utiap + "%";
                string uriString2 = string.Format("http://toss.boveda-creativa.com/capital.php?usuario={0}", Settings.Idusuario);
                var response2 = await httpRequest(uriString2);
                capital.Text = "$ "+response2.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ayuda", ex.Message, "OK");
            }
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
            foto_perfil.Source = ImageSource.FromStream(() =>
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
                await Upload(file, Settings.Idusuario + "_foto_" + now1 + ".jpg");

                file.Dispose();
                var foto1 = "http://toss.boveda-creativa.com/upload/" + Settings.Idusuario + "_foto_" + now1 + ".jpg";
                foto1 = WebUtility.UrlEncode(foto1);
                try { enviar_perfil(foto1); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); }

            }
            catch (Exception ex)
            {

            }

        }

        public async void enviar_perfil(string foto)
        {
            string uriString1 = string.Format("http://toss.boveda-creativa.com/actualizar_perfil.php?usuario={0}&foto={1}", Settings.Idusuario, foto);
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
                    Settings.Foto = "http://toss.boveda-creativa.com/upload/"+filename;
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
                { using (var sr = new StreamReader(responseStream)) { received = await sr.ReadToEndAsync(); } }
            }
            return received;
        }
    }
}

