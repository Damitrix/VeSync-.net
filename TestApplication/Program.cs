using System;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            VeSync v = new VeSync("Seb@creativemedium.net","Jasper44!", true);
            Console.WriteLine("========={Devices}=========\n\n\n");
            var x = v.GetDevices();
            int c = 0;
            foreach(var i in x)
            {
                Console.WriteLine("{0} - {1}", c, i.Name);
                c++;
            }

            while (true)
            {
                Console.WriteLine("\n\nEnter a device ID or -1 to exit");
                int r = int.Parse(Console.ReadLine());
                if(r == -1) { break; }

                if(r > x.Length - 1) { continue; }
                Console.WriteLine("\n0 for off\n1 for on");
                int r1 = int.Parse(Console.ReadLine());
                
                switch (r1)
                {
                    case 0:
                        x[r].TurnOff();
                        break;
                    case 1:
                        x[r].TurnOn();
                        break;
                }
            }

            Console.WriteLine("\n\nPress Enter to Exit...");
            Console.ReadLine();
        }
    }
}
