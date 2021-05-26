using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Acr.UserDialogs;
using ImageCircle.Forms.Plugin.Abstractions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;

namespace TossBetTracker
{
	public class Resetear : ContentPage
	{
		StackLayout stack1 = null;
		StackLayout pestanas = null;
		StackLayout modal = null;
		StackLayout stackboleto = null;
		AbsoluteLayout absoluteLayout = null;
		Entry entrycontrasena = null;

		public Resetear()
		{

			BackgroundColor = Color.White;
			Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
			Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
			StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
			NavigationPage.SetTitleView(this, stacknav);
			absoluteLayout = new AbsoluteLayout();
			stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 40, 20, 20), Spacing = 0 };
			ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack1 };
			//GENERALES


			AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0, 1, 1));
			absoluteLayout.Children.Add(scv1);

			absoluteLayout.Children.Add(Home.cargar_menu_inferior());

			//ASIGNAR LAYOUT

			this.Content = absoluteLayout;
			ConsultarP();
		}

		public async void ConsultarP()
		{
			try
			{
				Label Titulo = new Label() { Text = "Restaurar", FontSize = 26, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
				stack1.Children.Add(Titulo);
				Label contra = new Label() { Margin = new Thickness(20, 50, 20, 20), TextColor = Color.FromHex("#01528a"), FontSize = 18, Text = "Ingresa tu contraseña para confirmar:" };
				stack1.Children.Add(contra);
				entrycontrasena = new Entry() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, FontSize = 18 };
				stack1.Children.Add(entrycontrasena);
				Label config1 = new Label() { Margin = new Thickness(20, 60, 20, 10), TextColor = Color.FromHex("#c60b0b"), FontSize = 22, Text = "Alerta:", FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
				stack1.Children.Add(config1);
				Label config2 = new Label() { Margin = new Thickness(20, 10, 20, 30), TextColor = Color.FromHex("#c60b0b"), FontSize = 18, Text = "Al restaurar la cuenta, todas las apuestas y movimientos serán borrados, esta acción no se puede restaurar una vez realizada." };
				stack1.Children.Add(config2);
				Button botonresetear = new Button() { HorizontalOptions =  LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.End, Padding = new Thickness(30,20), FontSize = 18, TextColor = Color.White, BackgroundColor = Color.FromHex("#01528a"), CornerRadius = 10, Text = "Resetear" };
                botonresetear.Clicked += Botonresetear_Clicked;
				stack1.Children.Add(botonresetear);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Ayuda", ex.Message, "OK");
			}
		}

        private async void Botonresetear_Clicked(object sender, EventArgs e)
        {
            try
            {
				string uriString2 = string.Format("http://toss.boveda-creativa.com/validar.php?usuario={0}&password={1}", Settings.Idusuario, entrycontrasena.Text);
				var response2 = await httpRequest(uriString2);
				List<class_usuarios> valor = new List<class_usuarios>();
				valor = procesar2(response2);
				if (valor.Count == 0)
				{
					await DisplayAlert("Ayuda", "Contraseña incorrecta", "OK");
				}
				for (int i = 0; i < valor.Count(); i++)
				{
					if (int.Parse(valor.ElementAt(i).idusuario) > 0)
					{
						var confirmResult = await UserDialogs.Instance.ConfirmAsync("¿Estas seguro que deseas resetear tu cuenta?", "Alerta", "Resetear", "Cancelar");
						if (confirmResult)
						{
							string uriString3 = "http://toss.boveda-creativa.com/resetear.php?usuario=" + Settings.Idusuario + "&contrasena=" + entrycontrasena.Text;
							var response3 = await httpRequest(uriString3);
							await DisplayAlert("Confirmación", "Reseteo completado correctamente.", "OK");
						}
					}
					else
					{
						await DisplayAlert("Ayuda", "Contraseña incorrecta", "OK");
					}
				}
            }
            catch (Exception ex)
            {

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
								 notif = (string)r.Element("notif"),
								 foto = WebUtility.UrlDecode((string)r.Element("foto")),
								 telefono = (string)r.Element("telefono"),
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
				{ using (var sr = new StreamReader(responseStream)) { received = await sr.ReadToEndAsync(); } }
			}
			return received;
		}
	}
}

