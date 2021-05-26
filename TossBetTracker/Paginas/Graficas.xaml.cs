using Microcharts;
using SkiaSharp;
using Microcharts.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.Entry;
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace TossBetTracker
{
    public partial class Graficas : ContentPage
    {
        string idperiodo = "";
        string iddeporte = "";
        List<Entry> entries_aciertos = new List<Entry> { };
        List<Entry> entries_utilidad_capital = new List<Entry> { };
        List<Entry> entries_utilidad_apuesta = new List<Entry> { };
        

        public Graficas(string periodo, string deporte)
        {
            InitializeComponent();
            Title = "Graficas";
            idperiodo = periodo;
            iddeporte = deporte;
            cargar_aciertos();
            cargar_ut_capital();
            cargar_ut_ap();
        }

        public async void cargar_aciertos()
        {
            try
            {
                string uriString2 = string.Format("http://ec2-18-212-22-223.compute-1.amazonaws.com/graf_aciertos.php?usuario={0}&periodo={1}&deporte={2}", Settings.Idusuario, idperiodo, iddeporte);
                var response2 = await httpRequest(uriString2);
                List<class_grafica> valor = new List<class_grafica>();
                valor = procesar(response2);
                for (int i = 0; i < valor.Count; i++)
                {
                    entries_aciertos.Add(new Entry(int.Parse(valor.ElementAt(i).datoy))
                    {
                        Color = SKColor.Parse("#35688d"),
                        Label = valor.ElementAt(i).datox,
                        ValueLabel = valor.ElementAt(i).datoy
                    });
                }
                Chart2.Chart = new LineChart() { Entries = entries_aciertos };
            }
            catch (Exception ex)
            {

            }
        }

        public async void cargar_ut_capital()
        {
            try
            {
                string uriString2 = string.Format("http://ec2-18-212-22-223.compute-1.amazonaws.com/graf_ut_capital.php?usuario={0}&periodo={1}&deporte={2}", Settings.Idusuario, idperiodo, iddeporte);
                var response2 = await httpRequest(uriString2);
                List<class_grafica> valor = new List<class_grafica>();
                valor = procesar(response2);
                for (int i = 0; i < valor.Count; i++)
                {
                    entries_utilidad_capital.Add(new Entry(int.Parse(valor.ElementAt(i).datoy))
                    {
                        Color = SKColor.Parse("#35688d"),
                        Label = valor.ElementAt(i).datox,
                        ValueLabel = valor.ElementAt(i).datoy
                    });
                }
                Chart4.Chart = new LineChart() { Entries = entries_utilidad_capital };
                //Chart4.Chart = new LineChart() { Entries = entries };
            }
            catch (Exception ex)
            {

            }
        }

        public async void cargar_ut_ap()
        {
            try
            {
                string uriString2 = string.Format("http://ec2-18-212-22-223.compute-1.amazonaws.com/graf_ut_apuestas.php?usuario={0}&periodo={1}&deporte={2}", Settings.Idusuario, idperiodo, iddeporte);
                var response2 = await httpRequest(uriString2);
                List<class_grafica> valor = new List<class_grafica>();
                valor = procesar(response2);
                for (int i = 0; i < valor.Count; i++)
                {
                    entries_utilidad_apuesta.Add(new Entry(int.Parse(valor.ElementAt(i).datoy))
                    {
                        Color = SKColor.Parse("#35688d"),
                        Label = valor.ElementAt(i).datox,
                        ValueLabel = valor.ElementAt(i).datoy
                    });
                }
                Chart5.Chart = new LineChart() { Entries = entries_utilidad_apuesta };
                //Chart5.Chart = new LineChart() { Entries = entries };
            }
            catch (Exception ex)
            {

            }
        }

        public List<class_grafica> procesar(string respuesta)
        {
            List<class_grafica> items = new List<class_grafica>();
            if (respuesta == "")
            { }
            else
            {
                var doc = XDocument.Parse(respuesta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("valor")
                             select new class_grafica
                             {
                                 datox = (string)r.Element("datox"),
                                 datoy = (string)r.Element("datoy")
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
