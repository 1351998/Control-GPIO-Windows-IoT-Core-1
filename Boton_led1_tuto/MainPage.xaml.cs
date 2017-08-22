//---------------------------------------------------
//Led y boton fisco y virtual Windows IoT Core, este proyecto fue compilado para Aniversary Edition
// Alejandro Hazael San Román Plaza
//El siguiente codigo es representativo, auque el mismo si puede ser aplicado en escenarios reales,
//es necesario realizar ajustes en los metodos y el estilo de programación
//---------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;


namespace Boton_led1_tuto
{
    public sealed partial class MainPage : Page
    {
        private const int PIN_LED = 10; //Variable usada para identificar el pin del led
        private GpioPin Led1; //En este apartado se declara que la variable LED1 sera un puerto GPIO fisico 
        private const int PIN_BOTON1 = 11;//Pin del botón
        private GpioPin boton1; //Boton 1 es GPIO
        private GpioPinValue valor_led = GpioPinValue.Low;
        private SolidColorBrush Rojo = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush Gris = new SolidColorBrush(Windows.UI.Colors.Gray);
        public MainPage()
        {
            this.InitializeComponent();
            configuracion_gpio();//Funcion de configuracion de pines en MAIN
        }

        private void configuracion_gpio() //Funcion que servira para configurar puertos E/S
        {
            var gpio = GpioController.GetDefault(); //Variable del controlador del GPIO

            if (gpio == null)//Aqui se verifica si hay puertos GPIO disponibler por medio del controlador
            {

            }
            Led1 = gpio.OpenPin(PIN_LED);//Aqui asociamos nuestro puerto GPIO con el numero de puerto es decir LED1 = Puerto 10
            Led1.SetDriveMode(GpioPinDriveMode.Output);// Se asocia nuestro elemento led como puerto de salida
            boton1 = gpio.OpenPin(PIN_BOTON1);//Configuramos el puerto mediante el controlador creado anteriormenteel cual sera asignado al pin 11
            boton1.SetDriveMode(GpioPinDriveMode.Input); //Se configura el boton como un entrada, para este ejemplo no verificaremos pull up
            boton1.DebounceTimeout = TimeSpan.FromMilliseconds(50);//Tiempo para contar la pulsación como valida y evitar falsos positivos
            boton1.ValueChanged += boton1_ValueChanged; //Cuando se detecte cambios en el valor del GPIO, ejecutar la función encargada del cambio de estado del puerto

        }

        private void boton1_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)//Funcion que verifica el estado en boton es decir presionado o no
        {
            if(e.Edge == GpioPinEdge.FallingEdge)
            {
                valor_led = (valor_led == GpioPinValue.Low) ? //Si el valor del led es 0 lo colocara en 1 y viceversa
                GpioPinValue.High : GpioPinValue.Low;
                Led1.Write(valor_led);//Enciende el led
            }

            //Esta funcion es opcional y unicamente sirve para cuando este presionado el boton, nos muestre el estado del led
            //El metodo se creo como un procedimiento asincrono, ya que se ejecuta a la par de la funcion cambio del estado del boton
            //los colores se tienen que declarar como recursos
            var task = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
             {
                 if(e.Edge == GpioPinEdge.FallingEdge){ //Verifica el valor del led para colocar el color segun corresponda
                     led_virtual.Fill = (valor_led == GpioPinValue.Low) ?
                         Rojo : Gris;
                    }
             });
        }

        private void btn_encenderled_Click(object sender, RoutedEventArgs e)
        {
            valor_led = GpioPinValue.High;//Colocamos el valor del led en alto
            Led1.Write(valor_led);//Se enciende el led
        }

        private void btn_apagarled_Click(object sender, RoutedEventArgs e)
        {
            valor_led = GpioPinValue.Low;//Se establece el valor del led en 0 en su variable
            Led1.Write(valor_led);//Se escribe un 0 en el puerto GPIO del led
        }


        

    }
}
