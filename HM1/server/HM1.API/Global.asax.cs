using AppCore.DistributedServices;
using AppCore.Modules.Financial.DomainModel.Services.Interfaces;
using AppCore.Services.Indexer;
using AppCore.Services.Indexer.Interface;
using Autofac.Integration.WebApi;
using HM1.DomainModel;
using HM1.DomainModel.Context;
using HM1.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace HM1.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ConfigureIoC();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            AppCore.IoC.Container.Resolve<IIndexRegister>().RegisterEntityTypes(typeof(HM1Context).Assembly); //hack
            AppCore.IoC.Container.Resolve<IIndexWorker>().Start();
            AppCore.IoC.Container.ResolveKeyed<IWorkerPool>("ScheduledJournal").Start();
            AppCore.IoC.Container.ResolveKeyed<ITransactionPollManager>("ScheduledJournalPoll").Start(TimeSpan.FromSeconds(10));
        }

        protected void Application_End(object sender, EventArgs e)
        {
            AppCore.IoC.Container.Resolve<IIndexWorker>().Stop();
            AppCore.IoC.Container.ResolveKeyed<ITransactionPollManager>("ScheduledJournalPoll").Stop();
            AppCore.IoC.Container.ResolveKeyed<IWorkerPool>("ScheduledJournal").Stop();
        }

        public void ConfigureIoC()
        {
            Autofac.ContainerBuilder builder = new Autofac.ContainerBuilder();

            //builder.RegisterType<HR1Context>().As<IHR1Context>();
            builder.AddDomainServices();
            builder.AddIdentityServices();
            builder.AddDomainModelIntercepts();
            builder.AddSearchIndexerServices();
            builder.AddSearchIndexers(typeof(HM1Context).Assembly);  //hack
            builder.AddDistibutedWorkerPools();
            //builder.AddScriptingServices();


            //builder.RegisterControllers(typeof(Startup).Assembly);
            builder.RegisterApiControllers(typeof(Startup).Assembly);

            AppCore.IoC.Container.IOCContainer = builder.Build();

            var config = System.Web.Http.GlobalConfiguration.Configuration;
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(AppCore.IoC.Container.IOCContainer));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(AppCore.IoC.Container.IOCContainer);
        }
    }
}
