using System;
using System.Reflection;

namespace Jarvis.MetadataService.Models
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