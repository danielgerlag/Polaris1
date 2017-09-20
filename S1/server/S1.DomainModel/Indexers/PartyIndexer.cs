using AppCore.Services.Indexer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S1.DomainModel.Indexers
{
    public class PartyIndexer : EntityIndexer<Party>
    {
        protected override IEnumerable<SearchKeyword> BuildKeyWords(Party entity)
        {
            List<SearchKeyword> result = new List<SearchKeyword>();

            result.Add(new SearchKeyword(entity.Name));
            result.Add(new SearchKeyword(entity.FirstName));

            return result;
        }

        protected override SearchResult BuildSearchResult(Party entity)
        {
            SearchResult result = new SearchResult();

            result.EntityType = "Party";
            result.Entity = entity;
            result.ID = entity.ID;


            result.Name = entity.Name;
            //switch (entity.PartyType)
            //{
            //    case "I":
            //        result.Name = entity.Name + ", " + entity.FirstName;
            //        break;
            //    default:
            //        result.Name = entity.Name;
            //        break;
            //}

            result.Reference = result.Name;

            return result;
        }
    }
}
