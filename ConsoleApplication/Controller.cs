using System;

namespace ConsoleApplication
{
    public class Controller
    {
        const string okPrefix = "{OK}";
        const string errPrefix = "{ERR}";
        const string errServiceUnavaliable = "Service is unavaliable!";
        const string errEmptyRequest = "Empty request!";
        const string errNeedCountry = "Need country!";
        const string errNeedZipcode = "Need zipcode!";

        /// <summary>
        /// Get weather for requested location.
        /// </summary>
        /// <param name="country">Country code</param>
        /// <param name="zipcode">Zip (postal) code</param>
        /// <returns></returns>
        public static string CheckWeather(string country, string zipcode)
        {
            Service.Service Service = new Service.Service();
            try
            {
                return Service.GetWeather(country, zipcode);
            }
            catch (Exception ex)
            {
                return errServiceUnavaliable;
            }
        }

        /// <summary>
        /// Get timezone for requested location.
        /// </summary>
        /// <param name="country">Country code</param>
        /// <param name="zipcode">Zip (postal) code</param>
        /// <returns></returns>
        public static string CheckTimezone(string country, string zipcode)
        {
            Service.Service Service = new Service.Service();
            //ServiceReference1.ServiceClient client1 = new ServiceReference1.ServiceClient();
            try
            {
                return Service.GetTimezone(country, zipcode);
                //return client1.GetTimezone(country, zipcode);
            }
            catch (Exception ex)
            {
                return errServiceUnavaliable;
            }
        }

        /// <summary>
        /// Check request string.
        /// </summary>
        /// <param name="request">Request string (country, zipcode)</param>
        /// <returns></returns>
        public static string[] CheckRequest(string request)
        {
            if (request == null)
                return null;

            var response = new String[] { "", "" };
            if (request.Trim().Length == 0)
                return response;

            var chunks = request.Split(new char[] { ',' });
            switch (chunks.Length)
            {
                case 0:
                    break;
                case 1:
                    response[0] = chunks[0].Trim();
                    break;
                default:
                    response[0] = chunks[0].Trim();
                    response[1] = chunks[1].Trim();
                    break;
            }

            return response;
        }

        /// <summary>
        /// Execute request to Web API service.
        /// </summary>
        /// <param name="request">Request string (country, zipcode)</param>
        /// <returns></returns>
        public static string ExecuteRequest(string request)
        {
            var chunks = CheckRequest(request);
            if (chunks == null)
                return errPrefix + " " + errEmptyRequest;
            var response = "";
            if (chunks[0].Length == 0)
                response = errPrefix + " " + errNeedCountry;
            if (chunks[1].Length == 0)
                response = (response.Length == 0 ? errPrefix + " " : response + " ") + errNeedZipcode;
            if (response.Length != 0)
                return response;

            var result1 = Controller.CheckWeather(chunks[0], chunks[1]);
            if (result1.StartsWith(errPrefix))
                return result1;
            var result2 = Controller.CheckTimezone(chunks[0], chunks[1]);
            return result1 + " " + result2;
        }
    }
}
