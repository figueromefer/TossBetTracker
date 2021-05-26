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
using Acr.UserDialogs;

namespace TossBetTracker
{
    public class NGrupo : ContentPage
    {
        StackLayout stack1 = null;
        Entry e_titulo, e_descripcion = null;
        StackLayout stack_form = null;
        CircleImage fotogrupo = null;
        AbsoluteLayout absoluteLayout = null;
        MediaFile file = null;
        Stream stream = null;
        string foto = "";

        public NGrupo()
        {
            BackgroundColor = Color.FromHex("#01528a");
            Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
            Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
            StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
            NavigationPage.SetTitleView(this, stacknav);

            absoluteLayout = new AbsoluteLayout();
            stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(10, 0, 10, 10), Spacing = 0 };
            Label Titulo = new Label() { Text = "Nuevo Grupo", FontSize = 26, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
            
            StackLayout stacksuperior = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { Titulo }, Padding = new Thickness(0, 0, 0, 15) };
            stack1.Children.Add(stacksuperior);

            stack_form = new StackLayout() { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.StartAndExpand, Padding = new Thickness(20, 20, 20, 20), Spacing = 0 };
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack_form };
            Frame framegrupos = new Frame() { IsClippedToBounds = true, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, CornerRadius = 25, BackgroundColor = Color.White, Content = scv1, Padding = new Thickness(0) };
            stack1.Children.Add(framegrupos);

            //GENERALES
            AbsoluteLayout.SetLayoutFlags(stack1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(stack1, new Rectangle(0, 0.90, 1, 0.93));             absoluteLayout.Children.Add(stack1);

            //MENU INFERIOR
            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

            //ASIGNAR LAYOUT
            this.Content = absoluteLayout;
            cargar_formulario();
        }

        public async void cargar_formulario()
        {
            try
            {
                fotogrupo = new CircleImage
                {
                    BorderColor = Color.White,
                    BorderThickness = 3,
                    HeightRequest = 140,
                    WidthRequest = 140,
                    Margin = new Thickness(0, 30, 0, 30),
                    Aspect = Aspect.AspectFill,
                    HorizontalOptions = LayoutOptions.Center,
                    Source = ImageSource.FromFile("profile.png")
                };
                fotogrupo.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { try { Tomar_foto_perfil(); } catch (Exception ex) { DisplayAlert("Help", ex.Message, "OK"); } }), NumberOfTapsRequired = 1 });

                Label lbltitulo = new Label() { Text = "Título", FontSize = 12, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#000000"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                e_titulo = new Entry() { HorizontalOptions = LayoutOptions.FillAndExpand };
                Label lbldescripcion = new Label() { Text = "Descripción", FontSize = 12, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#000000"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
                e_descripcion = new Entry() { HorizontalOptions = LayoutOptions.FillAndExpand };

                Button Crear = new Button() { Margin = new Thickness(0,20,0,0),Text = "Siguiente", MinimumHeightRequest = 30, WidthRequest = 200, HorizontalOptions = LayoutOptions.Center, BorderRadius = 16, BackgroundColor = Color.FromHex("#2DC9EB"), TextColor = Color.FromHex("#ffffff"), FontSize = 20, FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                Crear.Clicked += CrearClicked;
                stack_form.Children.Add(fotogrupo);
                stack_form.Children.Add(lbltitulo);
                stack_form.Children.Add(e_titulo);
                stack_form.Children.Add(lbldescripcion);
                stack_form.Children.Add(e_descripcion);
                stack_form.Children.Add(Crear);
            }
            catch (Exception ex)
            {

            }
            
        }

        

        public async void CrearClicked(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Creando grupo …");
                string uriString2 = string.Format("http://toss.boveda-creativa.com/nuevo_grupo.php?usuario={0}&titulo={1}&descripcion={2}&autor{3}&foto={4}", Settings.Idusuario, e_titulo.Text, e_descripcion.Text, Settings.Idusuario, foto);
                var response2 = await httpRequest(uriString2);
                if (response2.ToString() != "")
                {
                    UserDialogs.Instance.HideLoading();
                    await Navigation.PushAsync(new NGrupo2(response2.ToString()));
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await DisplayAlert("Error", "Se presentó un error al crear el grupo", "OK");
                }
                
                
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", ex.Message, "OK");
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

        }

        public async void subir_perfil()
        {
            try
            {
                string now1 = DateTime.Now.ToString().Replace(' ', '_').Replace('/', '_').Replace(':', '_');
                if (file == null) { await DisplayAlert("Alerta", "Falta definir una foto.", "OK"); }
                await Upload(file, "_fotogrupo_" + now1 + ".jpg");

                var foto1 = "http://toss.boveda-creativa.com/upload/_fotogrupo_" + now1 + ".jpg";
                foto1 = WebUtility.UrlEncode(foto1);
                foto = foto1;
                
                fotogrupo.Source = ImageSource.FromStream(() =>
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

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
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

