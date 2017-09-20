using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Interface
{
    [Serializable]
    public class IndexQueueItem
    {
        public Type EntityType { get; set; }
        public int EntityID { get; set; }
        public bool Recursive { get; set; }

        public Type ContextType { get; set; }

        public override string ToString()
        {
            return EntityType.AssemblyQualifiedName + ":" + EntityID.ToString() + ":" + Recursive.ToString() + ":" + ContextType.AssemblyQualifiedName;
        }

        public static IndexQueueItem FromString(string s)
        {
            string[] items = s.Trim().Split(':');

            if (items.Length != 3)
                return null;

            int id = 0;
            bool recurse = false;

            if (Int32.TryParse(items[1], out id) && Boolean.TryParse(items[2], out recurse))
            {                
                IndexQueueItem result = new IndexQueueItem() { EntityType = Type.GetType(items[0]), EntityID = id, Recursive = recurse, ContextType = Type.GetType(items[3]) };
                return result;
            }
            return null;
        }
    }
}
