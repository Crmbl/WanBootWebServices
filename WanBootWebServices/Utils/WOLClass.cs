using System.Net.Sockets;

namespace WanBootWebServices.Utils
{
	/// <summary>
	/// Wake on Lan class.
	/// </summary>
    public class WOLClass : UdpClient
	{
		/// <summary>
		/// Send broadcast packet.
		/// </summary>
		public void SetClientToBrodcastMode()
		{
			if (Active)
				Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 0);
		}
	}
}