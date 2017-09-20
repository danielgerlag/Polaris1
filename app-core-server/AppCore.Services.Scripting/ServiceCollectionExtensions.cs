using Autofac;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services.Scripting
{
    public static class ServiceCollectionExtensions
    {
        public static Autofac.ContainerBuilder AddScriptingServices(this Autofac.ContainerBuilder services)
        {

            services.Register<ScriptEngine>(x => IronPython.Hosting.Python.CreateEngine())
                .InstancePerDependency().Keyed<ScriptEngine>("Python");

            //services.Register<ScriptEngine>(x => IronRuby.Ruby.CreateEngine())
            //    .InstancePerDependency().Keyed<ScriptEngine>("Ruby");

            services.RegisterType<ScriptRunner>().As<IScriptRunner>().InstancePerDependency();


            return services;
        }


    }
}
