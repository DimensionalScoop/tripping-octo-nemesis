using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSpaceShipSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Main();
            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                test.AddSubsystem(new Subsystem() { Priority = random.Next(10) });
            }
            //foreach (var item in test.Subsystems)
            //{
            //    Console.WriteLine(item.Priority);
            //}
        }
    }
}
