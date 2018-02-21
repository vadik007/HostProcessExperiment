using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var guid = Guid.NewGuid();
            var mutex = new Mutex(true, guid.ToString());
            Console.WriteLine($"Opened mutex {guid} and wait.");
            Console.ReadLine();
        }
    }
}
