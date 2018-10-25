using System;
using System.Threading;
using System.Threading.Tasks;

namespace Amaris.ETL.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            
            Console.WriteLine("Launching the ETL");
            var task = Task.Run(() => Logic.RunETL(cancellationTokenSource.Token, true));

            Console.ReadKey();

            Console.WriteLine("Requested to stop the ETL");
            cancellationTokenSource.Cancel();
            task.Wait(1000);
            Console.WriteLine("ETL stopped");
        }
    }
}