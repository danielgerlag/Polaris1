using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.Services.Indexer.Model
{
    [Table("EntityIndex", Schema = "Indexer")]
    public class EntityIndex
    {
        public EntityIndex()
        {
            Keywords = new HashSet<EntityKeyword>();
        }

        [Key]
        public long ID { get; set; }

        [Index]
        public int EntityTypeID { get; set; }

        public virtual EntityType EntityType { get; set; }

        [Index]
        public int EntityKey { get; set; }

        [Index]
        public int? AppTenantID { get; set; }

        [Index]
        public DateTime IndexTimeUTC { get; set; }

        public virtual ICollection<EntityKeyword> Keywords { get; set; }

    }
}
