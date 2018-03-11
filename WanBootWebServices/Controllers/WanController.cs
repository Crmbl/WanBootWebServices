using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WanBootWebServices.Utils;
using PPing = System.Net.NetworkInformation.Ping;

namespace WanBootWebServices.Controllers
{
	/// <summary>
	/// Controller for the web api requests.
	/// </summary>
	[RequireHttps]
	[Route("api/[controller]")]
    public class WanController : Controller
	{
		/// <summary>
		/// Defines the address of the computer to ping.
		/// </summary>
		private readonly string _computerAddress;

		/// <summary>
		/// Defines the mac address of the computer to boot.
		/// </summary>
		private readonly string _computerMacAddress;

		/// <summary>
		/// Defines the httpContextAccessor.
		/// </summary>
		private readonly IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// Init the httpContextAccessor.
		/// </summary>
	    public WanController(IHttpContextAccessor httpContextAccessor, IOptions<ApplicationSettings> settings)
	    {
		    _httpContextAccessor = httpContextAccessor;
		    var confSettings = settings.Value;
		    if (confSettings != null)
		    {
			    _computerAddress = confSettings.ComputerAddress;
			    _computerMacAddress = confSettings.ComputerMacAddr;
		    }
		}

		/// <summary>
		/// Defines the ping method.
		/// </summary>
		/// <returns></returns>
		[HttpGet("Ping"), Authorize]
	    public async Task Ping()
	    {
		    var response = _httpContextAccessor.HttpContext.Response;
		    response.Headers.Add("Content-Type", "text/event-stream");
			var ping = new PPing();
			var result = ping.Send(_computerAddress, 20000);
			await response.WriteAsync(result != null ? result.Status.ToString() : "error machine not reachable");
	    }

		/// <summary>
		/// Defines the method to boot up the computer.
		/// </summary>
		/// <returns></returns>
		[HttpGet("BootUp"), Authorize]
	    public async Task BootUp()
		{
			var response = _httpContextAccessor.HttpContext.Response;
			response.Headers.Add("Content-Type", "text/event-stream");

			using (UdpClient client = new UdpClient())
			{
				client.Connect(IPAddress.Parse(_computerAddress), 7);
				client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

				var offset = 0;
				byte[] buffer = new byte[102];
				for (var y = 0; y < 6; y++)
					buffer[offset++] = 0xFF;

				for (var y = 0; y < 16; y++)
				{
					var i = 0;
					for (var z = 0; z < 6; z++)
					{
						buffer[offset++] = byte.Parse(_computerMacAddress.Substring(i, 2), NumberStyles.HexNumber);
						i += 2;
					}
				}

				var result = client.Send(buffer, 102);
				await response.WriteAsync(result > 0 ? "Success" : "error sending magic packet");
			}
		}
	}
}
