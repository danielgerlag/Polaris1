using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.IoC
{
    public class Adaptor
    {
        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public T ResolveKeyed<T>(string key)
        {
            return Container.ResolveKeyed<T>(key);
        }

        public object Resolve(Type t)
        {
            return Container.Resolve(t);
        }
    }
}
