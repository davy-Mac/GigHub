using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace GigHub
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var settings = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings; // assigns the json format settings to a var
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver(); //changes the format of the api response from Pascal casing to camel casing
            settings.Formatting = Formatting.Indented; // creates indentation for the response format

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
