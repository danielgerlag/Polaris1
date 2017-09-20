using AppCore.Services.Identity;
using AppCore.Services.Identity.Services;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1.Services.Identity
{
    public static class ServiceCollectionExtensions
    {
        public static Autofac.ContainerBuilder AddIdentityServices(this Autofac.ContainerBuilder services)
        {

            services.RegisterType<TenantContext>()
                .As<IAppIdentityContext>()
                .As<ITenantContext>()
                .InstancePerDependency();

            services.RegisterType<TenantFilter>()
                .As<ITenantFilter>()                
                .InstancePerDependency();


            return services;
        }
    }
}
