using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerProcess
{
    class Program
    {
        private static CancellationTokenSource cts = new CancellationTokenSource();

        static void Main(string[] args)
        {
            Console.WriteLine("Enter mutex id");
            var readLine = Console.ReadLine();
            var mutexAquired = Mutex.TryOpenExisting(readLine, out var mutex);
            Task workTask;
            if (mutexAquired)
            {
                 workTask = Task.Run(() =>
                {
                    while (true)
                    {
                        Console.WriteLine("Working");
                        Thread.Sleep(1000);

                        if (cts.Token.IsCancellationRequested)
                        {
                            cts.Token.ThrowIfCancellationRequested();
                        }
                    }
                }, cts.Token).ContinueWith(task=> Console.WriteLine("All work is done"),TaskContinuationOptions.None);

                try
                {
                    mutex.WaitOne();
                }
                catch (AbandonedMutexException e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Kill switch flipped! Host process exited. ");
                }

                cts.Cancel();
                Console.WriteLine("Cancelation");
                workTask.Wait();
            }
            else
            {
                Console.WriteLine("Can't enter mutex lock");
            }

            Console.WriteLine("WorkerProcess exiting");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
