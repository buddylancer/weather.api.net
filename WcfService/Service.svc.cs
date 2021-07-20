using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using System.Web;

using System.Runtime.Serialization;
using System.Xml;

namespace WcfService
{
    public class Service : IService
    {
        const string okPrefix = "{OK}";
        const string errPrefix = "{ERR}";
        const string errBadRequest = "Bad request!";
        const string errBadParameters = "Bad API key, country or zipcode!";
        const string errCountryZipcode = "Country or zipcode not found!";
        const string errNeedCountry = "Need country!";
        const string errNeedZipcode = "Need zipcode!";
        const string errCountry = "Error in country!";
        const string errZipcode = "Error in zipcode!";

        private string KeyWeatherApi = null; // Weather API key
        private string KeyGoogleApi = null; // Google API key

        private string UrlWeatherApi = null; // Weather API url
        private string UrlLocationApi = null; // Google API url #1 (get place/location)
        private string UrlTimezoneApi = null; // Google API url #2 (get timezone)

        /// <summary>
        /// Get keys and urls from config
        /// </summary>
        private Service()
        {
            Properties.Settings defaultSettings = Properties.Settings.Default;

            KeyWeatherApi = defaultSettings.Key1;
            KeyGoogleApi = defaultSettings.Key2;

            UrlWeatherApi = defaultSettings.Url1;
            UrlLocationApi = defaultSettings.Url2;
            UrlTimezoneApi = defaultSettings.Url3;
        }

        public string GetWeatherAndTimezone(string country, string zipcode)
        {
            var response1 = GetWeather(country, zipcode);
            if (HasError(response1))
                return response1;
            var response2 = GetTimezone(country, zipcode);
            if (HasError(response2))
                return response2;
            return response1 + " " + response2;
        }

        /// <summary>
        /// Get weather for country & zipcode.
        /// </summary>
        /// <param name="country">Country code (RU, US, ...)</param>
        /// <param name="zipcode">Zip (postal) code (603000, ...)</param>
        /// <returns>Weather info</returns>
        public string GetWeather(string country, string zipcode)
        {
            var checkResult = CheckRequest(country, zipcode);
            if (checkResult.Length > 0)
                return MakeError(checkResult);

            string[] replacer = 
                { "{country}", country, "{zipcode}", zipcode, "{mode}", "xml", "{units}", "metric", "{key1}", KeyWeatherApi };
            var url = UrlWeatherApi.ReplaceMany(replacer);
            var xmlDocument = Request(url);
            if (xmlDocument == null)
                return MakeError(errBadRequest);

            var currentCityNode = xmlDocument.SelectSingleNode("/current/city");
            var city = currentCityNode.Attributes["name"].Value;
            var currentTemperatureNode = xmlDocument.SelectSingleNode("/current/temperature");
            string temperature = currentTemperatureNode.Attributes["value"].Value;
            return MakeResponse(new string[] { "city", city, "temperature", temperature });
        }

        /// <summary>
        /// Get timezone for country & zipcode.
        /// </summary>
        /// <param name="country">Country code (RU, US, ...)</param>
        /// <param name="zipcode">Zip (postal) code (603000, ...)</param>
        /// <returns>Timezone info</returns>
        public string GetTimezone(string country, string zipcode)
        {
            var checkResult = CheckRequest(country, zipcode);
            if (checkResult.Length > 0)
                return MakeError(checkResult);

            // Get location at first
            string[] replacer2 = 
                { "{country}", country, "{zipcode}", zipcode, "{key2}", KeyGoogleApi };
            var url2 = UrlLocationApi.ReplaceMany(replacer2);
            var doc2 = Request(url2);
            var statusNode = doc2.SelectSingleNode("GeocodeResponse/status");
            if (statusNode == null)
                return MakeError(errBadParameters);

            var status = statusNode.InnerText;
            if (!status.Equals("OK"))
            {
                if (status.Equals("REQUEST_DENIED"))
                {
                    var errorMessageNode = doc2.SelectSingleNode("GeocodeResponse/error_message");
                    if (errorMessageNode != null)
                        return MakeError(errorMessageNode.InnerText);
                    return MakeError(errBadParameters);
                }
                else if (status.Equals("ZERO_RESULTS"))
                    return MakeError(errCountryZipcode);
            }
            var locationNode = doc2.SelectSingleNode("GeocodeResponse/result/geometry/location");
            var longitude = locationNode.SelectSingleNode("lng").InnerText;
            var lattitude = locationNode.SelectSingleNode("lat").InnerText;

            // Get timezone next
            string[] replacer3 = { "{lat}", lattitude, "{lng}", longitude, "{timestamp}", (DateTime.Now.Ticks / 1000000).ToString(), "{key2}", KeyGoogleApi };
            var url3 = UrlTimezoneApi.ReplaceMany(replacer3);
            var doc3 = Request(url3);
            var timezoneNode = doc3.SelectSingleNode("TimeZoneResponse/time_zone_id");
            var timezone = timezoneNode.InnerText;
            return MakeResponse(new string[] {"timezone", timezone});
        }

        private XmlDocument Request(string url)
        {
            var webClient = new System.Net.WebClient();
            var result = "";
            try
            {
                var downloadedBytes = webClient.DownloadData(url);
                result = System.Text.Encoding.UTF8.GetString(downloadedBytes);
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(result);
                return xmlDocument;
            }
            catch (Exception ex)
            {
                //TODO -- process result here...
                return null;
            }
        }

        private string CheckRequest(string country, string zipcode)
        {
            string response1 = "";
            if (country == null)
                response1 = errNeedCountry;
            else if (country.Length == 0)
                response1 = errCountry;

            string response2 = "";
            if (zipcode == null)
                response2 = errNeedZipcode;
            else if (zipcode.Length == 0)
                response2 = errZipcode;

            string response = response1 + (response1.Length != 0 ? " " : "") + response2;
            return response;
        }

        private string MakeResponse(string[] messages)
        {
            string response = okPrefix + " ";
            for (int n = 0; n < messages.Length; n += 2)
                response += (n > 0 ? " ": "") + messages[n] + ":" + messages[n + 1] + ";";
            return response;
        }

        private string MakeError(string message)
        {
            return errPrefix + " " + message;
        }

        private bool HasError(string response)
        {
            return response.StartsWith(errPrefix);
        }
    }

    public static class StringExtensions
    {
        public static string ReplaceMany(this String str, String[] array)
        {
            for (int n = 0; n < array.Length; n += 2)
                str = str.Replace(array[n], array[n + 1]);
            return str;
        }
    }
}
