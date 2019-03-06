using Autofac;
using Nzh.Hero.Core.DbContext;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Nzh.Hero.Core.AutofacInjectModule
{
    public class AutofacModule:Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //领养AppService注入
            builder.RegisterAssemblyTypes(Assembly.Load("Altas.Framework.AppService"))
                .Where(t => t.Name.EndsWith("AppService")).AsSelf().InstancePerLifetimeScope();
            //api服务注入

            //dbcontext注入
            builder.RegisterType<SqlDbContext>().As<IsqlDbContext>().InstancePerLifetimeScope();
        }
    }
}
