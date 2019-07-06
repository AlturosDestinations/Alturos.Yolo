using Alturos.Yolo.WebService.Contract;
using log4net;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Formatting;
using System.Web.Http;
using Topshelf;

namespace Alturos.Yolo.WebService
{
    public class Controller : ServiceControl
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Controller));
        private const string SystemName = "Alturos Yolo WebService";

        private Container _container;
        private IDisposable _webApp;

        public bool Start(HostControl hostControl)
        {
            Log.Debug($"{nameof(Start)} - {SystemName}");

            this._container = new Container();
            this._container.Register<IObjectDetection, YoloObjectDetection>(Lifestyle.Singleton);

            var port = int.Parse(ConfigurationManager.AppSettings.Get("WebServerPort"));
            this.RegisterWebApi(port);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Log.Debug($"{nameof(Stop)} - {SystemName}");

            this._webApp?.Dispose();
            this._container?.Dispose();

            return true;
        }

        private void RegisterWebApi(int port)
        {
            var url = $"http://*:{port}";
            var fullUrl = url.Replace("*", "localhost");

            Log.Info($"{nameof(RegisterWebApi)} - Swagger: {fullUrl}/swagger/");

            try
            {
                this._webApp = WebApp.Start(url, (app) =>
                {
                    //Use JSON friendly default settings
                    var defaultSettings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Converters = new List<JsonConverter> { new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() }, }
                    };
                    JsonConvert.DefaultSettings = () => { return defaultSettings; };

                    var config = new HttpConfiguration();
                    config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(this._container);

                    //Specify JSON as the default media type
                    config.Formatters.Clear();
                    config.Formatters.Add(new JsonMediaTypeFormatter());
                    config.Formatters.JsonFormatter.SerializerSettings = defaultSettings;

                    //Route all requests to the RootController by default
                    config.Routes.MapHttpRoute("api", "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });
                    config.MapHttpAttributeRoutes();

                    //Tell swagger to generate documentation based on the XML doc file output from msbuild
                    config.EnableSwagger(c =>
                    {
                        c.SingleApiVersion("1.0", SystemName);
                    }).EnableSwaggerUi();

                    app.UseWebApi(config);
                });
            }
            catch (Exception exception)
            {
                Log.Error($"{nameof(RegisterWebApi)} - run first AllowWebserver.cmd", exception);
            }
        }
    }
}
