using AppCore.DistributedServices;
using AppCore.DomainModel.Abstractions.Intercepts;
using AppCore.Modules.Financial.DomainModel.Services.Interfaces;
using Autofac;
using HM1.DomainModel.Context;
using HM1.DomainModel.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HM1.DomainModel
{
    public static class ServiceCollectionExtensions
    {
        public static Autofac.ContainerBuilder AddDomainServices(this Autofac.ContainerBuilder services)
        {

            services.RegisterType<HM1Context>().As<IHM1Context>().InstancePerDependency();
            services.RegisterType<JournalBuilder>().InstancePerDependency();
            services.RegisterType<ScheduledJournalRunner>().As<IScheduledJournalRunner>().InstancePerDependency();
            services.RegisterType<JournalPoster>().As<IJournalPoster>().InstancePerDependency();


            //services.RegisterType<SequenceNumberGeneator>().As<ISequenceNumberGeneator>().InstancePerDependency();            

            services.RegisterType<MemoryQueue>()
                .Keyed<IQueueManager>("ScheduledJournalQueue")
                .SingleInstance();

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

        public static Autofac.ContainerBuilder AddDistibutedWorkerPools(this Autofac.ContainerBuilder services)
        {
            services.RegisterType<LockService>()
                .As<ILockService>()
                .WithParameter("connectionString", ConfigurationManager.ConnectionStrings["DB"].ConnectionString);

            services.RegisterType<DistributedServices.ScheduledJournalWorker>()
                .Keyed<IWorker>("ScheduledJournalWorker")
                .InstancePerDependency();

            services.RegisterType<DistributedServices.ScheduledJournalPool>()
                .Keyed<IWorkerPool>("ScheduledJournal")
                .WithParameter("threadCount", 1)
                .WithParameter("timeOut", TimeSpan.FromSeconds(5))
                .SingleInstance();

            services.RegisterType<DistributedServices.JournalPollManager>()
                .Keyed<ITransactionPollManager>("ScheduledJournalPoll")
                .SingleInstance();

            return services;
        }

    }
}
