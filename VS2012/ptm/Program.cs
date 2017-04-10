using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;


namespace ptm
{
 
    
    class Program
    {
        static void Main(string[] args)
        {
            Temperature Temp = new Temperature();  
            
            GetInfo("Win32_Processor", "Name");

            GetInfo("Win32_VideoController", "Name");

            Console.WriteLine("Fan speed /min");
            GetInfo("Win32_Fan", "DesiredSpeed");//

            Console.WriteLine("Battery status:");//1 - discharging, 2-unknown(plugged),Fully Charged (3),
            //Low (4),Critical (5),Charging (6),Charging and High (7),
            //Charging and Low (8),Charging and Critical (9),Undefined (10),Partially Charged (11)
            GetInfo("Win32_Battery", "BatteryStatus");

            Console.WriteLine("Core temperature:"); 
            Console.WriteLine(Temperature.Temperatures[0].CurrentValue);

            Console.Read();

        }


        private static void GetInfo(String component, String info)
        {
            ManagementObjectSearcher obj = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM " + component);
            foreach (ManagementObject mj in obj.Get())
            {
                Console.WriteLine(Convert.ToString(mj[info]));
            }
        }

    }
    }
   
