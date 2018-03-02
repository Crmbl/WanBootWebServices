namespace WanBootWebServices.Utils
{
	/// <summary>
	/// Defines the application settings found in the appsettings.json.
	/// </summary>
    public class ApplicationSettings
    {
		/// <summary>
		/// Defines the user for the api.
		/// </summary>
	    public string UserApi { get; set; }

		/// <summary>
		/// Defines the secret for the api.
		/// </summary>
	    public string PassApi { get; set; }

		/// <summary>
		/// Defines the computer address ipv4.
		/// </summary>
	    public string ComputerAddress { get; set; }

		/// <summary>
		/// Defines the computer mac address.
		/// </summary>
		public string ComputerMacAddr { get; set; }
    }
}
