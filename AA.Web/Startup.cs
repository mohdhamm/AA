using System.Reflection;
using System.Text.Json.Serialization;
using AA.Web.Profiles;
using AA.Web.Models;
using AA.Web.Interfaces;
using AA.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AA.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers()
				.AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("Default")).EnableSensitiveDataLogging());
			services.AddScoped<DbContext, AppDbContext>();

			services.AddCors(services =>
			{
				services.AddPolicy("CorsPolicy", builder =>
				builder.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod());
			});

			services.AddScoped<IOrderService, OrderService>();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "AA.Web", Version = "v1" });
			});

			var assemblies = new Assembly[]
			{
				typeof(OrderProfile).Assembly,
				typeof(Startup).Assembly,
				typeof(AppDbContext).Assembly,
				typeof(OrderService).Assembly
			};

			services.AddAutoMapper(config =>
			{
				config.AllowNullCollections = true;
			}, assemblies);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AA.Web v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseCors("CorsPolicy");

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}