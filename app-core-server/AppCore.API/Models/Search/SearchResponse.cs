using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.API.Models.Search
{
    public class SearchResponse
    {
        public SearchResponse()
        {
            Results = new List<SearchResponseLine>();
        }

        public List<SearchResponseLine> Results { get; set; }
    }

    public class SearchResponseLine
    {
        public int ID { get; set; }

        public string EntityType { get; set; }

        public string Reference { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public string Summary { get; set; }
    }
}
