using Autofac;
using Nzh.Hero.Core.DbContext;
using Nzh.Hero.IRepository;
using Nzh.Hero.IService;
using Nzh.Hero.Repository;
using Nzh.Hero.Service;
using System;

namespace Nzh.Hero.Extension
{
    public class AutofacExt : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterAssemblyTypes(Assembly.Load("Nzh.Hero.Service"))
            //    .Where(t => t.Name.EndsWith("Service")).AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SysUserService>().As<ISysUserService>().InstancePerLifetimeScope();
            builder.RegisterType<SysAreaService>().As<ISysAreaService>().InstancePerLifetimeScope();
            builder.RegisterType<SysDicService>().As<ISysDicService>().InstancePerLifetimeScope();
            builder.RegisterType<SysFuncService>().As<ISysFuncService>().InstancePerLifetimeScope();
            builder.RegisterType<SysMenuService>().As<ISysMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<SysRoleService>().As<ISysRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<DemoService>().As<IDemoService>().InstancePerLifetimeScope();
            builder.RegisterType<SysLogService>().As<ISysLogService>().InstancePerLifetimeScope();

            builder.RegisterType<SysUserRepository>().As<ISysUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SysAreaRepository>().As<ISysAreaRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SysDicRepository>().As<ISysDicRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SysFuncRepository>().As<ISysFuncRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SysMenuRepository>().As<ISysMenuRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SysRoleRepository>().As<ISysRoleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DemoRepository>().As<IDemoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SysLogRepository>().As<ISysLogRepository>().InstancePerLifetimeScope();

            builder.RegisterType<SysLogService>().As<ISysLogService>().InstancePerLifetimeScope();

            builder.RegisterType<SqlDbContext>().As<ISqlDbContext>().InstancePerLifetimeScope();

        }
    }
}
