using System.Web.Http;
using Jarvis.MetadataService;
using Jarvis.MetadataService.Support;
using Owin;

namespace Jarvis.MetadataService
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            ConfigureMetadata();
            ConfigureWebApi(appBuilder);
        }

        protected virtual void ConfigureMetadata()
        {
            Metadata.Configure(null);
        }

        protected virtual void ConfigureWebApi(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            // Web API configuration and services
            config.Filters.Add(new ValidateModelAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            appBuilder.UseWebApi(config);
        }
    }
}