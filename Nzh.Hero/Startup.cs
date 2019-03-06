using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog.Web;
using Nzh.Hero.Common.NLog;
using Nzh.Hero.Core.AutofacInjectModule;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.Core.Web;
using Nzh.Hero.ViewModel.Common;
using StackExchange.Redis;

namespace Nzh.Hero
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("Config/appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region 获取配置项

            SugarDbConn.DbConnectStr = this.Configuration.GetSection("DbConn:mysqlConn").Value;   //为数据库连接字符串赋值
            GlobalParamsDto.RpcUname = this.Configuration.GetSection("RpcUser:Username").Value;
            GlobalParamsDto.RpcPwd = this.Configuration.GetSection("RpcUser:Password").Value;

            #endregion

            #region hangfire配置

            string redisConn = this.Configuration.GetSection("redisConn:redisConnStr").Value;
            string redisPwd = this.Configuration.GetSection("redisConn:redisPwd").Value;
            var hgOpts = ConfigurationOptions.Parse(redisConn);
            hgOpts.AllowAdmin = true;
            hgOpts.Password = redisPwd;

            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            //nlog日志配置文件
            env.ConfigureNLog("Config/nlog.config");

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller=DeviceData}/{action=Index}/{id?}");
            });
            //扩展HttpContext
            app.UseStaticHttpContext();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            try
            {
                builder.RegisterModule(new AutofacModule());
            }
            catch (Exception e)
            {
                LogNHelper.Exception(e);
            }
        }
    }
}
