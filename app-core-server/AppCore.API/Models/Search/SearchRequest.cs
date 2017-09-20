using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.API.Models.Search
{
    public class SearchRequest
    {
        public string SearchQuery { get; set; }
        public string SearchType { get; set; }
    }
}
