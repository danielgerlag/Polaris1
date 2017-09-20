using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Interface
{
    public class SearchKeyword
    {
        public string Keyword { get; set; }

        public Type RefType { get; set; }

        public int? RefID { get; set; }

        public SearchKeyword(string keyword)
        {
            Keyword = keyword;
        }

        public SearchKeyword(string keyword, Type refType, int? refId)
        {
            Keyword = keyword;
            RefType = refType;
            RefID = refId;
        }

        public override int GetHashCode()
        {
            return Keyword.GetHashCode();
        }
    }
}
