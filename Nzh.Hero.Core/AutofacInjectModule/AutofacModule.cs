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
            builder.RegisterAssemblyTypes(Assembly.Load("Nzh.Hero.Service"))
                .Where(t => t.Name.EndsWith("Service")).AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SqlDbContext>().As<ISqlDbContext>().InstancePerLifetimeScope();
        }
    }
}
