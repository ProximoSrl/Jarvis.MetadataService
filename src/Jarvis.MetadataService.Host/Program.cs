using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jarvis.MetadataService.Support;
using log4net;
using Microsoft.Owin.Hosting;
using Topshelf;

namespace Jarvis.MetadataService.Host
{
    internal static class Program
    {
        static int Main(string[] args)
        {
            var exitCode = HostFactory.Run(host =>
            {
                host.UseOldLog4Net("log4net.config");

                host.Service<Bootstrapper>(service =>
                {
                    var uri = new Uri(ConfigurationManager.AppSettings["uri"]);

                    service.ConstructUsing(() => new Bootstrapper(uri));
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });

                host.RunAsNetworkService();

                host.SetDescription("Jarvis Metadata Service");
                host.SetDisplayName("Jarvis - Metadata service");
                host.SetServiceName("JarvisMetadataService");
            });

            return (int)exitCode;
        }
    }

    internal class HostStartup : Startup
    {
        protected override void ConfigureMetadata()
        {
            Metadata.Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
        }
    }

    internal class Bootstrapper
    {
        readonly Uri _uri;
        IDisposable _app;
        public Bootstrapper(Uri uri)
        {
            _uri = uri;
        }

        public void Start()
        {
            LogManager.GetLogger(this.GetType()).DebugFormat("Starting on {0}", _uri.AbsoluteUri);
            _app = WebApp.Start<HostStartup>(_uri.AbsoluteUri);
        }

        public void Stop()
        {
            _app.Dispose();
        }
    }
}
