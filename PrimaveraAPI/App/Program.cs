using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;

namespace PrimaveraAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = GetServerUrlsFromCommandLine(args);

            return WebHost.CreateDefaultBuilder(args)
               .UseConfiguration(config)
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseIISIntegration()
               .UseStartup<Startup>();
        }

        public static IConfigurationRoot GetServerUrlsFromCommandLine(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)

                .Build();
            var serverport = config.GetValue<int?>("port") ?? 4040;
            //var serverport = config.GetValue<int?>("port") ?? 26486;
            var serverurls = config.GetValue<string>("server.urls") ?? string.Format("http://*:{0}", serverport);
            var configDictionary = new Dictionary<string, string>
                                   {
                                       { "server.urls", serverurls },
                                       { "port", serverport.ToString() },
                                   };

            return new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddInMemoryCollection(configDictionary)
                .Build();
        }
    }
}
