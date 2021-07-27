using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TestApplication
{
    class Program
    {
        const String OK = "{OK}";
        const String ERR = "{ERR}";

        /// <summary>
        /// Test cases (request strings).
        /// </summary>
        private static List<String> Requests = new List<String>() {
            null,
            "",
            ",",
            "XXX,",
            ",XXX",
            "XXX,XXX",
            "US,00000",
            "RU,000000",
            "RU,999999",

            "US,35801",
            "US,32501",
            "US,33124",
            "US,32801",
            "US,05751",

            "GB,PO12",
            "GB,CB25",

            "RU,603000",
            "RU,121059"
        };

        /// <summary>
        /// Test responses for appropriate test requests.
        /// </summary>
        private static List<String> Responses = new List<String>() {
            ERR + " Empty request!", //null,
            ERR + " Need country! Need zipcode!", //"",
            ERR + " Need country! Need zipcode!", //",",
            ERR + " Need zipcode!", //"XXX,",
            ERR + " Need country!", //",XXX",
            ERR + " Bad request!", //"XXX,XXX",
            ERR + " Bad request!", //"US,00000",
            ERR + " Bad request!", //"RU,000000",
            ERR + " Bad request!", //"RU,999999",

            OK + " city:Huntsville; temperature:[^;]+; " + OK + " timezone:America/Chicago;", //"US,35801",
            OK + " city:Pensacola; temperature:[^;]+; " + OK + " timezone:America/Chicago;", //"US,32501",
            OK + " city:Miami; temperature:[^;]+; " + OK + " timezone:America/New_York;", //"US,33124",
            OK + " city:Orlando; temperature:[^;]+; " + OK + " timezone:America/New_York;", //"US,32801",
            OK + " city:Killington; temperature:[^;]+; " + OK + " timezone:America/New_York;", //"US,05751",

            OK + " city:Hardway; temperature:[^;]+; " + OK + " timezone:Europe/London;", //"GB,PO12",
            OK + " city:Waterbeach; temperature:[^;]+; " + OK + " timezone:Europe/London;", //"GB,CB25",
            
            OK + " city:Нижний Новгород; temperature:[^;]+; " + OK + " timezone:Europe/Moscow;", //"RU,603000",
            OK + " city:Москва 59; temperature:[^;]+; " + OK + " timezone:Europe/Moscow;" //"RU,121059"
        };

        /// <summary>
        /// Test application for testing Web API service.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int positiveTests = 0;
            int negativeTests = 0;

            // Enumerate through all tests requests
            for (int n = 0; n < Requests.Count; n++)
            {
                var request = Requests[n];

                // Get response from Web API service...
                var response = ConsoleApplication.Controller.ExecuteRequest(request);

                // ...and compare it with origin ("to be") response
                System.Console.Write("Request:" + request + " // Expected:" + response + " // ");
                if (Regex.IsMatch(response, Responses[n]))
                {
                    positiveTests++;
                    System.Console.WriteLine("OK");
                }
                else
                {
                    negativeTests++;
                    System.Console.WriteLine("ERR");
                }
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Total tests    : " + Requests.Count);
            System.Console.WriteLine("Positive tests : " + positiveTests);
            System.Console.WriteLine("Negative tests : " + negativeTests);

            System.Console.Write("Press Enter to exit...");
            System.Console.ReadLine();
        }
    }
}
