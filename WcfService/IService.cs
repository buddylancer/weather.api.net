using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string GetWeatherAndTimezone(string country, string zipcode);

        [OperationContract]
        string GetWeather(string country, string zipcode);

        [OperationContract]
        string GetTimezone(string country, string zipcode);
    }
}
