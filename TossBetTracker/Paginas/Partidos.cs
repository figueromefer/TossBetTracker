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
	public class Partidos : ContentPage
	{
		StackLayout stack1 = null;
		StackLayout pestanas = null;
		StackLayout modal = null;
		StackLayout stackboleto = null;
        AbsoluteLayout absoluteLayout = new AbsoluteLayout();
        Label Titulo = null;
        Image icono_deporte = null;
        string titulo_deporte = "";
        string foto_deporte = "";

        public Partidos(string deporte)
		{
            Settings.Deporte = deporte.Split('|')[0];
            titulo_deporte = deporte.Split('|')[1];
			string foto = deporte.Split('|')[1].ToUpper();

			
			if(foto == "SOCCER")
            {
				foto = "FIFA";
            }
			if (foto == "LMP")
			{
				foto = "MLB";
			}
			if (foto == "LMB")
			{
				foto = "MLB";
			}
			foto_deporte = foto + ".png";
			foto = foto.ToUpper();
			Image imagennav = new Image() { Source = ImageSource.FromFile("logo2.png"), WidthRequest = 100, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(25, 0, 0, 0) };
			Image imagenchat = new Image() { Opacity = 0, Source = ImageSource.FromFile("chats.png"), WidthRequest = 25, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 10, 0) };
			StackLayout stacknav = new StackLayout() { Children = { imagennav, imagenchat }, Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.Center, Spacing = 10 };
			NavigationPage.SetTitleView(this, stacknav);
			BackgroundColor = Color.White;

            stack1 = new StackLayout() { BackgroundColor = Color.Transparent, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(20, 40, 20, 20), Spacing = 0 };
            ScrollView scv1 = new ScrollView() { Orientation = ScrollOrientation.Vertical, Content = stack1, Padding=new Thickness(0,0,0,30) };

            //GENERALES
            AbsoluteLayout.SetLayoutFlags(scv1, AbsoluteLayoutFlags.All);             AbsoluteLayout.SetLayoutBounds(scv1, new Rectangle(0, 0, 1, 1));             absoluteLayout.Children.Add(scv1);

            absoluteLayout.Children.Add(Home.cargar_menu_inferior());

			//ASIGNAR LAYOUT

			this.Content = absoluteLayout;
            cargar_partidos();
			cargar_intro();
		}

		public async void cargar_intro()
		{
			try
			{
				if (!Settings.Primera.Contains("|PARTIDOS|"))
				{

					Label Titulointro = new Label() { Text = "Partidos", Margin = new Thickness(0, 0, 0, 30), FontSize = 30, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
					Label Contenidointro = new Label() { Text = "Selecciona un partido para registrar tu apuesta.", Margin = new Thickness(0, 0, 0, 0), FontSize = 22, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Regular", "Roboto-Regular.ttf#Roboto-Regular", null) };
					StackLayout stackcontenido = new StackLayout() { Children = { Titulointro, Contenidointro }, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, };
					StackLayout intro1 = new StackLayout() { Opacity = 0, Children = { stackcontenido }, BackgroundColor = Color.FromRgba(0, 0, 0, 0.8), VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = new Thickness(30, 50) };
					intro1.GestureRecognizers.Add(new TapGestureRecognizer
					{
						Command = new Command(() => {
							try
							{
								absoluteLayout.Children.Remove(intro1);
								Settings.Primera = Settings.Primera + "|PARTIDOS|";
							}
							catch (Exception ex)
							{
								Application.Current.MainPage.DisplayAlert("Ayuda5", ex.Message, "OK");
							}
						}),
						NumberOfTapsRequired = 1
					});
					AbsoluteLayout.SetLayoutFlags(intro1, AbsoluteLayoutFlags.All);
					AbsoluteLayout.SetLayoutBounds(intro1, new Rectangle(0, 0, 1, 1));
					absoluteLayout.Children.Add(intro1);
					await intro1.FadeTo(1, 700, Easing.Linear);
					await intro1.ScaleTo(1.1, 300);
					await intro1.ScaleTo(1, 300);
				}
			}
			catch (Exception ex)
			{

			}
		}

		public async void cargar_partidos()
		{
			try
			{
                Titulo = new Label() { Text = titulo_deporte, FontSize = 26, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
                icono_deporte = new Image() { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, WidthRequest = 55, Source = ImageSource.FromFile(foto_deporte) };
                StackLayout titulo_logo = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 4, Children = { icono_deporte, Titulo } };
                stack1.Children.Add(titulo_logo);
                string uriString2 = "http://toss.boveda-creativa.com/partidos.php?deporte=" + Settings.Deporte;
                UserDialogs.Instance.ShowLoading("Cargando partidos …");
				var response2 = await httpRequest(uriString2);
				List<class_partidos> valor = new List<class_partidos>();
				valor = procesar(response2);

				for (int j = 0; j < valor.Count(); j++)
				{
                    string partido1 = valor.ElementAt(j).HomeTeam + " vs " + valor.ElementAt(j).AwayTeam;
					Label vs = new Label() { Text = partido1, FontSize = 16, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
					string[] meses = {"","Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"};
					string fecha0 = valor.ElementAt(j).MatchTime.Split(' ')[0];
					fecha0 = fecha0.Split('-')[2] + "/" + meses[int.Parse(fecha0.Split('-')[1])] + "/" + fecha0.Split('-')[0];
					string hora = valor.ElementAt(j).MatchTime.Split(' ')[1];
					hora = hora.Split(':')[0] + ":" + hora.Split(':')[1] + " hrs";
					string fechastring = fecha0 + "\n" + hora;
					Label fecha = new Label() { Text = fechastring, FontSize = 10, HorizontalTextAlignment = TextAlignment.Start, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
					
					//MOMIOS
					Label momio1 = new Label() { Text = "VER APUESTAS", FontSize = 10, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#FFFFFF"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null) };
					Frame vermas = new Frame() { Padding = new Thickness(10,5), BackgroundColor = Color.FromHex("#2DC9EB"), Visual = VisualMarker.Material, HasShadow = true, ClassId = valor.ElementAt(j).MatchId, Content = momio1, HorizontalOptions = LayoutOptions.End, CornerRadius = 10};
					vermas.GestureRecognizers.Add(new TapGestureRecognizer
					{
						Command = new Command(() => {
							try
							{
								Settings.Partido = vermas.ClassId;
								Navigation.PushAsync(new Detalle_partido(titulo_deporte));
							}
							catch (Exception ex)
							{
								DisplayAlert("Ayuda", ex.Message, "OK");
							}
						}),
						NumberOfTapsRequired = 1
					});
                    
                    StackLayout cols_partido = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Children = { fecha, vermas} };
					StackLayout stack_partido = new StackLayout()
					{
						HorizontalOptions = LayoutOptions.FillAndExpand,
						VerticalOptions = LayoutOptions.Start,
						Padding = new Thickness(10, 5, 10, 5),
						Children =
						{
                            vs,
                            cols_partido
                        }
					};
                    Image moneda = new Image
                    {
                        WidthRequest = 30,
                        Aspect = Aspect.AspectFit,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center,
                        Source = ImageSource.FromFile(foto_deporte)
                    };
                    StackLayout cols_gen = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { moneda, stack_partido } };
                    Frame frameapuesta = new Frame() {Visual = VisualMarker.Material, Margin = new Thickness(0, 0, 0, 20), HasShadow = true, IsClippedToBounds = true, Padding = new Thickness(15, 5), Content = cols_gen };
                    stack1.Children.Add(frameapuesta);
                }
                if(valor.Count() == 0)
                {
					Label Titulonopartidos = new Label() { Text = "No hay partidos cercanos a la fecha actual disponibles. ", FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Start, TextColor = Color.FromHex("#01528a"), FontFamily = Device.OnPlatform("Roboto-Bold", "Roboto-Bold.ttf#Roboto-Bold", null), Margin = new Thickness(0,50,0,0) };
					stack1.Children.Add(Titulonopartidos);
                }
				UserDialogs.Instance.HideLoading();
			}
			catch (Exception ex)
			{
				string error = ex.Message;
				error = "";
				UserDialogs.Instance.HideLoading();
			}
		}

		public List<class_deportes> procesar2(string respuesta)
		{
			List<class_deportes> items = new List<class_deportes>();
			if (respuesta == "")
			{ }
			else
			{
				var doc = XDocument.Parse(respuesta);
				if (doc.Root != null)
				{
					items = (from r in doc.Root.Elements("valor")
							 select new class_deportes
							 {
								 id = (string)r.Element("id"),
								 activo = WebUtility.UrlDecode((string)r.Element("activo")),
								 Name = WebUtility.UrlDecode((string)r.Element("Name")),
								 SportId = WebUtility.UrlDecode((string)r.Element("SportId"))
							 }).ToList();
				}
			}
			return items;
		}

		public List<class_partidos> procesar(string respuesta)
		{
			List<class_partidos> items = new List<class_partidos>();
			if (respuesta == "")
			{ }
			else
			{
				var doc = XDocument.Parse(respuesta);
				if (doc.Root != null)
				{
					items = (from r in doc.Root.Elements("valor")
							 select new class_partidos
							 {
								 id = (string)r.Element("id"),
								 MatchId = WebUtility.UrlDecode((string)r.Element("MatchId")),
								 HomeTeam = WebUtility.UrlDecode((string)r.Element("HomeTeam")),
								 AwayTeam = WebUtility.UrlDecode((string)r.Element("AwayTeam")),
								 MatchTime = WebUtility.UrlDecode((string)r.Element("MatchTime"))
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

