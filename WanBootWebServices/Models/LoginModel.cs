namespace WanBootWebServices.Models
{
	/// <summary>
	/// The model for the token api calls.
	/// </summary>
    public class LoginModel
    {
		/// <summary>
		/// Defines the username.
		/// </summary>
	    public string Username { get; set; }

		/// <summary>
		/// /Defines the password.
		/// </summary>
	    public string Password { get; set; }
	}
}
