using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PoorLamport
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Mensaje> mensajesEsperaProceso1 = new ObservableCollection<Mensaje>();
        private ObservableCollection<Mensaje> mensajesEsperaProceso2 = new ObservableCollection<Mensaje>();
        private ObservableCollection<Mensaje> mensajesEsperaProceso3 = new ObservableCollection<Mensaje>();

        private Dictionary<int, List<Mensaje>> colaEspera = new Dictionary<int, List<Mensaje>>();

        public ObservableCollection<Mensaje> MensajesProceso1
        {
            get { return mensajesEsperaProceso1; }
        }

        public ObservableCollection<Mensaje> MensajesProceso2
        {
            get { return mensajesEsperaProceso2; }
        }

        public ObservableCollection<Mensaje> MensajesProceso3
        {
            get { return mensajesEsperaProceso3; }
        }

        List<Process> procesos;
        public MainWindow()
        {
            this.InitializeComponent();
            proceso1Mensajes.ItemsSource = MensajesProceso1;
            proceso2Mensajes.ItemsSource = MensajesProceso2;
            proceso3Mensajes.ItemsSource = MensajesProceso3;

            procesos = new List<Process>
            {
                new Process(1),
                new Process(2),
                new Process(3),
            };

            for (int i = 0; i < 3; i++)
            {
                colaEspera[i] = new List<Mensaje>();
            }
        }

        private void Recibir(int remitente, Mensaje mensaje, ObservableCollection<Mensaje> mensajes)
        {
            int tiempoMaximo = Math.Max(mensaje.Tiempo, procesos.ElementAt(remitente).Clock);
            int clock = tiempoMaximo + 1;
            procesos.ElementAt(remitente).Clock = tiempoMaximo;
            mensajes.Add(mensaje);

        }
        private void enviarMensaje1(object sender, RoutedEventArgs e)
        {
            int remitente = Int32.Parse(combobox1.Text);
            string mensaje = mensaje1.Text;
            int sentIndex = Int32.Parse(mensaje1Tiempo.Text) + 1;
            if (remitente == 2)
            {
                EnviarMensajeGeneral(remitente, 1, mensaje, MensajesProceso2, sentIndex);
            }
            else
            {
                if (remitente == 3)
                {
                    EnviarMensajeGeneral(remitente, 1, mensaje, MensajesProceso3, sentIndex);
                }
            }
            Actualizar();
        }
        private void enviarMensaje2(object sender, RoutedEventArgs e)
        {
            int remitente = Int32.Parse(combobox2.Text);
            string mensaje = mensaje2.Text;
            int sentIndex = Int32.Parse(mensaje2Tiempo.Text) + 1;
            if (remitente == 1)
            {
                EnviarMensajeGeneral(remitente, 2, mensaje, MensajesProceso1, sentIndex);
            }
            else
            {
                if (remitente == 3)
                {
                    EnviarMensajeGeneral(remitente, 2, mensaje, MensajesProceso3, sentIndex);
                }
            }
            Actualizar();
        }

        private void enviarMensaje3(object sender, RoutedEventArgs e)
        {
            int remitente = Int32.Parse(combobox3.Text);
            string mensaje = mensaje3.Text;
            int sentIndex = Int32.Parse(mensaje3Tiempo.Text) + 1;
            if (remitente == 1)
            {
                EnviarMensajeGeneral(remitente, 3, mensaje, MensajesProceso1, sentIndex);
            }
            else
            {
                if (remitente == 2)
                {
                    EnviarMensajeGeneral(remitente, 3, mensaje, MensajesProceso2, sentIndex);
                }
            }
            Actualizar();
        }


        public void EnviarMensajeGeneral(int remitente, int emisor, string mensaje, ObservableCollection<Mensaje> mensajes, int emisorNumero)
        {
            remitente--;
            emisor--;
            procesos.ElementAt(emisor).Clock = emisorNumero;

            Mensaje mensajesote = new Mensaje
            {
                Emisor = procesos.ElementAt(emisor),
                Remitente = procesos.ElementAt(remitente),
                Tiempo = procesos.ElementAt(emisor).Clock,
                Señal = mensaje
            };

            bool terminado = true;
            if (mensajesote.Tiempo <= procesos.ElementAt(remitente).Clock + 1 || mensajesote.Tiempo == 1)
            {
                Recibir(remitente, mensajesote, mensajes);
            }
            else
            {
                colaEspera[remitente].Add(mensajesote);
                terminado = false;
            }

            if (terminado)
            {
                if (colaEspera[remitente].Count > 0)
                {
                    do
                    {
                        foreach (Mensaje esperando in colaEspera[remitente])
                        {
                            if (esperando.Tiempo <= procesos.ElementAt(remitente).Clock + 1)
                            {
                                Recibir(remitente, esperando, mensajes);
                                colaEspera[remitente].Remove(esperando);
                                break;
                            }
                        }

                    } while (colaEspera[remitente].Count > 0);
                }
            }
        }
        private void Actualizar()
        {
            mensaje1Tiempo.Text = procesos[0].Clock.ToString();
            mensaje2Tiempo.Text = procesos[1].Clock.ToString();
            mensaje3Tiempo.Text = procesos[2].Clock.ToString();
        }
        private void ValidaCamposBox1(object sender, SelectionChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(mensaje1.Text) || String.IsNullOrEmpty(mensaje1Tiempo.Text) || combobox1.SelectedItem == null)
            {
                btn1.IsEnabled = false;
            }
            else
            {
                btn2.IsEnabled = true;
            }
        }

        private void ValidaCampos1(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(mensaje1.Text) || String.IsNullOrEmpty(mensaje1Tiempo.Text) || combobox1.SelectedItem == null)
            {
                btn1.IsEnabled = false;
            }
            else
            {
                btn1.IsEnabled = true;
            }
        }

        private void ValidaCamposBox2(object sender, SelectionChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(mensaje2.Text) || String.IsNullOrEmpty(mensaje2Tiempo.Text) || combobox2.SelectedItem == null)
            {
                btn2.IsEnabled = false;
            }
            else
            {
                btn2.IsEnabled = true;
            }
        }

        private void ValidaCampos2(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(mensaje2.Text) || String.IsNullOrEmpty(mensaje2Tiempo.Text) || combobox2.SelectedItem == null)
            {
                btn2.IsEnabled = false;
            }
            else
            {
                btn2.IsEnabled = true;
            }
        }

        private void ValidaCamposBox3(object sender, SelectionChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(mensaje3.Text) || String.IsNullOrEmpty(mensaje3Tiempo.Text) || combobox3.SelectedItem == null)
            {
                btn3.IsEnabled = false;
            }
            else
            {
                btn3.IsEnabled = true;
            }
        }

        private void ValidaCampos3(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(mensaje3.Text) || String.IsNullOrEmpty(mensaje3Tiempo.Text) || combobox3.SelectedItem == null)
            {
                btn3.IsEnabled = false;
            }
            else
            {
                btn3.IsEnabled = true;
            }
        }


    }
}
