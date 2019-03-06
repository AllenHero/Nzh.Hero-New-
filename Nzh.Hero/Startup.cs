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
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.Core.Web;
using Nzh.Hero.Extension;
using Nzh.Hero.IService;
using Nzh.Hero.Service;
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

            SugarDbConn.DbConnectStr = this.Configuration.GetSection("DbConn:mysqlConn").Value;   
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

            //services.AddTransient<ISysAreaService, SysAreaService>();
            //services.AddTransient<ISysDicService, SysDicService>();
            //services.AddTransient<ISysFuncService, SysFuncService>();
            //services.AddTransient<ISysMenuService, SysMenuService>();
            //services.AddTransient<ISysRoleService, SysRoleService>();
            //services.AddTransient<ISysUserService, SysUserService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            env.ConfigureNLog("Config/nlog.config");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
                routes.MapRoute(name: "areaRoute",
                    template: "{area:exists}/{controller=DeviceData}/{action=Index}/{id?}");
            });

            app.UseStaticHttpContext();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            try
            {
                //builder.RegisterModule(new AutofacModule());  
                builder.RegisterModule(new AutofacExt());
            }
            catch (Exception e)
            {
                LogNHelper.Exception(e);
            }
        }
    }
}
