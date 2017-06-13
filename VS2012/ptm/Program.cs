using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;



namespace ptm
{
    public class PortChat
    {
                static SerialPort _serialPort;

        static void Main(string[] args)//
        {

            string message;

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            _serialPort.PortName = SetPortName();
            _serialPort.BaudRate = 115200;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            


           
            while (true) 
            {
                
                Temperature Temp = new Temperature();

                message = GetInfo("Win32_Processor", "Name");

                message += GetInfo("Win32_VideoController", "Name");

                message += GetInfo("Win32_OperatingSystem", "Name");

                message += GetInfo("Win32_OperatingSystem", "OSArchitecture");
                
                if(GetInfo("Win32_Battery", "BatteryStatus")==":1")
                    message += ":Bateria nie podłączona do ładowania";
                else if (GetInfo("Win32_Battery", "BatteryStatus") == ":2")
                    message += ":Bateria podłączona do ładowania";

                message += ":Temperatura procesora " + Temperature.Temperatures.Last().CurrentValue.ToString() + "°C";


                message += GetInfo("Win32_Processor", "CurrentClockSpeed")+"MHz";


                message +=":Pamięć karty graficznej "+GetInfoB("Win32_VideoController", "AdapterRAM").ToString()+"Mb";
                

                message +=":Wolna pamięć RAM "+GetInfoKB("Win32_OperatingSystem", "FreePhysicalMemory").ToString()+"Mb";
                

                System.String buf1="";
                System.String buf2 = getCPUCounter().ToString();
                buf1 += buf2[0];
                
                if (buf2[1] != ',')
                {
                    buf1 += buf2[1];
                    if (buf2[2] != ',' )
                        buf1 += buf2[2];
                }
                
                message += ":Zużycie procesora " + buf1 +"%";
                
                
                //send a message through serial port
                _serialPort.WriteLine(message);
                
             
  
                Thread.Sleep(8000);

            }


            _serialPort.Close();
        }


        private static int GetInfoKB(String component, String info)
        {
            ManagementObjectSearcher obj = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM " + component);
            foreach (ManagementObject mj in obj.Get())
            {
                return Convert.ToInt32(mj[info])/1024;
            }
            return 0;
        }

        private static long GetInfoB(String component, String info)
        {
            ManagementObjectSearcher obj = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM " + component);
            foreach (ManagementObject mj in obj.Get())
            {
                return Convert.ToInt64(mj[info])/1048576;
            }
            return 0;
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




        public static object getCPUCounter()
        {

            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            dynamic firstValue = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            dynamic secondValue = cpuCounter.NextValue()+1;

            return secondValue;

        }





    }
}
