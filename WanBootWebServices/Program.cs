using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

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
				   .UseKestrel()
	               .UseIISIntegration()
				   .Build();

			//options =>
			//{
			//options.Listen(IPAddress.Loopback, 5000, listenOptions =>
			//{
			//	listenOptions.UseHttps("A GENRERER.pfx", "password");
			//});
			//}
	}
}