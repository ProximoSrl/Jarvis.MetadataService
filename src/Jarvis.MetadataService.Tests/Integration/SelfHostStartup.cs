using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jarvis.MetadataService.Web;
using Jarvis.MetadataService.Web.Support;
using Owin;

namespace Jarvis.MetadataService.Tests.Integration
{
    public class SelfHostStartup : Startup
    {
        protected override void ConfigureMetadata()
        {
            Metadata.Configure(Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, 
                "Data"
            ));
        }
    }
}
