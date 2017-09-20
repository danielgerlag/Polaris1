using AppCore.DomainModel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Interface
{
    public class SearchResult
    {
        public int ID { get; set; }

        public string EntityType { get; set; }

        public string ProductName { get; set; }

        public string Reference { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public string Summary { get; set; }

        public DateTime? LastModified { get; set; }

        public IDomainEntity Entity { get; set; } 

        public int Rank { get; set; }
    }
}
