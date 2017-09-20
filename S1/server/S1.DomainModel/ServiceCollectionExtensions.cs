using AppCore.DomainModel.Abstractions.Intercepts;
using Autofac;
using S1.DomainModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1.DomainModel
{
    public static class ServiceCollectionExtensions
    {
        public static Autofac.ContainerBuilder AddDomainServices(this Autofac.ContainerBuilder services)
        {

            services.RegisterType<S1Context>().As<IS1Context>().InstancePerDependency();                        

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
