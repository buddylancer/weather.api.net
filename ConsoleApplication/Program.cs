using System;

namespace ConsoleApplication
{
	class Program
	{
        /// <summary>
        /// Console application for testing Web API service.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
		{
            // Loop until just press Enter
            while (true)
            {
                System.Console.Write("Type country,zipcode (for example RU,603000): ");
                string line = System.Console.ReadLine();
                if (line.Trim().Length == 0)
                    break;

                // Get response from Web API service...
                var response = Controller.ExecuteRequest(line);
                // ...and print it
                System.Console.WriteLine(response);
            }
		}
	}
}
