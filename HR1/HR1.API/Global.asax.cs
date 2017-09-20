using AppCore.Services.Indexer;
using AppCore.Services.Indexer.Interface;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using HR1.DomainModel;
using HR1.DomainModel.Context;
using HR1.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace HR1.API
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


            AppCore.IoC.Container.Resolve<IIndexRegister>().RegisterEntityTypes(typeof(HR1Context).Assembly); //hack
            AppCore.IoC.Container.Resolve<IIndexWorker>().Start();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            AppCore.IoC.Container.Resolve<IIndexWorker>().Stop();
        }

        public void ConfigureIoC()
        {
            Autofac.ContainerBuilder builder = new Autofac.ContainerBuilder();

            //builder.RegisterType<HR1Context>().As<IHR1Context>();
            builder.AddDomainServices();
            builder.AddIdentityServices();
            builder.AddDomainModelIntercepts();
            builder.AddSearchIndexerServices();
            builder.AddSearchIndexers(typeof(HR1Context).Assembly);  //hack
            //builder.AddTransactionProcessingServices();
            //builder.AddScriptingServices();


            builder.RegisterControllers(typeof(Startup).Assembly);
            builder.RegisterApiControllers(typeof(Startup).Assembly);

            AppCore.IoC.Container.IOCContainer = builder.Build();

            var config = System.Web.Http.GlobalConfiguration.Configuration;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(AppCore.IoC.Container.IOCContainer));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(AppCore.IoC.Container.IOCContainer);
        }
    }
}
