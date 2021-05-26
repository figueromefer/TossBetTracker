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
	public class Configuraciones : ContentPage
	{
		StackLayout stack1 = null;
		StackLayout pestanas = null;
		StackLayout modal = null;
		StackLayout stackboleto = null;
		AbsoluteLayout absoluteLayout = null;


		public Configuraciones()
		{

			BackgroundColor = Color.White;
			//NavigationPage.SetTitleIconImageSource(this, "logo2.png");
			Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
			Label Titulo0 = new Label() { Text = "Configuraciones", WidthRequest = 100, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
			Image imagenchat = new Image() { IsVisible = false, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
			StackLayout stacknav = new StackLayout() { Children = { Titulo0, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
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
				Label Titulo = new Label() { Text = "Configuraciones", FontSize = 26, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
				stack1.Children.Add(Titulo);

                #region opcionesmomios
                Label lblamericana = new Label() { LineBreakMode = LineBreakMode.NoWrap, Text = "Ame.", TextColor = Color.White, FontSize = 12, HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, WidthRequest = 40 };
				Frame frame_americana = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.FromHex("#01528a"), Padding = new Thickness(10,3), Content = lblamericana };
				frame_americana.GestureRecognizers.Add(new TapGestureRecognizer
				{
					Command = new Command(() => {
						try
						{
							Settings.Momio = "";

							frame_americana.BackgroundColor = Color.FromHex("#01528a");
							Label lblinterna0 = (Label)frame_americana.Content;
							lblinterna0.TextColor = Color.White;

							StackLayout stackpadre = (StackLayout)frame_americana.Parent;
							Frame framecontrario = (Frame)stackpadre.Children[2];
							framecontrario.BackgroundColor = Color.White;
							Label lblinterna = (Label)framecontrario.Content;
							lblinterna.TextColor = Color.FromHex("#01528a");
						}
						catch (Exception ex)
						{
							Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
						}
					}),
					NumberOfTapsRequired = 1
				});
				Label lbldecimal = new Label() { LineBreakMode = LineBreakMode.NoWrap, Text = "Dec.", TextColor = Color.FromHex("#01528a"), FontSize = 12, HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, WidthRequest = 40 };
				Frame frame_decimal = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.White, Padding = new Thickness(10, 3), Content = lbldecimal};
				frame_decimal.GestureRecognizers.Add(new TapGestureRecognizer
				{
					Command = new Command(() => {
						try
						{
							Settings.Momio = "Decimal";

							frame_decimal.BackgroundColor = Color.FromHex("#01528a");
							Label lblinterna0 = (Label)frame_decimal.Content;
							lblinterna0.TextColor = Color.White;

							StackLayout stackpadre = (StackLayout)frame_decimal.Parent;
							Frame framecontrario = (Frame)stackpadre.Children[1];
							framecontrario.BackgroundColor = Color.White;
							Label lblinterna = (Label)framecontrario.Content;
							lblinterna.TextColor = Color.FromHex("#01528a");
						}
						catch (Exception ex)
						{
							Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
						}
					}),
					NumberOfTapsRequired = 1
				});
                if(Settings.Momio != "")
                {
					frame_americana.BackgroundColor = Color.White;
                    lblamericana.TextColor = Color.FromHex("#01528a");
					frame_decimal.BackgroundColor = Color.FromHex("#01528a");
					lbldecimal.TextColor = Color.White;
				}
				#endregion
				Label config0 = new Label() { TextColor = Color.FromHex("#01528a"), FontSize = 18, Text = "Formato de momios: ", HorizontalOptions = LayoutOptions.FillAndExpand };
				StackLayout stackmomios = new StackLayout() { Margin = new Thickness(20, 50, 10, 30), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Children = { config0,frame_americana, frame_decimal } };
				stack1.Children.Add(stackmomios);

				#region opciones_notif
				Label lblon = new Label() { LineBreakMode = LineBreakMode.NoWrap, Text = "SI", TextColor = Color.White, FontSize = 12, HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, WidthRequest = 40 };
				Frame frame_on = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.FromHex("#01528a"), Padding = new Thickness(10, 3), Content = lblon};
				frame_on.GestureRecognizers.Add(new TapGestureRecognizer
				{
					Command = new Command(async () => {
						try
						{
							Settings.Notif = "";
							string uriString4 = string.Format("http://toss.boveda-creativa.com/setnotif.php?id={0}&notif={1}", Settings.Idusuario, Settings.Notif);
							var response4 = await httpRequest(uriString4);
							frame_on.BackgroundColor = Color.FromHex("#01528a");
							Label lblinterna0 = (Label)frame_on.Content;
							lblinterna0.TextColor = Color.White;

							StackLayout stackpadre = (StackLayout)frame_on.Parent;
							Frame framecontrario = (Frame)stackpadre.Children[2];
							framecontrario.BackgroundColor = Color.White;
							Label lblinterna = (Label)framecontrario.Content;
							lblinterna.TextColor = Color.FromHex("#01528a");
						}
						catch (Exception ex)
						{
							Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
						}
					}),
					NumberOfTapsRequired = 1
				});
				Label lbloff = new Label() { LineBreakMode = LineBreakMode.NoWrap, Text = "NO", TextColor = Color.FromHex("#01528a"), FontSize = 12, HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Center, WidthRequest = 40 };
				Frame frame_off = new Frame() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, BackgroundColor = Color.White, Padding = new Thickness(10, 3), Content = lbloff};
				frame_off.GestureRecognizers.Add(new TapGestureRecognizer
				{
					Command = new Command(async () => {
						try
						{
							Settings.Notif = "-1";
							string uriString4 = string.Format("http://toss.boveda-creativa.com/setnotif.php?id={0}&notif={1}", Settings.Idusuario, Settings.Notif);
							var response4 = await httpRequest(uriString4);

							frame_off.BackgroundColor = Color.FromHex("#01528a");
							Label lblinterna0 = (Label)frame_off.Content;
							lblinterna0.TextColor = Color.White;

							StackLayout stackpadre = (StackLayout)frame_off.Parent;
							Frame framecontrario = (Frame)stackpadre.Children[1];
							framecontrario.BackgroundColor = Color.White;
							Label lblinterna = (Label)framecontrario.Content;
							lblinterna.TextColor = Color.FromHex("#01528a");
						}
						catch (Exception ex)
						{
							Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
						}
					}),
					NumberOfTapsRequired = 1
				});
				if (Settings.Notif == "-1")
				{
					frame_on.BackgroundColor = Color.White;
					lblon.TextColor = Color.FromHex("#01528a");
					frame_off.BackgroundColor = Color.FromHex("#01528a");
					lbloff.TextColor = Color.White;
				}
				#endregion

				Label config1 = new Label() { TextColor = Color.FromHex("#01528a"), FontSize = 18, Text = "Notificaciones", HorizontalOptions = LayoutOptions.FillAndExpand };
				StackLayout stacknotificaciones = new StackLayout() { Margin = new Thickness(20, 0, 10, 30), Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, Children = { config1, frame_on, frame_off } };
				stack1.Children.Add(stacknotificaciones);



				Label config2 = new Label() { Margin = new Thickness(20, 0, 20, 30), TextColor = Color.FromHex("#01528a"), FontSize = 18, Text = "Resetear cuenta" };
				stack1.Children.Add(config2);
				config2.GestureRecognizers.Add(new TapGestureRecognizer
				{
					Command = new Command(async() => {
						try
						{
							
								await Navigation.PushAsync(new Resetear());
							
						}
						catch (Exception ex)
						{
							 await Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
						}
					}),
					NumberOfTapsRequired = 1
				});
				Label config3 = new Label() { Margin = new Thickness(20, 0, 20, 30), TextColor = Color.FromHex("#01528a"), FontSize = 18, Text = "Cerrar sesión" };
                stack1.Children.Add(config3);
				config3.GestureRecognizers.Add(new TapGestureRecognizer
				{
					Command = new Command(async() => {
						try
						{
			                var res = await DisplayAlert("Opciones", "¿Deseas cerrar sesión?", "Cerrar sesión", "Cancelar");
			                if (res)
			                {
				                await Navigation.PushAsync(new Cerrar());
			                }
						}
						catch (Exception ex)
						{
							await Application.Current.MainPage.DisplayAlert("Ayuda", ex.Message, "OK");
						}
					}),
					NumberOfTapsRequired = 1
				});
			}
			catch (Exception ex)
			{
				await DisplayAlert("Ayuda", ex.Message, "OK");
			}
		}

		public async void cerrar()
		{
			try
			{
				var answer = await DisplayAlert("CERRAR SESIÓN", "¿Deseas cerrar sesión?", "SI", "NO");
				if (answer == true)
				{
					await Navigation.PushAsync(new Logout());
				}
			}
			catch (Exception ex)
			{

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
				{ using (var sr = new StreamReader(responseStream)) { received = await sr.ReadToEndAsync(); } }
			}
			return received;
		}
	}
}

