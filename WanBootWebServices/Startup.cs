using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using WanBootWebServices.Utils;

namespace WanBootWebServices
{
	/// <summary>
	/// Default startup class.
	/// </summary>
    public class Startup
    {
		/// <summary>
		/// Startup method.
		/// </summary>
	    public Startup(IHostingEnvironment env)
	    {
		    var builder = new ConfigurationBuilder()
			    .SetBasePath(env.ContentRootPath)
			    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
			    .AddEnvironmentVariables();

		    Configuration = builder.Build();
	    }

		/// <summary>
		/// Defines the configuration.
		/// </summary>
		public IConfiguration Configuration { get; }

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		public void ConfigureServices(IServiceCollection services)
        {
	        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		        .AddJwtBearer(options =>
		        {
			        options.TokenValidationParameters = new TokenValidationParameters
			        {
				        ValidateIssuer = true,
				        ValidateAudience = true,
				        ValidateLifetime = true,
				        ValidateIssuerSigningKey = true,
				        ValidIssuer = Configuration["Jwt:Issuer"],
				        ValidAudience = Configuration["Jwt:Issuer"],
				        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
			        };
		        });

	        services.AddOptions();
			services.Configure<ApplicationSettings>(options => Configuration.GetSection("ApplicationSettings").Bind(options));
	        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
	        services.AddMvc();
			services.Configure<MvcOptions>(options =>
			{
				options.Filters.Add(new RequireHttpsAttribute());
			});
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

	        app.UseAuthentication();
	        var options = new RewriteOptions().AddRedirectToHttps();
	        app.UseRewriter(options);
			app.UseMvc();
		}
    }
}
