using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Autofac;
using Autofac.Integration.WebApi;
using System.Web.Http;
using Newtonsoft.Json;
using AzureCDNService.Lib;
using Autofac.Builder;

[assembly: OwinStartup(typeof(AzureCDNService.App.Startup))]

namespace AzureCDNService.App
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<BlobService>().SingleInstance();
            builder.RegisterType<BlobService>().AsImplementedInterfaces<IBlobService, ConcreteReflectionActivatorData>();
            builder.RegisterApiControllers(typeof(Startup).Assembly);

            var container = builder.Build();

            app.Properties["host.AppName"] = "Azure CDN File Uploader";
            app.UseAutofacMiddleware(container);

            var webApiDependencyResolver = new AutofacWebApiDependencyResolver(container);
            var webApiConfiguration = ConfigureWebApi(webApiDependencyResolver);

            
            app.UseWebApi(webApiConfiguration);
            app.UseAutofacWebApi(webApiConfiguration);
        }

        private HttpConfiguration ConfigureWebApi(AutofacWebApiDependencyResolver resolver)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("Default",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            config.DependencyResolver = resolver;
            
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            return config;

        }
    }
}
