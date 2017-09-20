using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.API.Models.Search
{
    public class SuggestionResponse
    {
        public SuggestionResponse()
        {
            Suggestions = new List<string>();
        }

        public List<string> Suggestions { get; set; }
    }
}
