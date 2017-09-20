using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services.Scripting
{
    public class ScriptResult
    {
        //public T Context { get; set; }

        public bool Success { get; set; }

        public List<string> Log { get; set; } = new List<string>();
    }
}
