using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OData.Edm;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OData.UriParser;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace DotNet.AspNetCore.WebApi.OData.Sample
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
            var connection = @"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.AspNetCore.NewDb;Trusted_Connection=True;ConnectRetryCount=0";

            services.AddEntityFrameworkProxies();

            services.AddDbContext<Data.BloggingContext>(options =>
            {
                options.UseSqlServer(connection);
                options.UseLazyLoadingProxies(true);
            });

            services.AddAutoMapper();
            //services.AddAutoMapper(a =>
            //{
            //    var blogMapping = a.CreateMap<Data.Blog, Models.Blog>();

            //    //blogMapping.ProjectUsing((from) => new Models.Blog()
            //    //{
            //    //    BlogId = from.BlogId,
            //    //    Url = from.Url,
            //    //});
            //    blogMapping.ForAllMembers(r =>
            //    {
            //        r.Ignore();
            //        //r.ExplicitExpansion();
            //    });
            //    a.AllowNullCollections = true;
            //    a.AllowNullDestinationValues = true;
            //    //a.EnableNullPropagationForQueryMapping = true;
            //});
            
            services.AddMvc();
            services.AddOData();
            services.AddODataQueryFilter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var edmModel = GetEdmModel(app.ApplicationServices);

            app.UseMvc(routes =>
            {
                //routes.Select().Expand().Filter().OrderBy().MaxTop(null).Count();

                //routes.MapODataServiceRoute("OData", "odata", edmModel);

                routes.EnableDependencyInjection();

                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");


                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapODataServiceRoute("odata", "api", edmModel);
                routes.SetDefaultODataOptions(new Microsoft.AspNet.OData.ODataOptions { NullDynamicPropertyIsEnabled = true });
                routes.Filter();
                routes.MaxTop(null);
                routes.Select();
                routes.OrderBy();
                routes.Count();
                routes.Expand();
                routes.SetDefaultQuerySettings(new Microsoft.AspNet.OData.Query.DefaultQuerySettings { EnableCount = true, EnableFilter = true, MaxTop = null, EnableOrderBy = true, EnableExpand = true, EnableSelect = true });
                //routes.EnableDependencyInjection(b => b.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(ODataUriResolver), typeof(UnqualifiedCallAndEnumPrefixFreeResolver)));

            });
        }


        private static IEdmModel GetEdmModel(IServiceProvider serviceProvider)
        {
            var builder = new ODataConventionModelBuilder(serviceProvider);
            builder.EntitySet<Models.Blog>("Blogs");
            //builder.EntitySet<Data.Blog>("Blogs");
            return builder.GetEdmModel();
        }
    }
}
