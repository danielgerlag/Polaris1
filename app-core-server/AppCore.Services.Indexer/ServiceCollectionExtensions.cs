using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using AppCore.Services.Indexer.Interface;
using AppCore.Services.Indexer.Builder;
using AppCore.Services.Indexer.Store;

namespace AppCore.Services.Indexer
{
    public static class ServiceCollectionExtensions
    {
        public static Autofac.ContainerBuilder AddSearchIndexerServices(this Autofac.ContainerBuilder services)
        {

            services.RegisterType<IndexQueue>().As<IIndexQueue>().SingleInstance();
            services.RegisterType<IndexWorker>().As<IIndexWorker>().SingleInstance();
            services.RegisterType<IndexRegister>().As<IIndexRegister>().SingleInstance();
            services.RegisterType<IndexStore>().As<IIndexStore>().InstancePerDependency();
            services.RegisterType<SearchService>().As<ISearchService>().InstancePerDependency();

            return services;
        }

        public static Autofac.ContainerBuilder AddSearchIndexers(this Autofac.ContainerBuilder services, System.Reflection.Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            foreach (var t in types.Where(x => x.IsClass && !x.IsAbstract).Where(x => x.GetInterfaces().Any(y => y == typeof(IEntityIndexer))))
            {
                services.RegisterType(t).InstancePerDependency();
                //services.AddTransient(t);
            }

            return services;
        }

    }
}
