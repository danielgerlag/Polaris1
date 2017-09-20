using AppCore.Services.Indexer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR1.DomainModel.Indexers
{
    public class EmploymentIndexer : EntityIndexer<Employment>
    {
        protected override IEnumerable<SearchKeyword> BuildKeyWords(Employment entity)
        {
            List<SearchKeyword> result = new List<SearchKeyword>();

            result.Add(new SearchKeyword(entity.Employee.EmployeeNumber));

            if (entity.Employee.Party != null)
            {
                result.Add(new SearchKeyword(entity.Employee.Party.FirstName));
                result.Add(new SearchKeyword(entity.Employee.Party.Surname));
            }

            return result;
        }

        protected override SearchResult BuildSearchResult(Employment entity)
        {
            SearchResult result = new SearchResult();

            result.EntityType = "Employment";
            result.Entity = entity;
            result.ID = entity.ID;

            result.Name = entity.Employee.Party.Surname + ", " + entity.Employee.Party.FirstName;            

            result.Reference = entity.Employee.EmployeeNumber;

            return result;
        }
    }
}
