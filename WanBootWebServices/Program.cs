using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WanBootWebServices
{
	/// <summary>
	/// Program class.
	/// </summary>
    public class Program
    {
		/// <summary>
		/// Default main method.
		/// </summary>
		/// <param name="args"></param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

		/// <summary>
		/// Build web host.
		/// </summary>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
	               .CaptureStartupErrors(true)
	               .UseSetting(WebHostDefaults.DetailedErrorsKey, "true")
				   .UseStartup<Startup>()
				   .UseKestrel(SetHost)
				   .UseIISIntegration()
				   .Build();

		/// <summary>
		/// Get the conf file and set the passkey.
		/// </summary>
	    private static void SetHost(Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions options)
	    {
		    var configuration = (IConfiguration)options.ApplicationServices.GetService(typeof(IConfiguration));
			options.Listen(IPAddress.Parse("0.0.0.0"), 5000, listenOptions =>
			{
				listenOptions.UseHttps("cacert.pfx", configuration["ApplicationSettings:PassApi"]);
			});
		}
	}
}