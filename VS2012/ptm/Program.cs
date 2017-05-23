using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;




namespace ptm
{
    public class PortChat
    {
        //static bool _continue;
        static SerialPort _serialPort;

        static void Main(string[] args)
        {
            
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;


            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            _serialPort.PortName = SetPortName();
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            //_continue = true;


           



            //Console.WriteLine("Type QUIT to exit");

            while (true) //(_continue)
            {
                //message = Console.ReadLine();
                Temperature Temp = new Temperature();
                message = GetInfo("Win32_Processor", "Name") + GetInfo("Win32_VideoController", "Name") + GetInfo("Win32_Fan", "DesiredSpeed") +
               GetInfo("Win32_Battery", "BatteryStatus") + ":" + Temperature.Temperatures[0].CurrentValue.ToString();
                
                _serialPort.WriteLine(message);
                Thread.Sleep(8000);

            }


            _serialPort.Close();
        }

        private static string GetInfo(String component, String info)
        {
            ManagementObjectSearcher obj = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM " + component);
            foreach (ManagementObject mj in obj.Get())
            {
                return ":"+Convert.ToString(mj[info]);
            }
            return ":"+component;
        }

        // Display Port values and prompt user to enter a port.
        public static string SetPortName()
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine(" {0}", s);
            }

            Console.Write("Enter COM port value : ");
            portName = Console.ReadLine();
            return portName;
        }



    }
}
