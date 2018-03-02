using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WanBootWebServices.Models;

namespace WanBootWebServices.Controllers
{
	/// <summary>
	/// Api used to create a token to access the web api.
	/// </summary>
	[Route("api/[controller]")]
	public class TokenController : Controller
	{
		/// <summary>
		/// The config to use.
		/// </summary>
	    private readonly IConfiguration _config;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="config">The config to use.</param>
	    public TokenController(IConfiguration config)
	    {
		    _config = config;
	    }

		/// <summary>
		/// Access to the creation token method.
		/// </summary>
		/// <param name="login">The user needed to obtain a token.s</param>
		/// <returns></returns>
	    [AllowAnonymous]
	    [HttpPost]
	    public IActionResult CreateToken([FromBody]LoginModel login)
	    {
		    IActionResult response = Unauthorized();
		    var user = Authenticate(login);

		    if (user != null)
		    {
			    var tokenString = BuildToken(user);
			    response = Ok(new { token = tokenString });
		    }

		    return response;
	    }

		/// <summary>
		/// Create a token with specified values.
		/// </summary>
		/// <param name="user">The user logged in the web api.</param>
		/// <returns>Returns a new token generated.</returns>
	    private string BuildToken(UserModel user)
	    {
		    var claims = new[] {
			    new Claim(JwtRegisteredClaimNames.Sub, user.Name),
			    new Claim(JwtRegisteredClaimNames.Email, user.Email),
			    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		    };
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
		    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		    var token = new JwtSecurityToken(_config["Jwt:Issuer"],
			    _config["Jwt:Issuer"],
				claims,
			    expires: DateTime.Now.AddMinutes(5),
			    signingCredentials: creds);

		    return new JwtSecurityTokenHandler().WriteToken(token);
	    }

		/// <summary>
		/// Determines if the user match to return a token.
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
	    private UserModel Authenticate(LoginModel login)
	    {
		    UserModel user = null;
		    if (login.Username == _config["ApplicationSettings:UserApi"] && login.Password == _config["ApplicationSettings:PassApi"])
			    user = new UserModel { Name = "Axel Schaeffer", Email = "admin@axelchef.fr" };

		    return user;
	    }
    }
}