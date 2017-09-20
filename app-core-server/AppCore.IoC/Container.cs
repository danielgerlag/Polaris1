using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.IoC
{
    public class Container
    {
        public static IContainer IOCContainer { get; set; }

        public static T Resolve<T>()
        {
            return IOCContainer.Resolve<T>();
        }

        public static T ResolveKeyed<T>(string key)
        {
            return IOCContainer.ResolveKeyed<T>(key);
        }

        public static object Resolve(Type t)
        {
            return IOCContainer.Resolve(t);
        }

    }
}
