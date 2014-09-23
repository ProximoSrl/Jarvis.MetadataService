using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;

namespace Jarvis.MetadataService.Web.Models
{
    public class StatusModel
    {
        public StatusModel()
        {
            this.MachineName = Environment.MachineName;
            this.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
        }

        public string Version { get; set; }

        public string MachineName { get; set; }
    }
}