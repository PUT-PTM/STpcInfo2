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

            string message="";
            
            
            int i=0;
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


                if (GetInfo("Win32_Processor", "Name") != ":Win32_Processor")
                    message = GetInfo("Win32_Processor", "Name");
                else message = "";

                

                if (GetInfo("Win32_VideoController", "Name") != ":Win32_VideoController")
                message += GetInfo("Win32_VideoController", "Name");

                if (GetInfo("Win32_OperatingSystem", "Name") != ":Win32_OperatingSystem")
                {
                    while (GetInfo("Win32_OperatingSystem", "Name")[i] != '|')
                    {
                        message += GetInfo("Win32_OperatingSystem", "Name")[i];
                        i++;
                    }
                    i = 0;
                }
                if (GetInfo("Win32_OperatingSystem", "OSArchitecture") != ":Win32_OperatingSystem")
                message += GetInfo("Win32_OperatingSystem", "OSArchitecture");

                if (GetInfo("Win32_Battery", "BatteryStatus") != ":Win32_Battery")
                {
                    if (GetInfo("Win32_Battery", "BatteryStatus") == ":1")
                        message += ":Bateria nie podlaczona do ladowania";
                    else if (GetInfo("Win32_Battery", "BatteryStatus") == ":2")
                        message += ":Bateria podlaczona do ladowania";
                }

                if(Temperature.Temperatures.Last().CurrentValue.ToString()!=null)
                message += ":Temperatura procesora " + Temperature.Temperatures.Last().CurrentValue.ToString() + "°C";
                

                if (GetInfo("Win32_Processor", "CurrentClockSpeed") != ":Win32_Processor")
                message += ":"+GetInfo("Win32_Processor", "CurrentClockSpeed")+"MHz";

                if (GetInfo("Win32_Fan", "DesiredSpeed") != ":Win32_Fan")
                message +=":"+ GetInfo("Win32_Fan", "DesiredSpeed") + " Revolution/min";

                if (GetInfoB("Win32_VideoController", "AdapterRAM").ToString() != ":Win32_VideoController")
                message +=":Pamiec karty graficznej "+GetInfoB("Win32_VideoController", "AdapterRAM").ToString()+"Mb";

                if (getRAMsize() != ":Win32_ComputerSystem")
                message += ":Pamiec Ram: "+ getRAMsize();

                if (GetInfoKB("Win32_OperatingSystem", "FreePhysicalMemory").ToString() != ":Win32_OperatingSystem")
                message +=":Wolna pamięć RAM "+GetInfoKB("Win32_OperatingSystem", "FreePhysicalMemory").ToString()+"Mb";
                

                System.String buf1="";
                System.String buf2 = getCPUCounter().ToString();
                buf1 += buf2[0];
                
                if(buf2[1]!=null)
                    if (buf2[1] != ',')
                {
                    buf1 += buf2[1];
                    if (buf2[2] != ',' )
                        buf1 += buf2[2];
                }
                
                message += ":Zuzycie procesora " + buf1 +"%";
                
                
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

        public static String getRAMsize()
        {
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject item in moc)
            {
                return Convert.ToString(Math.Round(Convert.ToDouble(item.Properties["TotalPhysicalMemory"].Value) / 1048576, 0)) + " MB";
            }

            return "RAMsize";
        }

      


    }
}
{
    public class PortChat
    {

        static SerialPort _serialPort;

        static void Main(string[] args)//
        {

            string message="";
            
            
            int i=0;
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


                if (GetInfo("Win32_Processor", "Name") != ":Win32_Processor")
                    message = GetInfo("Win32_Processor", "Name");
                else message = "";

                

                if (GetInfo("Win32_VideoController", "Name") != ":Win32_VideoController")
                message += GetInfo("Win32_VideoController", "Name");

                if (GetInfo("Win32_OperatingSystem", "Name") != ":Win32_OperatingSystem")
                {
                    while (GetInfo("Win32_OperatingSystem", "Name")[i] != '|')
                    {
                        message += GetInfo("Win32_OperatingSystem", "Name")[i];
                        i++;
                    }
                    i = 0;
                }
                if (GetInfo("Win32_OperatingSystem", "OSArchitecture") != ":Win32_OperatingSystem")
                message += GetInfo("Win32_OperatingSystem", "OSArchitecture");

                if (GetInfo("Win32_Battery", "BatteryStatus") != ":Win32_Battery")
                {
                    if (GetInfo("Win32_Battery", "BatteryStatus") == ":1")
                        message += ":Bateria nie podlaczona do ladowania";
                    else if (GetInfo("Win32_Battery", "BatteryStatus") == ":2")
                        message += ":Bateria podlaczona do ladowania";
                }

                if(Temperature.Temperatures.Last().CurrentValue.ToString()!=null)
                message += ":Temperatura procesora " + Temperature.Temperatures.Last().CurrentValue.ToString() + "°C";
                

                if (GetInfo("Win32_Processor", "CurrentClockSpeed") != ":Win32_Processor")
                message += ":"+GetInfo("Win32_Processor", "CurrentClockSpeed")+"MHz";

                if (GetInfo("Win32_Fan", "DesiredSpeed") != ":Win32_Fan")
                message +=":"+ GetInfo("Win32_Fan", "DesiredSpeed") + " Revolution/min";

                if (GetInfoB("Win32_VideoController", "AdapterRAM").ToString() != ":Win32_VideoController")
                message +=":Pamiec karty graficznej "+GetInfoB("Win32_VideoController", "AdapterRAM").ToString()+"Mb";

                if (getRAMsize() != ":Win32_ComputerSystem")
                message += ":Pamiec Ram: "+ getRAMsize();

                if (GetInfoKB("Win32_OperatingSystem", "FreePhysicalMemory").ToString() != ":Win32_OperatingSystem")
                message +=":Wolna pamięć RAM "+GetInfoKB("Win32_OperatingSystem", "FreePhysicalMemory").ToString()+"Mb";
                

                System.String buf1="";
                System.String buf2 = getCPUCounter().ToString();
                buf1 += buf2[0];
                
                if(buf2[1]!=null)
                    if (buf2[1] != ',')
                {
                    buf1 += buf2[1];
                    if (buf2[2] != ',' )
                        buf1 += buf2[2];
                }
                
                message += ":Zuzycie procesora " + buf1 +"%";
                
                
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

        public static String getRAMsize()
        {
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject item in moc)
            {
                return Convert.ToString(Math.Round(Convert.ToDouble(item.Properties["TotalPhysicalMemory"].Value) / 1048576, 0)) + " MB";
            }

            return "RAMsize";
        }

      


    }
}
