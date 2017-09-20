using AppCore.DomainModel.Abstractions.Intercepts;
using Autofac;
using HR1.DomainModel.Context;
using HR1.DomainModel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR1.DomainModel
{
    public static class ServiceCollectionExtensions
    {
        public static Autofac.ContainerBuilder AddDomainServices(this Autofac.ContainerBuilder services)
        {

            services.RegisterType<HR1Context>().As<IHR1Context>().InstancePerDependency();

            services.RegisterType<AccountingEntitySetupManager>().As<IAccountingEntitySetupManager>().InstancePerDependency();

            //services.RegisterType<SequenceNumberGeneator>().As<ISequenceNumberGeneator>().InstancePerDependency();            

            return services;
        }

        public static Autofac.ContainerBuilder AddDomainModelIntercepts(this Autofac.ContainerBuilder services)
        {
            Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            foreach (var t in types.Where(x => x.IsClass && !x.IsAbstract).Where(x => x.GetInterfaces().Any(y => y == typeof(IEntityIntercept))))
            {
                services.RegisterType(t).InstancePerDependency();
                //services.AddTransient(t);
            }

            return services;
        }

    }
}
